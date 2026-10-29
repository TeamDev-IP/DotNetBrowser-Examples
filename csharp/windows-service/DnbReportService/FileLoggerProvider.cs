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
using System.IO;
using Microsoft.Extensions.Logging;

namespace DnbReportService
{
    /// <summary>
    ///     A minimal logger provider that appends lines to one text file, so
    ///     a service run can be read back without Event Viewer.
    /// </summary>
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly object sync = new object();
        private readonly string path;

        /// <summary>
        ///     Creates a provider that appends to <paramref name="path" />
        ///     and creates its folder if needed.
        /// </summary>
        public FileLoggerProvider(string path)
        {
            this.path = path;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

        /// <summary>
        ///     Creates a logger that prefixes lines with the short category
        ///     name, for example <c>ReportWorker</c>.
        /// </summary>
        public ILogger CreateLogger(string categoryName)
        {
            int dot = categoryName.LastIndexOf('.');
            string shortName = categoryName.Substring(dot + 1);
            return new FileLogger(this, shortName);
        }

        /// <summary>
        ///     Does nothing: every line is written and closed at once.
        /// </summary>
        public void Dispose()
        {
        }

        private void Write(string line)
        {
            lock (sync)
            {
                // A failed log write must not stop the service.
                try
                {
                    File.AppendAllText(path, line + Environment.NewLine);
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }
        }

        private sealed class FileLogger : ILogger
        {
            private readonly string category;
            private readonly FileLoggerProvider provider;

            public FileLogger(FileLoggerProvider provider, string category)
            {
                this.provider = provider;
                this.category = category;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogLevel.Information;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId,
                                    TState state, Exception exception,
                                    Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string line = $"{time} [{logLevel}] {category}: "
                              + formatter(state, exception);
                if (exception != null)
                {
                    line += Environment.NewLine + exception;
                }
                provider.Write(line);
            }
        }
    }
}
