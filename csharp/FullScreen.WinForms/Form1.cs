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

using System;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;

namespace FullScreen.WinForms
{
    /// <summary>
    ///     The example demonstrates how to implement custom full-screen handling.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly IBrowser browser;
        private readonly BrowserView browserView;
        private readonly IEngine engine;
        private Form fullScreenForm;

        #region Constructors

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
            browser.FullScreenEntered += OnFullScreenEntered;
            browser.FullScreenExited += OnFullScreenExited;
            browserView.InitializeFrom(browser);
            browser.Navigation.LoadUrl("http://www.w3.org/2010/05/video/mediaevents.html");
            Controls.Add(browserView);
        }

        #endregion

        #region Methods

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private void OnFullScreenEntered(object sender, FullScreenEnteredEventArgs e)
        {
            BeginInvoke((Action) (() =>
                                     {
                                         fullScreenForm = new Form();
                                         fullScreenForm.TopMost = true;
                                         fullScreenForm.ShowInTaskbar = false;
                                         fullScreenForm.FormBorderStyle = FormBorderStyle.None;
                                         fullScreenForm.WindowState = FormWindowState.Maximized;
                                         fullScreenForm.Owner = this;

                                         Controls.Remove(browserView);
                                         fullScreenForm.Controls.Add(browserView);
                                         fullScreenForm.Show();
                                     }));
        }

        private void OnFullScreenExited(object sender, FullScreenExitedEventArgs e)
        {
            BeginInvoke((Action) (() =>
                                     {
                                         fullScreenForm.Controls.Clear();
                                         Controls.Add(browserView);
                                         fullScreenForm.Hide();
                                         fullScreenForm.Close();
                                     }));
        }

        #endregion
    }
}