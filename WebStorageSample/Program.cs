using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WebStorageSample
{
    /// <summary>
    /// The sample demonstrates how to access WebStorage on
    /// the loaded web page using JxBrowser API.
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

                browser.DocumentLoadedInMainFrameEvent += delegate
                {
                    IWebStorage webStorage = browser.GetLocalWebStorage();
                    // Read and display the 'myKey' storage value.
                    Console.Out.WriteLine("The myKey value: " + webStorage["myKey"]);
                    // Modify the 'myKey' storage value.
                    webStorage.Set("myKey", "Hello from Local Storage");
                };

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadHTML(new LoadHTMLParams(
                       "<html><body><button onclick=\"myFunction()\">Modify 'myKey' value</button>" +
                               "<script>localStorage.myKey = \"Initial Value\";" +
                               "function myFunction(){alert(localStorage.myKey);}" +
                               "</script></body></html>",
                       "UTF-8",
                       "http://teamdev.com"));
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
