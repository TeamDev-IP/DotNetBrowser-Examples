#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Js;

namespace JavaScriptBridge.Promises
{
    /// <summary>
    ///     This example demonstrates how to work with JavaScript Promises
    ///     via JS-.NET bridge.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = new Size(700, 500);
                    byte[] htmlBytes = Encoding.UTF8.GetBytes(@"<html>
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
                                   </html>");

                    browser.Navigation
                           .LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}")
                           .Wait();

                    IJsObject window = browser.MainFrame
                                              .ExecuteJavaScript<IJsObject>("window")
                                              .Result;

                    // Prepare promise handlers.
                    Action<object> promiseResolvedHandler =
                        o => Console.WriteLine($"Success: {o}");
                    Action<object> promiseRejectedHandler =
                        o => Console.Error.WriteLine($"Error: {o}");

                    // Create a promise that is fulfilled.
                    Console.WriteLine("Create a promise that is fulfilled...");
                    IJsObject promise1 = window.Invoke<IJsObject>("CreatePromise", true);
                    // Append fulfillment and rejection handlers to the promise.
                    promise1.Invoke("then", promiseResolvedHandler, promiseRejectedHandler);

                    // Create a promise that is rejected.
                    Console.WriteLine("Create a promise that is rejected...");
                    IJsObject promise2 = window.Invoke<IJsObject>("CreatePromise", false);
                    // Append fulfillment and rejection handlers to the promise.
                    promise2.Invoke("then", promiseResolvedHandler, promiseRejectedHandler);

                    CreatePromiseAsync(window).Wait();
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        // #docfragment "JavaScriptBridge.Promises.Async"
        private static async Task CreatePromiseAsync(IJsObject window)
        {
            // IJsPromise can be integrated with async/await using TaskCompletionSource.

            // Create a promise that is fulfilled.
            Console.WriteLine("\nCreate another promise that is fulfilled...");
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            IJsPromise promise3 = window.Invoke<IJsPromise>("CreatePromise", true);
            promise3.Then(
                o => { Console.WriteLine($"Callback:Success: {o}"); tcs.SetResult(o); },
                e => tcs.SetException(new Exception(e?.ToString())));
            object result = await tcs.Task;
            Console.WriteLine($"Result: {result}");

            // Create a promise that is rejected.
            Console.WriteLine("\nCreate another promise that is rejected...");
            var tcs2 = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            IJsPromise promise4 = window.Invoke<IJsPromise>("CreatePromise", false);
            promise4.Then(
                o => tcs2.SetResult(o),
                e => { Console.WriteLine($"Callback:Error: {e}"); tcs2.SetResult(null); });
            await tcs2.Task;
        }
        // #enddocfragment "JavaScriptBridge.Promises.Async"
    }
}