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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DotNetBrowser.Cookies;

namespace CookiesSharing.WinForms
{
    /// <summary>
    ///     The example demonstrates how to share cookies between
    ///     two Engine instances.
    /// </summary>
    public partial class Form1 : Form
    {
        private BrowserForm browserForm1;
        private BrowserForm browserForm2;

        private string dataPath1 = Path.GetFullPath("data1");
        private string dataPath2 = Path.GetFullPath("data2");
        private IEnumerable<Cookie> cookies;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LaunchBrowser(ref browserForm1, dataPath1);
        }

        private void LaunchBrowser(ref BrowserForm browserForm, string dataPath,
                                   IEnumerable<Cookie> cookies = null)
        {
            if (browserForm == null || browserForm.IsDisposed)
            {
                browserForm = new BrowserForm(dataPath, cookies);
                browserForm.Text += $": {Path.GetFileNameWithoutExtension(dataPath)}";
                browserForm.Show(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LaunchBrowser(ref browserForm2, dataPath2, cookies);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (browserForm1 != null && !browserForm1.IsDisposed)
            {
                cookies = browserForm1.GetAllCookies();
                Debug.WriteLine("Cookies copied");
            }
        }
    }
}
