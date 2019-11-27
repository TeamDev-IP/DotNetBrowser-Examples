using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ExecuteCommandSample
{
    /// <summary>
    /// This sample demonstrates how to execute Browser commands such as Cut, Copy,
    /// Paste, Undo, SelectAll, InsertText etc.
    /// </summary>
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;

                Browser browser = BrowserFactory.Create(browserContext);

                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        // Inserts "TeamDev DotNetBrowser" text into Google Search field.
                        browser.ExecuteCommand(EditorCommand.INSERT_TEXT, "TeamDev DotNetBrowser");
                        // Inserts a new line into Google Search field to simulate Enter.
                        browser.ExecuteCommand(EditorCommand.INSERT_NEW_LINE);
                    }

                };

                browserView = new WPFBrowserView(browser);
                Content = browserView;

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://www.google.com");
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
