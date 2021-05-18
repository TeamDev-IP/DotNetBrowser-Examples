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

using System.Collections.Generic;
using DotNetBrowser.Frames;
using WebCrawling.Elements;

namespace WebCrawling.WebCrawler.Context
{
    /// <summary>
    ///     The base implementation of the web crawler context.
    /// </summary>
    public abstract class WebCrawlerContextBase : IWebCrawlerContext
    {
        private readonly LinksEnumerable linksEnumerable = new LinksEnumerable();

        #region Properties

        public IReadOnlyList<Link> CheckedLinks => linksEnumerable.CheckedLinks;
        protected string DomainUri { get; }
        public IEnumerable<Link> Links => linksEnumerable;

        #endregion

        #region Constructors

        protected WebCrawlerContextBase(string domainUri)
        {
            DomainUri = domainUri;
            linksEnumerable.TryAddLink(new Link(domainUri));
        }

        #endregion

        #region Methods

        public void MarkLinkAsChecked(Link uri)
        {
            linksEnumerable.TryAddCheckedLink(uri);
        }

        public abstract void ProcessFrame(string uri, IFrame frame);

        public virtual bool ShouldProcessPageContents(string uri) => uri.Contains(DomainUri);

        protected bool TryAddLink(Link link) => linksEnumerable.TryAddLink(link);

        #endregion
    }
}