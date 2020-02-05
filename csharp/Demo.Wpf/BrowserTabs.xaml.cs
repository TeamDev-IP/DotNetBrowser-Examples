#region Copyright

// Copyright 2020, TeamDev. All rights reserved.
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DotNetBrowser.Engine;
using DotNetBrowser.Logging;
using DotNetBrowser.Wpf.Dialogs;

namespace Demo.Wpf
{
    /// <inheritdoc cref="TabControl" />
    /// <summary>
    ///     Interaction logic for BrowserTabs.xaml
    /// </summary>
    public partial class BrowserTabs
    {
        private const int PaddingPerTab = 42;
        private const int DefaultPadding = 37;
        private IEngine engine;
        private RenderingMode renderingMode;

        #region Constructors

        public BrowserTabs()
        {
            InitializeComponent();
            CreateEngine();
            if (GetFirstTab() is BrowserTab tab)
            {
                tab.Browser = engine?.CreateBrowser();
                tab.RenderingModeStatus.Text = $"Mode: {renderingMode}";
            }
        }

        #endregion

        #region Methods

        public void DisposeEngine()
        {
            engine?.Dispose();
        }

        private void CreateEngine()
        {
            string[] arguments = Environment.GetCommandLineArgs();
            renderingMode = RenderingMode.HardwareAccelerated;
            if (arguments.FirstOrDefault(arg => arg.ToLower().Contains("lightweight")) != null)
            {
                renderingMode = RenderingMode.OffScreen;
            }
            if (arguments.FirstOrDefault(arg => arg.ToLower().Contains("enable-file-log")) != null)
            {
                LoggerProvider.Instance.Level = SourceLevels.Verbose;
                LoggerProvider.Instance.FileLoggingEnabled = true;
                string logFile = $"DotNetBrowser-WPF-{Guid.NewGuid()}.log";
                LoggerProvider.Instance.OutputFile = System.IO.Path.GetFullPath(logFile);
            }
            try
            {
                engine = EngineFactory.Create(new EngineOptions.Builder
                {
                    RenderingMode = renderingMode
                }.Build());
                engine.Downloads.StartDownloadHandler = new DefaultStartDownloadHandler(this);
                engine.Network.AuthenticateHandler = new DefaultAuthenticationHandler(this);
                engine.Disposed += (sender, args) =>
                {
                    if (args.ExitCode != 0)
                    {
                        string message = $"The Chromium engine exit code was {args.ExitCode:x8}";
                        Trace.WriteLine(message);
                        MessageBox.Show(message,
                                        "DotNetBrowser Warning", MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                };
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                MessageBox.Show(e.Message, "DotNetBrowser Initialization Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private TabItem GetFirstTab() => Items[0] as TabItem;

        private bool HasTabs() => !Equals(GetFirstTab(), NewButtonTab);

        private void OnAddTabButtonClicked(object sender, RoutedEventArgs e)
        {
            BrowserTab browserTab = new BrowserTab {Browser = engine?.CreateBrowser()};
            browserTab.RenderingModeStatus.Text = $"Mode: {renderingMode}";
            browserTab.Closed += OnBrowserTabClosed;
            Items.Insert(Items.Count - 1, browserTab);
            browserTab.IsSelected = true;
            UpdateTabsWidths();
        }

        private void OnBrowserTabClosed(object sender, EventArgs e)
        {
            if (sender is BrowserTab tab)
            {
                if (tab.IsSelected)
                {
                    TabItem firstTab = GetFirstTab();
                    firstTab.IsSelected = true;
                }
                Items.Remove(tab);

                if (!HasTabs())
                {
                    Application.Current.Dispatcher.Invoke(() => { Window.GetWindow(this)?.Close(); });
                }
            }
        }

        private void UpdateTabsWidths()
        {
            void UpdateTabsWidthsAction()
            {
                int browserTabsCount = Items.Count - 1;
                int paddings = browserTabsCount * PaddingPerTab + DefaultPadding;
                int browserTabWidth = ((int) ActualWidth - paddings)
                                      / browserTabsCount;

                browserTabWidth = browserTabWidth > 0 ? browserTabWidth : 0;

                foreach (TabItem tab in Items)
                {
                    if (tab is BrowserTab browserTab)
                    {
                        browserTab.Title.Width = browserTabWidth;
                    }
                }
            }

            Application.Current.Dispatcher.BeginInvoke((Action) UpdateTabsWidthsAction);
        }

        #endregion
    }
}