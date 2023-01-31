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

// #docfragment "OpenPopupHandler.WinForms"
using System;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.WinForms;

namespace Popups.WinForms
{
    public class OpenPopupHandler : IHandler<OpenPopupParameters>
    {
        private readonly Control parent;

        public OpenPopupHandler(Control parent)
        {
            this.parent = parent;
        }

        public void Handle(OpenPopupParameters parameters)
        {
            Action showPopupAction = () =>
            {
                ShowPopup(parameters.PopupBrowser,
                          parameters.Rectangle);
            };
            parent.BeginInvoke(showPopupAction);
        }

        private void ShowPopup(IBrowser popupBrowser, Rectangle rectangle)
        {
            BrowserView browserView = new BrowserView
            {
                Dock = DockStyle.Fill
            };

            browserView.InitializeFrom(popupBrowser);
            // Set the same popup handler for the popup browser itself.
            popupBrowser.OpenPopupHandler = new OpenPopupHandler(browserView);

            Form form = new Form();

            if(!rectangle.IsEmpty)
            {
                form.StartPosition = FormStartPosition.Manual;

                form.Location = new System.Drawing.Point(rectangle.Origin.X,
                                                         rectangle.Origin.Y);

                form.ClientSize = new System.Drawing.Size((int)rectangle.Size.Width,
                                                          (int)rectangle.Size.Height);

                browserView.Width = (int)rectangle.Size.Width;
                browserView.Height = (int)rectangle.Size.Height;
            }
            else
            {
                form.Width = 800;
                form.Height = 600;
            }

            form.Closed += delegate
            {
                form.Controls.Clear();

                if(!popupBrowser.IsDisposed)
                {
                    popupBrowser.Dispose();
                }
            };

            popupBrowser.TitleChanged += delegate (object sender, TitleChangedEventArgs e)
            {
                form.BeginInvoke((Action)(() => { form.Text = e.Title; }));
            };

            popupBrowser.Disposed += delegate
            {
                Action formCloseAction = () =>
                {
                    form.Controls.Clear();
                    form.Hide();
                    form.Close();
                    form.Dispose();
                };
                form.BeginInvoke(formCloseAction);
            };

            form.Controls.Add(browserView);
            form.Show();
        }
    }
}
// #enddocfragment "OpenPopupHandler.WinForms"
