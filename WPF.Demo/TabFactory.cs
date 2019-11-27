using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser;
using DotNetBrowser.WPF;
using System.Windows;

namespace Demo.WPF
{
    public class TabFactory
    {
        public static BrowserType BrowserType = BrowserType.HEAVYWEIGHT;        

        public static Tab CreateFirstTab()
        {
            return CreateTab("https://www.teamdev.com/dotnetbrowser");
        }

        public static Tab CreateTab()
        {
            return CreateTab("about:blank");
        }

        public static Tab CreateTab(String url)
        {
            Browser browser = BrowserFactory.Create(BrowserType);
            BrowserView browserView = new WPFBrowserView(browser);

            browser.DialogHandler = new WPFDefaultDialogHandler((UIElement)browserView);
            browser.DownloadHandler = new WPFDefaultDownloadHandler((UIElement)browserView);
            browser.ContextMenuHandler = new WPFDefaultContextMenuHandler((FrameworkElement)browserView, true);
            browser.Preferences.FireKeyboardEventsEnabled = false;
            browser.Preferences.FireMouseEventsEnabled = false;

            TabContent tabContent = new TabContent(browserView);

            TabCaption tabCaption = new TabCaption();
            tabCaption.SetTitle("about:blank");

            browserView.Browser.LoadURL(url);
            return new Tab(tabCaption, tabContent);
        }
    }
}