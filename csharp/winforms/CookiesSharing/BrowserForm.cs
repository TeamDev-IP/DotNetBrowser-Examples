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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Cookies;
using DotNetBrowser.Engine;

namespace CookiesSharing.WinForms
{
    public partial class BrowserForm : Form
    {
        private readonly IBrowser browser;
        private readonly ICookieStore cookieStore;
        private readonly IEngine engine;

        public BrowserForm(string userDataPath, IEnumerable<Cookie> cookies = null)
        {
            // Create and initialize the IEngine instance.
            EngineOptions.Builder builder = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated
            };

            if (!string.IsNullOrWhiteSpace(userDataPath))
            {
                builder.UserDataDirectory = userDataPath;
            }

            EngineOptions engineOptions = builder.Build();
            engine = EngineFactory.Create(engineOptions);

            // Create the IBrowser instance.
            browser = engine.CreateBrowser();
            cookieStore = engine?.Profiles.Default.CookieStore;
            UpdateCookies(cookies);
            InitializeComponent();

            FormClosed += (sender, args) =>
            {
                engine?.Dispose();
            };

            // Initialize the Windows Forms BrowserView control.
            browserView1.InitializeFrom(browser);

            browser.Navigation.FrameLoadFinished += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                    if (InvokeRequired)
                    {
                        BeginInvoke(new Action(() => { textBox1.Text = args.Browser.Url; }));
                    }
                    else
                    {
                        textBox1.Text = args.Browser.Url;
                    }
                }
            };

            browser.Navigation.LoadUrl("https://mail.google.com/");
        }

        public IEnumerable<Cookie> GetAllCookies() => cookieStore?.GetAllCookies().Result;

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                browser.Navigation.LoadUrl(textBox1.Text);
            }
        }

        private void UpdateCookies(IEnumerable<Cookie> cookies)
        {
            if (cookies != null)
            {
                List<Cookie> list = cookies.ToList();
                if (list.Count > 0)
                {
                    cookieStore.DeleteAllCookies().Wait();
                    foreach (Cookie cookie in list)
                    {
                        bool success = cookieStore.SetCookie(cookie).Result;
                    }

                    cookieStore.Flush();
                }
            }
        }
    }
}
