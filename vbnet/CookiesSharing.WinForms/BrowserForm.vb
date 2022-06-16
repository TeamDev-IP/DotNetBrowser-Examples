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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Cookies
Imports DotNetBrowser.Engine

Partial Public Class BrowserForm
	Inherits Form

	Private ReadOnly browser As IBrowser
	Private ReadOnly cookieStore As ICookieStore
	Private ReadOnly engine As IEngine

	Public Sub New(ByVal userDataPath As String, Optional ByVal cookies As IEnumerable(Of Cookie) = Nothing)
		' Create and initialize the IEngine instance.
		Dim builder As New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}

		If Not String.IsNullOrWhiteSpace(userDataPath) Then
			builder.UserDataDirectory = userDataPath
		End If

		Dim engineOptions As EngineOptions = builder.Build()
		engine = EngineFactory.Create(engineOptions)

		' Create the IBrowser instance.
		browser = engine.CreateBrowser()
		cookieStore = engine?.Profiles.Default.CookieStore
		UpdateCookies(cookies)
		InitializeComponent()

	    AddHandler textBox1.KeyUp, AddressOf textBox1_KeyUp

		AddHandler Me.FormClosed, Sub(sender, args)
			If engine IsNot Nothing Then
				engine.Dispose()
			End If
		End Sub

		' Initialize the Windows Forms BrowserView control.
		browserView1.InitializeFrom(browser)

		AddHandler browser.Navigation.FrameLoadFinished, Sub(sender, args)
			If args.Frame.IsMain Then
				If InvokeRequired Then
					BeginInvoke(New Action(Sub()
						textBox1.Text = args.Browser.Url
					End Sub))
				Else
					textBox1.Text = args.Browser.Url
				End If
			End If
		End Sub

		browser.Navigation.LoadUrl("https://mail.google.com/")
	End Sub

	Public Function GetAllCookies() As IEnumerable(Of Cookie)
		Return cookieStore?.GetAllCookies().Result
	End Function

	Private Sub textBox1_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
		If e.KeyCode = Keys.Enter Then
			browser.Navigation.LoadUrl(textBox1.Text)
		End If
	End Sub

	Private Sub UpdateCookies(ByVal cookies As IEnumerable(Of Cookie))
		If cookies IsNot Nothing Then
			Dim list As List(Of Cookie) = cookies.ToList()
			If list.Count > 0 Then
				cookieStore.DeleteAllCookies().Wait()
				For Each cookie As Cookie In list
					Dim success As Boolean = cookieStore.SetCookie(cookie).Result
				Next cookie

				cookieStore.Flush()
			End If
		End If
	End Sub
End Class
