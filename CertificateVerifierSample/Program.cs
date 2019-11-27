using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CertificateVerifierSample
{
    /// <summary>
    /// The sample demonstrates how to accept/reject SSL certificates using
    /// custom SSL certificate verifier.
    /// </summary>
    public class WindowMain : System.Windows.Window
    {
        private WPFBrowserView browserView;

        class TestCertificateVerifier : CertificateVerifier
        {
            public CertificateVerifyResult Verify(CertificateVerifyParams parameters)
            {
                // Reject SSL certificate for all "google.com" hosts.
                if (parameters.HostName.Contains("google.com"))
                {
                    return CertificateVerifyResult.INVALID;
                }
                return CertificateVerifyResult.OK;

            }
        }
        public WindowMain()
        {
            BrowserContext browserContext = BrowserContext.DefaultContext;
            Browser browser = BrowserFactory.Create(browserContext);

            //add custom request handler
            browser.Context.NetworkService.CertificateVerifier = new TestCertificateVerifier();

            browserView = new WPFBrowserView(browser);
            Content = browserView;

            Width = 1024;
            Height = 768;
            this.Loaded += WindowMain_Loaded;
        }

        void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            browserView.Browser.LoadURL("http://google.com");
        }

        [STAThread]
        public static void Main()
        {
            Application app = new Application();

            WindowMain wnd = new WindowMain();
            app.Run(wnd);

            var browser = wnd.browserView.Browser;
            wnd.browserView.Dispose();
            browser.Dispose();
        }
    }
}
