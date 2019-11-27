using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DOMSimulateClickSample
{
    public partial class Form1 : Form
    {
        private Browser browser;
        public Form1()
        {
            InitializeComponent();

            browser = BrowserFactory.Create();
            BrowserView browserView = new WinFormsBrowserView(browser)
            {
                Dock = DockStyle.Fill
            };
            browserView.Browser.DialogHandler = new WinFormsDefaultDialogHandler((Control)browserView.GetComponent());

            this.Controls.Add((Control)browserView.GetComponent());

            browserView.Browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    Browser myBrowser = e.Browser;
                    DOMDocument document = myBrowser.GetDocument();
                    DOMElement link = document.GetElementById("button");
                    if (link != null)
                    {
                        link.Click();
                    }
                }
            };
            browserView.Browser.LoadHTML("<html><body><button id='button' " +
                    "onclick=\"alert('Button has been clicked!');\">Click Me!</button>" +
                    "</body></html>");
        }
    }
}
