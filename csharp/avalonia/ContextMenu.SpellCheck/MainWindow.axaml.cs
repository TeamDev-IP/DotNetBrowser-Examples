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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.SpellCheck;

namespace ContextMenu.SpellCheck
{
    /// <summary>
    ///     The example demonstrates how to create a context menu with the SpellChecker functionality.
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
                              byte[] htmlBytes = Encoding.UTF8.GetBytes(@"<html>
                                    <head>
                                      <meta charset='UTF-8'>
                                    </head>
                                    <body>
                                    <textarea autofocus cols='30' rows='20'>Simpple mistakee</textarea>
                                    </body>
                                    </html>");
                              browser.Navigation
                                     .LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}");
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

            SpellCheckMenu spellCheckMenu = parameters.SpellCheckMenu;
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Avalonia.Controls.ContextMenu? cm = new();
                cm.Placement = PlacementMode.Pointer;
                Point point = new Point(parameters.Location.X, parameters.Location.Y);
                cm.PlacementRect = new Rect(point, new Size(1, 1));

                IEnumerable<string> suggestions = spellCheckMenu.DictionarySuggestions;
                if (suggestions != null)
                {
                    // Add menu items with suggestions.
                    foreach (string suggestion in suggestions)
                    {
                        MenuItem menuItem =
                            BuildMenuItem(suggestion, true, true,
                                          delegate
                                          {
                                              browser.ReplaceMisspelledWord(suggestion);
                                              tcs.TrySetResult(ShowContextMenuResponse
                                                 .Close());
                                          });
                        cm.Items.Add(menuItem);
                    }
                }

                // Add "Add to Dictionary" menu item.
                string addToDictionary =
                    spellCheckMenu.AddToDictionaryMenuItemText ?? "Add to Dictionary";

                cm.Items.Add(BuildMenuItem(addToDictionary, true, true, delegate
                {
                    if (!string.IsNullOrWhiteSpace(spellCheckMenu.MisspelledWord))
                    {
                        engine.Profiles.Default.SpellChecker?.CustomDictionary
                             ?.Add(spellCheckMenu.MisspelledWord);
                    }

                    tcs.TrySetResult(ShowContextMenuResponse.Close());
                }));

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