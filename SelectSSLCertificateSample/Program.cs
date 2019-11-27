using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SelectSSLCertificateSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private class MyDialogHandler : WPFDefaultDialogHandler
            {
                public MyDialogHandler(UIElement component) : base(component)
                { }

                public override CloseStatus OnSelectCertificate(CertificatesDialogParams parameters)
                {
                    List<Certificate> certificates = parameters.Certificates;
                    foreach (var cert in certificates)
                    {
                        Console.Out.WriteLine(cert);
                    }
                    return base.OnSelectCertificate(parameters);
                }
            }

            private WPFBrowserView browserView;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;

                Browser browser = BrowserFactory.Create(browserContext);

                browser.DialogHandler = new MyDialogHandler(this);

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("<URL that causes Select SSL Certificate dialog>");
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
}
