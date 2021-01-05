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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Events
Imports DotNetBrowser.Engine
Imports DotNetBrowser.WinForms

Namespace FullScreen.WinForms
    ''' <summary>
    '''     The example demonstrates how to implement custom full-screen handling.
    ''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private ReadOnly browser As IBrowser
		Private ReadOnly browserView As BrowserView
		Private ReadOnly engine As IEngine
		Private fullScreenForm As Form

		#Region "Constructors"

		Public Sub New()
			InitializeComponent()
			browserView = New BrowserView With {.Dock = DockStyle.Fill}
			engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build())
			browser = engine.CreateBrowser()
			AddHandler browser.FullScreenEntered, AddressOf OnFullScreenEntered
			AddHandler browser.FullScreenExited, AddressOf OnFullScreenExited
			browserView.InitializeFrom(browser)
			browser.Navigation.LoadUrl("http://www.w3.org/2010/05/video/mediaevents.html")
			Controls.Add(browserView)
		End Sub

		#End Region

		#Region "Methods"

		Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Sub OnFullScreenEntered(ByVal sender As Object, ByVal e As FullScreenEnteredEventArgs)
			BeginInvoke(CType(Sub()
										 fullScreenForm = New Form()
										 fullScreenForm.TopMost = True
										 fullScreenForm.ShowInTaskbar = False
										 fullScreenForm.FormBorderStyle = FormBorderStyle.None
										 fullScreenForm.WindowState = FormWindowState.Maximized
										 fullScreenForm.Owner = Me

										 Controls.Remove(browserView)
										 fullScreenForm.Controls.Add(browserView)
										 fullScreenForm.Show()
			End Sub, Action))
		End Sub

		Private Sub OnFullScreenExited(ByVal sender As Object, ByVal e As FullScreenExitedEventArgs)
			BeginInvoke(CType(Sub()
										 fullScreenForm.Controls.Clear()
										 Controls.Add(browserView)
										 fullScreenForm.Hide()
										 fullScreenForm.Close()
			End Sub, Action))
		End Sub

		#End Region
	End Class
End Namespace