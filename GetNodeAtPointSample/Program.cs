using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GetNodeAtPointSample
{
    /// <summary>
    /// This sample demonstrates how to get DOM Node at a specific point on the web page.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Browser browser = BrowserFactory.Create();
            browser.SetSize(700, 500);

            browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    DOMNodeAtPoint nodeAtPoint = browser.NodeAtPoint(50, 50);
                    Console.WriteLine("nodeAtPoint = " + nodeAtPoint.ToString());
                }
            };
            browser.LoadURL("http://www.teamdev.com");
        }
    }
}
