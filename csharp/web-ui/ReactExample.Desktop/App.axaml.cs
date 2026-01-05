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
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReactExample.Desktop.Rpc;

namespace ReactExample.Desktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var app = StartGrpcServer();
                MainWindow mainWindow = new MainWindow
                {
                    ServiceProvider = app.Services
                };

                desktop.MainWindow = mainWindow;
                desktop.Exit += (sender, args) => { (app as IDisposable).Dispose(); };
            }

            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        ///     Map all the custom gRPC services.
        /// </summary>
        /// <param name="app">The web application to perform mapping.</param>
        private static void MapGrpcServices(WebApplication app)
        {
            app.MapGrpcService<PrefsService>()
               .RequireCors("AllowAll"); // enforce CORS policy "AllowAll"
        }

        private WebApplication StartGrpcServer(params string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions
                   .ConfigureEndpointDefaults(listenOptions =>
                                              {
                                                  listenOptions.Protocols =
                                                      HttpProtocols.Http1AndHttp2;
                                              });
            });
            // Add gRPC support.
            builder.Services.AddGrpc();

            // Add service that works as an internal server for DotNetBrowser.
            builder.Services.AddSingleton<ISchemeHandler, ResourceRequestHandler>();

            // Add service that manages DotNetBrowser IEngine lifetime.
            builder.Services.AddSingleton<IEngineService, EngineService>();

            // Add CORS "AllowAll" policy.
            builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Grpc-Status", "Grpc-Message",
                                           "Grpc-Encoding", "Grpc-Accept-Encoding",
                                           "Grpc-Status-Details-Bin");
            }));


            WebApplication app = builder.Build();

            // Enable gRPC-Web and CORS middleware.
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
            app.UseCors();

            // Map all the custom gRPC services.
            MapGrpcServices(app);

            app.MapGet("/",
                       () =>
                           "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            app.Start();
            return app;
        }
    }
}