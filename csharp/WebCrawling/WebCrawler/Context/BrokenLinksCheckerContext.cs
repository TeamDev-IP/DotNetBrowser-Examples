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
using DotNetBrowser.Dom;
using DotNetBrowser.Frames;
using DotNetBrowser.Js;
using Serilog;
using WebCrawling.Elements;

namespace WebCrawling.WebCrawler.Context
{
    public class BrokenLinksCheckerContext : WebCrawlerContextBase
    {
        #region Properties

        public List<string> SubstringsToSkip { get; set; } = new List<string>();

        #endregion

        #region Constructors

        public BrokenLinksCheckerContext(string domainUri) : base(domainUri)
        {
        }

        #endregion

        #region Methods

        public override void ProcessFrame(string uri, IFrame frame)
        {
            if (frame == null)
            {
                return;
            }

            IEnumerable<IElement> linkNodes = frame.Document.GetElementsByTagName("a");
            foreach (IElement linkNode in linkNodes)
            {
                if (!linkNode.IsDisposed)
                {
                    try
                    {
                        var pageUrl = (linkNode as IJsObject)?.Properties["href"].ToString();
                        Log.Verbose("Resolved url: {Url} ", pageUrl);
                        Link link = new LinkElement(uri, linkNode.XPath, pageUrl);
                        if (!string.IsNullOrWhiteSpace(pageUrl)
                            && SubstringsToSkip.FirstOrDefault(pageUrl.Contains) == null)
                        {
                            TryAddLink(link);
                        }
                    }
                    catch (Exception ex)
                    {
                        //The frame may suddenly become disposed, and the corresponding node will be disposed as well.
                        Log.Warning(ex, "Exception was thrown when processing node.");
                    }
                }
            }
        }

        #endregion
    }
}