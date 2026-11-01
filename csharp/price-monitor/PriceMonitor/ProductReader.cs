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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Events;

namespace PriceMonitor
{
    /// <summary>
    ///     Loads a product page in a browser and reads the price and the
    ///     availability, either from the rendered page or from the JSON
    ///     response the page loads.
    /// </summary>
    public static class ProductReader
    {
        private static readonly TimeSpan PollInterval =
            TimeSpan.FromMilliseconds(250);

        // Product data is small; a bigger response is not what we want.
        private const int MaxBodyBytes = 1024 * 1024;

        // The first number, such as 1,299.00, in the price text.
        private static readonly Regex PriceNumber =
            new Regex(@"\d[\d,]*(\.\d+)?", RegexOptions.CultureInvariant);

        /// <summary>
        ///     Loads the page and waits until its script has filled in the
        ///     price, then reads the price element and the cart button.
        /// </summary>
        public static async Task<ProductSnapshot> ReadFromDomAsync(
            IBrowser browser, ProductOptions product, TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            await LoadAsync(browser, product.Url, timeout);

            // The load is finished when the HTML is, but the price comes
            // later, from a script. Poll until the element has text.
            DateTime deadline = DateTime.UtcNow + timeout;
            while (true)
            {
                IDocument document = browser.MainFrame.Document;
                IElement price =
                    document.GetElementByCssSelector(product.PriceSelector);
                string text = price?.InnerText.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    IElement button = document.GetElementByCssSelector(
                        product.CartButtonSelector)
                        ?? throw new InvalidOperationException(
                            $"No element matches {product.CartButtonSelector}.");
                    bool inStock = !button.Attributes.ContainsKey("disabled");
                    return new ProductSnapshot
                    {
                        Time = DateTimeOffset.Now,
                        Url = product.Url,
                        Price = ParsePrice(text),
                        InStock = inStock
                    };
                }
                if (DateTime.UtcNow > deadline)
                {
                    throw new TimeoutException(
                        $"No text in {product.PriceSelector} after {timeout}.");
                }
                await Task.Delay(PollInterval, cancellationToken);
            }
        }

        /// <summary>
        ///     Loads the page and captures the body of the JSON response
        ///     that the page requests for its product data.
        /// </summary>
        public static async Task<ProductSnapshot> ReadFromJsonAsync(
            IBrowser browser, ProductOptions product, TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            string json = await CaptureResponseAsync(
                browser, product.Url, new Uri(product.ApiUrl), timeout,
                cancellationToken);

            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            return new ProductSnapshot
            {
                Time = DateTimeOffset.Now,
                Url = product.Url,
                Price = root.GetProperty("price").GetDecimal(),
                InStock = root.GetProperty("inStock").GetBoolean()
            };
        }

        /// <summary>
        ///     Loads <paramref name="pageUrl" /> and returns the body of the
        ///     response from <paramref name="apiUrl" />, ignoring its query.
        /// </summary>
        private static async Task<string> CaptureResponseAsync(
            IBrowser browser, string pageUrl, Uri apiUrl,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            INetwork network = browser.Profile.Network;
            MemoryStream body = new MemoryStream();
            TaskCompletionSource<string> result =
                new TaskCompletionSource<string>(
                    TaskCreationOptions.RunContinuationsAsynchronously);

            // Only requests to this exact address, from this browser or from
            // a service worker, which has no browser: a script from another
            // site can't pass off its own response as the product data.
            bool IsApiRequest(UrlRequest request)
            {
                return (request.Browser == null || request.Browser == browser)
                       && Uri.TryCreate(request.Url, UriKind.Absolute, out Uri url)
                       && url.GetLeftPart(UriPartial.Path)
                       == apiUrl.GetLeftPart(UriPartial.Path);
            }

            // The events are raised for every request in the profile, and
            // the body comes in parts: collect the parts of the API request
            // until it completes.
            void OnBytes(object sender, ResponseBytesReceivedEventArgs e)
            {
                if (!IsApiRequest(e.UrlRequest))
                {
                    return;
                }
                lock (body)
                {
                    if (body.Length + e.Data.Length > MaxBodyBytes)
                    {
                        result.TrySetException(new IOException(
                            $"The response is larger than {MaxBodyBytes} bytes."));
                        return;
                    }
                    body.Write(e.Data, 0, e.Data.Length);
                }
            }

            void OnCompleted(object sender, RequestCompletedEventArgs e)
            {
                if (!IsApiRequest(e.UrlRequest))
                {
                    return;
                }
                if (e.Status == RequestStatus.Success && e.ResponseCode == 200)
                {
                    lock (body)
                    {
                        result.TrySetResult(Encoding.UTF8.GetString(body.ToArray()));
                    }
                }
                else
                {
                    result.TrySetException(new IOException(
                        $"{apiUrl.AbsolutePath}: {e.Status}, HTTP {e.ResponseCode}."));
                }
            }

            network.ResponseBytesReceived += OnBytes;
            network.RequestCompleted += OnCompleted;
            try
            {
                await LoadAsync(browser, pageUrl, timeout);
                try
                {
                    return await result.Task.WaitAsync(timeout, cancellationToken);
                }
                catch (TimeoutException)
                {
                    throw new TimeoutException(
                        $"No response from {apiUrl.AbsolutePath} after {timeout}.");
                }
            }
            finally
            {
                network.ResponseBytesReceived -= OnBytes;
                network.RequestCompleted -= OnCompleted;
            }
        }

        private static async Task LoadAsync(IBrowser browser, string url,
                                            TimeSpan timeout)
        {
            NavigationResult navigation =
                await browser.Navigation.LoadUrl(url, timeout);
            if (navigation.LoadResult != LoadResult.Completed)
            {
                throw new IOException(
                    $"Loading {url}: {navigation.LoadResult}, {navigation.Error}.");
            }
        }

        /// <summary>
        ///     Turns a price such as "$1,299.00" into 1299.00. Expects a dot
        ///     as the decimal separator; change it for stores that write
        ///     "1.299,00".
        /// </summary>
        private static decimal ParsePrice(string text)
        {
            Match number = PriceNumber.Match(text);
            if (!number.Success)
            {
                throw new FormatException($"No price in \"{text}\".");
            }
            return decimal.Parse(number.Value.Replace(",", ""),
                                 CultureInfo.InvariantCulture);
        }
    }
}
