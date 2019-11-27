using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser;
using DotNetBrowser.Events;
using System.Threading;

namespace ExecuteJavaScript
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
                    // Wait until main document of the web page is loaded completely.
                    if (e.IsMainFrame)
                    {
                        // Get HTML of the loaded web page and write it to Console.
                        Console.Out.WriteLine(e.Browser.GetHTML());
                        waitEvent.Set();
                    }
                };
                browser.LoadURL("http://www.teamdev.com");
                waitEvent.WaitOne();
            }
        }
    }
}