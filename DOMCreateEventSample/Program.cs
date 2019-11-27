using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.DOM.Events;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DOMCreateEventSample
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

                        var myEvent = browser.CreateEvent("MyEvent");

                        DOMNode root = document.GetElementById("root");

                        DOMEventHandler domEvent = delegate(object s, DOMEventArgs evt)
                        {
                            if (evt.EventType == myEvent.EventType)
                            {
                                DOMNode textNode = document.CreateTextNode("Some text");
                                DOMElement paragraph = document.CreateElement("p");
                                paragraph.AppendChild(textNode);
                                root.AppendChild(paragraph);
                            }
                        };

                        root.AddEventListener(myEvent, domEvent, false);

                        Thread.Sleep(3000);
                        root.DispatchEvent(myEvent);

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
