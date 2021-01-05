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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Frames;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net.Events;
using DotNetBrowser.Net.Handlers;

namespace AjaxResponseIntercept
{
    internal class Program
    {
        private static readonly Dictionary<string, HttpRequest> AjaxRequests =
            new Dictionary<string, HttpRequest>();

        #region Methods

        private static void Main(string[] args)
        {
            try
            {
                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder().Build()))
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");
                        engine.Network.SendUrlRequestHandler =
                            new Handler<SendUrlRequestParameters, SendUrlRequestResponse>(OnSendUrlRequest);
                        engine.Network.ResponseBytesReceived += OnResponseBytesReceived;
                        engine.Network.RequestCompleted += OnRequestCompleted;

                        browser.Navigation
                               .LoadUrl("https://www.w3schools.com/xml/tryit.asp?filename=tryajax_first")
                               .Wait();

                        IFrame demoFrame =
                            browser.AllFrames.FirstOrDefault(f => f.Document.GetElementById("demo") != null);

                        if (demoFrame != null)
                        {
                            //Click the button in the demo frame to make an AJAX request.
                            Console.WriteLine("Demo frame found");
                            demoFrame.Document.GetElementByTagName("button").Click();
                        }

                        //Wait for 15 seconds to be sure that at least some requests are completed.
                        Thread.Sleep(15000);

                        // The dictionary will contain some requests, including the one we sent by clicking the button.
                        string key = AjaxRequests.Keys.FirstOrDefault(k => k.Contains("ajax_info.txt"));
                        if (!string.IsNullOrEmpty(key))
                        {
                            HttpRequest ajaxRequest = AjaxRequests[key];
                            Console.WriteLine($"Response intercepted: \n{ajaxRequest.Response}");
                        }
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

        private static void OnRequestCompleted(object s, RequestCompletedEventArgs e)
        {
            //Here, we mark the requests as completed for the previously filtered URLs.
            string url = e.UrlRequest.Url;
            if (AjaxRequests.ContainsKey(url))
            {
                AjaxRequests[url].Complete();
            }
        }

        private static void OnResponseBytesReceived(object s, ResponseBytesReceivedEventArgs e)
        {
            //Here, we collect the response data for the previously filtered URLs.
            string url = e.UrlRequest.Url;
            if (AjaxRequests.ContainsKey(url))
            {
                HttpRequest httpRequest = AjaxRequests[url];
                if (httpRequest.MimeType == null)
                {
                    httpRequest.MimeType = e.MimeType;
                }

                httpRequest.AppendResponseBytes(e.Data);
            }
        }

        private static SendUrlRequestResponse OnSendUrlRequest(SendUrlRequestParameters arg)
        {
            // Here, we check the URL request and decide if we want to intercept a response for it.
            // For instance, we can check the resource type. This works for AJAX requests.
            // We can also check the arg.UrlRequest.Url to determine if this is a URL we need.
            if (arg.UrlRequest.ResourceType == ResourceType.Xhr)
            {
                AjaxRequests[arg.UrlRequest.Url] = new HttpRequest(arg.UrlRequest.Url, arg.UrlRequest.Method);
            }

            return SendUrlRequestResponse.Continue();
        }

        #endregion
    }
}