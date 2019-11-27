using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.DOM.Events;
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

namespace DOMEventsSample
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

            DOMEventHandler domEvent = delegate(object sender, DOMEventArgs e)
            {
                DOMEventType eventType = e.Type;
                Console.Out.WriteLine("handleEvent = " + eventType);
            };


            browserView.Browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    DOMDocument document = e.Browser.GetDocument();
                    DOMElement documentElement = document.DocumentElement;
                    documentElement.AddEventListener(DOMEventType.OnClick | DOMEventType.OnMouseDown | DOMEventType.OnMouseUp | DOMEventType.OnKeyDown, domEvent, false);
                }
            };

            browserView.Browser.LoadURL("http://www.teamdev.com");
        }
    }
}
