using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AccessingHTTPResponseData
{
    class Program
    {
        private class SampleNetworkDelegate : DefaultNetworkDelegate
        {
            public override void OnDataReceived(DataReceivedParams parameters) {
                
                if (parameters.MimeType.Equals("text/html"))
                {
                    Console.WriteLine("MimeType = " + parameters.MimeType + "\n");
                    Console.WriteLine("Charset = " + parameters.Charset + "\n");
                    Console.WriteLine("Data = ");
                    String data = null;
                    for (int i = 0; i < parameters.Data.Length; i++)
                    {                        
                        data += (char)parameters.Data[i];                        
                    }
                    Console.WriteLine(data);
                }                
            }           
        }

        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;
                browserContext.NetworkService.NetworkDelegate = new SampleNetworkDelegate();

                Browser browser = BrowserFactory.Create(browserContext);
                browserView = new WPFBrowserView(browser);

                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("teamdev.com");
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
