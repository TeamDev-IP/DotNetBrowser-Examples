using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MuteAudioSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private Button muteButton;
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

                layout.RowDefinitions.Add(gridRow1);
                layout.RowDefinitions.Add(gridRow2);

                Content = layout;

                browserView = new WPFBrowserView(BrowserFactory.Create());
                Grid.SetRow(browserView, 1);
                Grid.SetColumn(browserView, 0);

                muteButton = new Button();
                UpdateButtonText(muteButton, browserView.Browser);
                muteButton.Height = 23;
                muteButton.Click += muteButton_Click;
                Grid.SetRow(muteButton, 0);
                Grid.SetColumn(muteButton, 0);

                layout.Children.Add(muteButton);
                layout.Children.Add(browserView);

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            private void muteButton_Click(object sender, RoutedEventArgs e)
            {
                browserView.Browser.AudioMuted = !browserView.Browser.AudioMuted;
                UpdateButtonText(muteButton, browserView.Browser);
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadURL("https://www.youtube.com/");
            }

            private static void UpdateButtonText(Button button, Browser browser) 
            {
                Application.Current.Dispatcher.BeginInvoke((Action) (() => {
        
                button.Content = browser.AudioMuted ? "Unmute Audio" : "Mute Audio";
            }));

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
