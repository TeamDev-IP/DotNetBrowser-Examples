using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Browser browser = BrowserFactory.Create())
            {
                CookieStorage cookieStorage = browser.CookieStorage;
                List<Cookie> cookies = cookieStorage.GetAllCookies();
                foreach (Cookie cookie in cookies)
                {
                    Console.Out.WriteLine("cookie = " + cookie);
                }
            }
        }
    }
}
