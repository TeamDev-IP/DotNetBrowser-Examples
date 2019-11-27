using DotNetBrowser;
using DotNetBrowser.Events;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace HTMLToImageSample.OffScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            int viewWidth = 1024;
            int viewHeight = 20000;
            string[] switches = {
                    "--disable-gpu",
                    "--max-texture-size=" + viewHeight
            };
            BrowserPreferences.SetChromiumSwitches(switches);
            Browser browser = BrowserFactory.Create(BrowserType.LIGHTWEIGHT);

            browser.SetSize(viewWidth, viewWidth);
            ManualResetEvent waitEvent = new ManualResetEvent(false);
            browser.FinishLoadingFrameEvent += delegate (object sender, FinishLoadingEventArgs e)
            {
                // Wait until main document of the web page is loaded completely.
                if (e.IsMainFrame)
                {
                    waitEvent.Set();
                }
            };
            browser.LoadURL("teamdev.com/dotnetbrowser");
            waitEvent.WaitOne();

            // #3 Set the required document size.
            JSValue documentHeight = browser.ExecuteJavaScriptAndReturnValue(
                    "Math.max(document.body.scrollHeight, " +
                    "document.documentElement.scrollHeight, document.body.offsetHeight, " +
                    "document.documentElement.offsetHeight, document.body.clientHeight, " +
                    "document.documentElement.clientHeight);");
            JSValue documentWidth = browser.ExecuteJavaScriptAndReturnValue(
                    "Math.max(document.body.scrollWidth, " +
                    "document.documentElement.scrollWidth, document.body.offsetWidth, " +
                    "document.documentElement.offsetWidth, document.body.clientWidth, " +
                    "document.documentElement.clientWidth);");

            int scrollBarSize = 25;

            viewWidth = (int)documentWidth.GetNumber() + scrollBarSize;
            viewHeight = (int)documentHeight.GetNumber() + scrollBarSize;

            Debug.WriteLine("GetImage: {0} x {1}", viewWidth, viewHeight);

            Image img = browser.ImageProvider.GetImage(viewWidth, viewHeight);
            img.Save(@"teamdev.png", System.Drawing.Imaging.ImageFormat.Png);

            browser.Dispose();
        }
    }
}
