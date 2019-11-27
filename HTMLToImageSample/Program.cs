using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace HTMLToImageSample
{
    public partial class WindowMain : System.Windows.Window
    {
        private WPFBrowserView browserView;

        public WindowMain()
        {
            this.Loaded += delegate
            {

                int viewHeight = 20000;
                // Disables GPU process and changes maximum texture size
                // value from default 16384 to viewHeight. The maximum texture size value
                // indicates the maximum height of the canvas where Chromium
                // renders web page's content. If the web page's height
                // exceeds the maximum texture size, the part of outsize the
                // texture size will not be drawn and will be filled with
                // black color.
                String[] switches = {
                "--disable-gpu",
                "--disable-gpu-compositing",
                "--max-texture-size=" + viewHeight
                };
                BrowserPreferences.SetChromiumSwitches(switches);

                SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
                
                browserView = new WPFBrowserView(BrowserFactory.Create(BrowserType.LIGHTWEIGHT));
          //      Content = browserView;

                Browser browser = browserView.Browser;


                // #1 Set browser initial size
                browserView.Browser.SetSize(1280, 1024);

                // #2 Load web page and wait until web page is loaded completely.
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                FinishLoadingFrameHandler listener = new FinishLoadingFrameHandler((object sender, FinishLoadingEventArgs e) =>
                {
                    if (e.IsMainFrame)
                    {
                        resetEvent.Set();
                    }
                });
                browser.FinishLoadingFrameEvent += listener;
                try
                {
                    browser.LoadURL("teamdev.com/dotnetbrowser");
                    resetEvent.WaitOne(new TimeSpan(0, 0, 45));
                }
                finally
                {
                    browser.FinishLoadingFrameEvent -= listener;
                }

                // #3 Set the required document size.
                JSValue documentHeight = browserView.Browser.ExecuteJavaScriptAndReturnValue(
                        "Math.max(document.body.scrollHeight, " +
                        "document.documentElement.scrollHeight, document.body.offsetHeight, " +
                        "document.documentElement.offsetHeight, document.body.clientHeight, " +
                        "document.documentElement.clientHeight);");
                JSValue documentWidth = browserView.Browser.ExecuteJavaScriptAndReturnValue(
                        "Math.max(document.body.scrollWidth, " +
                        "document.documentElement.scrollWidth, document.body.offsetWidth, " +
                        "document.documentElement.offsetWidth, document.body.clientWidth, " +
                        "document.documentElement.clientWidth);");

                int scrollBarSize = 25;

                int viewWidth = (int)documentWidth.GetNumber() + scrollBarSize;
                viewHeight = (int)documentHeight.GetNumber() + scrollBarSize;
                var viewSize = new System.Drawing.Size(viewWidth, viewHeight);

                // #4 Register OnRepaint to get notifications
                // about paint events. We expect that web page will be completely rendered twice:
                // 1. When its size is updated.
                // 2. When HTML content is loaded and displayed.
                DrawingView drawingView = (DrawingView)browserView.GetInnerView();
                drawingView.OnRedraw += delegate(object sender, OnRedrawEventArgs e)
                {
                    // Make sure that all view content has been repainted.
                    if (e.UpdatedRect.Size.Equals(viewSize))
                    {
                        browserView.GetImage().Save(@"teamdev.png", ImageFormat.Png);
                        Application.Current.Shutdown();
                    }
                };
                browserView.Browser.SetSize(viewWidth, viewHeight);
            };
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
