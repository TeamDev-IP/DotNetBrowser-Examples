#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
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
                    DotNetBrowser.Ui.Bitmap contents = img.Contents;

                    // 4. Convert the bitmap to the required format and save it.
                    Bitmap bitmap = ToBitmap(contents);
                    bitmap.Save("image.png", ImageFormat.Png);

                    Console.WriteLine("Image saved.");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static Bitmap ToBitmap(DotNetBrowser.Ui.Bitmap contents)
        {
            int width = (int) contents.Size.Width;
            int height = (int) contents.Size.Height;

            byte[] data = contents.Pixels.ToArray();
            Bitmap bmp = new Bitmap(width,
                                    height,
                                    PixelFormat.Format32bppArgb);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                              ImageLockMode.WriteOnly, bmp.PixelFormat);

            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}