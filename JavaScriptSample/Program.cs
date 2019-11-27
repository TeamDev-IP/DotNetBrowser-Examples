using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JavaScriptSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Browser browser = BrowserFactory.Create();

            // Executes the passed JavaScript code asynchronously.
            browser.ExecuteJavaScript("document.write('<html><title>" +
                    "My Title</title><body><h1>Hello from DotNetBrowser!</h1></body></html>');");

            // Executes the passed JavaScript code and returns the result value.
            JSValue documentTitle = browser.ExecuteJavaScriptAndReturnValue("document.title");
            // Make sure that result value is a string and read its value
            if (documentTitle.IsString())
            {
                Console.Out.WriteLine("Document Title = " + documentTitle.GetString());
            }

            JSValue documentContent = browser.ExecuteJavaScriptAndReturnValue("document.body.innerHTML");
            if (documentContent.IsString())
            {
                Console.Out.WriteLine("New content: " + documentContent.GetString());
            }

        }
    }
}
