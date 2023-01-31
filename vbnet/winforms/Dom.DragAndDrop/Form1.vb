#Region "Copyright"

' Copyright © 2023, TeamDev. All rights reserved.
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
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Dom.Events
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Js
Imports DotNetBrowser.Navigation

Namespace Dom.DragAndDrop.WinForms
	''' <summary>
	'''     This example demonstrates two possible approaches to listen to the Drag and Drop events
	'''     for the particular DOM element
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private browser As IBrowser
		Private engine As IEngine

		Public Sub New()
		    EngineFactory.CreateAsync(New EngineOptions.Builder With {
                                         .RenderingMode = RenderingMode.HardwareAccelerated
                                         }.Build()) _
		    .ContinueWith(Sub(t)
                engine = t.Result
                browser = engine.CreateBrowser()
                browserView1.InitializeFrom(browser)
                browser.InjectJsHandler = New Handler(Of InjectJsParameters)(AddressOf OnInjectJs)

                AddHandler browser.ConsoleMessageReceived, Sub(sender, args)
                 Debug.WriteLine($"{args.LineNumber} > {args.Message}")
                End Sub

                Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<html>
                                <head>
                                  <meta charset='UTF-8'>
                                  <style type='text/css'>
                                    #dropZone {
                                        text-align: center;    

                                        width: 400px;
                                        padding: 50px 0;
                                        margin: 50px auto;
                                        
                                        background: #eee;
                                        border: 1px solid #ccc;
                                    }
                                  </style>
                                </head>
                                <body>
                                
                                <div id='dropZone'>
                                    Drop a file here.
                                </div >
                                </body>
                                </html>")

                browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)) _
                             .ContinueWith(AddressOf OnHtmlLoaded)
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())

			InitializeComponent()
			AddHandler Me.FormClosing, AddressOf Form1_FormClosing
		End Sub

		Public Sub Drop(ByVal data As IJsObject)
			WriteLine($"JavaScript Drop callback received: {data}")
			If data.Properties.Contains("length") Then
				Dim length As Double = DirectCast(data.Properties("length"), Double)
				For i As UInteger = 0 To length - 1
					Dim file As IJsObject = TryCast(data.Properties(i), IJsObject)
					If file IsNot Nothing Then
						WriteLine($"data[{i}] = {file.Properties("name")}")
					End If
				Next i
			End If
		End Sub

		Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs)
			engine?.Dispose()
		End Sub

		Private Sub OnHtmlLoaded(ByVal t As Task(Of NavigationResult))
			'Configure JavaScript event handlers to invoke .NET callback for files.
			browser.MainFrame.ExecuteJavaScript("
                                        var drop = function (event) {
	                                        event.preventDefault();
                                            var data = event.dataTransfer.files;
                                            window.external.Drop(data);
                                        };
                                        var allowDrop = function(event) {
                                            event.preventDefault();
                                        };
                                        document.getElementById(""dropZone"").addEventListener(""drop"", drop); 
                                        document.getElementById(""dropZone"").addEventListener(""dragover"", allowDrop); 
            ")

			'Configure DOM event handlers.
			Dim dropZone As IElement = browser.MainFrame.Document.GetElementById("dropZone")

			AddHandler dropZone.Events(New EventType("dragover")).EventReceived, Sub(s, e)
				e.Event.PreventDefault()
				WriteLine("DOM DragOver event received")
			End Sub

			AddHandler dropZone.Events(New EventType("drop")).EventReceived, Sub(s, e)
				e.Event.PreventDefault()
				WriteLine("DOM Drop event received")
			End Sub

			WriteLine("DOM DnD events configured")
		End Sub

		Private Sub OnInjectJs(ByVal p As InjectJsParameters)
			Dim window As IJsObject = p.Frame.ExecuteJavaScript(Of IJsObject)("window").Result
			window.Properties("external") = Me
		End Sub

		Private Sub WriteLine(ByVal line As String)
			BeginInvoke(CType(Sub() textBox1.AppendText(line & Environment.NewLine), Action))
		End Sub

	End Class
End Namespace
