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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace DnbReportService
{
    /// <summary>
    ///     A Windows service that renders HTML reports to PDF, PNG, and JSON
    ///     with DotNetBrowser in off-screen mode. Also runs in a console.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The service name. Also the Event Log source.
        /// </summary>
        public const string ServiceName = "DnbReportService";

        /// <summary>
        ///     Builds and runs the host.
        /// </summary>
        public static void Main(string[] args)
        {
            // A service starts in C:\Windows\System32, so resolve
            // appsettings.json and relative paths against the app folder.
            HostApplicationBuilderSettings settings =
                new HostApplicationBuilderSettings
                {
                    Args = args,
                    ContentRootPath = AppContext.BaseDirectory
                };
            HostApplicationBuilder builder =
                Host.CreateApplicationBuilder(settings);

            // Talks to the Service Control Manager when started by it.
            // In a console it does nothing, so the same exe runs there too.
            builder.Services.AddWindowsService(
                o => o.ServiceName = ServiceName);

            ReportServiceOptions options =
                ReportServiceOptions.Load(builder.Configuration);
            builder.Services.AddSingleton(options);
            ConfigureLogging(builder, options);

            // Hosted services start in order and stop in reverse order:
            // the engine starts first and is disposed last.
            builder.Services.AddSingleton<BrowserEngineHost>();
            builder.Services.AddHostedService(
                sp => sp.GetRequiredService<BrowserEngineHost>());
            builder.Services.AddHostedService<ReportWorker>();

            builder.Build().Run();
        }

        /// <summary>
        ///     Sends logs to the Event Log (when running as a service) and to
        ///     a text file in the output folder.
        /// </summary>
        private static void ConfigureLogging(HostApplicationBuilder builder,
                                             ReportServiceOptions options)
        {
            // AddWindowsService() adds the Event Log provider. An
            // administrator registers this source once, at install time.
            builder.Services.Configure<EventLogSettings>(s =>
            {
                s.SourceName = ServiceName;
                s.LogName = "Application";
            });
            string logFile = Path.Combine(options.OutputDirectory,
                                          ServiceName + ".log");
            builder.Logging.AddProvider(new FileLoggerProvider(logFile));
        }
    }
}
