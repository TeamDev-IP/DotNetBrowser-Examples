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
using System.Text;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Search;
using DotNetBrowser.Search.Handlers;

namespace FindText
{
    /// <summary>
    ///     This example demonstrates how to find text on the loaded web page.
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
                        browser.Size = new Size(700, 500);
                        byte[] htmlBytes = Encoding.UTF8.GetBytes("<html><body><p>Find me</p><p>Find me</p></body></html>");
                        browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait();

                        Thread.Sleep(2000);
                        // Find text from the beginning of the loaded web page.
                        string searchText = "find me";

                        IHandler<FindResultReceivedParameters> intermediateResultsHandler =
                            new Handler<FindResultReceivedParameters>(ProcessSearchResults);
                        Console.WriteLine("Find text (1/2)");

                        FindResult findResult =
                            browser.TextFinder.Find(searchText, null, intermediateResultsHandler).Result;
                        Console.Out.WriteLine($"Find Result: {findResult.SelectedMatch}/{findResult.NumberOfMatches}");
                        Console.WriteLine("Find text (2/2)");

                        findResult = browser.TextFinder.Find(searchText, null, intermediateResultsHandler).Result;
                        Console.Out.WriteLine($"Find Result: {findResult.SelectedMatch}/{findResult.NumberOfMatches}");
                        browser.TextFinder.StopFinding();
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

        private static void ProcessSearchResults(FindResultReceivedParameters args)
        {
            FindResult result = args.FindResult;

            if (args.IsSearchFinished)
            {
                Console.Out.WriteLine("Found: "
                                      + result.SelectedMatch
                                      + "/"
                                      + result.NumberOfMatches);
            }
            else
            {
                Console.Out.WriteLine("Search in progress... Found "
                                      + result.SelectedMatch
                                      + "/"
                                      + result.NumberOfMatches);
            }
        }

        #endregion
    }
}