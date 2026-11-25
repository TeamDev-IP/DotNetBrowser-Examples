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
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Capture;
using DotNetBrowser.Capture.Events;
using DotNetBrowser.Capture.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation.Events;

namespace ScreenSharing
{
    /// <summary>
    ///     This example demonstrates how to handle WebRTC screen sharing requests:
    ///     the app shows its own source picker, starts the session, and stops it.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Must match the message in share.html.
        private const string TrackEndedMessage = "screen-sharing:track-ended";

        private readonly IEngine engine;
        private readonly IBrowser browser;
        private ISession session;

        public MainWindow()
        {
            engine = EngineFactory.Create(new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated
            }.Build());
            browser = engine.CreateBrowser();

            // The page asks to share the screen; the handler decides what.
            browser.Capture.StartSessionHandler =
                new Handler<StartSessionParameters, StartSessionResponse>(OnStartSession);
            browser.Capture.SessionStarted += OnSessionStarted;

            // In DotNetBrowser 4.5.0, the session doesn't raise Stopped when the
            // user stops sharing in the Chromium notification, so the page
            // reports the end of its video track in the console.
            browser.ConsoleMessageReceived += OnConsoleMessageReceived;

            // The same applies when the user reloads the page while sharing.
            browser.Navigation.NavigationFinished += OnNavigationFinished;

            InitializeComponent();
            browserView.InitializeFrom(browser);

            string page = Path.Combine(AppContext.BaseDirectory, "share.html");
            browser.Navigation.LoadUrl(page);
        }

        private StartSessionResponse OnStartSession(StartSessionParameters parameters)
        {
            // Always return a response. If the handler throws, the page gets an
            // error, and the browser doesn't call the handler again.
            try
            {
                // The handler is called on a background thread: show the
                // picker on the UI thread and wait for the user's choice.
                return Dispatcher.Invoke(() =>
                {
                    // One session at a time, so the Stop button stops
                    // everything that is shared.
                    if (session != null)
                    {
                        return StartSessionResponse.Cancel();
                    }

                    SourcePicker picker = new SourcePicker(parameters) { Owner = this };
                    picker.ShowDialog();
                    return picker.Response;
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return StartSessionResponse.Cancel();
            }
        }

        private void OnSessionStarted(object sender, SessionStartedEventArgs e)
        {
            e.Session.Stopped += OnSessionStopped;
            Dispatcher.BeginInvoke(() =>
            {
                session = e.Session;
                stopButton.IsEnabled = true;
                statusText.Text = $"Sharing \"{e.Session.Source.Name}\".";
            });
        }

        private void OnSessionStopped(object sender, SessionStoppedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (session == e.Session)
                {
                    ClearSession();
                }
            });
        }

        private void OnConsoleMessageReceived(object sender,
                                              ConsoleMessageReceivedEventArgs e)
        {
            // Any script in the page can write this message. The example loads
            // only its own page.
            if (e.Message == TrackEndedMessage)
            {
                // Don't call Stop() on this session: in DotNetBrowser 4.5.0, it
                // still reports IsActive, and Stop() on it breaks the browser.
                Dispatcher.BeginInvoke(() => ClearSession());
            }
        }

        private void OnNavigationFinished(object sender, NavigationFinishedEventArgs e)
        {
            // A new document in the main frame ends the capture.
            if (e.HasCommitted && !e.IsSameDocument && e.Frame.IsMain)
            {
                Dispatcher.BeginInvoke(() => ClearSession());
            }
        }

        private void ClearSession()
        {
            if (session == null)
            {
                return;
            }
            session = null;
            stopButton.IsEnabled = false;
            statusText.Text = "Sharing stopped.";
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Stops the capture; the page's video track ends.
            session?.Stop();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
