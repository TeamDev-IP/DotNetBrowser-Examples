using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DOMCreateElementSample
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

                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        DOMDocument document = e.Browser.GetDocument();

                        DOMNode root = document.GetElementById("root");
                        DOMNode textNode = document.CreateTextNode("Some text");
                        DOMElement paragraph = document.CreateElement("p");
                        paragraph.AppendChild(textNode);
                        root.AppendChild(paragraph);
                    }
                };

                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadHTML("<html><body><div id='root'></div></body></html>");
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
