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
Imports DotNetBrowser.Engine

Namespace Kiosk.Wpf
	''' <summary>
	'''     This example demonstrates how to create a kiosk-like application
	'''     that shows a webpage using DotNetBrowser.
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

		Private browser As IBrowser
		Private engine As IEngine

		Public Sub New()
		    EngineFactory.CreateAsync(New EngineOptions.Builder With {
                                         .RenderingMode = RenderingMode.HardwareAccelerated
                                         }.Build()) _
            .ContinueWith(Sub(t)
                engine = t.Result
                browser = engine.CreateBrowser()
                BrowserView.InitializeFrom(browser)
                'Disable default context menu
                browser.ShowContextMenuHandler = Nothing
                browser.Navigation.LoadUrl("https://www.teamdev.com")
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())

			' Initialize Wpf Application UI.
			InitializeComponent()
		End Sub

		Private Sub MainWindow_OnClosed(ByVal sender As Object, ByVal e As EventArgs)
			browser?.Dispose()
			engine?.Dispose()
		End Sub

	End Class
End Namespace
