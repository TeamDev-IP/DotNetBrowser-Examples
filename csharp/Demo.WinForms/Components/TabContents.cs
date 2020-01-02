#region Copyright

// Copyright © 2020, TeamDev. All rights reserved.
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation.Events;

namespace DotNetBrowser.WinForms.Demo.Components
{
    public partial class TabContents : UserControl
    {
        private const string PngFilter = "PNG image (*.png)|*.png";
        private IBrowser browser;
        private string title;

        #region Properties

        public IBrowser Browser
        {
            get => browser;
            set
            {
                browser = value;
                if (browser != null)
                {
                    browserView.InitializeFrom(browser);
                    browser.TitleChanged += Browser_TitleChanged;
                    browser.StatusChanged += Browser_StatusChanged;
                    browser.Navigation.FrameLoadFinished += Navigation_FrameLoadFinished;
                    browser.PrintHandler = new Handler<PrintParameters, PrintStatus>(p => PrintStatus.ShowPrintPreview);
                    browser.ContextMenuHandler = browserView.ContextMenuHandler;
                    LoadUrl(AddressBar.Text);
                }
            }
        }

        public string Title
        {
            get => title;
            private set
            {
                title = value;
                TitleChanged?.Invoke(this, title);
            }
        }

        #endregion

        #region Events

        public event EventHandler Closed;
        public event EventHandler<string> TitleChanged;

        #endregion

        #region Constructors

        public TabContents()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public void CloseTab(bool raiseClosedEvent = false)
        {
            Browser?.Dispose();
            if (raiseClosedEvent)
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadUrl(AddressBar.Text);
            }
        }

        private void adobeFlashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("https://helpx.adobe.com/flash-player.html");
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Browser?.Navigation.GoBack();
        }

        private void Browser_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            BeginInvoke((Action) (() => { Status.Text = e.Text; }));
        }

        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            BeginInvoke((Action) (() => { Title = e.Title; }));
        }

        private void cssCursorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("https://developer.mozilla.org/en-US/docs/Web/CSS/cursor");
        }

        private void downloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://cloud.teamdev.com/downloads/dotnetbrowser/1.20/dotnetbrowser-1.20.zip");
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            Browser?.Navigation.GoForward();
        }

        private void googleMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://maps.google.com");
        }

        private void hideScrollbarsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (Browser != null)
            {
                Browser.Settings.ScrollbarsHidden = hideScrollbarsToolStripMenuItem.Checked;
            }
        }

        private void hTML5VideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://www.w3.org/2010/05/video/mediaevents.html");
        }

        private void javaScriptConsoleToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            jsConsoleLayoutPanel.Visible = javaScriptConsoleToolStripMenuItem.Checked;
        }

        private void javaScriptDialogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://www.javascripter.net/faq/alert.htm");
        }

        private void jsConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                jsConsoleOutput.Text += ">> " + jsConsoleInput.Text + Environment.NewLine;
                Browser?.MainFrame?.ExecuteJavaScript(jsConsoleInput.Text)
                       .ContinueWith(t =>
                       {
                           jsConsoleOutput.AppendText("<< " + t.Result + Environment.NewLine);
                           jsConsoleOutput.ScrollToCaret();
                           jsConsoleInput.Clear();
                       }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void LoadUrl(string url)
        {
            Browser?.Navigation.LoadUrl(url)
                   .ContinueWith(t => { UpdateControlsStates(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip.Show(menuButton, new Point(0, menuButton.Height));
        }

        private void Navigation_FrameLoadFinished(object sender, FrameLoadFinishedEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                BeginInvoke((Action) UpdateControlsStates);
            }
        }

        private void pDFViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://www.orimi.com/pdf-test.pdf");
        }

        private void popupWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://www.popuptest.com/");
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Browser?.MainFrame?.Print();
        }

        private void selectOptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("https://www.w3schools.com/tags/tryit.asp?filename=tryhtml_option");
        }

        private void takeScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog {Filter = PngFilter};
            if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                Bitmap bmp = browser.CreateBrowserImage().ToBitmap();
                bmp.Save(dialog.FileName, ImageFormat.Png);
            }
        }

        private void UpdateControlsStates()
        {
            AddressBar.Text = browser.Url;
            Title = browser.Title;
            BackButton.Enabled = browser.Navigation.CanGoBack();
            ForwardButton.Enabled = browser.Navigation.CanGoForward();
        }

        private void uploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadUrl("http://jkorpela.fi/forms/file.html#example");
        }

        #endregion
    }
}