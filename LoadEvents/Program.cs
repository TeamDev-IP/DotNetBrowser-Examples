using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser;
using DotNetBrowser.Events;
using System.Threading;

namespace LoadEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Browser browser = BrowserFactory.Create())
            {
                ManualResetEvent waitEvent = new ManualResetEvent(false);
                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    Console.Out.WriteLine("FinishLoadingFrame: URL = " + e.ValidatedURL +
                        ", IsMainFrame = " + e.IsMainFrame);
                    waitEvent.Set();
                };
                browser.StartLoadingFrameEvent += delegate(object sender, StartLoadingArgs e)
                {
                    Console.Out.WriteLine("StartLoadingFrame: URL = " + e.ValidatedURL +
                        ", IsMainFrame = " + e.IsMainFrame);
                };
                browser.DocumentLoadedInMainFrameEvent += delegate(object sender, LoadEventArgs e)
                {
                    Console.Out.WriteLine("DocumentLoadedInMainFrame");
                };
                browser.LoadURL("http://www.google.com");
                waitEvent.WaitOne();
            }
        }
    }
}
