#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Dom;
using DotNetBrowser.Dom.Events;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;
using DotNetBrowser.Navigation;

namespace Dom.DragAndDrop.WinForms
{
    /// <summary>
    ///     This example demonstrates two possible approaches to listen to the Drag and Drop events
    ///     for the particular DOM element.
    /// </summary>
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            Task.Run(() =>
                 {
                     engine = EngineFactory
                        .Create(new EngineOptions.Builder
                                    {
                                        RenderingMode = RenderingMode.HardwareAccelerated
                                    }
                                   .Build());
                     browser = engine.CreateBrowser();
                 })
                .ContinueWith(t =>
                 {
                     browserView1.InitializeFrom(browser);
                     browser.InjectJsHandler = new Handler<InjectJsParameters>(OnInjectJs);
                     browser.ConsoleMessageReceived += (sender, args) =>
                     {
                         Debug.WriteLine(args.LineNumber + " > " + args.Message);
                     };
                     byte[] htmlBytes = Encoding.UTF8.GetBytes(@"<html>
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
                                    </html>");
                     browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes))
                            .ContinueWith(OnHtmlLoaded);
                 }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
            FormClosing += Form1_FormClosing;
        }

        public void Drop(IJsObject data)
        {
            WriteLine("JavaScript Drop callback received: " + data);
            if (data.Properties.Contains("length"))
            {
                double length = (double) data.Properties["length"];
                for (uint i = 0; i < length; i++)
                {
                    IJsObject file = data.Properties[i] as IJsObject;
                    if (file != null)
                    {
                        WriteLine($"data[{i}] = {file.Properties["name"]}");
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            engine?.Dispose();
        }

        private void OnHtmlLoaded(Task<LoadResult> t)
        {
            //Configure JavaScript event handlers to invoke .NET callback for files.
            browser.MainFrame.ExecuteJavaScript(@"
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
            ");

            //Configure DOM event handlers.
            IElement dropZone = browser.MainFrame.Document.GetElementById("dropZone");
            dropZone.Events[new EventType("dragover")].EventReceived += (s, e) =>
            {
                e.Event.PreventDefault();
                WriteLine("DOM DragOver event received");
            };
            dropZone.Events[new EventType("drop")].EventReceived += (s, e) =>
            {
                e.Event.PreventDefault();
                WriteLine("DOM Drop event received");
            };
            WriteLine("DOM DnD events configured");
        }

        private void OnInjectJs(InjectJsParameters p)
        {
            IJsObject window = p.Frame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["external"] = this;
        }

        private void WriteLine(string line)
        {
            BeginInvoke((Action) (() => textBox1.AppendText(line + Environment.NewLine)));
        }
    }
}
