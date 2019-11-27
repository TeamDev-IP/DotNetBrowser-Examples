#region Copyright

// Copyright 2019, TeamDev. All rights reserved.
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
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.ContextMenu;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Logging;
using DotNetBrowser.SpellCheck;
using DotNetBrowser.WinForms;

namespace WinForms.ContextMenuSample
{
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;
        private readonly BrowserView webView;

        #region Constructors

        public Form1()
        {
            LoggerProvider.Instance.Level = SourceLevels.Verbose;
            LoggerProvider.Instance.FileLoggingEnabled = true;
            LoggerProvider.Instance.OutputFile = "C:\\log.txt";
            webView = new BrowserView {Dock = DockStyle.Fill};
            Task.Run(() =>
                 {
                     engine = EngineFactory.Create(new EngineOptions.Builder
                                                           {RenderingMode = RenderingMode.HardwareAccelerated}
                                                      .Build());
                     browser = engine.CreateBrowser();
                 })
                .ContinueWith(t =>
                 {
                     webView.InitializeFrom(browser);
                     browser.ContextMenuHandler =
                         new AsyncHandler<ContextMenuParameters, ContextMenuResponse>(ShowMenu);
                     browser.MainFrame.LoadHtml(@"<html>
<head>
  <meta charset='UTF-8'>
</head>
<body>
<textarea autofocus cols='30' rows='20'>Simpple mistakee</textarea>
</body>
</html>");
                 }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
            FormClosing += Form1_FormClosing;
            Controls.Add(webView);
        }

        #endregion

        #region Methods

        private MenuItem BuildMenuItem(string item, bool isEnabled,
                                       EventHandler clickHandler)
        {
            MenuItem result = new MenuItem();
            result.Text = item;
            result.Enabled = isEnabled;
            result.Click += clickHandler;

            return result;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private Task<ContextMenuResponse> ShowMenu(ContextMenuParameters parameters)
        {
            TaskCompletionSource<ContextMenuResponse> tcs = new TaskCompletionSource<ContextMenuResponse>();
            SpellCheckMenu spellCheckMenu = parameters.SpellCheckMenu;
            if (spellCheckMenu != null)
            {
                BeginInvoke(new Action(() =>
                {
                    ContextMenu popupMenu = new ContextMenu();
                    IEnumerable<string> suggestions = spellCheckMenu.DictionarySuggestions;
                    if (suggestions != null)
                    {
                        foreach (string suggestion in suggestions)
                        {
                            popupMenu.MenuItems.Add(BuildMenuItem(suggestion, true, delegate
                            {
                                browser.ReplaceMisspelledWord(suggestion);
                                tcs.TrySetResult(ContextMenuResponse.Close());
                            }));
                        }
                    }

                    string addToDictionary = spellCheckMenu.AddToDictionaryMenuItemText ?? "Add to Dictionary";
                    popupMenu.MenuItems.Add(BuildMenuItem(addToDictionary, true, delegate
                    {
                        engine.SpellCheckService?.CustomDictionary?.Add(spellCheckMenu.MisspelledWord);
                        tcs.TrySetResult(ContextMenuResponse.Close());
                    }));
                    popupMenu.Collapse += (sender, args) => { tcs.TrySetResult(ContextMenuResponse.Close()); };

                    Point location = new Point(parameters.Location.X, parameters.Location.Y);
                    popupMenu.Show(this, location);
                    tcs.TrySetResult(ContextMenuResponse.Close());
                }));
            }
            else
            {
                tcs.TrySetResult(ContextMenuResponse.Close());
            }

            return tcs.Task;
        }

        #endregion
    }
}