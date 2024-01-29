#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;

namespace WebSockets.InterceptData
{
    /// <summary>
    ///     This example demonstrates how to intercept web socket data
    ///     by using JS-.NET bridge capabilities.
    /// </summary>
    internal class Program
    {
        private const string JavaScript = @"var oldSocket = window.WebSocket;
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
                         };";

        private static readonly WebSocketCallback webSocketCallback = new WebSocketCallback();

        private static void Main(string[] args)
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = new Size(640, 480);

                    //Configure JavaScript injection
                    browser.InjectJsHandler = new Handler<InjectJsParameters>(OnInjectJs);
                    //Load web page for testing
                    browser.Navigation.LoadUrl(Path.GetFullPath("echo.html")).Wait();

                    // Connect to the socket by clicking the button on the web page
                    browser.MainFrame.Document.GetElementById("connect")?.Click();
                    Thread.Sleep(1000);

                    //Send some data
                    browser.MainFrame.Document.GetElementById("send")?.Click();
                    Thread.Sleep(1000);

                    //Disconnect from the socket
                    browser.MainFrame.Document.GetElementById("disconnect")?.Click();
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void OnInjectJs(InjectJsParameters parameters)
        {
            IJsObject window = parameters.Frame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["websocketCallback"] = webSocketCallback;

            parameters.Frame.ExecuteJavaScript(JavaScript);
        }

        public class WebSocketCallback
        {
            public void OnClose(IJsObject closeEvent)
            {
                Console.WriteLine("WebSocketCallback.OnClose");
            }

            public void OnMessage(IJsObject socket, object data)
            {
                Console.WriteLine("WebSocketCallback.OnMessage: " + data);
            }

            public void OnOpen(IJsObject socket)
            {
                Console.WriteLine("WebSocketCallback.OnOpen");
            }

            public void OnSend(IJsObject socket, object data)
            {
                Console.WriteLine("WebSocketCallback.OnSend: " + data);
            }
        }
    }
}
