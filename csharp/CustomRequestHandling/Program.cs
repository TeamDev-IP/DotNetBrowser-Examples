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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace CustomRequestHandling
{
    /// <summary>
    ///     This example demonstrates how to intercept all URL requests and handle a custom protocol.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            try
            {
                Handler<InterceptRequestParameters, InterceptRequestResponse> handler =
                    new Handler<InterceptRequestParameters, InterceptRequestResponse>(p =>
                    {
                        UrlRequestJobOptions options = new UrlRequestJobOptions
                        {
                            Headers = new List<HttpHeader>
                            {
                                new HttpHeader("Content-Type", "text/html", "charset=utf-8")
                            }
                        };
                        UrlRequestJob job = p.Network.CreateUrlRequestJob(p.UrlRequest, options);
                        Task.Run(() =>
                        {
                            // The request processing is performed in a background thread
                            // in order to avoid freezing the web page.
                            job.Write(Encoding.UTF8.GetBytes("Hello world!"));
                            job.Complete();
                        });

                        return InterceptRequestResponse.Intercept(job);
                    });

                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder
                {
                    Schemes = {{Scheme.Create("myscheme"), handler}}
                }.Build()))
                {
                    Console.WriteLine("Engine created");
                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");
                        LoadResult loadResult = browser.Navigation.LoadUrl("myscheme://test1").Result;
                        Console.WriteLine("Load result: " + loadResult);
                        Console.WriteLine("HTML: " + browser.MainFrame.Html);
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
    }
}