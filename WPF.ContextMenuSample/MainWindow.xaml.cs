using DotNetBrowser;
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

namespace WPF.ContextMenuSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
            webView.Browser.ContextMenuHandler = new MyContextMenuHandler((FrameworkElement)webView, true);
            webView.Browser.LoadURL("google.com");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!webView.IsDisposed)
            {
                webView.Browser.Dispose();
                webView.Dispose();
            }
        }


        private class MyContextMenuHandler : ContextMenuHandler
        {                     
            FrameworkElement view;
            bool IsShow;

            public MyContextMenuHandler(FrameworkElement view, bool IsShow)
            {
                this.view = view;
                this.IsShow = IsShow;
            }


            private MenuItem BuildMenuItem(string item, bool isEnabled, Visibility IsVisible, RoutedEventHandler clickHandler)
            {
                MenuItem result = new MenuItem();
                result.Header = item;
                result.Visibility = Visibility.Collapsed;
                result.Visibility = IsVisible;
                result.IsEnabled = isEnabled;
                result.Click += clickHandler;

                return result;
            }


            public void ShowContextMenu(ContextMenuParams parameters)
            {
                view.Dispatcher.BeginInvoke(new Action(() =>
                {
                    System.Windows.Controls.ContextMenu popupMenu = new System.Windows.Controls.ContextMenu();

                    if (!String.IsNullOrEmpty(parameters.LinkText))
                    {
                        popupMenu.Items.Add(BuildMenuItem("Open link in new window", true, Visibility.Visible, delegate
                        {
                            String linkURL = parameters.LinkURL;
                            Console.Out.WriteLine("linkURL = " + linkURL);
                        }));
                    }

                    Browser browser = parameters.Browser;

                    popupMenu.Items.Add(BuildMenuItem("Reload", true, Visibility.Visible, delegate
                    {
                        browser.Reload();
                    }));
                    
                    popupMenu.IsOpen = true;
                }));
            }
        }
    }
}
