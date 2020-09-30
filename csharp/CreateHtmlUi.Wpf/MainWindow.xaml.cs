#region Copyright

// Copyright 2020, TeamDev. All rights reserved.
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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Dom.Events;
using DotNetBrowser.Engine;
using DotNetBrowser.Navigation.Events;
using IInputElement = DotNetBrowser.Dom.IInputElement;

namespace CreateHtmlUi.Wpf
{
    /// <summary>
    ///    The sample demonstrates how to create a custom HTML UI using DotNetBrowser
    ///    and debug it using DevTools.
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser browser1;
        private IBrowser browser2;
        private IEngine engine;

        #region Constructors

        public MainWindow()
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

                     browser1.Navigation.FrameLoadFinished += browser1_FrameLoadFinished;
                     browser1.Navigation.LoadUrl(Path.GetFullPath("UI.html"));
                     browser2.Navigation.LoadUrl(browser1.DevTools.RemoteDebuggingUrl);
                 }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        #endregion

        #region Methods

        private void browser1_FrameLoadFinished(object sender, FrameLoadFinishedEventArgs e)
        {
            if (!e.Frame.IsDisposed && e.Frame.IsMain)
            {
                IDocument document = browser1.MainFrame.Document;
                IEnumerable<IElement> inputs = document.GetElementsByTagName("input");
                foreach (IElement element in inputs)
                {
                    if (element.Attributes["type"].ToLower().Equals("submit"))
                    {
                        element.Events.Click += OnSubmitClicked;
                    }
                }
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            engine.Dispose();
        }

        private void OnSubmitClicked(object sender, DomEventArgs e)
        {
            Task.Run(() =>
            {
                string login = string.Empty;
                string password = string.Empty;

                IDocument document = browser1.MainFrame.Document;

                login = ((IInputElement) document.GetElementById("login")).Value;
                password = ((IInputElement) document.GetElementById("password")).Value;

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show(this,
                                    "Login: "
                                    + login
                                    + "\nPassword: "
                                    + password, "Data");
                }));
            });
        }

        #endregion
    }
}