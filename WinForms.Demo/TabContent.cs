using DotNetBrowser;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class TabContent : TableLayoutPanel
    {
        public delegate void PropertyChangeHandler(string propertyName,
                                         object oldValue, object newValue);
        public event PropertyChangeHandler PropertyChangeEvent;

        private Browser browser;
        private BrowserView browserView;

        private ToolPanel toolBar;
        private Panel browserContainer;

        public TabContent(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;

            this.browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    if (PropertyChangeEvent != null)
                    {
                        PropertyChangeEvent.Invoke("PageTitleChanged", null, browser.Title);
                    }
                }
            };

            this.browserContainer = CreateBrowserContainer();
            this.browserContainer.Dock = DockStyle.Fill;


            this.toolBar = CreateToolBar(browserView);
            this.toolBar.Dock = DockStyle.Top;
            this.toolBar.AutoSize = true;

            this.ColumnCount = 1;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.RowCount = 2;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));


            this.Controls.Add(toolBar, 0, 0);
            this.Controls.Add(browserContainer, 0, 1);
        }

        private ToolPanel CreateToolBar(BrowserView browserView)
        {
            ToolPanel toolBar = new ToolPanel(browserView);
            return toolBar;
        }

        private Panel CreateBrowserContainer()
        {
            Panel container = new Panel();
            container.Controls.Add((Control)this.browserView.GetComponent());
            return container;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                browserView.Dispose();
                browser.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
