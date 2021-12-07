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
Imports System.Diagnostics
Imports System.Threading.Tasks
Imports System.Windows
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Input
Imports DotNetBrowser.Input.Mouse.Events

Namespace Zoom.Wpf
	''' <summary>
	'''     The example demonstrates how to implement zooming
	'''     on mouse scroll with Ctrl pressed.
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

		Private browser As IBrowser
		Private engine As IEngine

		Public Sub New()
			Try
				Task.Run(Sub()
						 engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())
						 browser = engine.CreateBrowser()
						 browser.Navigation.LoadUrl("teamdev.com")
						 browser.Mouse.WheelMoved.Handler = New Handler(Of IMouseWheelMovedEventArgs, InputEventResponse)(AddressOf OnMouseWheelMoved)
				End Sub).ContinueWith(Sub(t)
						 BrowserView.InitializeFrom(browser)
				End Sub, TaskScheduler.FromCurrentSynchronizationContext())

				InitializeComponent()
			Catch exception As Exception
				Debug.WriteLine(exception)
			End Try
		End Sub

		Private Sub EnableZoom(ByVal zoomEnabled As Boolean)
			If browser IsNot Nothing Then
				Debug.WriteLine("Zoom Enabled: " & zoomEnabled)
				browser.Zoom.Enabled = zoomEnabled
			End If
		End Sub

		Private Sub MainWindow_OnClosed(ByVal sender As Object, ByVal e As EventArgs)
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Function OnMouseWheelMoved(ByVal arg As IMouseWheelMovedEventArgs) As InputEventResponse
			If arg.Modifiers.ControlDown Then
				If arg.DeltaY > 0 Then
					Debug.WriteLine("Zoom In")
					browser.Zoom.In()
				Else
					Debug.WriteLine("Zoom Out")
					browser.Zoom.Out()
				End If
			End If

			Return InputEventResponse.Proceed
		End Function

		Private Sub ZoomEnabledCheckbox_OnChecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			EnableZoom(True)
		End Sub

		Private Sub ZoomEnabledCheckbox_OnUnchecked(ByVal sender As Object, ByVal e As RoutedEventArgs)
			EnableZoom(False)
		End Sub
	End Class
End Namespace