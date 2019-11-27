using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.DragDropSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            browserView.Browser.DragEnterEvent += Browser_DragEnterEvent;
            browserView.Browser.DragExitEvent += Browser_DragExitEvent;
            browserView.Browser.DropEvent += Browser_DropEvent;
        }

        private void Browser_DropEvent(object sender, DragDropEventArgs e)
        {
            Log("Drop event");
            PrintEventDetails(e);
        }

        private void Browser_DragExitEvent(object sender, DragDropEventArgs e)
        {
            Log("DragExit event");
            PrintEventDetails(e);
        }

        private void Browser_DragEnterEvent(object sender, DragDropEventArgs e)
        {
            Log("DragEnter event");
            PrintEventDetails(e);
        }

        private void PrintEventDetails(DragDropEventArgs e)
        {
            Log("Data type = " + e.DragDropDataType.ToString());
            Log("Data: ");
            foreach (string data in e.Data)
            {
                Log(data);
            }
            Log("");
        }
        private void Log(string text)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Output.AppendText(text+"\n");
                Output.ScrollToEnd();
            }));
        }
    }
}
