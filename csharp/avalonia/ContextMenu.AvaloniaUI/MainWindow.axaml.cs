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
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using MsBox.Avalonia;

namespace ContextMenu.AvaloniaUI
{
    /// <summary>
    ///     The sample demonstrates how to customize a context menu
    ///     for an IBrowser instance.
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser? browser;
        private IEngine? engine;


        public MainWindow()
        {
            EngineFactory.CreateAsync(RenderingMode.OffScreen)
                         .ContinueWith(t =>
                          {
                              engine = t.Result;
                              browser = engine.CreateBrowser();
                              BrowserView.InitializeFrom(browser);
                              ConfigureContextMenu();
                              browser.Navigation.LoadUrl("https://www.google.com/");
                          }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        private MenuItem BuildMenuItem(string item, bool isEnabled, bool isVisible,
                                       EventHandler<RoutedEventArgs> clickHandler)
        {
            MenuItem result = new MenuItem
            {
                Header = item,
                IsVisible = false
            };
            result.IsVisible = isVisible;
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

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Avalonia.Controls.ContextMenu? cm = new();
                cm.Placement = PlacementMode.Pointer;
                Point point = new Point(parameters.Location.X, parameters.Location.Y);
                cm.PlacementRect = new Rect(point, new Size(1, 1));

                if (!string.IsNullOrEmpty(parameters.LinkText))
                {
                    MenuItem buildMenuItem =
                        BuildMenuItem("Show the URL link", true, true,
                                      async (sender, args) =>
                                      {
                                          string linkURL = parameters.LinkUrl;
                                          Console.WriteLine($"linkURL = {linkURL}");
                                          var box =
                                              MessageBoxManager
                                                 .GetMessageBoxStandard("URL", linkURL);
                                          var result = await box.ShowAsync();
                                          tcs.TrySetResult(ShowContextMenuResponse.Close());
                                      });
                    cm.Items.Add(buildMenuItem);
                }

                MenuItem reloadMenuItem =
                    BuildMenuItem("Reload", true, true,
                                  (sender, args) =>
                                  {
                                      Console.WriteLine("Reload current web page");
                                      browser.Navigation.Reload();
                                      tcs.TrySetResult(ShowContextMenuResponse.Close());
                                  });
                cm.Items.Add(reloadMenuItem);

                cm.Closed += (s, a) => tcs.TrySetResult(ShowContextMenuResponse.Close());
                cm.Open(BrowserView);
            });

            return tcs.Task;
        }
        // #enddocfragment "ContextMenu.Implementation"

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}