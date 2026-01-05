#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Extensions.Handlers;
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
        private IEngine Engine { get; }
        private RenderingMode RenderingMode { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Closed += MainForm_Closed;

            Engine = CreateEngine();
            if (Engine == null)
            {
                Environment.Exit(0);
            }

            tabbedPane.RenderingMode = RenderingMode;
            tabbedPane.Engine = Engine;

            tabbedPane.SelectedTab.Contents.renderingMode.Text = RenderingMode.ToString();

            IBrowser browser = Engine?.CreateBrowser();
            tabbedPane.SelectedTab.Contents.Browser = browser;
            browser.Focus();
        }

        private IEngine CreateEngine()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            RenderingMode = RenderingMode.HardwareAccelerated;
            ProprietaryFeatures proprietaryFeatures = ProprietaryFeatures.None;
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

            if (arguments.FirstOrDefault(arg => arg.ToLower().Contains("proprietary")) != null)
            {
                proprietaryFeatures = ProprietaryFeatures.Aac
                                      | ProprietaryFeatures.H264
                                      | ProprietaryFeatures.Hevc
                                      | ProprietaryFeatures.Widevine;
            }

            try
            {
                IEngine engine = EngineFactory.Create(new EngineOptions.Builder
                {
                    RenderingMode = RenderingMode,
                    ProprietaryFeatures = proprietaryFeatures,
                    Schemes =
                    {
                        {Scheme.Http, new WinFormsInterceptRequestHandler()}
                    },
                    ChromiumSwitches = {"--force-renderer-accessibility"}
                }.Build());

                engine.Profiles.Default.Network.AuthenticateHandler = new DefaultAuthenticationHandler(this);
                engine.Profiles.Default.Permissions.RequestPermissionHandler =
                    new Handler<RequestPermissionParameters,
                        RequestPermissionResponse>(p => RequestPermissionResponse.Grant());
                engine.Profiles.Default.Extensions.InstallExtensionHandler =
                    new Handler<InstallExtensionParameters, InstallExtensionResponse>(p => InstallExtensionResponse
                       .Install);
                engine.Profiles.Default.Extensions.UninstallExtensionHandler =
                    new Handler<UninstallExtensionParameters, UninstallExtensionResponse>(p => UninstallExtensionResponse
                       .Uninstall);
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
            catch (NoLicenseException)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Thank you for trying DotNetBrowser in Windows Forms.")
                   .AppendLine()
                   .AppendLine("To run the demo, get the free trial license and insert it below:");

                return ShowLicenseDialog(msg.ToString()) ? CreateEngine() : null;
            }
            catch (InvalidLicenseException)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("The license key did not work. It may have an incorrect format, or is not suitable for this DotNetBrowser version.")
                   .AppendLine()
                   .AppendLine("Please enter the license key:");

                return ShowLicenseDialog(msg.ToString()) ? CreateEngine() : null;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                MessageBox.Show(e.Message, "DotNetBrowser Initialization Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return null;
            }
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            Engine?.Dispose();
        }

        private bool ShowLicenseDialog(string message)
        {
            LicenseDialog dialog = new LicenseDialog(message);
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }

            string license = dialog.License;
            if (string.IsNullOrWhiteSpace(license))
            {
                return false;
            }

            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string path = Path.Combine(directory, "dotnetbrowser.license");
            File.WriteAllText(Path.GetFullPath(path), license);
            return true;
        }
    }
}
