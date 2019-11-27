using DotNetBrowser;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Browser browser = BrowserFactory.Create())
            {
                ManualResetEvent waitEvent = new ManualResetEvent(false);

                // Listen to zoom changed events
                browser.Context.ZoomService.ZoomChangedEvent += delegate(object sender, ZoomEventArgs e)
                {
                    Console.Out.WriteLine("e.Url = " + e.Url);
                    Console.Out.WriteLine("e.ZoomLevel = " + e.ZoomLevel);
                };


                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    // Wait until main document of the web page is loaded completely.
                    if (e.IsMainFrame)
                    {
                        e.Browser.ZoomLevel = 2.0;
                        waitEvent.Set();
                    }
                };
                browser.LoadURL("http://www.teamdev.com");
                waitEvent.WaitOne();
            }
        }
    }
}
