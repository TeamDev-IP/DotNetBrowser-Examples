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
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AgentBrowser.Portal;
using Anthropic;
using AssistantAgent;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Profile;
using DotNetBrowser.Ui;
using GuardedBrowser;

namespace AgentBrowser
{
    /// <summary>
    ///     A browser for an AI assistant that cannot leave the limits the
    ///     application sets. The assistant drives the visible BrowserView;
    ///     the user watches, takes over, and approves submissions.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEngine engine;
        private readonly IBrowser browser;
        private readonly AuditLog log;
        private readonly InputGate gate;
        private readonly BrowserActions actions;
        private AssistantSession? session;
        private CancellationTokenSource? runCancellation;

        public MainWindow()
        {
            engine = EngineFactory.Create(new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated,
                Schemes = { { PortalSchemeHandler.Scheme, new PortalSchemeHandler() } }
            }.Build());

            // A clean session: nothing inherited, nothing kept after the app closes.
            IProfile profile = engine.Profiles.Create("assistant", ProfileType.Incognito);
            browser = profile.CreateBrowser();

            log = new AuditLog();
            BrowserPolicy policy = new()
            {
                AllowedSchemes = { "dnb" },
                ReadWriteHosts = { "portal" },
                BlockedPageUrl = PortalSchemeHandler.Origin + "blocked.html"
            };
            gate = new InputGate(browser, log);
            SubmitGuard submitGuard = new(browser, gate, log);
            PolicyEnforcer enforcer = new(browser, policy, gate, submitGuard, log);
            actions = new BrowserActions(browser, policy, gate, submitGuard, log, EncodePng);

            log.Attach(browser);
            gate.Attach();
            submitGuard.Attach();
            enforcer.Attach();

            InitializeComponent();
            browserView.InitializeFrom(browser);
            log.EntryAdded += entry => Dispatcher.BeginInvoke(() =>
            {
                logList.Items.Add($"{entry.Time:HH:mm:ss} [{entry.Category}] {entry.Message}");
                logList.ScrollIntoView(logList.Items[logList.Items.Count - 1]);
            });

            browser.Navigation.LoadUrl(PortalSchemeHandler.Origin);
        }

        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            runButton.IsEnabled = false;
            gate.HandBack();
            takeOverButton.IsEnabled = true;
            handBackButton.IsEnabled = false;
            runCancellation = new CancellationTokenSource();
            try
            {
                string report = await Session.RunAsync(taskBox.Text, runCancellation.Token);
                statusText.Text = report;
            }
            catch (OperationCanceledException)
            {
                statusText.Text = "Stopped.";
            }
            catch (Exception ex)
            {
                statusText.Text = $"The assistant failed: {ex.Message}";
                log.Record("assistant", $"Error: {ex.Message}");
            }
            finally
            {
                // The run is over, so the page belongs to the user again.
                session?.Resume();
                gate.TakeOver();
                takeOverButton.IsEnabled = false;
                handBackButton.IsEnabled = false;
                runButton.IsEnabled = true;
            }
        }

        private AssistantSession Session => session ??= CreateSession();

        private AssistantSession CreateSession()
        {
            // The client reads the API key from the ANTHROPIC_API_KEY environment variable.
            return new AssistantSession(new AnthropicClient(), actions, log)
            {
                StatusChanged = status => Dispatcher.BeginInvoke(() => statusText.Text = status),
                HandOverRequested = reason => Dispatcher.BeginInvoke(() => GiveToHuman(reason)),
                ApprovalRequested = request => Dispatcher.InvokeAsync(() =>
                    new ApprovalDialog(request) { Owner = this }.ShowDialog() == true).Task
            };
        }

        private void TakeOver_Click(object sender, RoutedEventArgs e)
        {
            GiveToHuman("You have the page. Click Hand back when you are done.");
        }

        private void GiveToHuman(string message)
        {
            session?.Pause();
            gate.TakeOver();
            takeOverButton.IsEnabled = false;
            handBackButton.IsEnabled = true;
            statusText.Text = message;
        }

        private void HandBack_Click(object sender, RoutedEventArgs e)
        {
            gate.HandBack();
            takeOverButton.IsEnabled = true;
            handBackButton.IsEnabled = false;
            statusText.Text = "The assistant has the page.";
            session?.Resume();
        }

        private byte[] EncodePng(Bitmap image) => Dispatcher.Invoke(() =>
        {
            int width = (int) image.Size.Width;
            int height = (int) image.Size.Height;
            BitmapSource source = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32,
                                                      null, image.Pixels.ToArray(), width * 4);
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(source));
            using MemoryStream stream = new();
            encoder.Save(stream);
            return stream.ToArray();
        });

        private void Window_Closed(object sender, EventArgs e)
        {
            runCancellation?.Cancel();
            browser.Dispose();
            engine.Dispose();
            log.Dispose();
        }
    }
}
