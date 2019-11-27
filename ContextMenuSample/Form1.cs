using DotNetBrowser;
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

namespace ContextMenuSample
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

            browser.ContextMenuHandler = new MyContextMenuHandler((Control)browserView);
            this.Controls.Add((Control)browserView.GetComponent());

            browser.LoadURL("http://www.google.com");

        }

        private class MyContextMenuHandler : ContextMenuHandler
        {
            Control view;
            public MyContextMenuHandler(Control view)
            {
                this.view = view;
            }

            public void ShowContextMenu(ContextMenuParams parameters)
            {
                System.Windows.Forms.ContextMenu popupMenu = new System.Windows.Forms.ContextMenu();
                if (!String.IsNullOrEmpty(parameters.LinkText))
                {
                    popupMenu.MenuItems.Add(new MenuItem("Open link in new window", delegate
                    {
                        String linkURL = parameters.LinkURL;
                        Console.Out.WriteLine("linkURL = " + linkURL);
                    }));
                }

                Browser browser = parameters.Browser;
                popupMenu.MenuItems.Add(new MenuItem("Reload", delegate
                {
                    browser.Reload();
                }));

                Point location = parameters.Location;

                view.Invoke(new Action(() =>
                {
                    popupMenu.Show(view, location);
                }));

            }
        }
    }
}
