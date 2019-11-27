using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DownloadSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            class SampleDownloadHandler : DownloadHandler
            {
                private ManualResetEvent waitEvent = new ManualResetEvent(false);

                public event EventHandler DownloadUpdated;

                public bool AllowDownload(DownloadItem download)
                {
                    download.DownloadEvent += delegate(object sender, DownloadEventArgs e)
                    {
                        Console.Clear();
                        downloadItem = e.Item;
                        Console.Out.WriteLine("Destination file: " +
                           download.DestinationFile);
                        if (downloadItem.Completed)
                        {
                            if (downloadItem.Canceled)
                            {
                                Console.Out.WriteLine("Download is canceled!");
                            }
                            else
                            {
                                Console.Out.WriteLine("Download is completed!");
                            }
                            waitEvent.Set();
                        }
                        else
                        {
                            Console.Out.Write("Complete: " +
                                   download.PercentComplete + "%");

                            if (downloadItem.Paused)
                            {
                                Console.Out.Write(" - Download is paused");
                            }
                        }

                        if (DownloadUpdated != null)
                        {
                            DownloadUpdated.Invoke(sender, new EventArgs());
                        }

                    };
                    return true;
                }

                public void Wait()
                {
                    waitEvent.WaitOne();
                }
            }


            private Button pauseButton;
            private Button resumeButton;
            private Button cancelButton;

            private Grid layout;
            Browser browser;
            private static DownloadItem downloadItem = null;

            public WindowMain()
            {
                BrowserContext browserContext = BrowserContext.DefaultContext;

                browser = BrowserFactory.Create(browserContext);

                var downloadHandler = new SampleDownloadHandler();
                downloadHandler.DownloadUpdated += delegate
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        CheckButton();
                    }));

                };
                browser.DownloadHandler = downloadHandler;

                layout = new Grid();
                ColumnDefinition gridCol1 = new ColumnDefinition();
                layout.ColumnDefinitions.Add(gridCol1);
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                RowDefinition gridRow2 = new RowDefinition();
                gridRow2.Height = new GridLength(45);
                RowDefinition gridRow3 = new RowDefinition();
                gridRow3.Height = new GridLength(45);

                layout.RowDefinitions.Add(gridRow1);
                layout.RowDefinitions.Add(gridRow2);
                layout.RowDefinitions.Add(gridRow3);

                Content = layout;

                pauseButton = new Button();
                pauseButton.Content = "Pause";
                pauseButton.Height = 23;
                pauseButton.Click += pauseButton_Click;
                Grid.SetRow(pauseButton, 0);
                Grid.SetColumn(pauseButton, 0);

                resumeButton = new Button();
                resumeButton.Content = "Resume";
                resumeButton.Height = 23;
                resumeButton.Click += resumeButton_Click;
                Grid.SetRow(resumeButton, 1);
                Grid.SetColumn(resumeButton, 0);

                cancelButton = new Button();
                cancelButton.Content = "Cancel";
                cancelButton.Height = 23;
                cancelButton.Click += cancelButton_Click;
                Grid.SetRow(cancelButton, 2);
                Grid.SetColumn(cancelButton, 0);


                layout.Children.Add(pauseButton);
                layout.Children.Add(resumeButton);
                layout.Children.Add(cancelButton);

                Width = 400;
                Height = 300;
                this.Loaded += WindowMain_Loaded;

                this.Closed += delegate
                {
                    browser.Dispose();
                };
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browser.LoadURL("ftp://ftp.teamdev.com/updates/jxbrowser-4.0-beta.zip");
            }

            void pauseButton_Click(object sender, RoutedEventArgs e)
            {
                downloadItem.Pause();
            }

            void resumeButton_Click(object sender, RoutedEventArgs e)
            {
                downloadItem.Resume();
            }

            void cancelButton_Click(object sender, RoutedEventArgs e)
            {
                downloadItem.Cancel();
            }

            private void CheckButton()
            {
                if (downloadItem.Canceled || downloadItem.Completed)
                {
                    pauseButton.IsEnabled = false;
                    resumeButton.IsEnabled = false;
                    cancelButton.IsEnabled = false;
                }
                else if (downloadItem.Paused)
                {
                    pauseButton.IsEnabled = false;
                    resumeButton.IsEnabled = true;
                    cancelButton.IsEnabled = true;
                }
                else 
                {
                    pauseButton.IsEnabled = true;
                    resumeButton.IsEnabled = false;
                    cancelButton.IsEnabled = true;
                }
            }
 
            [STAThread]
            public static void Main()
            {
                Application app = new Application();

                WindowMain wnd = new WindowMain();
                app.Run(wnd);
            }
        }
    }

}
