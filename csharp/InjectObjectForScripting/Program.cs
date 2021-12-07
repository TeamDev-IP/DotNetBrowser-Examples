#region Copyright

// Copyright © 2021, TeamDev. All rights reserved.
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
using System.Text;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;

namespace InjectObjectForScripting
{
    /// <summary>
    ///     This example demonstrates how to use InjectJsHandler for injecting "window.external"
    ///     object into the web page.
    /// </summary>
    internal class Program
    {
        private static void InjectObjectForScripting(InjectJsParameters p)
        {
            IJsObject window = p.Frame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["external"] = ObjectForScripting.Instance;
        }

        private static void Main(string[] args)
        {
            try
            {
                using (IEngine engine = EngineFactory.Create())
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");
                        browser.Size = new Size(700, 500);
                        browser.InjectJsHandler = new Handler<InjectJsParameters>(InjectObjectForScripting);

                        byte[] htmlBytes = Encoding.UTF8.GetBytes(@"<html>
                                     <body>
                                        <script type='text/javascript'>
                                            var SetTitle = function () 
                                            {
                                                 document.title = window.external.GetTitle();
                                            };
                                        </script>
                                     </body>
                                   </html>");
                        browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait();

                        browser.MainFrame.ExecuteJavaScript<IJsObject>("window.SetTitle();").Wait();

                        Console.WriteLine($"\tBrowser title: {browser.Title}");
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

        public sealed class ObjectForScripting
        {
            public static ObjectForScripting Instance { get; } = new ObjectForScripting();

            static ObjectForScripting()
            {
            }

            private ObjectForScripting()
            {
            }

            public string GetTitle() => "Document title from .NET";
        }
    }
}