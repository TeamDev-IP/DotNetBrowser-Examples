#region Copyright

// Copyright 2020, TeamDev. All rights reserved.
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
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Js;
using DotNetBrowser.WinForms;

namespace JavaScriptBridge.WinForms
{
    public partial class Form1 : Form
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;

        #region Constructors

        public Form1()
        {
            InitializeComponent();
            BrowserView webView = new BrowserView {Dock = DockStyle.Fill};
            tableLayoutPanel1.Controls.Add(webView, 1, 0);
            tableLayoutPanel1.SetRowSpan(webView, 2);
            engine = EngineFactory
               .Create(new EngineOptions.Builder
                           {
                               RenderingMode = RenderingMode.HardwareAccelerated
                           }
                          .Build());
            browser = engine.CreateBrowser();
            webView.InitializeFrom(browser);
            browser.ConsoleMessageReceived += (sender, args) => { };
            browser
               .MainFrame
               .LoadHtml(@"<html>
                        <head>
                          <meta charset='UTF-8'>
                          <style>body{padding: 0; margin: 0; width:100%; height: 100%;}
                                textarea.fill {padding: 2; margin: 0; width:100%; height:100%;}
                                button{position: absolute; bottom: 0; padding: 2; width:100%;}</style>
                        </head>
                        <body>
                        <div>
                        <textarea id='text' class='fill' autofocus cols='30' rows='20'>Sample text</textarea>
                        <button id='updateForm' type='button' onClick='updateForm(document.getElementById(""text"").value)'>&lt; Update Form</button> 
                        </div>
                        </body>
                        </html>")
               .Wait();
            IJsObject window = browser.MainFrame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["updateForm"] = (Action<string>) UpdateForm;
            FormClosing += Form1_FormClosing;
        }

        #endregion

        #region Methods

        public void UpdateForm(string value)
        {
            BeginInvoke((Action) (() => richTextBox1.Text = value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IJsObject textElement = browser.MainFrame.ExecuteJavaScript<IJsObject>("document.getElementById('text');")
                                           .Result;
            textElement.Properties["value"] = richTextBox1.Text;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            engine?.Dispose();
        }

        #endregion
    }
}