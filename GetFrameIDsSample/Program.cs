using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GetFrameIDsSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                Browser browser = BrowserFactory.Create();
                browserView = new WPFBrowserView(browser);

                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;

                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        // Get HTML of each frame on the web page
                        PrintFrameHierarhy(browser, BrowserFrameID.MAIN_FRAME_ID);
                    }
                };
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://docs.oracle.com/javase/8/docs/api/");
            }


            public static void PrintFrameHierarhy(Browser browser, long frameId)
            {
                var framesIds = browser.GetFramesIds(frameId);
                foreach (var id in framesIds)
                {
                    String html = browser.GetHTML(id);
                    Console.WriteLine(id + " HTML = " + html);
                    PrintFrameHierarhy(browser, id);
                }
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
