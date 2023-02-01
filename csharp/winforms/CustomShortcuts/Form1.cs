#region Copyright

// Copyright © 2023, TeamDev. All rights reserved.
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
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input;
using DotNetBrowser.Input.Keyboard.Events;

namespace CustomShortcuts.WinForms
{
    /// <summary>
    ///     This example demonstrates how to configure custom shortcuts for the
    ///     browser.
    /// </summary>
    public partial class Form1 : Form
    {
        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            EngineFactory.CreateAsync(new EngineOptions.Builder
                          {
                              RenderingMode = RenderingMode.HardwareAccelerated
                          }.Build())
                         .ContinueWith(t =>
                          {
                              engine = t.Result;
                              browser = engine.CreateBrowser();
                              browserView1.InitializeFrom(browser);
                              browser.Navigation.LoadUrl("https://teamdev.com");
                              // Set focus to browser.
                              browser.Focus();

                              browser.Keyboard.KeyPressed.Handler =
                                  new Handler<IKeyPressedEventArgs, InputEventResponse>(HandleKeyPress);
                          }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
            FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            engine?.Dispose();
        }

        private InputEventResponse HandleKeyPress(IKeyPressedEventArgs e)
        {
            Debug.WriteLine($"Key: {e.VirtualKey}");
            // Map Ctrl-'P' to "Print"
            if (e.Modifiers.ControlDown && e.VirtualKey == KeyCode.VkP)
            {
                Debug.WriteLine("Print");
                BeginInvoke((Action) (() => browser.MainFrame.Print()));
            }

            // Map Ctrl-'+' to "Zoom In"
            if (e.Modifiers.ControlDown && e.VirtualKey == KeyCode.Add)
            {
                Debug.WriteLine("Zoom In");
                BeginInvoke((Action) (() => browser.Zoom.In()));
            }

            // Map Ctrl-'-' to "Zoom Out"
            if (e.Modifiers.ControlDown && e.VirtualKey == KeyCode.Subtract)
            {
                Debug.WriteLine("Zoom Out");
                BeginInvoke((Action) (() => browser.Zoom.Out()));
            }

            return InputEventResponse.Proceed;
        }
    }
}
