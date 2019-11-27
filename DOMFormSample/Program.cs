using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DOMFormSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                Browser browser = BrowserFactory.Create();
                browserView = new WPFBrowserView(browser);

                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        DOMDocument document = e.Browser.GetDocument();
                        DOMInputElement firstName = (DOMInputElement)document.GetElementByName("firstName");
                        DOMInputElement lastName = (DOMInputElement)document.GetElementByName("lastName");
                        DOMInputElement agreement = (DOMInputElement)document.GetElementByName("agreement");
                        firstName.Value = "John";
                        lastName.Value = "Doe";
                        agreement.Checked = true;
                    }
                };

                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadHTML("<html><body><form name=\"myForm\">" +
                "First name: <input type=\"text\" name=\"firstName\"/><br/>" +
                "Last name: <input type=\"text\" name=\"lastName\"/><br/>" +
                "<input type='checkbox' name='agreement' value='agreed'>I agree<br>" +
                "<input type=\"button\" value=\"Save\"/>" +
                "</form></body></html>");
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
