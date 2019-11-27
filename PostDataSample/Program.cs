using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostDataSample
{
    /// <summary>
    /// This sample demonstrates how to read and modify POST parameters using NetworkDelegate.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            LoggerProvider.Instance.LoggingEnabled = true;
            LoggerProvider.Instance.ConsoleLoggingEnabled = true;

            Browser browser = BrowserFactory.Create();
            BrowserContext browserContext = browser.Context;
            NetworkService networkService = browserContext.NetworkService;
            networkService.NetworkDelegate = new SampleNetworkDelegate();

            browser.LoadURL(new LoadURLParams("http://localhost/", "key=value", "Content-Type:text/plaint\n"));
        }

        private class SampleNetworkDelegate : DefaultNetworkDelegate
        {
            public override void OnBeforeURLRequest(BeforeURLRequestParams parameters)
            {
                if ("POST" == parameters.Method)
                {
                    PostData post = parameters.PostData;
                    PostDataContentType contentType = post.ContentType;
                    if (contentType == PostDataContentType.FORM_URL_ENCODED)
                    {
                        FormData postData = (FormData)post;
                        postData.SetPair("key1", "value1", "value2");
                        postData.SetPair("key2", "value2");
                    }
                    else if (contentType == PostDataContentType.MULTIPART_FORM_DATA)
                    {
                        MultipartFormData postData = (MultipartFormData)post;
                        postData.SetPair("key1", "value1", "value2");
                        postData.SetPair("key2", "value2");
                        postData.SetFilePair("file3", "C:\\Test.zip");
                    }
                    else if (contentType == PostDataContentType.PLAIN_TEXT)
                    {
                        RawData postData = (RawData)post;
                        postData.Data = "raw data";
                    }
                    else if (contentType == PostDataContentType.BYTES)
                    {
                        BytesData data = (BytesData)post;
                        data.Data = Encoding.UTF8.GetBytes("My data");
                    }
                    parameters.PostData = post;
                }
            }
        }
    }
}
