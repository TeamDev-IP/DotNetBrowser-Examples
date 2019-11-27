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
using System.Windows.Controls.Primitives;

namespace NotificationSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;
            private Button btnCloseNotification;
            private Grid layout;
            private Notification notification;

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

                btnCloseNotification = new Button();
                btnCloseNotification.Content = "Close";
                btnCloseNotification.Height = 23;
                btnCloseNotification.Click += btnCloseNotification_Click;
                Grid.SetRow(btnCloseNotification, 0);
                Grid.SetColumn(btnCloseNotification, 0);

                Browser browser = BrowserFactory.Create();

                browser.PermissionHandler = new MyPermissionHandler();
                browser.Context.NotificationService.NotificationHandler = new MyNotificationHandler((NotificationEventArgs e) =>
                {
                    notification = e.Notification;
                    ShowNotification(notification);
                });

                browserView = new WPFBrowserView(browser);

                Grid.SetRow(browserView, 2);
                Grid.SetColumn(browserView, 0);

                layout.Children.Add(btnCloseNotification);
                layout.Children.Add(browserView);

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            private void btnCloseNotification_Click(object sender, RoutedEventArgs e)
            {
                notification.Close();
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadHTML(@"
<!DOCTYPE html>
<html>
<head>
    <title>Notifications Sample</title>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
</head>
<body>
    <script type='text/javascript'>

        let notification;

        let showNotification = function() {
            if (Notification.permission === 'granted') {
                _showNotification();
            } else {
                Notification.requestPermission(function (permission) {
                    if (permission === 'granted') {
                        _showNotification();
                    }
                });
            }
        };

        let _showNotification = function() {
            notification = new Notification('Title from JS:', {type: 0, body: 'Message from JS.'});
        };

        let closeNotification = function() {
            if (notification) {
                notification.close();
            }
        };

    </script>
    <div>
        <button onclick='showNotification()'>JS Show notification</button>
        <button onclick='closeNotification()'>JS Close notification</button>
    </div>
</body>
</html>"
                    );
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

            static void ShowNotification(Notification notification)
            {
                Window window = new Window();

                window.Top = 0;
                window.Left = 0;
                window.Width = 400;
                window.Height = 200;


                window.Closed += delegate
                {
                    notification.Close();
                };

                notification.OnClose += delegate
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        window.Content = null;
                        window.Hide();
                        window.Close();
                    }));
                };

                window.Content = notification.Message;
                window.Show();

            }

            class MyPermissionHandler : IPermissionHandler
            {
                public PermissionStatus OnRequestPermission(PermissionRequest request)
                {
                    if (request.Type == PermissionType.NOTIFICATIONS)
                    {
                        return PermissionStatus.GRANTED;
                    }
                    return PermissionStatus.DENIED;
                }
            }

            class MyNotificationHandler : INotificationHandler
            {
                private Action<NotificationEventArgs> action;

                public MyNotificationHandler(Action<NotificationEventArgs> action)
                {
                    this.action = action;
                }

                public void OnShowNotification(NotificationEventArgs args)
                {
                    Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        this.action(args);
                    }));
                }

            }
        }
    }
}
