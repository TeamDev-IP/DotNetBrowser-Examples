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
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Input
Imports DotNetBrowser.Input.Keyboard.Events

Namespace CustomShortcuts.WinForms
	''' <summary>
	'''		This example demonstrates how to configure custom shortcuts for the
    '''     browser.
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private browser As IBrowser
		Private engine As IEngine

		Public Sub New()
			Task.Run(Sub()
					 engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build())
					 browser = engine.CreateBrowser()
			End Sub).ContinueWith(Sub(t)
					 browserView1.InitializeFrom(browser)
					 browser.Navigation.LoadUrl("https://teamdev.com")
					 ' Set focus to browser.
					 browser.Focus()

					 browser.Keyboard.KeyPressed.Handler = New Handler(Of IKeyPressedEventArgs, InputEventResponse)(AddressOf HandleKeyPress)
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())
			InitializeComponent()
			AddHandler Me.FormClosing, AddressOf Form1_FormClosing
		End Sub

		Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
			engine?.Dispose()
		End Sub

		Private Function HandleKeyPress(ByVal e As IKeyPressedEventArgs) As InputEventResponse
			Debug.WriteLine("Key: " & e.VirtualKey.ToString())
			' Map Ctrl-'P' to "Print"
			If e.Modifiers.ControlDown AndAlso e.VirtualKey = KeyCode.VkP Then
				Debug.WriteLine("Print")
				BeginInvoke(CType(Sub() browser.MainFrame.Print(), Action))
			End If

			' Map Ctrl-'+' to "Zoom In"
			If e.Modifiers.ControlDown AndAlso e.VirtualKey = KeyCode.Add Then
				Debug.WriteLine("Zoom In")
				BeginInvoke(CType(Sub() browser.Zoom.In(), Action))
			End If

			' Map Ctrl-'-' to "Zoom Out"
			If e.Modifiers.ControlDown AndAlso e.VirtualKey = KeyCode.Subtract Then
				Debug.WriteLine("Zoom Out")
				BeginInvoke(CType(Sub() browser.Zoom.Out(), Action))
			End If

			Return InputEventResponse.Proceed
		End Function

	End Class
End Namespace
