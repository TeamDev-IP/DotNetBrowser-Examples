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