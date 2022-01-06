#Region "Copyright"

' Copyright © 2022, TeamDev. All rights reserved.
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
Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Js

Namespace InjectObjectForScripting
	''' <summary>
	'''     This example demonstrates how to use InjectJsHandler for injecting "window.external"
	'''     object into the web page.
	''' </summary>
	Friend Class Program

		Private Shared Sub InjectObjectForScripting(ByVal p As InjectJsParameters)
			Dim window As IJsObject = p.Frame.ExecuteJavaScript(Of IJsObject)("window").Result
			window.Properties("external") = ObjectForScripting.Instance
		End Sub

		Public Shared Sub Main(ByVal args() As String)
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")
						browser.Size = New Size(700, 500)
						browser.InjectJsHandler = New Handler(Of InjectJsParameters)(AddressOf InjectObjectForScripting)
					    Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<html>
                                     <body>
                                        <script type='text/javascript'>
                                            var SetTitle = function () 
                                            {
                                                 document.title = window.external.GetTitle();
                                            };
                                        </script>
                                     </body>
                                   </html>")
					    browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait()

						browser.MainFrame.ExecuteJavaScript(Of IJsObject)("window.SetTitle();").Wait()

						Console.WriteLine(vbTab & "Browser title: " & browser.Title)
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try

			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

		Public NotInheritable Class ObjectForScripting

			Public Shared ReadOnly Property Instance() As New ObjectForScripting()

			Shared Sub New()
			End Sub

			Private Sub New()
			End Sub

			Public Function GetTitle() As String
				Return "Document title from .NET"
			End Function

		End Class
	End Class
End Namespace
