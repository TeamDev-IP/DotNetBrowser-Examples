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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Logging;
using DotNetBrowser.Net;
using DotNetBrowser.Permissions.Handlers;
using DotNetBrowser.WinForms.Dialogs;

namespace DotNetBrowser.WinForms.Demo
{
    /// <summary>
    ///     The Demo WinForms sample with a basic functionality
    ///     for testing purposes.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Properties

        private IEngine Engine { get; }
        private RenderingMode RenderingMode { get; set; }

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            Closed += MainForm_Closed;

            Engine = CreateEngine();

            tabbedPane.RenderingMode = RenderingMode;
            tabbedPane.Engine = Engine;

            tabbedPane.SelectedTab.Contents.renderingMode.Text = RenderingMode.ToString();

            Task.Run(() => Engine?.CreateBrowser())
                .ContinueWith(t =>
                              {
                                  IBrowser browser = t.Result;
                                  tabbedPane.SelectedTab.Contents.Browser = browser;
                                  browser.Focus();
                              },
                              TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region Methods

        private IEngine CreateEngine()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            RenderingMode = RenderingMode.HardwareAccelerated;
            if (arguments.FirstOrDefault(arg => arg.ToLower().Contains("lightweight")) != null)
            {
                RenderingMode = RenderingMode.OffScreen;
            }
            if (arguments.FirstOrDefault(arg => arg.ToLower().Contains("enable-file-log")) != null)
            {
                LoggerProvider.Instance.Level = SourceLevels.Verbose;
                LoggerProvider.Instance.FileLoggingEnabled = true;
                string logFile = $"DotNetBrowser-WinForms-{Guid.NewGuid()}.log";
                LoggerProvider.Instance.OutputFile = Path.GetFullPath(logFile);
            }

            try
            {
                IEngine engine = EngineFactory.Create(new EngineOptions.Builder
                {
                    RenderingMode = RenderingMode,
                    Schemes =
                    {
                        { Scheme.Http, new WinFormsInterceptRequestHandler() }
                    }
                }.Build());
                engine.Profiles.Default.Network.AuthenticateHandler = new DefaultAuthenticationHandler(this);
                engine.Profiles.Default.Permissions.RequestPermissionHandler =
                    new Handler<RequestPermissionParameters, RequestPermissionResponse>(p => RequestPermissionResponse.Grant());
                engine.Disposed += (sender, args) =>
                {
                    if (args.ExitCode != 0)
                    {
                        string message = $"The Chromium engine exit code was {args.ExitCode:x8}";
                        Trace.WriteLine(message);
                        MessageBox.Show(message,
                                        "DotNetBrowser Warning", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    }
                };
                return engine;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                MessageBox.Show(e.Message, "DotNetBrowser Initialization Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return null;
            }
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            Engine?.Dispose();
        }

        #endregion
    }
}