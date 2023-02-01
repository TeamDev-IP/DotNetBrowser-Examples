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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Navigation.Handlers

Namespace MailToHandling.WinForms
	''' <summary>
	'''     This example demonstrates how to open an external application
	'''     for handling a specific URI scheme, e.g. "mailto".
	''' 
	'''		Please note that an email client should be present in the operating system 
    '''     for this example to work properly.
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private browser As IBrowser
		Private engine As IEngine

		Public Sub New()
		    Dim engineOptions As EngineOptions = New DotNetBrowser.Engine.EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build()

		    EngineFactory.CreateAsync(engineOptions).ContinueWith(Sub(t)
		        engine = t.Result
		        browser = engine.CreateBrowser()
                browser.Navigation.StartNavigationHandler = New Handler(Of StartNavigationParameters, StartNavigationResponse)(AddressOf OnStartNavigation)
                browserView1.InitializeFrom(browser)
                browser.Navigation.LoadUrl("https://www.teamdev.com/contact")
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())
			InitializeComponent()
		End Sub

		Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Function OnStartNavigation(ByVal parameters As StartNavigationParameters) As StartNavigationResponse
			Dim url As String = parameters.Url

			Debug.WriteLine("OnStartNavigation: url = " & url)


			If url.StartsWith("mailto:") Then
				' If navigate to mailto: link, the default mail client should be opened.
				' For this purpose, it is enough to launch this URL as a command line.
				Process.Start(url)
				
			    ' The navigation request in the browser should be ignored in this case.
				Return StartNavigationResponse.Ignore()
			End If

			Return StartNavigationResponse.Start()
		End Function

	End Class
End Namespace
