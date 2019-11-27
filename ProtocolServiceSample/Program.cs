using System;
using System.Text;
using System.Threading;
using DotNetBrowser;
using DotNetBrowser.Protocols;

namespace ProtocolServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Browser browser = BrowserFactory.Create())
            {
                //Event for detecting if the page is loaded
                ManualResetEvent loadedEvent = new ManualResetEvent(false);

                browser.FinishLoadingFrameEvent += (finishSender, finishArgs) =>
                {
                    if (finishArgs.IsMainFrame)
                    {
                        Console.WriteLine(browser.GetHTML());
                        loadedEvent.Set();
                    }
                };

                //Registering the handler for the specified protocol
                browser.Context.ProtocolService.Register("https", new HttpsHandler());

                //Loading Url with the same protocol as registered
                browser.LoadURL("https://request.url");

                //Waiting the page loading
                loadedEvent.WaitOne();
            }

            Console.ReadKey();
        }
    }


    //The instance of this type will handle the requests of the specified protocol
    public class HttpsHandler : IProtocolHandler
    {
        //This method should provide the response for the specified request
        public IUrlResponse Handle(IUrlRequest request)
        {
            string htmlContent = "Request Url: " + request.Url + "\n";
            return new UrlResponse(Encoding.UTF8.GetBytes(htmlContent));
        }
    }
}
