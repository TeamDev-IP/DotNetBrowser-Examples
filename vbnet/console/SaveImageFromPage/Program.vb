#Region "Copyright"

' Copyright © 2023, TeamDev. All rights reserved.
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

Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Ui

Namespace SaveImageFromPage
    ''' <summary>
    '''     This example demonstrates how to obtain an image from the web page
    '''     and save it as file.
    ''' </summary>
    Friend Class Program
        Public Shared Sub Main(args() As String)
            Dim browserSize As New Size(500, 500)
            Dim builder  = New EngineOptions.Builder With {
                    .RenderingMode = RenderingMode.OffScreen,
                    .FileAccessFromFilesAllowed = true
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
                    Dim bitmap As Drawing.Bitmap = ToBitmap(contents)
                    bitmap.Save("image.png", ImageFormat.Png)

                    Console.WriteLine("Image saved.")
                End Using
            End Using

            Console.WriteLine("Press any key to terminate...")
            Console.ReadKey()
        End Sub

        Private Shared Function ToBitmap(contents As Bitmap) As Drawing.Bitmap
            Dim width = CInt(contents.Size.Width)
            Dim height = CInt(contents.Size.Height)

            Dim data() As Byte = contents.Pixels.ToArray()
            Dim bmp As New Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb)

            Dim bmpData As BitmapData = bmp.LockBits(
                New Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly,
                bmp.PixelFormat)

            Marshal.Copy(data, 0, bmpData.Scan0, data.Length)
            bmp.UnlockBits(bmpData)
            Return bmp
        End Function
    End Class
End Namespace
