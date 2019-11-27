using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AjaxCallsFilterSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            private class TestResourceHandler : ResourceHandler
            {
                public bool CanLoadResource(ResourceParams parameters)
                {
                    if (parameters.ResourceType == ResourceType.XHR)
                    {
                        Console.WriteLine("Suppress ajax call - " + parameters.URL);
                        return false;
                    }

                    return true;
                }
            }

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;

                Browser browser = BrowserFactory.Create(browserContext);
                
                // Suppress/filter all ajax calls
                browser.Context.NetworkService.ResourceHandler = new TestResourceHandler();

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://www.w3schools.com/ajax/ajax_examples.asp");
            }

            [STAThread]
            public static void Main()
            {
                Application app = new Application();

                WindowMain wnd = new WindowMain();
                app.Run(wnd);

                var browser = wnd.browserView.Browser;
                wnd.browserView.Dispose();
                browser.Dispose();
            }
        }
    }
}
