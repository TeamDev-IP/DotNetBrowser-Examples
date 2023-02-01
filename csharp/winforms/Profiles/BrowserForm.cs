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

using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.WinForms;

namespace Profiles.WinForms
{
    public partial class BrowserForm : Form
    {
        private readonly BrowserView browserView;
        private IBrowser browser;

        public IBrowser Browser
        {
            get { return browser; }
            set
            {
                browser = value;
                if (browser != null)
                {
                    browserView.InitializeFrom(browser);
                    LoadUrl(AddressBar.Text);
                }
            }
        }

        public BrowserForm()
        {
            browserView = new BrowserView {Dock = DockStyle.Fill};
            InitializeComponent();
            Controls.Add(browserView);
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadUrl(AddressBar.Text);
            }
        }

        private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            browser?.Dispose();
        }

        private void LoadUrl(string address)
        {
            browser?.Navigation
                   ?.LoadUrl(address)
                    .ContinueWith(t => { UpdateControlsStates(); },
                                  TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateControlsStates()
        {
            if (!Browser.IsDisposed)
            {
                AddressBar.Text = Browser.Url;
                Text = Browser.Title;
            }
        }
    }
}
