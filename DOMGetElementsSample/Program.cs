using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DOMGetElementsSample
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
                        DOMDocument document = e.Browser.GetDocument();
                        List<DOMNode> divs = document.GetElementsByTagName("div");
                        foreach (DOMNode div in divs)
                        {
                            if (div.NodeType == DOMNodeType.ElementNode)
                            {
                                DOMElement divElement = (DOMElement)div;
                                Console.Out.WriteLine(@"class = {0}; offsetTop = {1}; offsetLeft = {2}; offsetWidth = {3}; offsetHeight = {4}
                                                        ; clientTop = {5}; clientLeft = {6}; clientWidth = {7}; clientHeight = {8}
                                                        ; scrollTop = {9}; scrollLeft = {10}; scrollWidth = {11}; scrollHeight = {12}"
                                    , divElement.GetAttribute("class")
                                    , divElement.OffsetTop
                                    , divElement.OffsetLeft
                                    , divElement.OffsetWidth
                                    , divElement.OffsetHeight
                                    , divElement.ClientTop
                                    , divElement.ClientLeft
                                    , divElement.ClientWidth
                                    , divElement.ClientHeight
                                    , divElement.ScrollTop
                                    , divElement.ScrollLeft
                                    , divElement.ScrollWidth
                                    , divElement.ScrollHeight
                                    );
                            }
                        }
                        waitEvent.Set();
                    }
                };
                browser.LoadURL("http://www.google.com");
                waitEvent.WaitOne();
                Thread.Sleep(100000);
            }
        }
    }
}
