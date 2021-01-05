#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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

using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;

namespace NETCore30.WinForms
{
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            BrowserView webView = new BrowserView {Dock = DockStyle.Fill};
            Task.Run(() =>
                {
                    engine = EngineFactory.Create(new EngineOptions.Builder
                            {RenderingMode = RenderingMode.HardwareAccelerated}
                        .Build());
                    browser = engine.CreateBrowser();
                })
                .ContinueWith(t =>
                {
                    webView.InitializeFrom(browser);
                    browser.Navigation.LoadUrl("https://www.teamdev.com/");
                }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
            FormClosing += Form1_FormClosing;
            Controls.Add(webView);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}