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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PriceMonitor
{
    /// <summary>
    ///     Checks every product on a schedule, compares the result with the
    ///     latest snapshot in the history, and sends an alert on a change.
    /// </summary>
    public sealed class MonitorWorker : BackgroundService
    {
        private readonly BrowserEngineHost engineHost;
        private readonly PriceHistory history;
        private readonly ILogger<MonitorWorker> logger;
        private readonly DiscordNotifier notifier;
        private readonly MonitorOptions options;

        /// <summary>
        ///     Creates the worker.
        /// </summary>
        public MonitorWorker(BrowserEngineHost engineHost, PriceHistory history,
                             DiscordNotifier notifier, MonitorOptions options,
                             ILogger<MonitorWorker> logger)
        {
            this.engineHost = engineHost;
            this.history = history;
            this.notifier = notifier;
            this.options = options;
            this.logger = logger;
        }

        /// <summary>
        ///     Runs the checks until the host stops.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TimeSpan interval = TimeSpan.FromSeconds(Math.Max(1, options.IntervalSeconds));
            TimeSpan pageDelay = TimeSpan.FromSeconds(Math.Max(0, options.PageDelaySeconds));
            while (!stoppingToken.IsCancellationRequested)
            {
                for (int i = 0; i < options.Products.Count; i++)
                {
                    // Don't hit the site with all pages at once.
                    if (i > 0)
                    {
                        await Task.Delay(pageDelay, stoppingToken);
                    }
                    ProductOptions product = options.Products[i];
                    try
                    {
                        await CheckAsync(product, stoppingToken);
                    }
                    catch (Exception e) when (!stoppingToken.IsCancellationRequested)
                    {
                        // One product's failure, such as a locked history
                        // file, doesn't stop the others.
                        logger.LogError(e, "{Product}: the check failed.", product.Name);
                    }
                }
                await Task.Delay(interval, stoppingToken);
            }
        }

        private async Task CheckAsync(ProductOptions product,
                                      CancellationToken stoppingToken)
        {
            ProductSnapshot current = await ReadAsync(product, stoppingToken);
            if (current == null)
            {
                return;
            }

            ProductSnapshot previous = history.GetLatest(product.Url);
            if (previous == null)
            {
                logger.LogInformation("{Product}: {Price}, in stock: {InStock}.",
                                      product.Name, Money(current.Price),
                                      current.InStock);
                history.Add(current);
                return;
            }
            if (previous.Price == current.Price && previous.InStock == current.InStock)
            {
                logger.LogInformation("{Product}: no change.", product.Name);
                return;
            }

            string text = Describe(previous, current, out bool good);
            try
            {
                await notifier.SendAsync(product.Name, product.Url, text, good,
                                         stoppingToken);
            }
            catch (Exception e) when (!stoppingToken.IsCancellationRequested)
            {
                // Not saved, so the next check finds the change again and
                // retries the alert.
                logger.LogError(e, "{Product}: the alert failed.", product.Name);
                return;
            }
            history.Add(current);
        }

        /// <summary>
        ///     Reads the product page, retrying with a growing pause. Returns
        ///     null if every attempt fails.
        /// </summary>
        private async Task<ProductSnapshot> ReadAsync(ProductOptions product,
                                                      CancellationToken stoppingToken)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(options.PageTimeoutSeconds);
            int attempts = Math.Max(1, options.Attempts);
            for (int attempt = 1; attempt <= attempts; attempt++)
            {
                try
                {
                    IEngine engine = await engineHost.GetEngineAsync();
                    using IBrowser browser = engine.CreateBrowser();
                    // On stop, closing the browser ends a load that is still
                    // running.
                    using CancellationTokenRegistration closeOnStop =
                        stoppingToken.Register(browser.Dispose);
                    if (product.Source == DataSource.Json)
                    {
                        return await ProductReader.ReadFromJsonAsync(
                            browser, product, timeout, stoppingToken);
                    }
                    return await ProductReader.ReadFromDomAsync(
                        browser, product, timeout, stoppingToken);
                }
                catch (Exception) when (stoppingToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException(stoppingToken);
                }
                catch (Exception e)
                {
                    logger.LogWarning(e, "{Product}: attempt {Attempt} of {Attempts} "
                                      + "failed.", product.Name, attempt, attempts);
                }
                if (attempt < attempts)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10 * attempt), stoppingToken);
                }
            }
            return null;
        }

        /// <summary>
        ///     Describes what changed, for example "Back in stock at 150.00.
        ///     Price dropped from 180.00 to 150.00." <paramref name="good" />
        ///     is true when the change is good news for a buyer: the product
        ///     can be bought, now or at a lower price.
        /// </summary>
        private static string Describe(ProductSnapshot previous, ProductSnapshot current,
                                       out bool good)
        {
            List<string> changes = new List<string>();
            good = false;
            if (!previous.InStock && current.InStock)
            {
                changes.Add($"Back in stock at {Money(current.Price)}.");
                good = true;
            }
            else if (previous.InStock && !current.InStock)
            {
                changes.Add("Sold out.");
            }
            if (current.Price < previous.Price)
            {
                changes.Add($"Price dropped from {Money(previous.Price)} "
                            + $"to {Money(current.Price)}.");
                good = current.InStock;
            }
            else if (current.Price > previous.Price)
            {
                changes.Add($"Price went up from {Money(previous.Price)} "
                            + $"to {Money(current.Price)}.");
            }
            return string.Join(" ", changes);
        }

        private static string Money(decimal price)
        {
            return price.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
