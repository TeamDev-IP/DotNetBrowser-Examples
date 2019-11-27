using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NetworkDelegateSample
{
    class Program
    {
        private class SampleNetworkDelegate : DefaultNetworkDelegate
        {
            public override void OnBeforeURLRequest(BeforeURLRequestParams parameters) {
                // If navigate to teamdev.com, then change URL to google.com.
                if (parameters.Url == "http://www.teamdev.com/") {
                    parameters.SetUrl("www.google.com");
                }
            }

            public override void OnBeforeSendHeaders(BeforeSendHeadersParams parameters) {
                // If navigate to google.com, then print User-Agent header value.
                if (parameters.Url == "http://www.google.com/") {
                    HttpHeaders headers = parameters.Headers;
                    Console.WriteLine("User-Agent: " + headers.GetHeader("User-Agent"));
                }
            }
        }

        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;
                browserContext.NetworkService.NetworkDelegate = new SampleNetworkDelegate();

                Browser browser = BrowserFactory.Create(browserContext);
                browserView = new WPFBrowserView(browser);

                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://www.teamdev.com/");
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
