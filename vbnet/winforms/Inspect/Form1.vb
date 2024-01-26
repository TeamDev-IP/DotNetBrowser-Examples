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
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Input
Imports DotNetBrowser.Input.Mouse.Events

Namespace Inspect.WinForms
	''' <summary>
	'''     This example demonstrates how to get DOM Node at a specific point on the web page.
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private ReadOnly browser As IBrowser
		Private ReadOnly engine As IEngine

		Public Sub New()
			engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build())
			browser = engine.CreateBrowser()
			browser.Mouse.Moved.Handler = New Handler(Of IMouseMovedEventArgs, InputEventResponse)(AddressOf OnMouseMoved)
			InitializeComponent()
			browserView1.InitializeFrom(browser)
			browser.Navigation.LoadUrl("https://html5test.teamdev.com")
		End Sub

		Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Sub GetNodeAtPoint(ByVal locationPoint As Point)
			Dim inspection As PointInspection = browser.MainFrame.Inspect(locationPoint)
			Dim inspectionNode As INode = If(inspection.UrlNode, inspection.Node)
			statusLabel1.Text = If(inspectionNode?.XPath, String.Empty)
		End Sub

		Private Function OnMouseMoved(ByVal arg As IMouseMovedEventArgs) As InputEventResponse
			BeginInvoke(CType(Sub() GetNodeAtPoint(arg.Location), Action))
			Return InputEventResponse.Proceed
		End Function

	End Class
End Namespace
