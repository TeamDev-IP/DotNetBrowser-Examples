using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PrintSample
{
    class XPSPrintHandler : PrintHandler
    {
        public PrintStatus OnPrint(PrintJob printJob)
        {
            PrintSettings printSettings = printJob.PrintSettings;
            printSettings.PrinterName = "Microsoft XPS Document Writer";
            printSettings.Landscape = false;
            printSettings.PrintBackgrounds = false;
            printSettings.ColorModel = ColorModel.COLOR;
            printSettings.DuplexMode = DuplexMode.SIMPLEX;
            printSettings.DisplayHeaderFooter = true;
            printSettings.Copies = 1;
            printSettings.PaperSize = PaperSize.ISO_A4;

            List<DotNetBrowser.PageRange> ranges = new List<DotNetBrowser.PageRange>();
            ranges.Add(new DotNetBrowser.PageRange(0, 3));
            printSettings.PageRanges = ranges;

            printJob.PrintJobEvent += delegate(object sender, PrintJobEventArgs e)
            {
                Console.WriteLine("Printing is finished successfully: " + e.Success);
            };            
            return PrintStatus.CONTINUE;
        }
    }

    public class WindowMain : System.Windows.Window
    {
        private Button printButton;
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

            printButton = new Button();
            printButton.Content = "Print";
            printButton.Height = 23;
            printButton.Click += printButton_Click;
            Grid.SetRow(printButton, 0);
            Grid.SetColumn(printButton, 0);

            browserView = new WPFBrowserView(BrowserFactory.Create());            
            Grid.SetRow(browserView, 1);
            Grid.SetColumn(browserView, 0);

            layout.Children.Add(printButton);
            layout.Children.Add(browserView);
           

            Width = 1024;
            Height = 768;

            browserView.Browser.PrintHandler = new XPSPrintHandler();
            this.Loaded += WindowMain_Loaded;            
        }

        void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            browserView.Browser.LoadURL("http://www.teamdev.com/services");
        }

        void printButton_Click(object sender, RoutedEventArgs e)
        {
            browserView.Browser.Print();
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
