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

Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Js
Imports DotNetBrowser.WinForms

Namespace JavaScriptBridge.WinForms
    ''' <summary>
    '''     This example demonstrates how to use JS-.NET bridge in WinForms applications.
    ''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private ReadOnly browser As IBrowser
		Private ReadOnly engine As IEngine

		#Region "Constructors"

		Public Sub New()
			InitializeComponent()
			Dim webView = New BrowserView With {.Dock = DockStyle.Fill}
			tableLayoutPanel1.Controls.Add(webView, 1, 0)
			tableLayoutPanel1.SetRowSpan(webView, 2)
			engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build())
			browser = engine.CreateBrowser()
			webView.InitializeFrom(browser)
			AddHandler browser.ConsoleMessageReceived, Sub(sender, args)
			End Sub
		    Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<html>
                        <head>
                          <meta charset='UTF-8'>
                          <style>body{padding: 0; margin: 0; width:100%; height: 100%;}
                                textarea.fill {padding: 2; margin: 0; width:100%; height:100%;}
                                button{position: absolute; bottom: 0; padding: 2; width:100%;}</style>
                        </head>
                        <body>
                        <div>
                        <textarea id='text' class='fill' autofocus cols='30' rows='20'>Sample text</textarea>
                        <button id='updateForm' type='button' onClick='updateForm(document.getElementById(""text"").value)'>&lt; Update Form</button> 
                        </div>
                        </body>
                        </html>")
		    browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait()
			Dim window As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("window").Result
			window.Properties("updateForm") = CType(AddressOf UpdateForm, Action(Of String))
			AddHandler Me.FormClosing, AddressOf Form1_FormClosing
		End Sub

		#End Region

		#Region "Methods"

		Public Sub UpdateForm(ByVal value As String)
			BeginInvoke(CType(Sub() richTextBox1.Text = value, Action))
		End Sub

		Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button1.Click
			Dim textElement As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("document.getElementById('text');").Result
			textElement.Properties("value") = richTextBox1.Text
		End Sub

		Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
			engine?.Dispose()
		End Sub

		#End Region
	End Class
End Namespace