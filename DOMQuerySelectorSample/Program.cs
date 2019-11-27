using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DOMQuerySelectorSample
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
                        DOMElement documentElement = document.DocumentElement;
                        // Get the div with id = "root".
                        DOMNode divRoot = documentElement.QuerySelector("#root");
                        // Get all paragraphs.
                        List<DOMNode> paragraphs = divRoot.QuerySelectorAll("p");

                        foreach (var paragraph in paragraphs)
                        {
                            Console.Out.WriteLine("paragraph.NodeValue = " + ((DOMElement)paragraph).InnerText);
                        }
                        waitEvent.Set();
                    }
                };
                browser.LoadHTML(
                    "<html><body><div id='root'>" +
                    "<p>paragraph1</p>" +
                    "<p>paragraph2</p>" +
                    "<p>paragraph3</p>" +
                    "</div></body></html>");
                waitEvent.WaitOne();
            }
        }
    }
}
