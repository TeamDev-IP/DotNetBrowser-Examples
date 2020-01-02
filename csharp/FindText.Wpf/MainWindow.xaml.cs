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
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Search;

namespace FindText.Wpf
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
                    browserView.InitializeFrom(browser);

                    browser.Navigation.LoadUrl("https://teamdev.com/dotnetbrowser");
                }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        #endregion

        #region Methods

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            browser.TextFinder.StopFinding();
            textBox.Text = "";
        }

        private void findButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != string.Empty)
            {
                browser.TextFinder.Find(textBox.Text).ContinueWith((t) =>
                {
                    if (t.Result.NumberOfMatches == 0)
                    {
                        MessageBox.Show("No matches!");
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }

        #endregion
    }
}