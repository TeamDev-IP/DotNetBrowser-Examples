using Avalonia.Controls;
using DotNetBrowser.AvaloniaUi;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace ReactExample.Desktop
{
    public partial class MainWindow : Window
    {
        private const string Url = ResourceRequestHandler.Domain;
        public IBrowser? Browser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public ICollection<string>? Addresses => ServiceProvider?.GetService<IServer>()?.Features.Get<IServerAddressesFeature>()?.Addresses.ToList();
        public IServiceProvider ServiceProvider { get; set; }

        private void OnInjectJs(InjectJsParameters p)
        {
            dynamic window = p.Frame.ExecuteJavaScript("window").Result;
            if (window != null)
            {
                window.rpcAddress = Addresses?.FirstOrDefault();
            }
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            Browser?.Dispose();
        }

        private async void Window_Opened(object? sender, EventArgs e)
        {
            // Create the IBrowser instance.
            Browser = ServiceProvider.GetService<IEngineService>()?.CreateBrowser();
            if (Browser == null)
            {
                var logger = ServiceProvider.GetService<ILogger<MainWindow>>();
                logger?.LogError("Failed to create the IBrowser instance.");
                Close();
                return;
            }
            // Initialize the Avalonia UI BrowserView control.
            BrowserView.InitializeFrom(Browser);

#if DEBUG
            Browser.DevTools.Show();
#endif
            Browser.InjectJsHandler = new Handler<InjectJsParameters>(OnInjectJs);
            await Browser.Navigation.LoadUrl(Url);
        }
    }
}
