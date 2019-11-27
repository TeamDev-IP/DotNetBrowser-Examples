using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * By default all Browser instances with same BrowserContext share
 * cookies and cache data. This sample demonstrates how to create two isolated
 * Browser instances that don't share cookies and cache data.
 */
namespace BrowserContextSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // This Browser instance will store cookies and user data files in "user-data-dir-one" dir.

            String browserOneUserDataDir = Path.GetFullPath("user-data-dir-one");
            Directory.CreateDirectory(browserOneUserDataDir);
            Browser browserOne = BrowserFactory.Create(new BrowserContext(new BrowserContextParams(browserOneUserDataDir)));

            // This Browser instance will store cookies and user data files in "user-data-dir-two" dir.
            String browserTwoUserDataDir = Path.GetFullPath("user-data-dir-two");
            Directory.CreateDirectory(browserTwoUserDataDir);
            Browser browserTwo = BrowserFactory.Create(new BrowserContext(new BrowserContextParams(browserTwoUserDataDir)));

            // The browserOne and browserTwo will not see the cookies and cache data files of each other.
        }
    }
}
