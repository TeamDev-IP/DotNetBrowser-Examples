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
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Chromium;
using DotNetBrowser.Engine;
using DotNetBrowser.Engine.Events;
using DotNetBrowser.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PriceMonitor.MockShop;

namespace PriceMonitor
{
    /// <summary>
    ///     Owns the single <see cref="IEngine" /> of the monitor: creates it
    ///     on start, creates a new one if Chromium exits on its own, and
    ///     disposes it on stop.
    /// </summary>
    public sealed class BrowserEngineHost : IHostedService
    {
        private readonly ILogger<BrowserEngineHost> logger;
        private readonly MonitorOptions options;
        private readonly MockShopHandler mockShop;
        private readonly object sync = new object();
        private IEngine engine;
        private volatile bool stopping;

        /// <summary>
        ///     Creates the host. The engine is created in
        ///     <see cref="StartAsync" />.
        /// </summary>
        public BrowserEngineHost(ILogger<BrowserEngineHost> logger,
                                 MonitorOptions options, MockShopHandler mockShop)
        {
            this.logger = logger;
            this.options = options;
            this.mockShop = mockShop;
        }

        /// <summary>
        ///     Creates the engine.
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(options.DataDirectory);
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
            current?.Dispose();
            logger.LogInformation("Engine disposed.");
            return Task.CompletedTask;
        }

        private async Task CreateEngineAsync()
        {
            EngineOptions.Builder builder = new EngineOptions.Builder
            {
                // No window: render into memory.
                RenderingMode = RenderingMode.OffScreen,
                // Keep everything Chromium writes in the data folder. The
                // crash dumps default to the home folder, which a service
                // account may not have.
                ChromiumDirectory = Path.Combine(options.DataDirectory, "Chromium"),
                UserDataDirectory = Path.Combine(options.DataDirectory, "UserData"),
                CrashDumpDirectory = Path.Combine(options.DataDirectory, "CrashDumps")
            };
            if (options.MockShop.Enabled)
            {
                // Answers the requests to https://shop.example/ only.
                builder.Schemes.Add(Scheme.Https, mockShop);
            }

            IEngine created = await EngineFactory.CreateAsync(builder.Build());

            // If the host started to stop while Chromium was starting,
            // StopAsync() missed this engine: dispose it here.
            bool orphan;
            lock (sync)
            {
                orphan = stopping;
                if (!orphan)
                {
                    engine = created;
                    created.Disposed += OnEngineDisposed;
                }
            }
            if (orphan)
            {
                created.Dispose();
                return;
            }
            logger.LogInformation("Engine created: Chromium {Version}.",
                                  ChromiumInfo.Version);
        }

        private void OnEngineDisposed(object sender, EngineDisposedEventArgs e)
        {
            if (!stopping)
            {
                logger.LogError("Chromium exited unexpectedly with code {Code}.",
                                e.ExitCode);
            }
        }
    }
}
