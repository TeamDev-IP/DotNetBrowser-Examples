#Region "Copyright"

' Copyright 2019, TeamDev. All rights reserved.
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
Imports System.Net
Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Net

Namespace CustomRequestHandlingSample
	Friend Class Program
		#Region "Methods"

		Public Shared Sub Main()
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					engine.NetworkService.UrlRequestHandler = New CustomUrlRequestHandler()
					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")
						Dim loadResult As LoadResult = browser.Navigation.LoadUrl("myscheme://test1").Result
						Console.WriteLine("Load result: " & loadResult)
						Console.WriteLine("HTML: " & browser.MainFrame.Html)
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try
			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

		#End Region

		Private Class CustomUrlRequestHandler
			Implements IUrlRequestHandler

			#Region "Methods"

			Public Sub HandleRequest(ByVal interceptedRequest As IInterceptedUrlRequest) Implements IUrlRequestHandler.HandleRequest
				Console.WriteLine("Intercepted request to URL:" & interceptedRequest.UrlRequest.Url)
				interceptedRequest.Write(Encoding.UTF8.GetBytes("Hello world!"))
				interceptedRequest.Complete()
			End Sub

            Public Function InterceptRequest(ByVal request As UrlRequestData) As InterceptUrlRequestResult Implements IUrlRequestHandler.InterceptRequest
				If request.Request.Url.StartsWith("myscheme") Then
					Return InterceptUrlRequestResult.Intercept(HttpStatusCode.OK)
				End If
				Return InterceptUrlRequestResult.Continue()
			End Function

			#End Region
		End Class
	End Class
End Namespace