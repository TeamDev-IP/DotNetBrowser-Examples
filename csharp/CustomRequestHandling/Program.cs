#region Copyright

// Copyright 2019, TeamDev. All rights reserved.
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
using System.Net;
using System.Text;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net;

namespace CustomRequestHandling
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

                    engine.NetworkService.UrlRequestHandler = new CustomUrlRequestHandler();
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

        #endregion

        private class CustomUrlRequestHandler : IUrlRequestHandler
        {
            #region Methods

            public void HandleRequest(IInterceptedUrlRequest interceptedRequest)
            {
                Console.WriteLine("Intercepted request to URL:" + interceptedRequest.UrlRequest.Url);
                interceptedRequest.Write(Encoding.UTF8.GetBytes("Hello world!"));
                interceptedRequest.Complete();
            }

            public InterceptUrlRequestResult InterceptRequest(UrlRequestData request)
            {
                if (request.Request.Url.StartsWith("myscheme"))
                {
                    return InterceptUrlRequestResult.Intercept(HttpStatusCode.OK);
                }
                return InterceptUrlRequestResult.Continue();
            }

            #endregion
        }
    }
}