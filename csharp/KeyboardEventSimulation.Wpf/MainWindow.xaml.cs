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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Navigation;
using DotNetBrowser.WPF;

namespace KeyboardEventSimulation.Wpf
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser browser;
        private BrowserView browserView;
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
                                                          }
                                                          .Build());
                        browser = engine.CreateBrowser();
                    })
                    .ContinueWith(t =>
                    {
                        browserView = new BrowserView();
                        // Embed BrowserView component into main layout.
                        mainLayout.Children.Add(browserView);

                        browserView.InitializeFrom(browser);

                        browser.MainFrame.LoadHtml(@"<html>
                                            <body>
                                                <input type='text' autofocus></input>
                                            </body>
                                           </html>")
                               .ContinueWith(SimulateInput);
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                // Initialize WPF Application UI.
                InitializeComponent();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        #endregion

        #region Methods

        private async void SimulateInput(Task<LoadResult> e)
        {
            if (e.Result == LoadResult.Completed)
            {
                await Task.Delay(2000);
                IKeyboard keyboard = browser.Keyboard;
                SimulateKey(keyboard, KeyCode.VkH, "H");
                SimulateKey(keyboard, KeyCode.VkE, "e");
                SimulateKey(keyboard, KeyCode.VkL, "l");
                SimulateKey(keyboard, KeyCode.VkL, "l");
                SimulateKey(keyboard, KeyCode.VkO, "o");
            }
        }

        private static void SimulateKey(IKeyboard keyboard, KeyCode key, string keyChar)
        {
            KeyDownEventArgs keyDownEventArgs = new KeyDownEventArgs
            {
                KeyChar = keyChar,
                VirtualKey = key
            };

            KeyPressEventArgs keyPressEventArgs = new KeyPressEventArgs
            {
                KeyChar = keyChar,
                VirtualKey = key
            };
            KeyUpEventArgs keyUpEventArgs = new KeyUpEventArgs
            {
                VirtualKey = key
            };

            keyboard.KeyDown.Raise(keyDownEventArgs);
            keyboard.KeyPress.Raise(keyPressEventArgs);
            keyboard.KeyUp.Raise(keyUpEventArgs);
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