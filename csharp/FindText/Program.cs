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
    ///     This example demonstrates how to perform text search on the loaded web page.
    /// </summary>
    internal class Program
    {
        // #docfragment "FindText"
        private const string Html = "<html><body><p>Find me</p><p>Find me</p></body></html>";

        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = new Size(700, 500);

                    byte[] htmlBytes = Encoding.UTF8.GetBytes(Html);
                    browser.Navigation
                           .LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}")
                           .Wait();
                    // Add a timeout to make sure the web page is rendered completely.
                    Thread.Sleep(2000);

                    // Find text from the beginning of the loaded web page.
                    string searchText = "find me";

                    IHandler<FindResultReceivedParameters> intermediateResultsHandler =
                        new Handler<FindResultReceivedParameters>(ProcessSearchResults);

                    Console.WriteLine("Find text (1/2)");
                    ITextFinder textFinder = browser.TextFinder;
                    FindResult findResult =
                        textFinder.Find(searchText, null, intermediateResultsHandler)
                                  .Result;

                    int selectedMatch = findResult.SelectedMatch;
                    int count = findResult.NumberOfMatches;
                    Console.WriteLine($"Find Result: {selectedMatch}/{count}");

                    Console.WriteLine("Find text (2/2)");
                    findResult = textFinder
                                .Find(searchText, null, intermediateResultsHandler)
                                .Result;

                    selectedMatch = findResult.SelectedMatch;
                    count = findResult.NumberOfMatches;
                    Console.WriteLine($"Find Result: {selectedMatch}/{count}");

                    textFinder.StopFinding();
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void ProcessSearchResults(FindResultReceivedParameters args)
        {
            FindResult result = args.FindResult;

            if (args.IsSearchFinished)
            {
                Console.WriteLine("Found: "
                                  + result.SelectedMatch
                                  + "/"
                                  + result.NumberOfMatches);
            }
            else
            {
                Console.WriteLine("Search in progress... Found "
                                  + result.SelectedMatch
                                  + "/"
                                  + result.NumberOfMatches);
            }
        }
        // #enddocfragment "FindText"
    }
}