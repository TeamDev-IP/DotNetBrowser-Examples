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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.WinForms

Namespace Kiosk.WinForms
	''' <summary>
	'''     This example demonstrates how to create a kiosk-like application
	'''     that shows a webpage using DotNetBrowser.
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private ReadOnly Property Browser() As IBrowser
		Private ReadOnly Property BrowserView() As BrowserView


		Private ReadOnly Property Engine() As IEngine

		Public Sub New()
			InitializeComponent()
			Engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())
			Browser = Engine.CreateBrowser()
			BrowserView = New BrowserView With {.Dock = DockStyle.Fill}

			BrowserView.InitializeFrom(Browser)
			'Disable default context menu
			Browser.ShowContextMenuHandler = Nothing
			Controls.Add(BrowserView)

			Browser.Navigation.LoadUrl("https://www.teamdev.com")
		End Sub

		Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
			Browser?.Dispose()
			Engine?.Dispose()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			TopMost = True
			FormBorderStyle = FormBorderStyle.None
			WindowState = FormWindowState.Maximized
		End Sub

	End Class
End Namespace
