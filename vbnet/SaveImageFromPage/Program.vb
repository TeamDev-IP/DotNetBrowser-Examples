#Region "Copyright"

' Copyright 2021, TeamDev. All rights reserved.
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
Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Js

Namespace SaveImageFromPage
	''' <summary>
	'''     This example demonstrates how to obtain an image from the web page and save it as file.
	''' </summary>
	Friend Class Program
		#Region "Methods"

		Public Shared Function FixBase64ForImage(ByVal image As String) As String
			Dim sbText As New StringBuilder(image, image.Length)
			sbText.Replace(vbCrLf, String.Empty)
			sbText.Replace(" ", String.Empty)
			Dim base64ForImage As String = sbText.ToString()
			'Remove prefix
			base64ForImage = base64ForImage.Split(","c)(1)
			Return base64ForImage
		End Function

		Public Shared Sub Main(ByVal args() As String)
			Try
				Dim browserSize As New Size(500, 500)
			    Dim builder  = New EngineOptions.Builder With {
                    .RenderingMode = RenderingMode.OffScreen
                }
                builder.ChromiumSwitches.Add("--allow-file-access-from-files")
			    Using engine As IEngine = EngineFactory.Create(builder.Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						' 1. Resize browser to the required dimension.
						browser.Size = browserSize

						' 2. Load the required web page and wait until it is loaded completely.
						browser.Navigation.LoadUrl(Path.GetFullPath("sample.html")).Wait()

						' 3. Create canvas, set its width and height
						Dim canvas As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("document.createElement('canvas');").Result
						Dim image As IElement = browser.MainFrame.Document.GetElementByTagName("img")

						Dim width As String = image.Attributes("width")
						canvas.Properties("width") = width
						Dim height As String = image.Attributes("height")
						canvas.Properties("height") = height

						' 4. Get the canvas context and draw the image on it
						Dim ctx As IJsObject = TryCast(canvas.Invoke("getContext", "2d"), IJsObject)
						ctx.Invoke("drawImage", image, 0, 0)

						' 5. Get the data URL and convert it to bytes
						Dim dataUrl As String = TryCast(canvas.Invoke("toDataURL", "image/png"), String)
						Console.WriteLine("Data URL: " & dataUrl)
						Dim bitmapData() As Byte = Convert.FromBase64String(FixBase64ForImage(dataUrl))

						' 4. Save image to file.
						Using fs As New FileStream("image.png", FileMode.Create, FileAccess.Write)
							fs.Write(bitmapData, 0, bitmapData.Length)
						End Using

						Console.WriteLine("Image saved.")
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try

			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

		#End Region
	End Class
End Namespace