using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestoreBrowserSample
{
    /// <summary>
    /// This sample demonstrates how to restore Browser instance after its
    /// native process unexpectedly terminated. In general to rest Browser owser instance you just need to load the same or another URL.
    /// </summary>
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;

                Browser browser = BrowserFactory.Create(browserContext);

                browser.RenderGoneEvent += delegate(object sender, RenderEventArgs e)
                {
                    // Restore Browser instance by loading the same URL
                    browser.LoadURL(e.Browser.URL);
                };

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://www.google.com");

                Console.Out.WriteLine("Run 'Task Manager' app and kill the 'DotNetBrowser.Chromium.exe' " +
                "process with the '--type=renderer' command line argument.");
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
