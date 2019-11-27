using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.KeyboardEventSimulateSample
{
   /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         Browser browser;
        WPFBrowserView browserView;

        public MainWindow()
        {
            // Initialize WPF Application UI.
            InitializeComponent();

            // Create WPF BrowserView component.
            browser = BrowserFactory.Create();
            browserView = new WPFBrowserView(browser);
            // Embed BrowserView component into main layout.
            mainLayout.Children.Add(browserView);
            browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {

                    int key = KeyInterop.VirtualKeyFromKey(Key.H);
                    KeyParams paramers = new KeyParams((VirtualKeyCode)key, 'H');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    key = KeyInterop.VirtualKeyFromKey(Key.L);
                    paramers = new KeyParams((VirtualKeyCode)key, 'e');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    key = KeyInterop.VirtualKeyFromKey(Key.L);
                    paramers = new KeyParams((VirtualKeyCode)key, 'l');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    key = KeyInterop.VirtualKeyFromKey(Key.L);
                    paramers = new KeyParams((VirtualKeyCode)key, 'l');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    key = KeyInterop.VirtualKeyFromKey(Key.O);
                    paramers = new KeyParams((VirtualKeyCode)key, 'o');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                }
            };

            browserView.Browser.LoadHTML(@"<html>
                                            <body>
                                                <input type='text' autofocus></input>
                                            </body>
                                           </html>");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Dispose BrowserView when close app window.
            browserView.Dispose();
            browser.Dispose();
        }
    }
}
