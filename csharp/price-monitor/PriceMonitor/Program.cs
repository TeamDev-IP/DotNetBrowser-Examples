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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceMonitor.MockShop;

namespace PriceMonitor
{
    /// <summary>
    ///     A background job that watches product pages for price and stock
    ///     changes with DotNetBrowser and posts the changes to Discord. Runs
    ///     in a console, as a Windows service, and as a systemd service.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The service name. The Event Log source is the app name, which
        ///     is the same.
        /// </summary>
        public const string ServiceName = "PriceMonitor";

        /// <summary>
        ///     Builds and runs the host.
        /// </summary>
        public static void Main(string[] args)
        {
            // A Windows service starts in C:\Windows\System32, so resolve
            // appsettings.json and relative paths against the app folder.
            HostApplicationBuilderSettings settings =
                new HostApplicationBuilderSettings
                {
                    Args = args,
                    ContentRootPath = AppContext.BaseDirectory
                };
            HostApplicationBuilder builder =
                Host.CreateApplicationBuilder(settings);

            // Each one does nothing unless its service manager started
            // the process, so the same build also runs in a console.
            builder.Services.AddWindowsService(
                o => o.ServiceName = ServiceName);
            builder.Services.AddSystemd();

            MonitorOptions options = MonitorOptions.Load(builder.Configuration);
            AddSecrets(builder.Configuration, options);
            builder.Services.AddSingleton(options);
            builder.Services.AddSingleton(new MockShopHandler(options.MockShop));
            builder.Services.AddSingleton<PriceHistory>();
            builder.Services.AddSingleton<DiscordNotifier>();

            // Hosted services start in order and stop in reverse order:
            // the engine starts first and is disposed last.
            builder.Services.AddSingleton<BrowserEngineHost>();
            builder.Services.AddHostedService(
                sp => sp.GetRequiredService<BrowserEngineHost>());
            builder.Services.AddHostedService<MonitorWorker>();

            builder.Build().Run();
        }

        /// <summary>
        ///     Adds the places the Discord webhook URL can come from, besides
        ///     environment variables: user secrets for a console run, and
        ///     <c>secrets.json</c> in the data folder for a service.
        /// </summary>
        private static void AddSecrets(ConfigurationManager configuration,
                                       MonitorOptions options)
        {
            configuration.AddUserSecrets(typeof(Program).Assembly,
                                         optional: true);
            string file = Path.Combine(options.DataDirectory, "secrets.json");
            configuration.AddJsonFile(file, optional: true);

            // Environment variables win over both files.
            configuration.AddEnvironmentVariables();

            // The value is in the configuration now. Remove the variable,
            // so the Chromium processes, including the ones that run the
            // pages, don't inherit the webhook URL.
            foreach (string name in Environment.GetEnvironmentVariables().Keys)
            {
                if (name.Equals("Discord__WebhookUrl",
                                StringComparison.OrdinalIgnoreCase)
                    || name.Equals("Discord:WebhookUrl",
                                   StringComparison.OrdinalIgnoreCase))
                {
                    Environment.SetEnvironmentVariable(name, null);
                }
            }
        }
    }
}
