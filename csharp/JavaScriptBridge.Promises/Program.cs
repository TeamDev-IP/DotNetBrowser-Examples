#region Copyright

// Copyright 2020, TeamDev. All rights reserved.
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
using System.Collections.Generic;
using System.Diagnostics;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Js;
using DotNetBrowser.Logging;

namespace JavaScriptBridge.Promises
{
    internal class Program
    {
        #region Methods

        public static void Main()
        {
            try
            {
                LoggerProvider.Instance.Level = SourceLevels.Information;
                //LoggerProvider.Instance.ConsoleLoggingEnabled = true;
                LoggerProvider.Instance.FileLoggingEnabled = true;
                LoggerProvider.Instance.OutputFile = "dnb.log";
                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder().Build()))
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");
                        browser.Size = new Size(700, 500);
                        browser.ConsoleMessageReceived += BrowserOnConsoleMessageReceived;
                        browser.MainFrame.LoadHtml(@"<html>
                                     <body>
                                        <script type='text/javascript'>
                                            var CreatePromise = function () 
                                            {
                                                 return new Promise(function(resolve, reject) {
                                                    setTimeout(function() {
                                                        console.log('Resolving...');
                                                        resolve('foo');
                                                    }, 300);
                                                 });
                                            };
                                            var resolver = function(value) {
                                                console.log(value);
                                            }
                                        </script>
                                     </body>
                                   </html>")
                               .Wait();

                        IJsObject promise = browser.MainFrame.ExecuteJavaScript<IJsObject>(@"new Promise(function(resolve, reject) {
                                                    setTimeout(function() {
                                                        console.log('Resolving...');
                                                        resolve('foo');
                                                    }, 300);
                                                 })").Result;
                        IJsObject resolver = browser.MainFrame.ExecuteJavaScript<IJsObject>("window.resolver").Result;

                        Console.WriteLine($"Promise: {promise}");
                        Console.WriteLine($"Resolver: {resolver}");
                        foreach (string name in promise.Properties.Names)
                        {
                            Console.WriteLine($"Property name: {name}");
                        }

                        foreach (string name in promise.Properties.OwnPropertyNames)
                        {
                            Console.WriteLine($"Own property name: {name}");
                        }

                        object result = promise.Invoke("then", resolver);
                        Console.WriteLine($"Result: {result}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void BrowserOnConsoleMessageReceived(object sender, ConsoleMessageReceivedEventArgs e)
        {
            Console.WriteLine("<" + e.Message);
        }

        #endregion

    }
}