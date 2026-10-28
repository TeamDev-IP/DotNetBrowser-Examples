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
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Chromium;
using DotNetBrowser.Engine;
using DotNetBrowser.Engine.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;

namespace DnbReportService
{
    /// <summary>
    ///     Owns the single <see cref="IEngine" /> of the service: creates it
    ///     on start, creates a new one if Chromium exits on its own, and
    ///     disposes it on stop.
    /// </summary>
    public sealed class BrowserEngineHost : IHostedService
    {
        private readonly ILogger<BrowserEngineHost> logger;
        private readonly ReportServiceOptions options;
        private readonly object sync = new object();
        private IEngine engine;
        private volatile bool stopping;

        /// <summary>
        ///     Creates the host. The engine is created in
        ///     <see cref="StartAsync" />.
        /// </summary>
        public BrowserEngineHost(ILogger<BrowserEngineHost> logger,
                                 ReportServiceOptions options)
        {
            this.logger = logger;
            this.options = options;
        }

        /// <summary>
        ///     Logs the environment and creates the engine.
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LogEnvironment();

            // The service account must be able to write to all three.
            Directory.CreateDirectory(options.UserDataDirectory);
            Directory.CreateDirectory(options.ChromiumDirectory);
            Directory.CreateDirectory(options.OutputDirectory);

            await CreateEngineAsync();
        }

        /// <summary>
        ///     Returns the engine. If Chromium has exited on its own, for
        ///     example after a crash, creates a new engine first.
        /// </summary>
        public async Task<IEngine> GetEngineAsync()
        {
            if (!stopping && (engine == null || engine.IsDisposed))
            {
                logger.LogWarning("The engine is gone; creating a new one.");
                await CreateEngineAsync();
            }
            return engine;
        }

        /// <summary>
        ///     Disposes the engine, which also ends the Chromium processes.
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            IEngine current;
            lock (sync)
            {
                stopping = true;
                current = engine;
                engine = null;
            }
            if (current != null)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                current.Dispose();
                logger.LogInformation("Engine disposed in {Ms} ms.",
                                      stopwatch.ElapsedMilliseconds);
            }
            return Task.CompletedTask;
        }

        private async Task CreateEngineAsync()
        {
            EngineOptions engineOptions = new EngineOptions.Builder
            {
                // No window and no desktop: render into memory.
                RenderingMode = RenderingMode.OffScreen,
                // The default is a new temporary folder for every engine.
                UserDataDirectory = options.UserDataDirectory,
                // The default is the temp folder of the service account.
                ChromiumDirectory = options.ChromiumDirectory
            }.Build();

            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                engine = await EngineFactory.CreateAsync(engineOptions);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Engine creation failed after {Ms} ms.",
                                   stopwatch.ElapsedMilliseconds);
                throw;
            }

            // If the service started to stop while Chromium was starting,
            // StopAsync() may have missed this engine: dispose it here.
            IEngine created;
            IEngine orphan = null;
            lock (sync)
            {
                created = engine;
                if (stopping)
                {
                    orphan = engine;
                    engine = null;
                }
            }
            if (stopping)
            {
                orphan?.Dispose();
                return;
            }
            created.Disposed += OnEngineDisposed;

            // The options now hold the binaries folder actually used,
            // with the platform subfolder appended.
            logger.LogInformation(
                "Engine created in {Ms} ms: Chromium {Version} in {Folder}.",
                stopwatch.ElapsedMilliseconds, ChromiumInfo.Version,
                created.Options.ChromiumDirectory);
        }

        private void OnEngineDisposed(object sender, EngineDisposedEventArgs e)
        {
            if (!stopping)
            {
                logger.LogError("Chromium exited unexpectedly with code {Code}.",
                                e.ExitCode);
            }
        }

        /// <summary>
        ///     Logs what to check first when a service misbehaves: the
        ///     account, the session, whether the SCM started the process,
        ///     and the folders Chromium will use.
        /// </summary>
        private void LogEnvironment()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    logger.LogInformation(
                        "Account {Account}, session {Session}, "
                        + "running as a service: {IsService}, "
                        + "ChromiumDirectory {Chromium}, "
                        + "UserDataDirectory {Data}.",
                        identity.Name, process.SessionId,
                        WindowsServiceHelpers.IsWindowsService(),
                        options.ChromiumDirectory,
                        options.UserDataDirectory);
                }
            }
        }
    }
}
