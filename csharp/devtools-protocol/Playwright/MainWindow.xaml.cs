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

using System.Diagnostics;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Microsoft.Playwright;
using IBrowser = DotNetBrowser.Browser.IBrowser;

namespace Playwright
{
    public partial class MainWindow : Window
    {
        private const string Url = "https://html5test.teamdev.com/";
        private const int RemoteDebuggingPort = 9223;
        private const string LocationUrl = "https://www.infobyip.com/browsergeolocation.php";
        private readonly IBrowser browser;
        private readonly IEngine engine;

        public MainWindow()
        {
            // Create and initialize the IEngine instance.
            EngineOptions engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated,
                LicenseKey = "",
                RemoteDebuggingPort = RemoteDebuggingPort
            }.Build();
            engine = EngineFactory.Create(engineOptions);

            // Create the IBrowser instance.
            browser = engine.CreateBrowser();

            InitializeComponent();

            // Initialize the WPF BrowserView control.
            browserView.InitializeFrom(browser);

            browser.Navigation.LoadUrl(Url).ContinueWith(async _ => await ConnectAsync());
        }

        private async Task ConnectAsync()
        {
            try
            {
                using IPlaywright playwright = await Microsoft.Playwright.Playwright.CreateAsync();

                // Connect to the browser using CDP
                Microsoft.Playwright.IBrowser playwrightBrowser =
                    await playwright.Chromium
                                    .ConnectOverCDPAsync($"http://localhost:{RemoteDebuggingPort}");

                IBrowserContext browserContext = playwrightBrowser.Contexts[0];
                await browserContext.GrantPermissionsAsync(["geolocation"]);
                await browserContext.SetGeolocationAsync(new Geolocation
                {
                    Latitude = 42.746635f,
                    Longitude = -75.770045f
                });

                IPage page = browserContext.Pages[0];
                await page.GotoAsync(LocationUrl);
                await page.WaitForSelectorAsync("title");

                // Scroll the map into view
                await page.Locator("#map").ScrollIntoViewIfNeededAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                MessageBox.Show($"Failed to connect: {e.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
