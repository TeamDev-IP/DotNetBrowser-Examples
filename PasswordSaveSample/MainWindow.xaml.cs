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
using DotNetBrowser;

namespace PasswordSaveSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BrowserView.Browser.PasswordManagerClient.PasswordSubmitted += PasswordManagerClient_PasswordSubmitted;
            BrowserView.Browser.PasswordManagerClient.UpdatePasswordSubmitted += PasswordManagerClient_UpdatePasswordSubmitted;
            BrowserView.Browser.LoadURL("http://demo.hongkiat.com/html5-loginpage/index.html");
        }

        void PasswordManagerClient_PasswordSubmitted(object sender, PasswordEventArgs e)
        {
            var window = this;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var result = MessageBox.Show(window, 
                    "Do you want to save password for " + e.Login + "?\nWebsite: " + e.Url, 
                    "Save Password", 
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question,
                    MessageBoxResult.Cancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        e.Action = SaveAction.Save;
                        break;
                    case MessageBoxResult.No:
                        e.Action = SaveAction.Blacklist;
                        break;
                }
            }));
        }

        void PasswordManagerClient_UpdatePasswordSubmitted(object sender, PasswordEventArgs e)
        {
            var window = this;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var result = MessageBox.Show(window,
                    "Do you want to update password for " + e.Login + "?\nWebsite: " + e.Url,
                    "Update Password",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Cancel);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        e.Action = SaveAction.Update;
                        break;
                }
            }));
        }

    }
}
