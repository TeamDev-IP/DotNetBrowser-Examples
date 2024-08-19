#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DotNetBrowser.Browser;
using DotNetBrowser.Cast;
using DotNetBrowser.Engine;

namespace Chromecast.Wpf
{
    ///<summary>
    ///    The sample demonstrates how to use Chromecast functionality in DotNetBrowser.
    ///</summary>
    public partial class MainWindow : Window
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;
        private ICastSession castSession;

        public ObservableCollection<Receiver> Items { get; set; } =
            new ObservableCollection<Receiver>();

        public MainWindow()
        {
            InitializeComponent();

            EngineOptions engineOptions = new EngineOptions.Builder
            {
                MediaRoutingEnabled = true
            }.Build();

            engine = EngineFactory.Create(engineOptions);
            browser = engine.CreateBrowser();
            browserView.InitializeFrom(browser);

            browser.Navigation.NavigationFinished += (sender, args) =>
            {
                Dispatcher.BeginInvoke(new Action(() => navigationBar.Text =
                                                            browser.Url));
            };

            browser.Navigation.LoadUrl("https://youtube.com");
        }

        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            if (castSession != null)
            {
                castSession.Stop();
                castSession = null;
                receiversBox.SelectedItem = null;
            }

            browser.Navigation.LoadUrl(navigationBar.Text);
        }

        private void Receivers_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                receiversBox.ItemsSource = null;
                Items.Clear();
                browser.Profile.MediaCasting.Receivers.Refresh();

                IReadOnlyList<IMediaReceiver> mediaReceivers
                    = browser.Profile.MediaCasting.Receivers.AllAvailable;

                foreach (IMediaReceiver receiver in mediaReceivers)
                {
                    Items.Add(new Receiver
                    {
                        MediaReceiver = receiver,
                        Name = receiver.Name
                    });
                }

                receiversBox.ItemsSource = Items;
            }));
        }

        private async void ReceiversBox_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (castSession != null)
            {
                try
                {
                    castSession.Stop();
                }
                catch (ObjectDisposedException)
                {
                }

                castSession = null;
            }

            if (receiversBox.SelectedItem != null)
            {
                Receiver selectedReceiver = receiversBox.SelectedItem as Receiver;
                try
                {
                    castSession =
                        await browser.Cast.CastContent(selectedReceiver?.MediaReceiver);
                }
                catch (CastSessionStartFailedException ex)
                {
                    MessageBox.Show(this, ex.Message, "CastSessionStartFailedException");
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            engine.Dispose();
        }
    }
}