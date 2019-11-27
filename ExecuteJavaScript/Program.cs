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
                        // Execute JavaScript code and get return value from JavaScript.
                        JSValue returnValue = e.Browser.ExecuteJavaScriptAndReturnValue("document.title");
                        // Make sure that return value is a string.
                        if (returnValue.IsString())
                        {
                            // Extract string value from JSValue and write it to Console.
                            Console.Out.WriteLine("The \"document.title\" JavaScript code returns \"" +
                                returnValue.GetString() + "\"");
                        }
                        waitEvent.Set();
                    }
                };
                browser.LoadURL("http://www.google.com");
                waitEvent.WaitOne();
            }
        }
    }
}
