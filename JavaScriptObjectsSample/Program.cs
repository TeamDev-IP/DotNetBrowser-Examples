using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptObjectsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Browser browser = BrowserFactory.Create();

            JSValue document = browser.ExecuteJavaScriptAndReturnValue("document");
            if (document.IsObject())
            {
                // document.title = "New Title"
                document.AsObject().SetProperty("title", "New Title");

                // document.write("Hello World!")
                JSValue write = document.AsObject().GetProperty("write");
                if (write.IsFunction())
                {
                    write.AsFunction().Invoke(document.AsObject(), "Hello World!");
                }
            }

            JSValue documentContent = browser.ExecuteJavaScriptAndReturnValue("document.body.innerText");
            if (documentContent.IsString())
            {
                Console.Out.WriteLine("New content: " + documentContent.GetString());
            }

        }
    }
}
