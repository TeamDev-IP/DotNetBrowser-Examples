#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation.Handlers;

namespace MailToHandling.WinForms
{
    /// <summary>
    ///     This example demonstrates how to open an external application
    ///     for handling a specific URI scheme, e.g. "mailto".
    ///     Please note that an email client should be present in the operating system
    ///     for this example to work properly.
    /// </summary>
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            Task.Run(() =>
                 {
                     EngineOptions engineOptions = new EngineOptions.Builder
                         {
                             RenderingMode = RenderingMode.HardwareAccelerated
                         }
                        .Build();
                     engine = EngineFactory.Create(engineOptions);
                     browser = engine.CreateBrowser();
                     browser.Navigation.StartNavigationHandler =
                         new Handler<StartNavigationParameters, StartNavigationResponse>(OnStartNavigation);
                 })
                .ContinueWith(t =>
                 {
                     browserView1.InitializeFrom(browser);
                     browser.Navigation.LoadUrl("https://www.teamdev.com/contact");
                 }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private StartNavigationResponse OnStartNavigation(StartNavigationParameters parameters)
        {
            string url = parameters.Url;

            Debug.WriteLine("OnStartNavigation: url = " + url);


            if (url.StartsWith("mailto:"))
            {
                // If navigate to mailto: link, the default mail client should be opened.
                // For this purpose, it is enough to launch this URL as a command line.
                Process.Start(url);

                // The navigation request in the browser should be ignored in this case.
                return StartNavigationResponse.Ignore();
            }

            return StartNavigationResponse.Start();
        }
    }
}
