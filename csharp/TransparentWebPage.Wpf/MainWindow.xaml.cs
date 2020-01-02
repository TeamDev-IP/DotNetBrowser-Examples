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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace TransparentWebPage.Wpf
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
            try
            {
                Task.Run(() =>
                    {
                        engine = EngineFactory.Create(new EngineOptions.Builder
                        {
                            RenderingMode = RenderingMode.OffScreen
                        }.Build());
                        browser = engine.CreateBrowser();
                        browser.Settings.TransparentBackgroundEnabled = true;
                    })
                    .ContinueWith(t =>
                    {
                        WebBrowser1.InitializeFrom(browser);
                        browser.MainFrame.LoadHtml(
                                                   "<html>\n"
                                                   + "     <body>"
                                                   + "         <div style='background: yellow; opacity: 0.7;'>\n"
                                                   + "             This text is in the yellow half-transparent div."
                                                   + "        </div>\n"
                                                   + "         <div style='background: red;'>\n"
                                                   + "             This text is in the red opaque div and should appear as is."
                                                   + "        </div>\n"
                                                   + "         <div>\n"
                                                   + "             This text is in the non-styled div and should appear as a text on the completely transparent background."
                                                   + "        </div>\n"
                                                   + "    </body>\n"
                                                   + " </html>");
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                InitializeComponent();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        #endregion

        #region Methods

        private void Window_Closed(object sender, EventArgs e)
        {
            engine?.Dispose();
        }

        #endregion
    }
}