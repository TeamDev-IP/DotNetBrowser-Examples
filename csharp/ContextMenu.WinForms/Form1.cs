﻿#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Logging;
using DotNetBrowser.SpellCheck;
using DotNetBrowser.WinForms;

namespace ContextMenu.WinForms
{
    /// <summary>
    ///     The example demonstrates how to customize a context menu
    ///     for an IBrowser instance.
    /// </summary>
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;


        public Form1()
        {
            LoggerProvider.Instance.Level = SourceLevels.Verbose;
            LoggerProvider.Instance.FileLoggingEnabled = true;
            LoggerProvider.Instance.OutputFile = "log.txt";
            BrowserView webView = new BrowserView {Dock = DockStyle.Fill};

            EngineFactory.CreateAsync(new EngineOptions.Builder
                          {
                              RenderingMode = RenderingMode.HardwareAccelerated
                          }.Build())
                         .ContinueWith(t =>
                          {
                              engine = t.Result;
                              browser = engine.CreateBrowser();
                              webView.InitializeFrom(browser);
                              // #docfragment "ContextMenu.WinForms.Configuration"
                              browser.ShowContextMenuHandler =
                                  new AsyncHandler<ShowContextMenuParameters, ShowContextMenuResponse
                                  >(ShowMenu);
                              // #enddocfragment "ContextMenu.WinForms.Configuration"

                              browser.Navigation.LoadUrl("https://www.google.com/");
                          }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
            FormClosing += Form1_FormClosing;
            Controls.Add(webView);
        }

        protected void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        // #docfragment "ContextMenu.WinForms.Implementation"
        private ToolStripItem BuildMenuItem(string item, bool isEnabled,
                                            EventHandler clickHandler)
        {
            ToolStripItem result = new ToolStripMenuItem
            {
                Text = item,
                Enabled = isEnabled
            };
            result.Click += clickHandler;

            return result;
        }

        private Task<ShowContextMenuResponse> ShowMenu(ShowContextMenuParameters parameters)
        {
            TaskCompletionSource<ShowContextMenuResponse> tcs =
                new TaskCompletionSource<ShowContextMenuResponse>();

            SpellCheckMenu spellCheckMenu = parameters.SpellCheckMenu;
            if (spellCheckMenu != null)
            {
                BeginInvoke(new Action(() =>
                {
                    ContextMenuStrip popupMenu = new ContextMenuStrip();
                    if(!string.IsNullOrEmpty(parameters.LinkText))
                    {
                        ToolStripItem buildMenuItem =
                            BuildMenuItem("Show the URL link", true,
                                          (sender, args) =>
                                          {
                                              string linkURL = parameters.LinkUrl;
                                              Console.WriteLine($"linkURL = {linkURL}");
                                              MessageBox.Show(linkURL, "URL");
                                              tcs.TrySetResult(ShowContextMenuResponse.Close());
                                          });
                        popupMenu.Items.Add(buildMenuItem);
                    }

                    ToolStripItem reloadMenuItem =
                        BuildMenuItem("Reload", true, 
                                      (sender, args) =>
                                      {
                                          Console.WriteLine("Reload current web page");
                                          browser.Navigation.Reload();
                                          tcs.TrySetResult(ShowContextMenuResponse.Close());
                                      });
                    popupMenu.Items.Add(reloadMenuItem);

                    // Close context menu when the browser requests focus back.
                    EventHandler<FocusRequestedEventArgs> onFocusRequested = null;
                    onFocusRequested = (sender, args) =>
                    {
                        BeginInvoke((Action) (() => popupMenu.Close()));
                        parameters.Browser.FocusRequested -= onFocusRequested;
                    };
                    parameters.Browser.FocusRequested += onFocusRequested;

                    // Handle the menu closed event.
                    ToolStripDropDownClosedEventHandler menuOnClosed = null;
                    menuOnClosed = (sender, args) =>
                    {
                        bool itemNotClicked =
                            args.CloseReason != ToolStripDropDownCloseReason.ItemClicked;
                        if (itemNotClicked)
                        {
                            tcs.TrySetResult(ShowContextMenuResponse.Close());
                        }

                        popupMenu.Closed -= menuOnClosed;
                    };
                    popupMenu.Closed += menuOnClosed;

                    // Show the context menu.
                    Point location = new Point(parameters.Location.X, parameters.Location.Y);
                    popupMenu.Show(this, location);
                    tcs.TrySetResult(ShowContextMenuResponse.Close());
                }));
            }
            else
            {
                tcs.TrySetResult(ShowContextMenuResponse.Close());
            }

            return tcs.Task;
        }
        // #enddocfragment "ContextMenu.WinForms.Implementation"
    }
}
