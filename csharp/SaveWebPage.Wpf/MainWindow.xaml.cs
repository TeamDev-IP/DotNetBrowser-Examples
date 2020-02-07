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

using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Navigation;
using DotNetBrowser.Wpf;

namespace WPF.SaveWebPage
{
    /// <summary>
    ///     Demonstrates how to embed WPF BrowserView component into WPF Application,
    ///     load and display HTML content from a string.
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
                     engine = EngineFactory.Create(new EngineOptions.Builder
                                                       {
                                                           RenderingMode = RenderingMode.OffScreen
                                                       }
                                                      .Build());

                     browser = engine.CreateBrowser();
                 })
                .ContinueWith(t =>
                 {
                     // Create WPF BrowserView component.
                     BrowserView browserView = new BrowserView();
                     // Embed BrowserView component into main layout.
                     mainLayout.Children.Add(browserView);

                     browserView.InitializeFrom(browser);

                     browser.Navigation.LoadUrl("https://www.teamdev.com/")
                            .ContinueWith(SaveWebPage);
                 }, TaskScheduler.FromCurrentSynchronizationContext());
            // Initialize WPF Application UI.
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void SaveWebPage(Task<LoadResult> obj)
        {
            string filePath = Path.GetFullPath("SavedPages\\index.html");
            string dirPath = Path.GetFullPath("SavedPages\\resources");
            Directory.CreateDirectory(dirPath);
            browser.SaveWebPage(filePath, dirPath, SavePageType.CompletePage);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Dispose browser and engine when close app window.
            browser.Dispose();
            engine.Dispose();
        }

        #endregion
    }
}