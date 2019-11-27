using System;
using System.IO;
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
using DotNetBrowser.WPF;

namespace WPF.LoadHTML
{
    /// <summary>
    /// Demonstrates how to embed WPF BrowserView component into WPF Application, 
    /// load and display HTML content from string.
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
            // Load HTML from string into BrowserView.
            browser.FinishLoadingFrameEvent += (s, e) =>
                {
                    if (e.IsMainFrame)
                    {
                        String filePath = "D:\\SavedPages\\index.html";
                        String dirPath = "D:\\SavedPages\\resources";
                        browserView.Browser.SaveWebPage(filePath, dirPath, SavePageType.COMPLETE_HTML);
                    }
                };
            browserView.Browser.LoadURL("teamdev.com");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Dispose BrowserView when close app window.
            browserView.Dispose();
            browser.Dispose();
        }
    }
}
