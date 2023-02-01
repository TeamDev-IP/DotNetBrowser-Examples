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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace Kiosk.Wpf
{
    /// <summary>
    ///     This example demonstrates how to create a kiosk-like application
    ///     that shows a webpage using DotNetBrowser.
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser browser;
        private IEngine engine;

        public MainWindow()
        {
            EngineFactory.CreateAsync(new EngineOptions.Builder
                          {
                              RenderingMode = RenderingMode.HardwareAccelerated
                          }.Build())
                         .ContinueWith(t =>
                          {
                              engine = t.Result;
                              browser = engine.CreateBrowser();
                              BrowserView.InitializeFrom(browser);
                              //Disable default context menu
                              browser.ShowContextMenuHandler = null;
                              browser.Navigation.LoadUrl(@"https://www.teamdev.com");
                          }, TaskScheduler.FromCurrentSynchronizationContext());

            // Initialize Wpf Application UI.
            InitializeComponent();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
