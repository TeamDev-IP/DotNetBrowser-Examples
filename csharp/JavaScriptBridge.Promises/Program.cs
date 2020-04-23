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
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Js;

namespace JavaScriptBridge.Promises
{
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
                        browser.Size = new Size(700, 500);
                        browser.MainFrame.LoadHtml(@"<html>
                                     <body>
                                        <script type='text/javascript'>
                                            function CreatePromise(success) 
                                            {
                                                 return new Promise(function(resolve, reject) {
                                                    if(success) {
                                                        resolve('Promise fulfilled.');
                                                    }
                                                    else {
                                                        reject('Promise rejected.');
                                                    }
                                                 });
                                            };
                                        </script>
                                     </body>
                                   </html>")
                               .Wait();
                        IJsObject window = browser.MainFrame.ExecuteJavaScript<IJsObject>("window").Result;
                        //Prepare promise handlers
                        Action<object> promiseResolvedHandler = o => Console.WriteLine("Success: " + o);
                        Action<object> promiseRejectedHandler = o => Console.Error.WriteLine("Error: " + o);

                        //Create a promise that is fulfilled
                        Console.WriteLine("Create a promise that is fulfilled...");
                        IJsObject promise1 = window.Invoke<IJsObject>("CreatePromise", true);
                        //Append fulfillment and rejection handlers to the promise
                        promise1.Invoke("then", promiseResolvedHandler, promiseRejectedHandler);

                        //Create a promise that is rejected
                        Console.WriteLine("Create a promise that is rejected...");
                        IJsObject promise2 = window.Invoke<IJsObject>("CreatePromise", false);
                        //Append fulfillment and rejection handlers to the promise
                        promise2.Invoke("then", promiseResolvedHandler, promiseRejectedHandler);
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