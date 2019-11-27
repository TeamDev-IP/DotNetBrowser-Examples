using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XPathSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;

                Browser browser = BrowserFactory.Create(browserContext);

                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        DOMDocument document = browser.GetDocument();
                        XPathResult result = document.Evaluate("count(//div)");
                        // If the expression is not a valid XPath expression or the document
                        // element is not available, we'll get an error.
                        if (result.IsError)
                        {
                            Console.WriteLine("Error: " + result.ErrorMessage);
                            return;
                        }

                        // Make sure that result is a number.
                        if (result.IsNumber)
                        {
                            Console.WriteLine("Result: " + result.Number);
                        }
                    }

                };

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://www.teamdev.com/jxbrowser");
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
