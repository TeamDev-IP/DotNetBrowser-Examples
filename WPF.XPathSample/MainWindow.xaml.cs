
using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace XPath_Sample_WPF_RF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private WPFBrowserView browserView;
        BrowserContext browserContext;
        Browser browser;

        public MainWindow()
        {
            
            browserContext = BrowserContext.DefaultContext;
            browser = BrowserFactory.Create(browserContext);

            browserView = new WPFBrowserView(browser);
            Content = browserView;

            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainLayout.Children.Add((UIElement)browserView.GetComponent());

            ManualResetEvent waitEvent = new ManualResetEvent(false);
            browser.FinishLoadingFrameEvent += delegate (object sender1, FinishLoadingEventArgs e1)
            {
                // Wait until main document of the web page is loaded completely.
                if (e1.IsMainFrame)
                {                                      
                    waitEvent.Set();
                }
            };
            browser.LoadURL("teamdev.com");
            waitEvent.WaitOne();                                

            label1.Content = "Press button 'XPath' to view an info about <div> on this page";
            label2.Content = "";
            label3.Content = "";
            label4.Content = "";
            label5.Content = "";
            button1.Content = "XPath";

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!browserView.IsDisposed)
            {
                browserView.Browser.Dispose();
                browserView.Dispose();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            DOMDocument document = browser.GetDocument();
			
			//XPath expressions
            XPathResult resultNum = document.Evaluate("count(//div)");
            XPathResult resultNumSpec = document.Evaluate("count(//div[@class='col-sm-4 prod-thumb'])");
            XPathResult result = document.Evaluate("string( / html / body / div[2] / div[2] / div[1] / div[2] / div / div[1] / div[2] / h3 / a)");
            XPathResult resultNodes = document.Evaluate("/ html / body / div[2] / div[2] / div[1] / div[2] / div / *");
			
			//Check if XPath result type is correct
            if (resultNum.IsNumber && resultNumSpec.IsNumber)
            {
                label1.Content = "Quantity of all <div> = " + resultNum.Number;
                label2.Content = "Quantity of <div> with class 'col-sm-4 prod-thumb' = " + resultNumSpec.Number;
            }
            else
            {
                label1.Content = "Error";
                label2.Content = "Error";
            }
            
			//Check if XPath result type is correct
            if (result.IsString)
            {
                label3.Content = "Text from selected element: " + result.String;
            }
            else
            {
                label3.Content = "Error";
            }

			//Check if XPath result type is correct
            if (resultNodes.IsIterator)
                label4.Content = resultNodes.ResultType;
            else
                label4.Content = "Mistake";

            //Get list with DOMNodes
            var tmp = resultNodes.Iterator.Select(item => item.Node.Children);

            try
            {
                //Extract list with DOMNodes          
                foreach (var item in tmp)
                {
                    foreach (var item1 in item)
                    {
                        //Extract DOMNode from the list
                        label4.Content = "Node type: " + item1.NodeType;
                        label5.Content = "Node name: " + item1.NodeName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }       
        
    }
}
