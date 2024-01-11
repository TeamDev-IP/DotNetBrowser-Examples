#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Js;
using DotNetBrowser.WinForms;

namespace ObservePageChanges.WinForms
{
    /// <summary>
    ///     This example demonstrates how to observe web page content changes on
    ///     .NET side using MutationObserver and JS-.NET bridge.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly IBrowser browser;
        private readonly BrowserView browserView;
        private readonly IEngine engine;

        public Form1()
        {
            InitializeComponent();
            browserView = new BrowserView {Dock = DockStyle.Fill};
            engine = EngineFactory
               .Create(new EngineOptions.Builder
                           {
                               RenderingMode = RenderingMode.HardwareAccelerated
                           }
                          .Build());
            browser = engine.CreateBrowser();
            browserView.InitializeFrom(browser);

            Controls.Add(browserView);
            Task.Run(() =>
            {
                browser.Navigation.LoadUrl(Path.GetFullPath("page.html")).Wait();

                // After the page is loaded successfully, we can configure the observer.
                ConfigureObserver();
            });
        }

        public void CharacterDataChanged(string innerText)
        {
            Console.WriteLine(innerText);
        }

        private void ConfigureObserver()
        {
            // Inject the listener .NET object into Javascript
            IJsObject window = browser.MainFrame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["MutationObserverListener"] = this;

            // The script for configuring MutationObserver to observe the changes of
            // the element with id 'countdown'.
            string observerScript =
                "var spanElement = document.getElementById('countdown');"
                + "var observer = new MutationObserver("
                + "function(mutations){"
                + "window.MutationObserverListener.CharacterDataChanged(spanElement.innerHTML);"
                + "});"
                + "var config = { childList: true };"
                + "observer.observe(spanElement, config);";

            // Execute the script that configures the observer.
            // After the observer is configured, the .NET side starts receiving element changes.
            browser.MainFrame.ExecuteJavaScript(observerScript);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
