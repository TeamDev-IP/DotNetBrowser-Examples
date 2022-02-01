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

Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Js

''' <summary>
'''     This example demonstrates how to intercept web socket data
'''     by using JS-.NET bridge capabilities.
''' </summary>
Friend Class Program
    Private Const JavaScript As String = "var oldSocket = window.WebSocket;
                         window.WebSocket = function (url){
                            var socket = new oldSocket(url);
                            socket.onopen = () => {
                                window.websocketCallback.OnOpen(socket);
                                this.onopen();
                            };
                            socket.onmessage = (message) => {
                                window.websocketCallback.OnMessage(socket,message.data);
                                this.onmessage(message);
                            };
                            var onclose = socket.onclose;
                            socket.onclose = (closeEvent) => {
                                this.onclose();
                                window.websocketCallback.OnClose(closeEvent);
                                this.close(closeEvent);
                             };

                            this.close = (event)=> {socket.close();};
                            this.send = (data) => {
                                window.websocketCallback.OnSend(socket,data);
                                socket.send(data);
                            };
                         };"

    Private Shared ReadOnly myWebSocketCallback As New WebSocketCallback()

    Public Shared Sub Main(ByVal args() As String)
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(640, 480)

                'Configure JavaScript injection
                browser.InjectJsHandler = New Handler(Of InjectJsParameters)(AddressOf OnInjectJs)
                'Load web page for testing
                browser.Navigation.LoadUrl("https://www.websocket.org/echo.html").Wait()

                ' Connect to the socket by clicking the button on the web page
                browser.MainFrame.Document.GetElementById("connect")?.Click()
                Thread.Sleep(1000)

                'Send some data
                browser.MainFrame.Document.GetElementById("send")?.Click()
                Thread.Sleep(1000)

                'Disconnect from the socket
                browser.MainFrame.Document.GetElementById("disconnect")?.Click()
                Thread.Sleep(1000)
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Sub OnInjectJs(ByVal parameters As InjectJsParameters)
        Dim window As IJsObject = parameters.Frame.ExecuteJavaScript(Of IJsObject)("window").Result
        window.Properties("websocketCallback") = myWebSocketCallback

        parameters.Frame.ExecuteJavaScript(JavaScript)
    End Sub

    Public Class WebSocketCallback

        Public Sub OnClose(ByVal closeEvent As IJsObject)
            Console.WriteLine("WebSocketCallback.OnClose")
        End Sub

        Public Sub OnMessage(ByVal socket As IJsObject, ByVal data As Object)
            Console.WriteLine($"WebSocketCallback.OnMessage: {data}")
        End Sub

        Public Sub OnOpen(ByVal socket As IJsObject)
            Console.WriteLine("WebSocketCallback.OnOpen")
        End Sub

        Public Sub OnSend(ByVal socket As IJsObject, ByVal data As Object)
            Console.WriteLine($"WebSocketCallback.OnSend: {data}")
        End Sub

    End Class
End Class
