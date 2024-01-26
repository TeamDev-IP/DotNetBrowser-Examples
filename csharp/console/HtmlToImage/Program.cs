#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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
using System.Linq;
using System.Runtime.InteropServices;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Size = DotNetBrowser.Geometry.Size;

namespace HtmlToImage
{
    /// <summary>
    ///     This example demonstrates how to get a screenshot of the web page
    ///     and save it as a PNG image.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            uint viewWidth = 1024;
            uint viewHeight = 20000;
            Size browserSize = new Size(viewWidth, viewHeight);

            using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder
                   {
                       RenderingMode = RenderingMode.OffScreen,
                       ChromiumSwitches = {"--disable-gpu", "--max-texture-size=" + viewHeight}
                   }.Build()))
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    // #docfragment "HtmlToImage"
                    // 1. Resize browser to the required dimension.
                    browser.Size = browserSize;

                    // 2. Load the required web page and wait until it is loaded completely.
                    Console.WriteLine("Loading https://html5test.teamdev.com");
                    browser.Navigation
                           .LoadUrl("https://html5test.teamdev.com")
                           .Wait();

                    // 3. Take the bitmap of the currently loaded web page. Its size will be 
                    // equal to the current browser's size.
                    DotNetBrowser.Ui.Bitmap image = browser.TakeImage();
                    Console.WriteLine("Browser image taken");

                    // 4. Convert the bitmap to the required format and save it.
                    Bitmap bitmap = ToBitmap(image);
                    bitmap.Save("screenshot.png", ImageFormat.Png);
                    Console.WriteLine("Browser image saved");
                    // #enddocfragment "HtmlToImage"
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        // #docfragment "HtmlToImage.Conversion"
        public static Bitmap ToBitmap(DotNetBrowser.Ui.Bitmap bitmap)
        {
            int width = (int) bitmap.Size.Width;
            int height = (int) bitmap.Size.Height;

            byte[] data = bitmap.Pixels.ToArray();
            Bitmap bmp = new Bitmap(width,
                                    height,
                                    PixelFormat.Format32bppRgb);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                              ImageLockMode.WriteOnly, bmp.PixelFormat);

            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        // #enddocfragment "HtmlToImage.Conversion"
    }
}
