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

namespace WPF.FindTextSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            browserView.Browser.LoadURL("google.com");
        }

        private void findButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != String.Empty)
            {
				if (browserView.Browser.FindText(new SearchParams(textBox.Text)).NumberOfMatches == 0)
                {
                    MessageBox.Show("No matches!");
                }
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            browserView.Browser.StopFindingText(StopFindAction.CLEAR_SELECTION);
			textBox.Text = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!browserView.Browser.IsDisposed())
            {
                browserView.Dispose();
                browserView.Browser.Dispose();
            }
        }
    }
}
