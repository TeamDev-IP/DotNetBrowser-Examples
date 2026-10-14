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
using DotNetBrowser.Browser;
using DotNetBrowser.Navigation;

namespace GuardedBrowser
{
    public sealed record AuditEntry(DateTime Time, string Category, string Message);

    /// <summary>
    ///     Records what the browser and the assistant do: navigations, blocked
    ///     actions, tool calls, console messages. Entries go to subscribers
    ///     and to a log file next to the application.
    /// </summary>
    public sealed class AuditLog : IDisposable
    {
        private readonly StreamWriter writer;
        private readonly object guard = new();
        private bool disposed;

        public event Action<AuditEntry>? EntryAdded;

        public string FilePath { get; }

        public AuditLog(string? filePath = null)
        {
            FilePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "agent-browser.log");
            FileStream file = new(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(file) { AutoFlush = true };
        }

        public void Record(string category, string message)
        {
            AuditEntry entry = new(DateTime.Now, category, message);
            lock (guard)
            {
                // A run that is still finishing may record after the window closed.
                if (disposed)
                {
                    return;
                }
                writer.WriteLine($"{entry.Time:HH:mm:ss.fff} [{entry.Category}] {entry.Message}");
            }
            EntryAdded?.Invoke(entry);
        }

        /// <summary>
        ///     Subscribes to the browser events worth keeping on the record.
        /// </summary>
        public void Attach(IBrowser browser)
        {
            INavigation navigation = browser.Navigation;
            navigation.NavigationStarted += (_, e) =>
            {
                if (e.IsMainFrame && !e.IsSameDocument)
                {
                    Record("navigation", $"Started {e.Url}");
                }
            };
            navigation.NavigationRedirected += (_, e) => Record("navigation", $"Redirected to {e.Url}");
            navigation.NavigationFinished += (_, e) =>
            {
                if (e.HasCommitted && !e.IsSameDocument)
                {
                    string status = e.IsErrorPage ? $"error {e.ErrorCode}" : $"HTTP {e.ResponseCode}";
                    Record("navigation", $"Finished {e.Url} ({status})");
                }
            };
            browser.ConsoleMessageReceived += (_, e) =>
                Record("console", $"{e.Level}: {e.Message}");
        }

        public void Dispose()
        {
            lock (guard)
            {
                disposed = true;
                writer.Dispose();
            }
        }
    }
}
