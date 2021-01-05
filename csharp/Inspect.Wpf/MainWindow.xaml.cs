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
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input;
using DotNetBrowser.Input.Mouse.Events;
using Point = DotNetBrowser.Geometry.Point;

namespace Inspect.Wpf
{
    /// <summary>
    ///     This example demonstrates how to get DOM Node at a specific point on the web page.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;

        #region Constructors

        public MainWindow()
        {
            engine = EngineFactory
               .Create(new EngineOptions.Builder
                           {
                               RenderingMode = RenderingMode.HardwareAccelerated
                           }
                          .Build());
            browser = engine.CreateBrowser();
            browser.Mouse.Moved.Handler = new Handler<IMouseMovedEventArgs, InputEventResponse>(OnMouseMoved);
            InitializeComponent();
            browserView1.InitializeFrom(browser);
            browser.Navigation.LoadUrl("https://www.teamdev.com/dotnetbrowser");
        }

        #endregion

        #region Methods

        private void GetNodeAtPoint(Point location)
        {
            double scale = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice.M11 ?? 1;
            location = new Point((int)Math.Round(location.X * scale), (int)Math.Round(location.Y * scale));
            PointInspection inspection = browser.MainFrame.Inspect(location);
            INode inspectionNode = inspection.UrlNode ?? inspection.Node;
            statusLabel1.Content = inspectionNode?.XPath ?? string.Empty;
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private InputEventResponse OnMouseMoved(IMouseMovedEventArgs arg)
        {
            Dispatcher.BeginInvoke((Action) (() => GetNodeAtPoint(arg.Location)));
            return InputEventResponse.Proceed;
        }

        #endregion
    }
}