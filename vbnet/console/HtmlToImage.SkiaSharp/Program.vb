#Region "Copyright"

' Copyright © 2024, TeamDev. All rights reserved.
' 
' Redistribution and use in source and/or binary forms, with or without
' modification, must retain the above copyright notice and the following
' disclaimer.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#End Region

Imports System
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Ui
Imports SkiaSharp

''' <summary>
'''     This example demonstrates how to get a screenshot of the web page
'''     and save it as a PNG image using SkiaSharp.
''' </summary>
Friend Class Program
    Public Shared Sub Main()
        Dim viewWidth As UInteger = 1024
        Dim viewHeight As UInteger = 20000
        Dim browserSize As New Size(viewWidth, viewHeight)

        Dim builder = New EngineOptions.Builder With {
            .RenderingMode = RenderingMode.OffScreen
        }
        builder.ChromiumSwitches.Add("--disable-gpu")
        builder.ChromiumSwitches.Add("--max-texture-size=" & viewHeight)

        Using engine As IEngine = EngineFactory.Create(builder.Build())
            Using browser As IBrowser = engine.CreateBrowser()
                ' #docfragment "HtmlToImage.SkiaSharp"
                ' 1. Resize browser to the required dimension.
                browser.Size = browserSize

                ' 2. Load the required web page and wait until it is loaded completely.
                Console.WriteLine("Loading https://html5test.teamdev.com")
                browser.Navigation.LoadUrl("https://html5test.teamdev.com").Wait()

                ' 3. Take the bitmap of the currently loaded web page. Its size will be 
                ' equal to the current browser's size.
                Dim image As Bitmap = browser.TakeImage()
                Console.WriteLine("Browser image taken")

                ' 4. Convert the bitmap to the required format and save it.
                Dim skBitmap As SKBitmap = ToSKBitmap(image)

                Using stream = File.OpenWrite(Path.GetFullPath("screenshot.png"))
                    Dim d As SKData = SKImage.FromBitmap(skBitmap).Encode(SKEncodedImageFormat.Png, 100)
                    d.SaveTo(stream)
                End Using
                Console.WriteLine("Browser image saved")
                ' #enddocfragment "HtmlToImage.SkiaSharp"
            End Using
        End Using
    End Sub

    ' #docfragment "HtmlToImage.SKBitmap.Conversion"
    Public Shared Function ToSKBitmap(browserBitmap As DotNetBrowser.Ui.Bitmap) As SKBitmap

        Dim width = CInt(browserBitmap.Size.Width)
        Dim height = CInt(browserBitmap.Size.Height)

        Dim data() As Byte = browserBitmap.Pixels.ToArray()
        Dim bitmap As SKBitmap = New SKBitmap()
        Dim gcHandle As GCHandle =
            GCHandle.Alloc(data, GCHandleType.Pinned)
        Dim info As SKImageInfo = New SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul)

        Dim ptr As IntPtr = gcHandle.AddrOfPinnedObject()
        Dim rowBytes = info.RowBytes
        bitmap.InstallPixels(info, ptr, rowBytes, Sub() gcHandle.Free())

        Return bitmap
    End Function
    ' #enddocfragment "HtmlToImage.SKBitmap.Conversion"
End Class
