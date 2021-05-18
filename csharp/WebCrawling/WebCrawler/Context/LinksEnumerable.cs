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
using System.Collections;
using System.Collections.Generic;
using Serilog;
using WebCrawling.Elements;

namespace WebCrawling.WebCrawler.Context
{
    public sealed class LinksEnumerable : IEnumerable<Link>
    {
        private readonly Lazy<LinksEnumerator> enumerator = new Lazy<LinksEnumerator>();

        #region Properties

        public IReadOnlyList<Link> CheckedLinks => enumerator.Value.CheckedUrls;

        #endregion

        #region Methods

        public IEnumerator<Link> GetEnumerator() => enumerator.Value;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool TryAddCheckedLink(Link link)
        {
            if (string.IsNullOrWhiteSpace(link.Url))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(link));
            }

            return enumerator.Value.TryAddCheckedLink(link);
        }

        public bool TryAddLink(Link link)
        {
            if (string.IsNullOrWhiteSpace(link.Url))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(link));
            }

            return enumerator.Value.TryAddLink(link);
        }

        #endregion

        private class LinksEnumerator : IEnumerator<Link>
        {
            #region Properties

            internal List<Link> CheckedUrls { get; } = new List<Link>();
            private Stack<Link> LinksToCheck { get; } = new Stack<Link>();

            public Link Current => LinksToCheck.Pop();

            object IEnumerator.Current => Current;

            #endregion

            #region Methods

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                Log.Verbose("Remaining links: {Count}", LinksToCheck.Count);
                return LinksToCheck.Count > 0;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public bool TryAddCheckedLink(Link link)
            {
                if (CheckedUrls.Contains(link))
                {
                    return false;
                }

                CheckedUrls.Add(link);
                return true;
            }

            public bool TryAddLink(Link link)
            {
                if (CheckedUrls.Contains(link) || LinksToCheck.Contains(link))
                {
                    return false;
                }

                LinksToCheck.Push(link);
                return true;
            }

            #endregion
        }
    }
}