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

namespace DOMSetNodeValueSample
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

            this.Controls.Add((Control)browserView.GetComponent());

            browserView.Browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    DOMDocument document = e.Browser.GetDocument();
                    DOMElement button = document.GetElementById("button-id");
                    button.Children[0].NodeValue = "New Button Name";
                }
            };
            browserView.Browser.LoadHTML("<html><body><button id='button-id'>Button</button></body></html>");
        }
    }
}
