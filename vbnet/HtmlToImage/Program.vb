#Region "Copyright"

' Copyright © 2020, TeamDev. All rights reserved.
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
Imports System.Runtime.InteropServices
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Ui

Friend Class Program

#Region "Methods"

    Public Shared Sub Main()
        Dim viewWidth As UInteger = 1024
        Dim viewHeight As UInteger = 20000
        Dim browserSize As New Size(viewWidth, viewHeight)
        Try
            Dim builder = New EngineOptions.Builder With {
                    .RenderingMode = RenderingMode.OffScreen
                    }
            builder.ChromiumSwitches.Add("--disable-gpu")
            builder.ChromiumSwitches.Add("--max-texture-size=" & viewHeight)

            Using engine As IEngine = EngineFactory.Create(builder.Build())
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    ' 1. Resize browser to the required dimension.
                    browser.Size = browserSize

                    ' 2. Load the required web page and wait until it is loaded completely.
                    Console.WriteLine("Loading http://www.teamdev.com/dotnetbrowser")
                    browser.Navigation.LoadUrl("http://www.teamdev.com/dotnetbrowser").Wait()

                    ' 3. Take the bitmap of the currently loaded web page. Its size will be 
                    ' equal to the current browser's size.
                    Dim image As Bitmap = browser.TakeImage()
                    Console.WriteLine("Browser image taken")

                    ' 4. Convert the bitmap to the required format and save it.
                    Dim bitmap As Drawing.Bitmap = ToBitmap(image)
                    bitmap.Save("screenshot.png", ImageFormat.Png)
                    Console.WriteLine("Browser image saved")
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try
        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Public Shared Function ToBitmap(bitmap As Bitmap) As Drawing.Bitmap
        Dim width = CInt(bitmap.Size.Width)
        Dim height = CInt(bitmap.Size.Height)

        Dim data() As Byte = bitmap.Pixels.ToArray()
        Dim bmp As New Drawing.Bitmap(width, height, PixelFormat.Format32bppRgb)
        Dim bmpData As BitmapData = bmp.LockBits(New Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                                 ImageLockMode.WriteOnly, bmp.PixelFormat)

        Marshal.Copy(data, 0, bmpData.Scan0, data.Length)
        bmp.UnlockBits(bmpData)
        Return bmp
    End Function

#End Region
End Class