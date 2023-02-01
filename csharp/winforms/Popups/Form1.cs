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

using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;

namespace Popups.WinForms
{
    /// <summary>
    ///     This example demonstrates how to implement and configure a custom
    ///     OpenPopupHandler to customize displaying the pop-ups.
    /// </summary>
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            EngineFactory.CreateAsync(new EngineOptions.Builder().Build())
                         .ContinueWith(t =>
                          {
                              engine = t.Result;
                              browser = engine.CreateBrowser();
                              browserView.InitializeFrom(browser);
                              browser.OpenPopupHandler = new OpenPopupHandler(browserView);
                              browser?.Navigation.LoadUrl(Path.GetFullPath("popup.html"));
                          }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            engine?.Dispose();
        }
    }
}
