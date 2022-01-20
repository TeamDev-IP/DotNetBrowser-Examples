#region Copyright

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
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Navigation;
using DotNetBrowser.WinForms;

namespace KeyboardEventSimulation.WinForms
{
    /// <summary>
    ///     This example demonstrates how to simulate keypress.
    /// </summary>
    public partial class Form1 : Form
    {
        private const string Html = @"<html>
                                          <body>
                                            <input type='text' autofocus></input>
                                          </body>
                                        </html>";

        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            InitializeComponent();
            Closing += Form1_Closing;

            Task.Run(() =>
                 {
                     engine = EngineFactory.Create(new EngineOptions.Builder
                                                       {
                                                           RenderingMode =
                                                               RenderingMode.OffScreen
                                                       }
                                                      .Build());

                     browser = engine.CreateBrowser();
                 })
                .ContinueWith(t =>
                 {
                     BrowserView browserView = new BrowserView();
                     // Embed BrowserView component into main layout.
                     Controls.Add(browserView);
                     browserView.InitializeFrom(browser);
                     byte[] htmlBytes = Encoding.UTF8.GetBytes(Html);
                     browser.Navigation
                            .LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}")
                            .ContinueWith(SimulateInput);
                 }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            // Dispose browser and engine when close app window.
            browser.Dispose();
            engine.Dispose();
        }

        private async void SimulateInput(Task<LoadResult> e)
        {
            if (e.Result == LoadResult.Completed)
            {
                await Task.Delay(2000);
                // #docfragment "KeyboardEventSimulation.Usage"
                IKeyboard keyboard = browser.Keyboard;
                SimulateKey(keyboard, KeyCode.VkH, "H");
                SimulateKey(keyboard, KeyCode.VkE, "e");
                SimulateKey(keyboard, KeyCode.VkL, "l");
                SimulateKey(keyboard, KeyCode.VkL, "l");
                SimulateKey(keyboard, KeyCode.VkO, "o");
                SimulateKey(keyboard, KeyCode.Space, " ");
                // Simulate input of some non-letter characters.
                SimulateKey(keyboard, KeyCode.Vk5, "%", new KeyModifiers {ShiftDown = true});
                SimulateKey(keyboard, KeyCode.Vk2, "@", new KeyModifiers {ShiftDown = true});
                // #enddocfragment "KeyboardEventSimulation.Usage"
            }
        }

        // #docfragment "KeyboardEventSimulation.Implementation"
        private static void SimulateKey(IKeyboard keyboard, KeyCode key, string keyChar,
                                        KeyModifiers modifiers = null)
        {
            modifiers = modifiers ?? new KeyModifiers();
            KeyPressedEventArgs keyDownEventArgs = new KeyPressedEventArgs
            {
                KeyChar = keyChar,
                VirtualKey = key,
                Modifiers = modifiers
            };

            KeyTypedEventArgs keyPressEventArgs = new KeyTypedEventArgs
            {
                KeyChar = keyChar,
                VirtualKey = key,
                Modifiers = modifiers
            };
            KeyReleasedEventArgs keyUpEventArgs = new KeyReleasedEventArgs
            {
                VirtualKey = key,
                Modifiers = modifiers
            };

            keyboard.KeyPressed.Raise(keyDownEventArgs);
            keyboard.KeyTyped.Raise(keyPressEventArgs);
            keyboard.KeyReleased.Raise(keyUpEventArgs);
        }
        // #enddocfragment "KeyboardEventSimulation.Implementation"
    }
}
