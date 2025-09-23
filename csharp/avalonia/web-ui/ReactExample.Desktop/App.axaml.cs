using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactExample.Desktop.Rpc;
using Microsoft.Extensions.Logging;

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
                desktop.Exit += (sender, args) =>
                {
                    (app as IDisposable).Dispose();
                };
            }

            base.OnFrameworkInitializationCompleted();
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
                serverOptions.ConfigureEndpointDefaults(listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
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
        /// <summary>
        ///     Map all the custom gRPC services.
        /// </summary>
        /// <param name="app">The web application to perform mapping.</param>
        private static void MapGrpcServices(WebApplication app)
        {
            app.MapGrpcService<PrefsService>()
                            .RequireCors("AllowAll"); // enforce CORS policy "AllowAll"
        }
    }
}