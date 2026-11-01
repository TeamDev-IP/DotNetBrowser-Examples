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

namespace PriceMonitor
{
    /// <summary>
    ///     Where the monitor reads a product's price and availability from.
    /// </summary>
    public enum DataSource
    {
        /// <summary>
        ///     The rendered page: the elements the page's script fills in.
        /// </summary>
        Dom,

        /// <summary>
        ///     The JSON response the page loads in the background.
        /// </summary>
        Json
    }

    /// <summary>
    ///     The <c>Monitor</c> section of appsettings.json.
    /// </summary>
    public sealed class MonitorOptions
    {
        private const string SectionName = "Monitor";

        /// <summary>
        ///     The folder for the Chromium binaries, the Chromium profile,
        ///     and the price history. Relative to the app folder.
        /// </summary>
        public string DataDirectory { get; set; } = "Data";

        /// <summary>
        ///     The pause between two checks of all products, in seconds.
        /// </summary>
        public int IntervalSeconds { get; set; } = 300;

        /// <summary>
        ///     The pause between two pages within one check, in seconds.
        /// </summary>
        public int PageDelaySeconds { get; set; } = 5;

        /// <summary>
        ///     How long to wait for a page, and then for its data, in seconds.
        /// </summary>
        public int PageTimeoutSeconds { get; set; } = 30;

        /// <summary>
        ///     How many times to try a page before giving up until the next
        ///     check.
        /// </summary>
        public int Attempts { get; set; } = 3;

        /// <summary>
        ///     The products to watch.
        /// </summary>
        public List<ProductOptions> Products { get; set; } =
            new List<ProductOptions>();

        /// <summary>
        ///     The demo shop served from the app's resources.
        /// </summary>
        public MockShopOptions MockShop { get; set; } = new MockShopOptions();

        /// <summary>
        ///     Binds the section and resolves the data folder against the app
        ///     folder.
        /// </summary>
        public static MonitorOptions Load(IConfiguration configuration)
        {
            MonitorOptions options = new MonitorOptions();
            configuration.GetSection(SectionName).Bind(options);
            string expanded =
                Environment.ExpandEnvironmentVariables(options.DataDirectory);
            options.DataDirectory =
                Path.GetFullPath(expanded, AppContext.BaseDirectory);
            options.PageTimeoutSeconds = Math.Max(1, options.PageTimeoutSeconds);
            foreach (ProductOptions product in options.Products)
            {
                product.Validate();
            }
            return options;
        }
    }

    /// <summary>
    ///     One product page and where its data is.
    /// </summary>
    public sealed class ProductOptions
    {
        /// <summary>
        ///     The name used in the log and in the alerts.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The product page.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Where to read the data from: the rendered page or the JSON
        ///     response the page loads.
        /// </summary>
        public DataSource Source { get; set; } = DataSource.Dom;

        /// <summary>
        ///     For <see cref="DataSource.Dom" />: the element with the price.
        /// </summary>
        public string PriceSelector { get; set; }

        /// <summary>
        ///     For <see cref="DataSource.Dom" />: the "Add to cart" button.
        ///     The product is sold out when the button is disabled.
        /// </summary>
        public string CartButtonSelector { get; set; }

        /// <summary>
        ///     For <see cref="DataSource.Json" />: the address of the request
        ///     that returns the product data. The query part is ignored.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        ///     Fails at start, with the product name, if a setting the data
        ///     source needs is missing.
        /// </summary>
        public void Validate()
        {
            bool json = Source == DataSource.Json;
            bool valid = json
                             ? Uri.TryCreate(ApiUrl, UriKind.Absolute, out Uri _)
                             : !string.IsNullOrWhiteSpace(PriceSelector)
                               && !string.IsNullOrWhiteSpace(CartButtonSelector);
            if (string.IsNullOrWhiteSpace(Url) || !valid)
            {
                string needed = json ? "ApiUrl" : "PriceSelector and CartButtonSelector";
                throw new InvalidOperationException(
                    $"Product \"{Name}\": set Url and {needed} in appsettings.json.");
            }
        }
    }

    /// <summary>
    ///     The demo shop. Its prices and stock change on a timer, so the
    ///     monitor has something to report.
    /// </summary>
    public sealed class MockShopOptions
    {
        /// <summary>
        ///     Whether to serve the demo shop at https://shop.example/.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///     How long each state of the demo shop lasts, in seconds.
        /// </summary>
        public int ChangeEverySeconds { get; set; } = 90;
    }
}
