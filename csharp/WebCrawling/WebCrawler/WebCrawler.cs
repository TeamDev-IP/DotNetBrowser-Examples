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
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Downloads.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Frames;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Navigation.Events;
using DotNetBrowser.Net;
using Serilog;
using WebCrawling.Elements;
using WebCrawling.WebCrawler.Context;

namespace WebCrawling.WebCrawler
{
    public class WebCrawler
    {
        private readonly IWebCrawlerContext context;

        #region Properties

        public Size InitialSize { get; set; } = new Size(1024, 768);

        public TimeSpan NavigationDelay { get; set; } = TimeSpan.FromMilliseconds(800);

        #endregion

        #region Constructors

        public WebCrawler(IWebCrawlerContext context)
        {
            this.context = context;
            Log.Information("Web crawler created");
        }

        #endregion

        #region Methods

        public async Task Process()
        {
            Log.Information("Start processing links.");
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = InitialSize;

                    foreach (Link uri in context.Links)
                    {
                        await ProcessLink(browser, uri);
                        await Task.Delay(NavigationDelay);
                    }
                }
            }

            Log.Information("Processing links finished.");
        }

        private static async Task<NetError> Navigate(IBrowser browser, string url)
        {
            TaskCompletionSource<NetError> errorCodeTcs = new TaskCompletionSource<NetError>();
            EventHandler<NavigationFinishedEventArgs> eventHandler = null;
            eventHandler = (sender, args) =>
            {
                if (args.HasCommitted && args.IsErrorPage || args.ErrorCode == NetError.Aborted)
                {
                    if (args.Url == url)
                    {
                        errorCodeTcs.TrySetResult(args.ErrorCode);
                    }
                }

                args.Navigation.NavigationFinished -= eventHandler;
            };
            browser.Navigation.NavigationFinished += eventHandler;

#pragma warning disable 4014
            browser.Navigation.LoadUrl(url)
                   .ContinueWith(t =>
#pragma warning restore 4014
                                 {
                                     if (!t.IsFaulted && !t.IsCanceled)
                                     {
                                         switch (t.Result)
                                         {
                                             case LoadResult.Completed:
                                                 errorCodeTcs.TrySetResult(NetError.Ok);
                                                 break;
                                             case LoadResult.Failed:
                                                 errorCodeTcs.TrySetResult(NetError.Undefined);
                                                 break;
                                             case LoadResult.Stopped:
                                                 errorCodeTcs.TrySetResult(NetError.Aborted);
                                                 break;
                                             default:
                                                 throw new ArgumentOutOfRangeException();
                                         }
                                     }
                                     else if (t.IsFaulted)
                                     {
                                         errorCodeTcs.TrySetResult(NetError.TimedOut);
                                     }
                                     else
                                     {
                                         errorCodeTcs.TrySetCanceled();
                                     }
                                 });

            return await errorCodeTcs.Task;
        }

        private async Task ProcessLink(IBrowser browser, Link uri)
        {
            Log.Information("Navigating to {Uri}", uri.Url);
            NetError netError = await Navigate(browser, uri.Url);
            uri.ErrorCode = netError;
            context.MarkLinkAsChecked(uri);
            if (netError == NetError.Ok && context.ShouldProcessPageContents(uri.Url))
            {
                foreach (IFrame frame in browser.AllFrames)
                {
                    context.ProcessFrame(uri.Url, frame);
                }
            }
        }

        #endregion
    }
}