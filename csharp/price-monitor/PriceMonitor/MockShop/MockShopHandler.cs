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
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace PriceMonitor.MockShop
{
    /// <summary>
    ///     Serves a demo sneaker shop at https://shop.example/ from the
    ///     app's resources, so the sample doesn't depend on a real store.
    ///     Like many real stores, a product page arrives without its price:
    ///     a script requests the product data as JSON and fills it in. The
    ///     prices and the stock change on a timer.
    /// </summary>
    public sealed class MockShopHandler
        : IHandler<InterceptRequestParameters, InterceptRequestResponse>
    {
        /// <summary>
        ///     The address of the demo shop. The domain is reserved for
        ///     examples, so the handler never shadows a real site.
        /// </summary>
        public const string Origin = "https://shop.example/";

        private const string ResourcePrefix = "PriceMonitor.MockShop.";

        private static readonly Assembly Assembly =
            typeof(MockShopHandler).Assembly;

        // How long the product data takes to "arrive", as over a network.
        private static readonly TimeSpan ApiDelay = TimeSpan.FromMilliseconds(800);

        // The states each product goes through, one after another.
        private static readonly Dictionary<string, Product> Products =
            new Dictionary<string, Product>
            {
                ["stride-runner-volt"] = new Product(
                    "Stride Runner \"Volt\"",
                    new[] { 180m, 180m, 150m, 150m },
                    new[] { true, false, false, true }),
                ["court-classic-low"] = new Product(
                    "Court Classic Low",
                    new[] { 120m, 120m, 99m, 120m },
                    new[] { true, true, true, true })
            };

        private readonly MockShopOptions options;
        private readonly DateTime started = DateTime.UtcNow;

        /// <summary>
        ///     Creates the handler. The first state starts now.
        /// </summary>
        public MockShopHandler(MockShopOptions options)
        {
            this.options = options;
        }

        /// <summary>
        ///     Answers the requests to the demo shop and lets every other
        ///     request through.
        /// </summary>
        public InterceptRequestResponse Handle(InterceptRequestParameters parameters)
        {
            string url = parameters.UrlRequest.Url;
            if (!url.StartsWith(Origin, StringComparison.Ordinal))
            {
                return InterceptRequestResponse.Proceed();
            }

            string path = new Uri(url).AbsolutePath.TrimStart('/');
            string[] parts = path.Split('/');

            if (parts.Length == 3 && parts[0] == "api" && parts[1] == "products"
                && Products.TryGetValue(parts[2], out Product apiProduct))
            {
                return Content(parameters, ProductJson(parts[2], apiProduct),
                               "application/json", ApiDelay);
            }
            if (parts.Length == 2 && parts[0] == "products"
                && Products.ContainsKey(parts[1]))
            {
                return Content(parameters, Resource("product.html"), "text/html");
            }

            byte[] file = TryResource(path);
            if (file == null)
            {
                return Status(parameters, HttpStatusCode.NotFound);
            }
            return Content(parameters, file, MimeTypeOf(path));
        }

        private byte[] ProductJson(string id, Product product)
        {
            TimeSpan elapsed = DateTime.UtcNow - started;
            int state = (int)(elapsed.TotalSeconds / options.ChangeEverySeconds)
                        % product.Prices.Length;
            string price =
                product.Prices[state].ToString("0.00", CultureInfo.InvariantCulture);
            string inStock = product.InStock[state] ? "true" : "false";
            string json = "{"
                          + $"\"id\":\"{id}\","
                          + $"\"name\":\"{product.Name.Replace("\"", "\\\"")}\","
                          + $"\"price\":{price},"
                          + "\"currency\":\"USD\","
                          + $"\"inStock\":{inStock}"
                          + "}";
            return Encoding.UTF8.GetBytes(json);
        }

        private static InterceptRequestResponse Content(InterceptRequestParameters p,
                                                        byte[] bytes,
                                                        string contentType,
                                                        TimeSpan delay = default)
        {
            UrlRequestJobOptions options = new UrlRequestJobOptions
            {
                HttpStatusCode = HttpStatusCode.OK,
                Headers =
                {
                    new HttpHeader("Content-Type", contentType),
                    new HttpHeader("Cache-Control", "no-store")
                }
            };
            UrlRequestJob job = p.Network.CreateUrlRequestJob(p.UrlRequest, options);

            // The job can be completed later, after the handler returns.
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                // The browser may be gone by now, for example on stop.
                try
                {
                    job.Write(bytes);
                    job.Complete();
                }
                catch (ObjectDisposedException)
                {
                }
            });
            return InterceptRequestResponse.Intercept(job);
        }

        private static InterceptRequestResponse Status(InterceptRequestParameters p,
                                                       HttpStatusCode code)
        {
            UrlRequestJobOptions options = new UrlRequestJobOptions { HttpStatusCode = code };
            UrlRequestJob job = p.Network.CreateUrlRequestJob(p.UrlRequest, options);
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }

        private static byte[] Resource(string name)
        {
            return TryResource(name)
                   ?? throw new FileNotFoundException($"Missing shop resource {name}");
        }

        private static byte[] TryResource(string name)
        {
            using Stream stream =
                Assembly.GetManifestResourceStream(ResourcePrefix + name);
            if (stream == null)
            {
                return null;
            }
            using MemoryStream memory = new MemoryStream();
            stream.CopyTo(memory);
            return memory.ToArray();
        }

        private static string MimeTypeOf(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".css":
                    return "text/css";
                case ".js":
                    return "text/javascript";
                case ".svg":
                    return "image/svg+xml";
                default:
                    return "application/octet-stream";
            }
        }

        private sealed class Product
        {
            public Product(string name, decimal[] prices, bool[] inStock)
            {
                Name = name;
                Prices = prices;
                InStock = inStock;
            }

            public string Name { get; }

            public decimal[] Prices { get; }

            public bool[] InStock { get; }
        }
    }
}
