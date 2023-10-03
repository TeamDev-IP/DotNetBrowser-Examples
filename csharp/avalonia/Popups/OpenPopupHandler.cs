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

// #docfragment "OpenPopupHandler.Avalonia"
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using DotNetBrowser.AvaloniaUi;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;

namespace Popups
{
    public class OpenPopupHandler : IHandler<OpenPopupParameters>
    {
        public void Handle(OpenPopupParameters parameters)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ShowPopup(parameters.PopupBrowser,
                          parameters.Rectangle);
            });
        }

        private void ShowPopup(IBrowser popupBrowser, Rectangle rectangle)
        {
            BrowserView browserView = new BrowserView();
            browserView.InitializeFrom(popupBrowser);
            // Set the same popup handler for the popup browser itself.
            popupBrowser.OpenPopupHandler = new OpenPopupHandler();

            Window window = new Window();

            if (!rectangle.IsEmpty)
            {
                window.Position = new PixelPoint(rectangle.Origin.X, rectangle.Origin.Y);
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
                Dispatcher.UIThread.InvokeAsync(() => window.Title = e.Title);
            };

            popupBrowser.Disposed += delegate
            {
                Dispatcher.UIThread.InvokeAsync(() =>
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
// #enddocfragment "OpenPopupHandler.Avalonia"