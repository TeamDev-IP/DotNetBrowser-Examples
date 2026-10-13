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
using System.Collections.Generic;
using System.IO;
using System.Windows;
using CefSharp;

namespace CefSharpApp
{
    /// <summary>
    ///     This example demonstrates how to simulate keyboard and mouse input with CefSharp.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string page = Path.Combine(AppContext.BaseDirectory,
                                       "bucket-list.html");
            browser.Address = new Uri(page).AbsoluteUri;
        }

        private async void TypeTask_Click(object sender, RoutedEventArgs e)
        {
            IBrowserHost host = browser.GetBrowserHost();
            host.SendFocusEvent(true);
            await browser.EvaluateScriptAsync(
                "document.getElementById('task').focus()");
            TypeText(host, "Learn to surf");
        }

        private async void ClickAdd_Click(object sender, RoutedEventArgs e)
        {
            // There is no DOM API, so the button position comes from JavaScript.
            JavascriptResponse response = await browser.EvaluateScriptAsync(@"
                (() => {
                    const rect = document.getElementById('add').getBoundingClientRect();
                    return [rect.x + rect.width / 2, rect.y + rect.height / 2];
                })()");
            List<object> center = (List<object>)response.Result;
            int x = Convert.ToInt32(center[0]);
            int y = Convert.ToInt32(center[1]);

            IBrowserHost host = browser.GetBrowserHost();
            MouseEvent mouseEvent = new MouseEvent(x, y, CefEventFlags.None);
            host.SendMouseMoveEvent(mouseEvent, false);
            host.SendMouseClickEvent(mouseEvent, MouseButtonType.Left, false, 1);
            host.SendMouseClickEvent(mouseEvent, MouseButtonType.Left, true, 1);
        }

        private static void TypeText(IBrowserHost host, string text)
        {
            foreach (char c in text)
            {
                // Key codes of letters, digits, and space equal their
                // uppercase character codes.
                int keyCode = char.ToUpperInvariant(c);
                CefEventFlags modifiers = char.IsUpper(c)
                    ? CefEventFlags.ShiftDown
                    : CefEventFlags.None;

                host.SendKeyEvent(new KeyEvent
                {
                    Type = KeyEventType.RawKeyDown,
                    WindowsKeyCode = keyCode,
                    Modifiers = modifiers
                });
                host.SendKeyEvent(new KeyEvent
                {
                    Type = KeyEventType.Char,
                    WindowsKeyCode = c,
                    Modifiers = modifiers
                });
                host.SendKeyEvent(new KeyEvent
                {
                    Type = KeyEventType.KeyUp,
                    WindowsKeyCode = keyCode,
                    Modifiers = modifiers
                });
            }
        }
    }
}
