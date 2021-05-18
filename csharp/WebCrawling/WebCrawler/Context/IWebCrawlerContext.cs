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
    ///     The web crawler context.
    /// </summary>
    public interface IWebCrawlerContext
    {
        #region Properties

        /// <summary>
        ///     Get the enumerable containing all the links that should be navigated.
        ///     The web crawler processes these links one by one, until no links left to process.
        /// </summary>
        IEnumerable<Link> Links { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Marks the corresponding link as already checked.
        /// </summary>
        /// <param name="uri">The Link object that should be marked as checked.</param>
        void MarkLinkAsChecked(Link uri);

        /// <summary>
        ///     Processes the frame on the loaded web page.
        ///     The implementation of this method can access the DOM elements in this frame.
        /// </summary>
        /// <param name="url">The URL of the loaded web page.</param>
        /// <param name="frame">The frame from this web page.</param>
        void ProcessFrame(string url, IFrame frame);

        /// <summary>
        ///     Determines whether the contents of the loaded web page
        ///     with the provided URL should be processed.
        /// </summary>
        /// <param name="url">The URL of the loaded web page</param>
        /// <returns>true if there is a need to process this web page contents, false otherwise.</returns>
        bool ShouldProcessPageContents(string url);

        #endregion
    }
}