#region Copyright

// Copyright © 2025, TeamDev. All rights reserved.
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
using System.Linq;
using Avalonia.Controls;
using DotNetBrowser.AvaloniaUi;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Handlers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ReactExample.Desktop
{
    public partial class MainWindow : Window
    {
        private const string Url = ResourceRequestHandler.Domain;

        public ICollection<string>? Addresses => ServiceProvider?.GetService<IServer>()
          ?.Features.Get<IServerAddressesFeature>()
          ?.Addresses.ToList();

        public IBrowser? Browser { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

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
