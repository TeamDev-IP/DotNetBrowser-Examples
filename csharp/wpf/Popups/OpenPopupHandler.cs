#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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

// #docfragment "OpenPopupHandler.Wpf"
using System;
using System.Windows;
using System.Windows.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Wpf;

namespace Popups.Wpf
{
    public class OpenPopupHandler : IHandler<OpenPopupParameters>
    {
        private readonly FrameworkElement parent;

        private Dispatcher Dispatcher => parent?.Dispatcher
                                         ?? Application.Current.Dispatcher;

        public OpenPopupHandler(FrameworkElement parentElement)
        {
            parent = parentElement;
        }

        public void Handle(OpenPopupParameters parameters)
        {
            Action showPopupAction = () =>
            {
                ShowPopup(parameters.PopupBrowser,
                          parameters.Rectangle);
            };
            Dispatcher.BeginInvoke(showPopupAction);
        }

        private void ShowPopup(IBrowser popupBrowser, Rectangle rectangle)
        {
            BrowserView browserView = new BrowserView();
            browserView.InitializeFrom(popupBrowser);
            // Set the same popup handler for the popup browser itself.
            popupBrowser.OpenPopupHandler = new OpenPopupHandler(browserView);

            Window window = new Window {Owner = Window.GetWindow(parent)};

            if (!rectangle.IsEmpty)
            {
                window.Top = rectangle.Origin.Y;
                window.Left = rectangle.Origin.X;
                window.SizeToContent = SizeToContent.WidthAndHeight;

                browserView.Width = rectangle.Size.Width;
                browserView.Height = rectangle.Size.Height;
            }
            else
            {
                window.Width = 800;
                window.Height = 600;
            }

            window.Closed += (sender, args) =>
            {
                window.Content = null;
                if (!popupBrowser.IsDisposed)
                {
                    popupBrowser.Dispose();
                }
            };

            popupBrowser.TitleChanged += (sender, e) =>
            {
                Dispatcher?.BeginInvoke((Action) (() => window.Title = e.Title));
            };

            popupBrowser.Disposed += delegate
            {
                Dispatcher?.Invoke(() =>
                {
                    window.Content = null;
                    window.Hide();
                    window.Close();
                });
            };

            window.Content = browserView;
            window.Show();
        }
    }
}
// #enddocfragment "OpenPopupHandler.Wpf"
