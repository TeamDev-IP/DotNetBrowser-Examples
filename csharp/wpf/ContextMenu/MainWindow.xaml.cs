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

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;

namespace ContextMenu.Wpf
{
    /// <summary>
    ///     The sample demonstrates how to customize a context menu
    ///     for an IBrowser instance.
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser browser;
        private IEngine engine;


        public MainWindow()
        {
            EngineFactory.CreateAsync(new EngineOptions.Builder
                          {
                              RenderingMode = RenderingMode.OffScreen
                          }.Build())
                         .ContinueWith(t =>
                          {
                              engine = t.Result;
                              browser = engine.CreateBrowser();
                              WebView.InitializeFrom(browser);
                              ConfigureContextMenu();
                              browser.Navigation.LoadUrl("https://www.google.com/");
                          }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        private MenuItem BuildMenuItem(string item, bool isEnabled, Visibility isVisible,
                                       RoutedEventHandler clickHandler)
        {
            MenuItem result = new MenuItem
            {
                Header = item,
                Visibility = Visibility.Collapsed
            };
            result.Visibility = isVisible;
            result.IsEnabled = isEnabled;
            result.Click += clickHandler;

            return result;
        }

        private void ConfigureContextMenu()
        {
            // #docfragment "ContextMenu.Configuration"
            browser.ShowContextMenuHandler =
                new AsyncHandler<ShowContextMenuParameters, 
                                 ShowContextMenuResponse>(ShowContextMenu);
            // #enddocfragment "ContextMenu.Configuration"
        }

        // #docfragment "ContextMenu.Implementation"
        private Task<ShowContextMenuResponse> ShowContextMenu(
            ShowContextMenuParameters parameters)
        {
            TaskCompletionSource<ShowContextMenuResponse> tcs =
                new TaskCompletionSource<ShowContextMenuResponse>();

            WebView.Dispatcher?.BeginInvoke(new Action(() =>
            {
                System.Windows.Controls.ContextMenu popupMenu =
                    new System.Windows.Controls.ContextMenu();

                if (!string.IsNullOrEmpty(parameters.LinkText))
                {
                    MenuItem buildMenuItem =
                        BuildMenuItem("Show the URL link", true,
                                      Visibility.Visible,
                                      (sender, args) =>
                                      {
                                          string linkURL = parameters.LinkUrl;
                                          Console.WriteLine($"linkURL = {linkURL}");
                                          MessageBox.Show(linkURL, "URL");
                                          tcs.TrySetResult(ShowContextMenuResponse.Close());
                                      });
                    popupMenu.Items.Add(buildMenuItem);
                }

                MenuItem reloadMenuItem =
                    BuildMenuItem("Reload", true, Visibility.Visible,
                                  (sender, args) =>
                                  {
                                      Console.WriteLine("Reload current web page");
                                      browser.Navigation.Reload();
                                      tcs.TrySetResult(ShowContextMenuResponse.Close());
                                  });
                popupMenu.Items.Add(reloadMenuItem);

                popupMenu.Closed += (sender, args) =>
                {
                    tcs.TrySetResult(ShowContextMenuResponse.Close());
                };

                popupMenu.IsOpen = true;
            }));

            return tcs.Task;
        }
        // #enddocfragment "ContextMenu.Implementation"

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
