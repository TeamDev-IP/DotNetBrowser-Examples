﻿#region Copyright

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
using DotNetBrowser.Browser;

namespace ComWrapper.WinForms.Impl
{
    internal class BrowserImpl : IComBrowser
    {
        #region Properties

        internal IBrowser Browser { get; }

        public IComDocument Document
        {
            get
            {
                CheckInitialized();
                return new DocumentImpl(Browser.MainFrame.Document);
            }
        }

        public string Html
        {
            get
            {
                CheckInitialized();
                return Browser.MainFrame.Html;
            }
        }

        #endregion

        #region Constructors

        public BrowserImpl(IBrowser browser)
        {
            Browser = browser;
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            Browser.Dispose();
        }

        public void LoadUrl(string url)
        {
            CheckInitialized();
            Browser.Navigation.LoadUrl(url);
        }

        public void LoadUrlAndWait(string url)
        {
            CheckInitialized();
            Browser.Navigation.LoadUrl(url).Wait();
        }

        private void CheckInitialized()
        {
            if (Browser.IsDisposed)
            {
                throw new InvalidOperationException("Browser is already disposed.");
            }
        }

        #endregion
    }
}