using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DOMGetAttributesSample
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
                    if (e.IsMainFrame)
                    {
                        DOMDocument document = e.Browser.GetDocument();
                        DOMElement link = document.GetElementById("link");
                        Dictionary<String, String> attributes = link.Attributes;
                        foreach (var attribute in attributes)
                        {
                            Console.Out.WriteLine(attribute.Key + " = " + attribute.Value);
                        }
                        waitEvent.Set();
                    }
                };
                browser.LoadHTML("<html><body><a href='#' id='link' title='link title'></a></body></html>");
                waitEvent.WaitOne();
            }
        }
    }
}
