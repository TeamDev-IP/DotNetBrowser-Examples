#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
//
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using DotNetBrowser.Ui;

namespace GuardedBrowser
{
    /// <summary>
    ///     Encodes a DotNetBrowser bitmap as PNG without a UI framework dependency.
    /// </summary>
    internal static class PngEncoder
    {
        private static readonly uint[] CrcTable = BuildCrcTable();

        public static byte[] Encode(Bitmap bitmap)
        {
            int width = (int) bitmap.Size.Width;
            int height = (int) bitmap.Size.Height;
            var pixels = bitmap.Pixels;

            // One filter byte per row, then RGB. The source is BGRA.
            byte[] raw = new byte[height * (1 + width * 3)];
            int offset = 0;
            for (int y = 0; y < height; y++)
            {
                raw[offset++] = 0;
                for (int x = 0; x < width; x++)
                {
                    int i = (y * width + x) * 4;
                    raw[offset++] = pixels[i + 2];
                    raw[offset++] = pixels[i + 1];
                    raw[offset++] = pixels[i];
                }
            }

            using MemoryStream png = new();
            png.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });

            byte[] header = new byte[13];
            WriteBigEndian(header, 0, width);
            WriteBigEndian(header, 4, height);
            header[8] = 8; // bit depth
            header[9] = 2; // colour type: RGB
            WriteChunk(png, "IHDR", header);

            using (MemoryStream compressed = new())
            {
                using (ZLibStream zlib = new(compressed, CompressionLevel.Optimal, leaveOpen: true))
                {
                    zlib.Write(raw);
                }
                WriteChunk(png, "IDAT", compressed.ToArray());
            }

            WriteChunk(png, "IEND", Array.Empty<byte>());
            return png.ToArray();
        }

        private static void WriteChunk(Stream stream, string type, byte[] data)
        {
            byte[] length = new byte[4];
            WriteBigEndian(length, 0, data.Length);
            stream.Write(length);
            byte[] typeBytes = Encoding.ASCII.GetBytes(type);
            stream.Write(typeBytes);
            stream.Write(data);
            uint crc = Crc(data, Crc(typeBytes, 0xFFFFFFFF)) ^ 0xFFFFFFFF;
            byte[] crcBytes = new byte[4];
            WriteBigEndian(crcBytes, 0, (int) crc);
            stream.Write(crcBytes);
        }

        private static uint Crc(byte[] data, uint crc)
        {
            foreach (byte b in data)
            {
                crc = CrcTable[(crc ^ b) & 0xFF] ^ (crc >> 8);
            }
            return crc;
        }

        private static void WriteBigEndian(byte[] buffer, int offset, int value)
        {
            buffer[offset] = (byte) (value >> 24);
            buffer[offset + 1] = (byte) (value >> 16);
            buffer[offset + 2] = (byte) (value >> 8);
            buffer[offset + 3] = (byte) value;
        }

        private static uint[] BuildCrcTable()
        {
            uint[] table = new uint[256];
            for (uint n = 0; n < 256; n++)
            {
                uint c = n;
                for (int k = 0; k < 8; k++)
                {
                    c = (c & 1) != 0 ? 0xEDB88320 ^ (c >> 1) : c >> 1;
                }
                table[n] = c;
            }
            return table;
        }
    }
}
