#Region "Copyright"

' Copyright © 2026, TeamDev. All rights reserved.
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

Imports System.IO
Imports System.Runtime.InteropServices
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Ui
Imports SkiaSharp

Namespace SaveImageFromPage
    ''' <summary>
    '''     This example demonstrates how to obtain an image from the web page
    '''     and save it as file.
    ''' </summary>
    Friend Class Program
        Public Shared Sub Main(args() As String)
            Dim browserSize As New Size(500, 500)
            Dim builder = New EngineOptions.Builder With {
                    .RenderingMode = RenderingMode.OffScreen,
                    .FileAccessFromFilesAllowed = True
            }
            Using engine As IEngine = EngineFactory.Create(builder.Build())
                Using browser As IBrowser = engine.CreateBrowser()
                    ' 1. Resize browser to the required dimension.
                    browser.Size = browserSize

                    ' 2. Load the required web page and wait until it is loaded completely.
                    browser.Navigation.LoadUrl(Path.GetFullPath("sample.html")).Wait()

                    ' 3. Fetch image contents from the IMG tag.
                    Dim img = TryCast(browser.MainFrame.Document.GetElementByTagName("img"),
                                      IImageElement)
                    Dim contents As Bitmap = img.Contents

                    ' 4. Convert the bitmap to the required format and save it.
                    ' #docfragment "SaveImageFromPage.SKBitmap.Conversion"
                    Dim skBitmap As SKBitmap = ToSKBitmap(contents)
                    Using stream = File.OpenWrite(Path.GetFullPath("image.png"))
                        Dim d As SKData = SKImage.FromBitmap(skBitmap).Encode(SKEncodedImageFormat.Png, 100)
                        d.SaveTo(stream)
                    End Using
                    ' #enddocfragment "SaveImageFromPage.SKBitmap.Conversion"

                    Console.WriteLine("Image saved.")
                End Using
            End Using

            Console.WriteLine("Press any key to terminate...")
            Console.ReadKey()
        End Sub

        Private Shared Function ToSKBitmap(browserBitmap As Bitmap) As SKBitmap
            Dim width = CInt(browserBitmap.Size.Width)
            Dim height = CInt(browserBitmap.Size.Height)

            Dim data() As Byte = browserBitmap.Pixels.ToArray()
            Dim bitmap As New SKBitmap()
            Dim gcHandle As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
            Dim info As New SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul)

            Dim ptr As IntPtr = gcHandle.AddrOfPinnedObject()
            Dim rowBytes = info.RowBytes
            bitmap.InstallPixels(info, ptr, rowBytes, Sub() gcHandle.Free())

            Return bitmap
        End Function
    End Class
End Namespace