using DotNetBrowser;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindTextSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Browser browser = BrowserFactory.Create())
            {
                ManualResetEvent waitEvent = new ManualResetEvent(false);
                browser.FinishLoadingFrameEvent += delegate (object sender, FinishLoadingEventArgs e)
                {
                    // Wait until main document of the web page is loaded completely.
                    if (e.IsMainFrame)
                    {
                        SearchParams request = new SearchParams("find me");
                        // Find text from the beginning of the loaded web page.
                        SearchResult result = browser.FindText(request);
                        Console.Out.WriteLine(result.CurrentMatch + "/" + result.NumberOfMatches);
                        // Find the same text again from the currently selected match.
                        result = browser.FindText(request);
                        Console.Out.WriteLine(result.CurrentMatch + "/" + result.NumberOfMatches);
                        waitEvent.Set();
                    }
                };

                browser.SetSize(700, 500);
                browser.LoadHTML("<html><body><p>Find me</p><p>Find me</p></body></html>");
                waitEvent.WaitOne();
                Thread.Sleep(5000);
            }
        }
    }
}
