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

Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

Namespace DevTools.WinForms
	''' <summary>
	'''		This example demonstrates a possible way to configure
    '''     remote debugging port and use DevTools in the application.
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private browser1 As IBrowser
		Private browser2 As IBrowser
		Private engine As IEngine

		Public Sub New()
			Task.Run(Sub()
					 engine = EngineFactory.Create(New EngineOptions.Builder With {
						 .RenderingMode = RenderingMode.HardwareAccelerated,
						 .RemoteDebuggingPort = 9222
					 } .Build())
					 browser1 = engine.CreateBrowser()
					 browser2 = engine.CreateBrowser()
			End Sub).ContinueWith(Sub(t)
					 browserView1.InitializeFrom(browser1)
					 browserView2.InitializeFrom(browser2)

					 browser1.Navigation.LoadUrl("https://www.teamdev.com")
					 browser2.Navigation.LoadUrl(browser1.DevTools.RemoteDebuggingUrl)
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())
			InitializeComponent()
		End Sub

		Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
			engine?.Dispose()
		End Sub

	End Class
End Namespace
