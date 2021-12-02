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
using System.Linq;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Frames;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net.Handlers;

namespace AjaxCallsFilter
{
    /// <summary>
    ///     The sample demonstrates how to suppress Ajax calls by registering the LoadResourceHandler.
    /// </summary>
    internal class Program
    {
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
                        engine.Profiles.Default.Network.SendUrlRequestHandler =
                            new Handler<SendUrlRequestParameters, SendUrlRequestResponse>(CanLoadResource);

                        browser.Navigation
                               .LoadUrl("https://www.w3schools.com/xml/tryit.asp?filename=tryajax_first")
                               .Wait();

                        IFrame demoFrame =
                            browser.AllFrames.FirstOrDefault(f => f.Document.GetElementById("demo") != null);

                        if (demoFrame != null)
                        {
                            Console.WriteLine("Demo frame found");
                            demoFrame.Document.GetElementByTagName("button").Click();
                        }

                        Thread.Sleep(5000);
                        Console.WriteLine("Demo HTML: " + demoFrame?.Document.GetElementById("demo").InnerHtml);
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

        private static SendUrlRequestResponse CanLoadResource(SendUrlRequestParameters arg)
        {
            if (arg.UrlRequest.ResourceType == ResourceType.Xhr)
            {
                Console.WriteLine("Suppress ajax call - " + arg.UrlRequest.Url);
                return SendUrlRequestResponse.Cancel();
            }

            return SendUrlRequestResponse.Continue();
        }
    }
}