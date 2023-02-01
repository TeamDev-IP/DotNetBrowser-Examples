#region Copyright

// Copyright © 2023, TeamDev. All rights reserved.
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
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Ui;
using SkiaSharp;

/// <summary>
///     This example demonstrates how to get a screenshot of the web page
///     and save it as a PNG image using SkiaSharp.
/// </summary>

uint viewWidth = 1024;
uint viewHeight = 20000;
Size browserSize = new Size(viewWidth, viewHeight);

using(IEngine engine = EngineFactory.Create(new EngineOptions.Builder
{
    RenderingMode = RenderingMode.OffScreen,
    ChromiumSwitches = { "--disable-gpu", "--max-texture-size=" + viewHeight }
}.Build()))
{
    using(IBrowser browser = engine.CreateBrowser())
    {
        // #docfragment "HtmlToImage.SkiaSharp"
        // 1. Resize browser to the required dimension.
        browser.Size = browserSize;

        // 2. Load the required web page and wait until it is loaded completely.
        Console.WriteLine("Loading https://www.teamdev.com/dotnetbrowser");
        browser.Navigation
                .LoadUrl("https://www.teamdev.com/dotnetbrowser")
                .Wait();

        // 3. Take the bitmap of the currently loaded web page. Its size will be 
        // equal to the current browser's size.
        Bitmap image = browser.TakeImage();
        Console.WriteLine("Browser image taken");

        // 4. Convert the bitmap to the required format and save it.
        SKBitmap skBitmap = ToSKBitmap(image);

        using(var stream = File.OpenWrite(Path.GetFullPath("screenshot.png"))) 
        {
            SKData d = SKImage.FromBitmap(skBitmap).Encode(SKEncodedImageFormat.Png, 100);
            d.SaveTo(stream); 
        }
        // #enddocfragment "HtmlToImage.SkiaSharp"
    }
    Console.WriteLine("Browser image saved");
}

// #docfragment "HtmlToImage.SKBitmap.Conversion"
static SKBitmap ToSKBitmap(Bitmap browserBitmap)
{
    int width = (int)browserBitmap.Size.Width;
    int height = (int)browserBitmap.Size.Height;

    byte[] data = browserBitmap.Pixels.ToArray();
    SKBitmap bitmap = new();
    GCHandle gcHandle = 
        GCHandle.Alloc(data, GCHandleType.Pinned);
    SKImageInfo info = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

    IntPtr ptr = gcHandle.AddrOfPinnedObject();
    int rowBytes = info.RowBytes;
    bitmap.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });

    return bitmap;
}
// #enddocfragment "HtmlToImage.SKBitmap.Conversion"
