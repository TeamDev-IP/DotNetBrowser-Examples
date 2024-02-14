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

Imports System
Imports System.IO
Imports System.Windows.Forms
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Js

Namespace ElementVisibility.WinForms
	''' <summary>
	'''		This example demonstrates how to change DOM element visibility
	'''		by modifying its CSS style.
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private ReadOnly browser As IBrowser
		Private ReadOnly engine As IEngine

		Public Sub New()
			engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())

			browser = engine.CreateBrowser()
			InitializeComponent()
			browserView1.InitializeFrom(browser)

			browser.Navigation.LoadUrl(Path.GetFullPath("index.html"))
		End Sub

		Private Sub ChangeImageVisibility(sender As Object, e As EventArgs) Handles button1.Click
			Dim document As IDocument = browser.MainFrame.Document

			' Find element on the loaded web page.
			Dim img As IElement = document.GetElementById("img")

			' Transform it into JsObject.
			Dim imgObject As IJsObject = TryCast(img, IJsObject)

			' Apply the desired style.
			Dim style As IJsObject = TryCast(imgObject.Properties("style"), IJsObject)

			If style.Properties("display").ToString() <> "none" Then
				style.Properties("display") = "none"
			Else
				style.Properties("display") = "inline"
			End If
		End Sub

		Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
			engine?.Dispose()
		End Sub
	End Class
End Namespace
