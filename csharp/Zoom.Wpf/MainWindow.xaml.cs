#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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
using DotNetBrowser.Handlers;
using DotNetBrowser.Input;
using DotNetBrowser.Input.Mouse.Events;

namespace Zoom.Wpf
{
    /// <summary>
    ///     The example demonstrates how to implement zooming
    ///     on mouse scroll with Ctrl pressed.
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
                             RenderingMode = RenderingMode.HardwareAccelerated
                         }.Build());
                         browser = engine.CreateBrowser();
                         browser.Navigation.LoadUrl("teamdev.com");
                         browser.Mouse.WheelMoved.Handler =
                             new Handler<IMouseWheelMovedEventArgs, InputEventResponse>(OnMouseWheelMoved);
                     })
                    .ContinueWith(t =>
                     {
                         BrowserView.InitializeFrom(browser);
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

        private void EnableZoom(bool zoomEnabled)
        {
            if (browser != null)
            {
                Debug.WriteLine("Zoom Enabled: " + zoomEnabled);
                browser.Zoom.Enabled = zoomEnabled;
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private InputEventResponse OnMouseWheelMoved(IMouseWheelMovedEventArgs arg)
        {
            if (arg.Modifiers.ControlDown)
            {
                if (arg.DeltaY > 0)
                {
                    Debug.WriteLine("Zoom In");
                    browser.Zoom.In();
                }
                else
                {
                    Debug.WriteLine("Zoom Out");
                    browser.Zoom.Out();
                }
            }

            return InputEventResponse.Proceed;
        }

        private void ZoomEnabledCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            EnableZoom(true);
        }

        private void ZoomEnabledCheckbox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EnableZoom(false);
        }

        #endregion
    }
}