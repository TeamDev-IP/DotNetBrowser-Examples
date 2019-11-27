using DotNetBrowser;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/**
 * The sample demonstrates how to configure Browser instance to use custom proxy settings.
 * By default Browser instance uses system proxy settings.
 */
namespace ProxySample
{
    class Program
    {
        static void Main(string[] args)
        {
            String dataDir = Path.GetFullPath("chromium-data");
            BrowserContextParams contextParams = new BrowserContextParams(dataDir);

            // Browser will automatically detect proxy settings.
            //    contextParams.ProxyConfig = new AutoDetectProxyConfig();

            // Browser will not use a proxy server.
            //    contextParams.ProxyConfig = new DirectProxyConfig();

            // Browser will use proxy settings received from proxy auto-config (PAC) file.
            //contextParams.ProxyConfig = new URLProxyConfig("<pac-file-url>");

            // Browser will use custom user's proxy settings.
            String proxyRules = "http=foo:80;https=foo:80;ftp=foo:80;socks=foo:80";
            String exceptions = "<local>";  // bypass proxy server for local web pages
            contextParams.ProxyConfig = new CustomProxyConfig(proxyRules, exceptions);

            // Creates Browser instance with context configured to use specified proxy settings.
            Browser browser = BrowserFactory.Create(new BrowserContext(contextParams));
            // Handle proxy authorization.
            browser.Context.NetworkService.NetworkDelegate = new MyNetworkDelegate();

            ManualResetEvent loadedEvent = new ManualResetEvent(false);
            ManualResetEvent failedEvent = new ManualResetEvent(false);
            browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    Log(e.Browser, "FINISH - " + e.Browser.Title);
                    loadedEvent.Set();
                    failedEvent.Set();
                }
            };

            browser.FailLoadingFrameEvent += delegate(object sender, FailLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    Log(e.Browser, "FAIL - " + e.ErrorDescription);
                    failedEvent.Set();
                }
            };


            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();

            browser.Context.ProxyConfig = new SystemProxyConfig();
            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();

            browser.Context.ProxyConfig = new AutoDetectProxyConfig();
            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();

            browser.Context.ProxyConfig = new DirectProxyConfig();
            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();

            browser.Context.ProxyConfig = new URLProxyConfig("http://test/"); ;
            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();

            browser.Context.ProxyConfig = new CustomProxyConfig(proxyRules, exceptions);
            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();

            browser.Context.ProxyConfig = new CustomProxyConfig(proxyRules, "https://www.teamdev.com");
            browser.LoadURL("https://www.teamdev.com/");
            failedEvent.WaitOne(new TimeSpan(0, 0, 30));
            loadedEvent.WaitOne(new TimeSpan(0, 0, 30));
            failedEvent.Reset();
            loadedEvent.Reset();
            Thread.Sleep(new TimeSpan(0, 0, 30));
            browser.Dispose();
        }

        private static void Log(Browser browser, string status)
        {
            var proxyConfig = browser.Context.ProxyConfig;
            string proxyType = "";
            string proxyAutoConfigFileURL = "";
            string proxyRules = "";
            string exceptions = "";

            if (proxyConfig is SystemProxyConfig)
            {
                proxyType = "SystemProxy";
            }
            if (proxyConfig is AutoDetectProxyConfig)
            {
                proxyType = "AutoDetectProxy";
            }
            if (proxyConfig is DirectProxyConfig)
            {
                proxyType = "DirectProxy";
            }
            if (proxyConfig is URLProxyConfig)
            {
                proxyType = "URLProxy";
                proxyAutoConfigFileURL = ((URLProxyConfig)proxyConfig).ProxyAutoConfigFileURL;
            }
            if (proxyConfig is CustomProxyConfig)
            {
                proxyType = "CustomProxy";
                proxyRules = ((CustomProxyConfig)proxyConfig).ProxyRules;
                exceptions = ((CustomProxyConfig)proxyConfig).Exceptions;
            }

            Console.Out.WriteLine(@"ProxyType : '{0}' | ProxyAutoConfigFileURL : '{1}' | ProxyRules : '{2}' | Exceptions : '{3}' | Status : '{4}'"
                                , proxyType, proxyAutoConfigFileURL, proxyRules, exceptions, status);

        }

        private class MyNetworkDelegate : DefaultNetworkDelegate
        {
            public override bool OnAuthRequired(AuthRequiredParams parameters)
            {
                // If proxy server requires login/password, provide it programmatically.
                if (parameters.IsProxy)
                {
                    parameters.Username = "proxy-username";
                    parameters.Password = "proxy-password";
                    return false;
                }
                return true;
            }
        }
    }


}
