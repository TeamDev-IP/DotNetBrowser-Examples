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

Imports System
Imports System.Windows
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Input
Imports DotNetBrowser.Input.Mouse.Events
Imports Point = DotNetBrowser.Geometry.Point

Namespace Inspect.Wpf
	''' <summary>
	'''     This example demonstrates how to get DOM Node at a specific point on the web page.
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

		Private ReadOnly browser As IBrowser
		Private ReadOnly engine As IEngine

		Public Sub New()
			engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build())
			browser = engine.CreateBrowser()
			browser.Mouse.Moved.Handler = New Handler(Of IMouseMovedEventArgs, InputEventResponse)(AddressOf OnMouseMoved)
			InitializeComponent()
			browserView1.InitializeFrom(browser)
			browser.Navigation.LoadUrl("https://www.teamdev.com/dotnetbrowser")
		End Sub

		Private Sub GetNodeAtPoint(ByVal location As Point)
			Dim scale = If(PresentationSource.FromVisual(Me)?.CompositionTarget?.TransformToDevice.M11, 1)
			location = New Point(CInt(Math.Truncate(Math.Round(location.X * scale))), CInt(Math.Truncate(Math.Round(location.Y * scale))))
			Dim inspection As PointInspection = browser.MainFrame.Inspect(location)
			Dim inspectionNode As INode = If(inspection.UrlNode, inspection.Node)
			statusLabel1.Content = If(inspectionNode?.XPath, String.Empty)
		End Sub

		Private Sub MainWindow_OnClosed(ByVal sender As Object, ByVal e As EventArgs)
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Function OnMouseMoved(ByVal arg As IMouseMovedEventArgs) As InputEventResponse
			Dispatcher.BeginInvoke(CType(Sub() GetNodeAtPoint(arg.Location), Action))
			Return InputEventResponse.Proceed
		End Function

	End Class
End Namespace
