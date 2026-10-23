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
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Navigation;
using DotNetBrowser.Ui;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DnbReportService
{
    /// <summary>
    ///     Runs the configured report jobs at start and then every
    ///     <see cref="ReportServiceOptions.IntervalSeconds" /> seconds.
    /// </summary>
    public sealed class ReportWorker : BackgroundService
    {
        private static readonly Size ViewportSize = new Size(1280, 900);
        private static readonly TimeSpan LoadTimeout = TimeSpan.FromSeconds(60);
        private static readonly TimeSpan ReadyTimeout = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan PrintTimeout = TimeSpan.FromSeconds(60);

        private static readonly JsonSerializerOptions JsonOptions =
            new JsonSerializerOptions { WriteIndented = true };

        private readonly BrowserEngineHost engineHost;
        private readonly ILogger<ReportWorker> logger;
        private readonly ReportServiceOptions options;

        /// <summary>
        ///     Creates the worker. The jobs start in <see cref="ExecuteAsync" />.
        /// </summary>
        public ReportWorker(BrowserEngineHost engineHost,
                            ILogger<ReportWorker> logger,
                            ReportServiceOptions options)
        {
            this.engineHost = engineHost;
            this.logger = logger;
            this.options = options;
        }

        /// <summary>
        ///     Runs one cycle of jobs per interval until the service stops.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TimeSpan interval =
                TimeSpan.FromSeconds(Math.Max(1, options.IntervalSeconds));
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunCycleAsync(stoppingToken);
                try
                {
                    await Task.Delay(interval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async Task RunCycleAsync(CancellationToken stoppingToken)
        {
            int ok = 0;
            int failed = 0;
            foreach (ReportJob job in options.Jobs)
            {
                // Start no new job once the service is stopping.
                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                // A failed job is logged; the service keeps running.
                try
                {
                    await RunJobAsync(job);
                    ok++;
                }
                catch (Exception e)
                {
                    failed++;
                    logger.LogError(e, "Job {Job} failed.", job.Name);
                }
            }
            logger.LogInformation("Cycle finished: {Ok} ok, {Failed} failed.",
                                  ok, failed);
        }

        private async Task RunJobAsync(ReportJob job)
        {
            string url = job.ResolveUrl();
            string stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string baseName = Path.Combine(options.OutputDirectory,
                                           $"{job.Name}-{stamp}");
            Stopwatch stopwatch = Stopwatch.StartNew();

            IEngine engine = await engineHost.GetEngineAsync();
            using (IBrowser browser = engine.CreateBrowser())
            {
                // Off-screen browsers have no window, so set the viewport.
                browser.Size = ViewportSize;

                NavigationResult result =
                    await browser.Navigation.LoadUrl(url, LoadTimeout);
                if (result.LoadResult != LoadResult.Completed)
                {
                    throw new InvalidOperationException(
                        $"Loading {url} ended with {result.LoadResult} "
                        + $"({result.Error}).");
                }

                // A finished load is not a finished paint. On a cold
                // profile, a screenshot taken right after the load can be
                // half-painted. Wait for the page to say it is ready, then
                // give the compositor a moment.
                bool ready = await WaitUntilReadyAsync(browser, job.ReadyScript);
                logger.LogInformation(
                    "Job {Job}: loaded {Url} in {Ms} ms; ready={Ready}.",
                    job.Name, url, stopwatch.ElapsedMilliseconds, ready);
                await Task.Delay(Math.Max(0, options.CaptureDelayMs));

                if (job.Png)
                {
                    Bitmap image = browser.TakeImage();
                    PngEncoder.Save(image, baseName + ".png");
                    LogFile(job, baseName + ".png");
                }
                if (job.Json)
                {
                    SaveJson(browser, job, url, ready, baseName + ".json");
                    LogFile(job, baseName + ".json");
                }
                if (job.Pdf)
                {
                    await PdfExport.SaveAsync(browser, baseName + ".pdf")
                                   .WaitAsync(PrintTimeout);
                    LogFile(job, baseName + ".pdf");
                }
            }
            logger.LogInformation("Job {Job} done in {Ms} ms.", job.Name,
                                  stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        ///     Polls <paramref name="script" /> until it returns true, so
        ///     content drawn by JavaScript after the load is in the output.
        /// </summary>
        private static async Task<bool> WaitUntilReadyAsync(IBrowser browser,
                                                            string script)
        {
            if (string.IsNullOrWhiteSpace(script))
            {
                return true;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < ReadyTimeout)
            {
                bool ready = await browser.MainFrame
                                          .ExecuteJavaScript<bool>($"!!({script})");
                if (ready)
                {
                    return true;
                }
                await Task.Delay(100);
            }
            return false;
        }

        /// <summary>
        ///     Saves data read from the DOM: the title and the text of the
        ///     element that matches the job's selector.
        /// </summary>
        private static void SaveJson(IBrowser browser, ReportJob job,
                                     string url, bool ready, string path)
        {
            string text = null;
            if (!string.IsNullOrEmpty(job.Selector))
            {
                IElement element = browser.MainFrame.Document
                                          .GetElementByCssSelector(job.Selector);
                text = element?.InnerText;
            }
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                ["job"] = job.Name,
                ["url"] = url,
                ["capturedAt"] = DateTimeOffset.Now,
                ["ready"] = ready,
                ["title"] = browser.Title,
                ["selector"] = job.Selector,
                ["text"] = text
            };
            File.WriteAllText(path, JsonSerializer.Serialize(data, JsonOptions));
        }

        private void LogFile(ReportJob job, string path)
        {
            long size = new FileInfo(path).Length;
            logger.LogInformation("Job {Job}: wrote {File} ({Size} bytes).",
                                  job.Name, Path.GetFileName(path), size);
        }
    }
}
