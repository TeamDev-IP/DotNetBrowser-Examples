using DotNetBrowser;
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

namespace WPF.GestureEventSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            label.Content = "Hold Ctrl key and make two finger tap on the screen.";
            label.HorizontalContentAlignment = HorizontalAlignment.Center;

            browserView.GestureEvent += Browser_GestureEvent;

            browserView.Browser.LoadURL("https://www.teamdev.com/dotnetbrowser");
        }

        private void Browser_GestureEvent(object sender, DotNetBrowser.Events.GestureEventArgs e)
        {
            if (e.IsCtrlDown && e.GestureType == GestureType.TWO_FINGER_TAP)
            {
                MessageBox.Show("Success!");
            }
        }

        private void Browser_HandleGestureEvent(object sender, DotNetBrowser.Events.HandledGestureEventArgs e)
        {
            e.Handled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            browserView.Browser.Dispose();
            browserView.Dispose();
        }

        private void HandleGestureEventsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            browserView.Browser.HandleGestureEvent += Browser_HandleGestureEvent;
        }

        private void HandleGestureEventsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            browserView.Browser.HandleGestureEvent -= Browser_HandleGestureEvent;
        }
    }
}
