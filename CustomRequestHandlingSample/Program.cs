using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomRequestHandlingSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            private class CustomResponseEventArgs : EventArgs
            {
                public string Url { get; private set; }

                public CustomResponseEventArgs(string url)
                {
                    Url = url;
                }
            }
            private delegate void CustomResponseHandler(object sender, CustomResponseEventArgs e);

            private class CustomLoadHandler : DefaultLoadHandler
            {
                public event CustomResponseHandler CustomResponseEvent;

                public override bool OnLoad(LoadParams loadParams)
                {
                    if (loadParams.Url.StartsWith(@"myscheme://"))
                    {
                        var customResponseEvent = CustomResponseEvent;
                        if (customResponseEvent != null)
                        {
                            customResponseEvent.Invoke(this, new CustomResponseEventArgs(loadParams.Url));
                        }
                    }

                    return false;
                }
            }

            public WindowMain()
            {
                Browser browser = BrowserFactory.Create();

                //add custom request handler
                var customLoadHandler = new CustomLoadHandler();

                customLoadHandler.CustomResponseEvent += delegate (object sender, CustomResponseEventArgs e)
                {
                    if (e.Url.Contains(@"myscheme://test1"))
                    {
                        browser.Stop();
                        browser.LoadURL(@"http://google.com");
                    }
                };

                browser.LoadHandler = customLoadHandler;

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("myscheme://test1");
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
