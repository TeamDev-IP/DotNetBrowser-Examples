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

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PriceMonitor
{
    /// <summary>
    ///     Posts alerts to a Discord channel through a webhook.
    /// </summary>
    public sealed class DiscordNotifier
    {
        private const int Green = 0x127A3E;
        private const int Red = 0xB42318;

        // Not from IHttpClientFactory: its handlers log every request URL,
        // and the webhook URL is a secret.
        private readonly HttpClient client = new HttpClient();
        private readonly IConfiguration configuration;
        private readonly ILogger<DiscordNotifier> logger;

        /// <summary>
        ///     Creates the notifier. The webhook URL is read on every alert,
        ///     from the <c>Discord:WebhookUrl</c> setting.
        /// </summary>
        public DiscordNotifier(IConfiguration configuration,
                               ILogger<DiscordNotifier> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        ///     Posts one message with a card that links to the product page.
        ///     Without a webhook URL, only logs the alert.
        /// </summary>
        public async Task SendAsync(string product, string url, string text,
                                    bool good, CancellationToken cancellationToken)
        {
            logger.LogInformation("Alert: {Product}: {Text}", product, text);

            // The URL is a secret: anyone who has it can post to the channel.
            string webhookUrl = configuration["Discord:WebhookUrl"];
            if (string.IsNullOrEmpty(webhookUrl))
            {
                logger.LogWarning("Discord:WebhookUrl is not set; alert not sent.");
                return;
            }

            object message = new
            {
                username = "Price monitor",
                embeds = new[]
                {
                    new
                    {
                        title = product,
                        url,
                        description = text,
                        color = good ? Green : Red
                    }
                }
            };
            using StringContent content = new StringContent(
                JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
            using HttpResponseMessage response =
                await client.PostAsync(webhookUrl, content, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}
