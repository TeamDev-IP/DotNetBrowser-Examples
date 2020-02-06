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
using System.Linq;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace NetworkHandlers
{
    /// <summary>
    ///     This example demonstrates how to intercept a URL request,
    ///     redirect to another web site and get a User-Agent string for the web site.
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
                        engine.Network.SendUrlRequestHandler =
                            new Handler<SendUrlRequestParameters, SendUrlRequestResponse>(OnSendUrlRequest);
                        engine.Network.SendHeadersHandler =
                            new Handler<SendHeadersParameters, SendHeadersResponse>(OnSendHeaders);

                        Console.WriteLine("Loading https://www.teamdev.com/");
                        browser.Navigation.LoadUrl("https://www.teamdev.com/").Wait();
                        Console.WriteLine($"Loaded URL: {browser.Url}");
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

        public static SendHeadersResponse OnSendHeaders(SendHeadersParameters parameters)
        {
            // If navigate to google.com, then print User-Agent header value.
            if (parameters.UrlRequest.Url == "https://www.google.com/")
            {
                IEnumerable<IHttpHeader> headers = parameters.Headers;
                Console.WriteLine("User-Agent: "
                                  + headers.FirstOrDefault(h => h.Name.Equals("User-Agent"))?.Values.FirstOrDefault());
            }

            return SendHeadersResponse.Continue();
        }

        public static SendUrlRequestResponse OnSendUrlRequest(SendUrlRequestParameters parameters)
        {
            // If navigate to teamdev.com, then change URL to google.com.
            if (parameters.UrlRequest.Url == "https://www.teamdev.com/")
            {
                Console.WriteLine("Redirecting to  https://www.google.com/");
                return SendUrlRequestResponse.Override("https://www.google.com");
            }

            return SendUrlRequestResponse.Continue();
        }

        #endregion
    }
}