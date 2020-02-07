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
using DotNetBrowser.Navigation.Events;

namespace LoadEvents
{
    /// <summary>
    ///     The sample demonstrates how to receive notifications about
    ///     web page loading progress.
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
                        browser.Navigation.FrameLoadFinished += delegate(object sender, FrameLoadFinishedEventArgs e)
                        {
                            Console.Out.WriteLine($"FrameLoadFinished: URL = {e.ValidatedUrl},"
                                                  + $" IsMainFrame = {e.Frame.IsMain}");
                        };

                        browser.Navigation.LoadStarted += delegate { Console.Out.WriteLine("LoadStarted"); };
                        browser.Navigation.NavigationStarted += delegate(object sender, NavigationStartedEventArgs e)
                        {
                            Console.Out.WriteLine($"NavigationStarted: Url = {e.Url}");
                        };

                        browser.Navigation.FrameDocumentLoadFinished +=
                            delegate(object sender, FrameDocumentLoadFinishedEventArgs e)
                            {
                                Console.Out.WriteLine($"FrameDocumentLoadFinished: IsMainFrame = {e.Frame.IsMain}");
                            };

                        browser.Navigation.LoadUrl("https://www.google.com").Wait();
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