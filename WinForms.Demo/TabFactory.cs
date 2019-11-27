using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class TabFactory
    {
        public static BrowserType BrowserType = BrowserType.HEAVYWEIGHT;

        public static Tab CreateFirstTab()
        {
            return CreateTab("http://www.teamdev.com/dotnetbrowser");
        }

        public static Tab CreateTab()
        {
            return CreateTab("about:blank");
        }

        public static Tab CreateTab(String url)
        {
            Browser browser = BrowserFactory.Create(BrowserType);
            WinFormsBrowserView browserView = new WinFormsBrowserView(browser)
            {
                Dock = DockStyle.Fill
            };

            browser.DialogHandler = new WinFormsDefaultDialogHandler((Control)browserView);
            browser.DownloadHandler = new WinFormsDefaultDownloadHandler((Control)browserView);
            browser.ContextMenuHandler = new WinFormsDefaultContextMenuHandler((Control)browserView, true);
            browser.Preferences.FireKeyboardEventsEnabled = false;
            browser.Preferences.FireMouseEventsEnabled = false;

            TabContent tabContent = new TabContent(browserView);

            TabCaption tabCaption = new TabCaption();
            tabCaption.SetTitle("about:blank");

            tabContent.PropertyChangeEvent += delegate(string propertyName,
                                         object oldValue, object newValue)
            {
                tabCaption.SetTitle(newValue.ToString());
            };

            browserView.Browser.LoadURL(url);
            return new Tab(tabCaption, tabContent);
        }
    }
}
