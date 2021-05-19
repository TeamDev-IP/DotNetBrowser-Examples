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
using Serilog;

namespace WebCrawling
{
    /// <summary>
    ///     This listener is used for redirecting the DotNetBrowser logs to Serilog.
    ///     The listener attachment to the corresponding sources is performed in App.config.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class CrawlerTraceListener : TraceListener
    {
        private readonly string initializeData;
        private readonly Lazy<ILogger> logger;

        #region Constructors

        public CrawlerTraceListener(string initializeData)
        {
            this.initializeData = initializeData;
            logger = new Lazy<ILogger>(() => Log.ForContext("SourceContext", initializeData));
        }

        #endregion

        #region Methods

        public override void Write(string message)
        {
            logger.Value.Information(message);
        }

        public override void WriteLine(string message)
        {
            logger.Value.Information(message);
        }

        #endregion
    }
}