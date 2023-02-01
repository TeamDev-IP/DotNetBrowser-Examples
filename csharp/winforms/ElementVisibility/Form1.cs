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
using System.IO;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Js;

namespace ElementVisibility.WinForms
{
    /// <summary>
    ///     This example demonstrates how to change DOM element visibility
    ///     by modifying its CSS style.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;

        public Form1()
        {
            engine = EngineFactory.Create(new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated
            }.Build());

            browser = engine.CreateBrowser();
            InitializeComponent();
            browserView1.InitializeFrom(browser);

            browser.Navigation.LoadUrl(Path.GetFullPath("index.html"));
        }

        private void ChangeImageVisibility(object sender, EventArgs e)
        {
            IDocument document = browser.MainFrame.Document;

            // Find element on the loaded web page.
            IElement img = document.GetElementById("img");

            // Transform it into JsObject.
            IJsObject imgObject = img as IJsObject;

            // Apply the desired style.
            IJsObject style = imgObject.Properties["style"] as IJsObject;

            if (style.Properties["display"].ToString() != "none")
            {
                style.Properties["display"] = "none";
            }
            else
            {
                style.Properties["display"] = "inline";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            engine?.Dispose();
        }
    }
}
