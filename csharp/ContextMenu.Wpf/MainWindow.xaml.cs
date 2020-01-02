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
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DotNetBrowser.Browser;
using DotNetBrowser.ContextMenu;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;

namespace ContextMenu.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser browser;
        private IEngine engine;

        #region Constructors

        public MainWindow()
        {
            Task.Run(() =>
                {
                    engine = EngineFactory.Create(new EngineOptions.Builder {RenderingMode = RenderingMode.OffScreen}
                                                      .Build());
                    browser = engine.CreateBrowser();
                })
                .ContinueWith(t =>
                {
                    webView.InitializeFrom(browser);
                    browser.ContextMenuHandler = new AsyncHandler<ContextMenuParameters, ContextMenuResponse>(ShowMenu);
                    browser.Navigation.LoadUrl("https://www.google.com/");
                }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        #endregion

        #region Methods

        private MenuItem BuildMenuItem(string item, bool isEnabled, Visibility IsVisible,
                                       RoutedEventHandler clickHandler)
        {
            MenuItem result = new MenuItem();
            result.Header = item;
            result.Visibility = Visibility.Collapsed;
            result.Visibility = IsVisible;
            result.IsEnabled = isEnabled;
            result.Click += clickHandler;

            return result;
        }

        private Task<ContextMenuResponse> ShowMenu(ContextMenuParameters parameters)
        {
            TaskCompletionSource<ContextMenuResponse> tcs = new TaskCompletionSource<ContextMenuResponse>();
            webView.Dispatcher?.BeginInvoke(new Action(() =>
            {
                System.Windows.Controls.ContextMenu popupMenu = new System.Windows.Controls.ContextMenu();

                if (!string.IsNullOrEmpty(parameters.LinkText))
                {
                    popupMenu.Items.Add(BuildMenuItem("Open link in new window", true, Visibility.Visible, delegate
                    {
                        string linkURL = parameters.LinkUrl;
                        Console.WriteLine($"linkURL = {linkURL}");
                        tcs.TrySetResult(ContextMenuResponse.Close());
                    }));
                }

                popupMenu.Items.Add(BuildMenuItem("Reload", true, Visibility.Visible, delegate
                {
                    Console.WriteLine("Reload current web page");
                    browser.Navigation.Reload();
                    tcs.TrySetResult(ContextMenuResponse.Close());
                }));
                popupMenu.Closed += (sender, args) => { tcs.TrySetResult(ContextMenuResponse.Close()); };
                popupMenu.IsOpen = true;
            }));
            return tcs.Task;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }

        #endregion
    }
}