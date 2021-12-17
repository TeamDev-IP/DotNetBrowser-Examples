#region Copyright

// Copyright © 2021, TeamDev. All rights reserved.
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

// #docfragment "Embedding.Wpf"
using System;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace Embedding.Wpf
{
    /// <summary>
    ///     This example demonstrates how to embed DotNetBrowser
    ///     into a WPF application.
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Url = "https://teamdev.com/dotnetbrowser";
        private readonly IBrowser browser;
        private readonly IEngine engine;

        public MainWindow()
        {
            // Create and initialize the IEngine instance.
            EngineOptions engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated,
                // Set the license key programmatically.
                LicenseKey = "your_license_key_goes_here"
            }.Build();
            engine = EngineFactory.Create(engineOptions);

            // Create the IBrowser instance.
            browser = engine.CreateBrowser();

            InitializeComponent();

            // Initialize the WPF BrowserView control.
            browserView.InitializeFrom(browser);
            browser.Navigation.LoadUrl(Url);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
// #enddocfragment "Embedding.Wpf"