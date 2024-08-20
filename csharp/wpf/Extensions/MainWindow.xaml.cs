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
using System;
using System.IO;

namespace Extensions.Wpf
{
    /// <summary>
    ///     The sample demonstrates how to install and use 
    ///     Chrome Extensions in DotNetBrowser.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string ExtensionPath =
            Path.Combine(Directory.GetCurrentDirectory(), "cjpalhdlnbpafiamejdnhcphjbkeiagm.crx");
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
            if (extension == null)
            {
                IExtensions extensions = engine.Profiles.Default.Extensions;
                extension = extensions.Install(ExtensionPath).Result;
            }
        }

        private void OnLaunchExtension(object sender, RoutedEventArgs e)
        {
            extension?.GetAction(browser).Click();

            // By default, DotNetBrowser will open a new window with the extension's
            // popup when the action is "clicked".
            //
            // It's easy to override by implementing your own event handler, as shown below.
            // For example, you can use it to automatically configure the extension and
            // never show the pop-up end-users.
            //
            // browser.OpenExtensionActionPopupHandler =
            //    new Handler<OpenExtensionActionPopupParameters, OpenExtensionActionPopupResponse>(p =>
            //    {
            //        p.PopupBrowser.Navigation.FrameLoadFinished += (s, e) =>
            //        {
            //            if (e.Frame.IsMain)
            //            {
            //                e.Frame.ExecuteJavaScript("document.querySelector('input').click()");
            //            }
            //        };
            //        return OpenExtensionActionPopupResponse.Open();
            //    });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            engine.Dispose();
        }
    }
}
