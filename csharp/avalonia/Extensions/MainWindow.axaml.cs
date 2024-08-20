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

using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Extensions;

namespace Extensions.AvaloniaUi
{
    /// <summary>
    ///     The sample demonstrates how to install and use
    ///     Chrome Extensions in DotNetBrowser.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string ExtensionPath =
            Path.Combine(Directory.GetCurrentDirectory(), "cjpalhdlnbpafiamejdnhcphjbkeiagm.crx");

        public static readonly DirectProperty<MainWindow, IExtension> ExtensionProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, IExtension>(
             nameof(Extension),
             o => o.Extension,
             (o, v) => o.Extension = v);

        private readonly IBrowser browser;
        private readonly IEngine engine;
        private IExtension extension;

        public IExtension Extension
        {
            get { return extension; }
            private set { SetAndRaise(ExtensionProperty, ref extension, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Create and initialize the IEngine instance.
            EngineOptions engineOptions = new EngineOptions.Builder
            {
                LicenseKey = "",
                RenderingMode = RenderingMode.HardwareAccelerated
            }.Build();
            engine = EngineFactory.Create(engineOptions);

            // Create the IBrowser instance.
            browser = engine.CreateBrowser();

            // Initialize the Avalonia UI BrowserView control.
            BrowserView.InitializeFrom(browser);
            browser.Navigation.LoadUrl("teamdev.com");
        }

        // Determines whether the "Install" button is enabled.
        public bool CanInstallExtension(object msg) => msg == null;

        // Determines whether the "Launch" button is enabled.
        public bool CanLaunchExtension(object msg) => msg != null;

        public void InstallExtension(object msg)
        {
            if (Extension == null)
            {
                IExtensions extensions = engine.Profiles.Default.Extensions;
                Extension = extensions.Install(ExtensionPath).Result;
            }
        }

        public void LaunchExtension(object msg)
        {
            Extension?.GetAction(browser).Click();

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
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
