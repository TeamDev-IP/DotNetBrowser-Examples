#region Copyright

// Copyright © 2023, TeamDev. All rights reserved.
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

using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Extensions.Handlers;
using DotNetBrowser.Extensions;
using DotNetBrowser.Handlers;
using System.ComponentModel;

namespace Extensions.Wpf
{
    /// <summary>
    ///     The sample demonstrates how to install and use 
    ///     Chrome Extensions in DotNetBrowser.
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ExtensionUrl =
            "https://chrome.google.com/webstore/detail/ublock-origin/cjpalhdlnbpafiamejdnhcphjbkeiagm/related?hl=en";
        private IBrowser browser;
        private IEngine engine;
        private IExtension extension;

        public MainWindow()
        {
            // Create and initialize the IEngine instance.
            EngineOptions engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated
            }.Build();
            engine = EngineFactory.Create(engineOptions);

            // Create the IBrowser instance.
            browser = engine.CreateBrowser();

            InitializeComponent();

            // Initialize the WPF BrowserView control.
            WebView.InitializeFrom(browser);
            browser.Navigation.LoadUrl("https://teamdev.com");
        }

        private void OnInstallExtension(object sender, RoutedEventArgs e)
        {
            var extensions = engine.Profiles.Default.Extensions;
            extensions.ValidatePermissionsHandler =
                new Handler<ValidatePermissionsParameters, ValidatePermissionsResponse>(
                    p => ValidatePermissionsResponse.Grant()
               );
            extension = extensions.Install(ExtensionUrl).Result;
            browser.OpenExtensionActionPopupHandler = new PopupHandler(WebView);
        }

        private void OnLaunchExtension(object sender, RoutedEventArgs e)
        {
            if (extension != null)
            {
                extension.GetAction(browser).Click();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
