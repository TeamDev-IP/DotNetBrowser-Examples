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
using Avalonia.Controls;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace LocalAiAssistant
{
    public partial class MainWindow : Window
    {
        private const string AppUrl = LocalAppSchemeHandler.Domain;

        private readonly IBrowser browser;
        private readonly IEngine engine;

        public MainWindow()
        {
            // Create and initialize the IEngine instance.
            engine = EngineFactory.Create(new EngineOptions.Builder
            {
                LicenseKey = "",
                RenderingMode = RenderingMode.HardwareAccelerated,
                Schemes =
                {
                    // Register the custom scheme handler to serve local app files.
                    { LocalAppSchemeHandler.Scheme, new LocalAppSchemeHandler() }
                }
            }.Build());

            // Create the IBrowser instance.
            browser = engine.CreateBrowser();

            // Initialize the Avalonia UI.
            InitializeComponent();

            // Initialize the Avalonia UI BrowserView control.
            BrowserView.InitializeFrom(browser);

            // Load the app page.
            browser.Navigation.LoadUrl(AppUrl);
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
