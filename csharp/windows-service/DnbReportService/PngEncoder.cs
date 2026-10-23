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
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using DotNetBrowser.Ui;

namespace DnbReportService
{
    /// <summary>
    ///     Writes a DotNetBrowser <see cref="Bitmap" /> as a PNG file without
    ///     WPF, WinForms, or System.Drawing.
    /// </summary>
    /// <remarks>
    ///     The bitmap holds pre-multiplied BGRA pixels. Web pages are opaque,
    ///     so the encoder writes RGB and drops the alpha channel.
    /// </remarks>
    internal static class PngEncoder
    {
        private static readonly byte[] Signature =
        {
            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
        };

        private static readonly uint[] CrcTable = CreateCrcTable();

        /// <summary>
        ///     Writes <paramref name="bitmap" /> to <paramref name="path" />
        ///     as an 8-bit RGB PNG file.
        /// </summary>
        public static void Save(Bitmap bitmap, string path)
        {
            int width = (int)bitmap.Size.Width;
            int height = (int)bitmap.Size.Height;
            IReadOnlyList<byte> pixels = bitmap.Pixels;

            using (FileStream file = File.Create(path))
            {
                file.Write(Signature);

                byte[] header = new byte[13];
                BinaryPrimitives.WriteInt32BigEndian(header.AsSpan(0), width);
                BinaryPrimitives.WriteInt32BigEndian(header.AsSpan(4), height);
                header[8] = 8; // Bit depth.
                header[9] = 2; // Color type: RGB.
                WriteChunk(file, "IHDR", header);

                WriteChunk(file, "IDAT", Compress(pixels, width, height));
                WriteChunk(file, "IEND", Array.Empty<byte>());
            }
        }

        private static byte[] Compress(IReadOnlyList<byte> pixels, int width,
                                       int height)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (ZLibStream zlib = new ZLibStream(
                           output, CompressionLevel.Optimal, true))
                {
                    byte[] row = new byte[1 + width * 3];
                    for (int y = 0; y < height; y++)
                    {
                        row[0] = 0; // Filter: none.
                        int source = y * width * 4;
                        for (int x = 0; x < width; x++)
                        {
                            int target = 1 + x * 3;
                            row[target] = pixels[source + 2];
                            row[target + 1] = pixels[source + 1];
                            row[target + 2] = pixels[source];
                            source += 4;
                        }
                        zlib.Write(row);
                    }
                }
                return output.ToArray();
            }
        }

        private static void WriteChunk(Stream stream, string type,
                                       byte[] data)
        {
            byte[] typeBytes = Encoding.ASCII.GetBytes(type);
            byte[] number = new byte[4];

            BinaryPrimitives.WriteInt32BigEndian(number, data.Length);
            stream.Write(number);
            stream.Write(typeBytes);
            stream.Write(data);

            uint crc = UpdateCrc(0xFFFFFFFF, typeBytes);
            crc = UpdateCrc(crc, data) ^ 0xFFFFFFFF;
            BinaryPrimitives.WriteUInt32BigEndian(number, crc);
            stream.Write(number);
        }

        private static uint UpdateCrc(uint crc, byte[] data)
        {
            foreach (byte value in data)
            {
                crc = CrcTable[(crc ^ value) & 0xFF] ^ (crc >> 8);
            }
            return crc;
        }

        private static uint[] CreateCrcTable()
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
