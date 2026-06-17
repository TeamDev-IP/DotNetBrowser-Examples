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
using System.Linq;
using System.Runtime.InteropServices;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Ui;
using SkiaSharp;
using Size = DotNetBrowser.Geometry.Size;

namespace SaveImageFromPage
{
    /// <summary>
    ///     This example demonstrates how to obtain an image from the web page
    ///     and save it as file.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Size browserSize = new Size(500, 500);
            using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.OffScreen,
                FileAccessFromFilesAllowed = true
            }.Build()))
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    // 1. Resize browser to the required dimension.
                    browser.Size = browserSize;

                    // 2. Load the required web page and wait until it is loaded completely.
                    browser.Navigation.LoadUrl(Path.GetFullPath("sample.html")).Wait();

                    // 3. Fetch image contents from the IMG tag.
                    IImageElement img =
                        browser.MainFrame.Document
                               .GetElementByTagName("img") as IImageElement;
                    Bitmap contents = img.Contents;

                    // 4. Convert the bitmap to the required format and save it.
                    SKBitmap skBitmap = ToSKBitmap(contents);
                    using (var stream = File.OpenWrite(Path.GetFullPath("image.png")))
                    {
                        SKData d = SKImage.FromBitmap(skBitmap).Encode(SKEncodedImageFormat.Png, 100);
                        d.SaveTo(stream);
                    }

                    Console.WriteLine("Image saved.");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        // #docfragment "SaveImageFromPage.SKBitmap.Conversion"
        private static SKBitmap ToSKBitmap(Bitmap browserBitmap)
        {
            int width = (int) browserBitmap.Size.Width;
            int height = (int) browserBitmap.Size.Height;

            byte[] data = browserBitmap.Pixels.ToArray();
            SKBitmap bitmap = new SKBitmap();
            GCHandle gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            SKImageInfo info = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

            IntPtr ptr = gcHandle.AddrOfPinnedObject();
            int rowBytes = info.RowBytes;
            bitmap.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });

            return bitmap;
        }
        // #enddocfragment "SaveImageFromPage.SKBitmap.Conversion"
    }
}