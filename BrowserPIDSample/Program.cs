using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BrowserPIDSample
{
    /// <summary>
    /// Demonstrates how to get PID of Chromium render process where DOM and
    /// JavaScript of the currently loaded web page are running.
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

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;

                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        // Get PID of Chromium render process associated with
                        // the current Browser instance
                        long pid = browser.RenderProcessInfo.Pid;
                    }
                };
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("https://google.com");
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
