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
using System.Diagnostics;
using System.IO;
using DotNetBrowser.Geometry;
using DotNetBrowser.Logging;
using Serilog;
using Serilog.Filters;
using WebCrawling.Elements;
using WebCrawling.WebCrawler.Context;

namespace WebCrawling
{
    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            LoggerProvider.Instance.Level = SourceLevels.Information;

            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Logger(lc => lc
                                             .Filter.ByExcluding(Matching.WithProperty("SourceContext"))
                                             .WriteTo.Console()
                                             .MinimumLevel.Debug())
                        .WriteTo.Logger(lc => lc
                                             .Filter.ByExcluding(Matching.WithProperty("SourceContext"))
                                             .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Hour)
                                             .MinimumLevel.Verbose())
                        .WriteTo.Logger(lc => lc
                                             .Filter.ByIncludingOnly(Matching.WithProperty("SourceContext",
                                                                                           "DotNetBrowser"))
                                             .WriteTo.File("dotnetbrowser-.txt", rollingInterval: RollingInterval.Hour,
                                                           outputTemplate: "{Message:lj}{NewLine}")
                                             .MinimumLevel.Information())
                        .CreateLogger();

            string domainUri = "https://dotnetbrowser-support.teamdev.com/release-notes";
            BrokenLinksCheckerContext context = new BrokenLinksCheckerContext(domainUri)
            {
                SubstringsToSkip = {"apidoc", "mailto:", "tel:", ".zip"}
            };

            try
            {
                WebCrawler.WebCrawler webCrawler = new WebCrawler.WebCrawler(context)
                {
                    InitialSize = new Size(1024, 768),
                    NavigationDelay = TimeSpan.FromMilliseconds(800)
                };
                webCrawler.Process().Wait();
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to crawl the URLs");
            }
            finally
            {
                using (StreamWriter outputFile = new StreamWriter(Path.GetFullPath("results.csv")))
                {
                    outputFile.WriteLine("Link, Result, Page, XPath");
                    foreach (Link checkedUrl in context.CheckedLinks)
                    {
                        LinkElement linkElement = checkedUrl as LinkElement;
                        outputFile
                           .WriteLine($"\"{checkedUrl.Url}\", {checkedUrl.ErrorCode}, \"{linkElement?.PageUrl}\", {linkElement?.XPath}");
                    }

                    outputFile.Flush();
                }

                Log.CloseAndFlush();
            }
        }

        #endregion
    }
}