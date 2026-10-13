#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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

namespace GuardedBrowser
{
    /// <summary>
    ///     The limits the application sets for the assistant's browser.
    ///     Everything not listed here is blocked.
    /// </summary>
    public sealed class BrowserPolicy
    {
        /// <summary>
        ///     Hosts the assistant may open and read, but not act on.
        /// </summary>
        public HashSet<string> ReadOnlyHosts { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     Hosts the assistant may read, click, type into, and submit forms on.
        /// </summary>
        public HashSet<string> ReadWriteHosts { get; } = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     URL schemes the assistant may navigate to.
        /// </summary>
        public HashSet<string> AllowedSchemes { get; } =
            new(StringComparer.OrdinalIgnoreCase) { "https" };

        /// <summary>
        ///     The page shown instead of a blocked destination.
        /// </summary>
        public string BlockedPageUrl { get; set; } = "about:blank";

        public bool CanRead(string url)
        {
            return TryParse(url, out Uri? uri)
                   && AllowedSchemes.Contains(uri.Scheme)
                   && (ReadOnlyHosts.Contains(uri.Host)
                       || ReadWriteHosts.Contains(uri.Host));
        }

        public bool CanAct(string url)
        {
            return TryParse(url, out Uri? uri)
                   && AllowedSchemes.Contains(uri.Scheme)
                   && ReadWriteHosts.Contains(uri.Host);
        }

        private static bool TryParse(string url, out Uri uri)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out uri!)
                   && !string.IsNullOrEmpty(uri.Host);
        }
    }
}
