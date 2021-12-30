#region Copyright

// Copyright © 2021, TeamDev. All rights reserved.
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
using System.Text;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Js;

namespace SaveImageFromPage
{
    /// <summary>
    ///     This example demonstrates how to obtain an image from the web page and save it as file.
    /// </summary>
    internal class Program
    {
        public static string FixBase64ForImage(string image)
        {
            StringBuilder sbText = new StringBuilder(image, image.Length);
            sbText.Replace("\r\n", string.Empty);
            sbText.Replace(" ", string.Empty);
            string base64ForImage = sbText.ToString();
            //Remove prefix
            base64ForImage = base64ForImage.Split(',')[1];
            return base64ForImage;
        }

        private static void Main(string[] args)
        {
                Size browserSize = new Size(500, 500);
                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder
                       {
                           RenderingMode = RenderingMode.OffScreen,
                           ChromiumSwitches = {"--allow-file-access-from-files"}
                       }.Build()))
                {
                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        // 1. Resize browser to the required dimension.
                        browser.Size = browserSize;

                        // 2. Load the required web page and wait until it is loaded completely.
                        browser.Navigation.LoadUrl(Path.GetFullPath("sample.html")).Wait();

                        // 3. Create canvas, set its width and height
                        IJsObject canvas = browser
                                          .MainFrame
                                          .ExecuteJavaScript<
                                               IJsObject>("document.createElement('canvas');")
                                          .Result;
                        IElement image = browser.MainFrame.Document.GetElementByTagName("img");

                        string width = image.Attributes["width"];
                        canvas.Properties["width"] = width;
                        string height = image.Attributes["height"];
                        canvas.Properties["height"] = height;

                        // 4. Get the canvas context and draw the image on it
                        IJsObject ctx = canvas.Invoke("getContext", "2d") as IJsObject;
                        ctx.Invoke("drawImage", image, 0, 0);

                        // 5. Get the data URL and convert it to bytes
                        string dataUrl = canvas.Invoke("toDataURL", "image/png") as string;
                        Console.WriteLine($"Data URL: {dataUrl}");
                        byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(dataUrl));

                        // 4. Save image to file.
                        using (FileStream fs =
                               new FileStream("image.png", FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(bitmapData, 0, bitmapData.Length);
                        }

                        Console.WriteLine("Image saved.");
                    }
                }

                Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }
}