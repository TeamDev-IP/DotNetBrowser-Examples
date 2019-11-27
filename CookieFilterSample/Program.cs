using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieFilterSample
{
    /// <summary>
    /// The sample demonstrates how to suppress/filter incoming and outgoing cookies.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Browser browser = BrowserFactory.Create();

            // Suppress/filter all incoming and outgoing cookies.
            browser.Context.NetworkService.NetworkDelegate = new MyNetworkDelegate();
        }

        private class MyNetworkDelegate : DefaultNetworkDelegate
        {
            public override bool OnCanSetCookies(String url, List<Cookie> cookies)
            {
                return false;
            }

            public override bool OnCanGetCookies(String url, List<Cookie> cookies)
            {
                return false;
            }

        }
    }
}
