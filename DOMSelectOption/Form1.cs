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

namespace DOMSelectOption
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
                    DOMSelectElement select = (DOMSelectElement)document.GetElementById("select-tag");
                    select.Value = "opel";
                }
            };
            browserView.Browser.LoadHTML("<html><body><select id='select-tag'>\n" +
                    "  <option value=\"volvo\">Volvo</option>\n" +
                    "  <option value=\"saab\">Saab</option>\n" +
                    "  <option value=\"opel\">Opel</option>\n" +
                    "  <option value=\"audi\">Audi</option>\n" +
                    "</select></body></html>"); ;

        }
    }
}
