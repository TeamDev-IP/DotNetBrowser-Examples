#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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
using System.IO;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Input.Mouse;
using DotNetBrowser.Input.Mouse.Events;

namespace DotNetBrowserApp
{
    /// <summary>
    ///     This example demonstrates how to simulate keyboard and mouse input with DotNetBrowser.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEngine engine;
        private readonly IBrowser browser;

        public MainWindow()
        {
            engine = EngineFactory.Create();
            browser = engine.CreateBrowser();

            InitializeComponent();
            browserView.InitializeFrom(browser);

            string page = Path.Combine(AppContext.BaseDirectory,
                                       "bucket-list.html");
            browser.Navigation.LoadUrl(page);
        }

        private void TypeTask_Click(object sender, RoutedEventArgs e)
        {
            browser.Focus();
            browser.MainFrame.Document.GetElementById("task").Focus();
            TypeText(browser.Keyboard, "Learn to surf");
        }

        private void ClickAdd_Click(object sender, RoutedEventArgs e)
        {
            IElement addButton = browser.MainFrame.Document.GetElementById("add");
            browser.Mouse.SimulateClick(MouseButton.Left, addButton);
        }

        private static void TypeText(IKeyboard keyboard, string text)
        {
            foreach (char c in text)
            {
                // Key codes of letters, digits, and space equal their
                // uppercase character codes.
                KeyCode key = (KeyCode)char.ToUpperInvariant(c);
                string keyChar = c.ToString();
                KeyModifiers modifiers = new KeyModifiers
                {
                    ShiftDown = char.IsUpper(c)
                };

                keyboard.KeyPressed.Raise(new KeyPressedEventArgs
                {
                    VirtualKey = key,
                    KeyChar = keyChar,
                    Modifiers = modifiers
                });
                keyboard.KeyTyped.Raise(new KeyTypedEventArgs
                {
                    VirtualKey = key,
                    KeyChar = keyChar,
                    Modifiers = modifiers
                });
                keyboard.KeyReleased.Raise(new KeyReleasedEventArgs
                {
                    VirtualKey = key,
                    Modifiers = modifiers
                });
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
