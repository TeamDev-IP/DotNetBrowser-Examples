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

using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace DevTools.WinForms
{
    /// <summary>
    ///     This example demonstrates a possible way to configure
    ///     remote debugging port and use DevTools in the application.
    /// </summary>
    public partial class Form1 : Form
    {
        private IBrowser browser1;
        private IBrowser browser2;
        private IEngine engine;

        public Form1()
        {
            Task.Run(() =>
                 {
                     engine = EngineFactory
                        .Create(new EngineOptions.Builder
                                    {
                                        RenderingMode = RenderingMode.HardwareAccelerated,
                                        RemoteDebuggingPort = 9222
                                    }
                                   .Build());
                     browser1 = engine.CreateBrowser();
                     browser2 = engine.CreateBrowser();
                 })
                .ContinueWith(t =>
                 {
                     browserView1.InitializeFrom(browser1);
                     browserView2.InitializeFrom(browser2);

                     browser1.Navigation.LoadUrl("https://www.teamdev.com");
                     browser2.Navigation.LoadUrl(browser1.DevTools.RemoteDebuggingUrl);
                 }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            engine?.Dispose();
        }
    }
}