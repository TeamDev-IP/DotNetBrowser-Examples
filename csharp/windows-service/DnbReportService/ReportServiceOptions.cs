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
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DnbReportService
{
    /// <summary>
    ///     The <c>ReportService</c> section of appsettings.json.
    /// </summary>
    public sealed class ReportServiceOptions
    {
        private const string SectionName = "ReportService";

        /// <summary>
        ///     The Chromium profile folder. Must be writable by the service
        ///     account and used by one engine at a time.
        /// </summary>
        public string UserDataDirectory { get; set; }

        /// <summary>
        ///     Where the Chromium binaries are extracted. Must be writable by
        ///     the service account. One folder serves every account.
        /// </summary>
        public string ChromiumDirectory { get; set; }

        /// <summary>
        ///     Where the PDF, PNG, and JSON files and the text log go.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        ///     The pause between two cycles of jobs, in seconds.
        /// </summary>
        public int IntervalSeconds { get; set; } = 60;

        /// <summary>
        ///     The pause between "ready" and the screenshot, in milliseconds.
        ///     A finished load is not always a finished paint, especially on
        ///     a cold profile.
        /// </summary>
        public int CaptureDelayMs { get; set; } = 500;

        /// <summary>
        ///     The pages to render in every cycle.
        /// </summary>
        public List<ReportJob> Jobs { get; set; } = new List<ReportJob>();

        /// <summary>
        ///     Binds the section and expands environment variables in paths.
        ///     Relative paths are resolved against the app folder.
        /// </summary>
        public static ReportServiceOptions Load(IConfiguration configuration)
        {
            ReportServiceOptions options = new ReportServiceOptions();
            configuration.GetSection(SectionName).Bind(options);
            options.UserDataDirectory = ExpandPath(options.UserDataDirectory,
                                                   "UserDataDirectory");
            options.ChromiumDirectory = ExpandPath(options.ChromiumDirectory,
                                                   "ChromiumDirectory");
            options.OutputDirectory = ExpandPath(options.OutputDirectory,
                                                 "OutputDirectory");
            return options;
        }

        private static string ExpandPath(string path, string name)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new InvalidOperationException(
                    $"Set {SectionName}:{name} in appsettings.json.");
            }
            string expanded = Environment.ExpandEnvironmentVariables(path);
            return Path.GetFullPath(expanded, AppContext.BaseDirectory);
        }
    }

    /// <summary>
    ///     One page to load and the outputs to produce from it.
    /// </summary>
    public sealed class ReportJob
    {
        /// <summary>
        ///     The job name, used as the prefix of the output files.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     An absolute URL, or a file name relative to the app folder.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     An optional JavaScript expression polled after the load until
        ///     it is true, for pages that draw their content by script.
        /// </summary>
        public string ReadyScript { get; set; }

        /// <summary>
        ///     The CSS selector of the element whose text goes to the JSON
        ///     file.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        ///     Whether to print the page to a PDF file.
        /// </summary>
        public bool Pdf { get; set; } = true;

        /// <summary>
        ///     Whether to save a screenshot of the viewport as a PNG file.
        /// </summary>
        public bool Png { get; set; } = true;

        /// <summary>
        ///     Whether to save the title and the selected text as JSON.
        /// </summary>
        public bool Json { get; set; } = true;

        /// <summary>
        ///     Resolves <see cref="Url" /> to an absolute URL.
        /// </summary>
        public string ResolveUrl()
        {
            if (Uri.TryCreate(Url, UriKind.Absolute, out Uri absolute))
            {
                return absolute.AbsoluteUri;
            }
            string file = Path.Combine(AppContext.BaseDirectory, Url);
            return new Uri(file).AbsoluteUri;
        }
    }
}
