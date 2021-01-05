#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Frames;
using DotNetBrowser.Js;

namespace WebStorage
{
    /// <summary>
    ///     The sample demonstrates how to access WebStorage on
    ///     the loaded web page using DotNetBrowser API.
    /// </summary>
    internal class Program
    {
        #region Methods

        public static void Main()
        {
            try
            {
                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder().Build()))
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");

                        browser.MainFrame
                               .LoadHtml(new LoadHtmlParameters("<html><body>"
                                                                + "<script>localStorage.myKey = \"Initial Value\";"
                                                                + "function myFunction(){return localStorage.myKey;}"
                                                                + "</script></body></html>"
                                                                )
                                {
                                    BaseUrl = "https://teamdev.com",
                                    Replace = true
                                })
                               .Wait();
                        IWebStorage webStorage = browser.MainFrame.LocalStorage;
                        // Read and display the 'myKey' storage value.
                        Console.Out.WriteLine("The initial myKey value: " + webStorage["myKey"]);
                        // Modify the 'myKey' storage value.
                        webStorage["myKey"] = "Hello from Local Storage";
                        string updatedValue = browser.MainFrame.ExecuteJavaScript<IJsObject>("window")
                                                     .Result.Invoke<string>("myFunction");
                        Console.Out.WriteLine("The updated myKey value: " + updatedValue);
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

        #endregion
    }
}