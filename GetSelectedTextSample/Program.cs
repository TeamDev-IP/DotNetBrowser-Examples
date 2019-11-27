using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GetSelectedTextSample
{
    /// <summary>
    /// The sample demonstrates how to get selected text on a web page.
    /// </summary>
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private Button btnSelectedText;
            private Button btnSelectedHtml;
            private WPFBrowserView browserView;
            private Grid layout;

            public WindowMain()
            {
                layout = new Grid();
                ColumnDefinition gridCol1 = new ColumnDefinition();
                layout.ColumnDefinitions.Add(gridCol1);
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                RowDefinition gridRow2 = new RowDefinition();
                gridRow2.Height = new GridLength(45);
                RowDefinition gridRow3 = new RowDefinition();

                layout.RowDefinitions.Add(gridRow1);
                layout.RowDefinitions.Add(gridRow2);
                layout.RowDefinitions.Add(gridRow3);

                Content = layout;

                btnSelectedText = new Button();
                btnSelectedText.Content = "Get Selected Text";
                btnSelectedText.Height = 23;
                btnSelectedText.Click += btnSelectedText_Click;
                Grid.SetRow(btnSelectedText, 0);
                Grid.SetColumn(btnSelectedText, 0);

                btnSelectedHtml = new Button();
                btnSelectedHtml.Content = "Get Selected HTML";
                btnSelectedHtml.Height = 23;
                btnSelectedHtml.Click += btnSelectedHtml_Click;
                Grid.SetRow(btnSelectedHtml, 1);
                Grid.SetColumn(btnSelectedHtml, 0);


                browserView = new WPFBrowserView(BrowserFactory.Create());
                Grid.SetRow(browserView, 2);
                Grid.SetColumn(browserView, 0);

                layout.Children.Add(btnSelectedText);
                layout.Children.Add(btnSelectedHtml);
                layout.Children.Add(browserView);
   
                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("http://www.teamdev.com");
            }

            void btnSelectedText_Click(object sender, RoutedEventArgs e)
            {
                Console.Out.WriteLine(browserView.Browser.GetSelectedText());
            }

            void btnSelectedHtml_Click(object sender, RoutedEventArgs e)
            {
                Console.Out.WriteLine(browserView.Browser.GetSelectedHTML());
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
