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

using System;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;

namespace SeleniumChromeDriver
{
    public partial class Form1 : Form
    {
        private const int RemoteDebuggingPort = 9222;

        private IEngine engine;
        private IBrowser browser;
        private BrowserView browserView;
        private SeleniumInstance seleniumInstance;

        public Form1()
        {
            InitializeComponent();
            Closed += Form1_Closed;
            Load += Form1_Load;

            InitializeBrowser();
            seleniumInstance = new SeleniumInstance(RemoteDebuggingPort);
            seleniumInstance.Connected += SeleniumInstance_Connected;
        }

        private void SeleniumInstance_Connected()
        {
            if (InvokeRequired)
            {
                Invoke((Action)SeleniumInstance_Connected);
                return;
            }

            Activate();
        }

        private void InitializeBrowser()
        {
            EngineOptions engineOptions = new EngineOptions.Builder()
            {
                ChromiumSwitches =
                    {
                        "--enable-automation"
                    },
                WebSecurityDisabled = true,
                RemoteDebuggingPort = RemoteDebuggingPort
            }
            .Build();

            engine = EngineFactory.Create(engineOptions);
            browser = engine.CreateBrowser();

            browser.MainFrame.LoadHtml("<h1>Waiting for Selenium...</h1>");

            browserView = new BrowserView() { Dock = DockStyle.Fill };
            browserView.InitializeFrom(browser);
            Controls.Add(browserView);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await seleniumInstance.ConnectAndRun();
        }

        private void Form1_Closed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
