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

namespace PrintToPDFSample
{
    class MyPDFPrintHandler : PrintHandler
    {
        Func<PrintSettings, PrintSettings> func;

        public MyPDFPrintHandler(Func<PrintSettings, PrintSettings> func)
        {
            this.func = func;
        }

        public PrintStatus OnPrint(PrintJob printJob)
        {
            PrintSettings printSettings = func(printJob.PrintSettings);
            printSettings.PrintToPDF = true;
            printSettings.Landscape = true;
            printSettings.PrintBackgrounds = true;
            printSettings.PageMargins = new PageMargins(20, 40, 40, 20);
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
            printButton.Content = "Print PDF";
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

            this.Loaded += WindowMain_Loaded;
        }

        void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            browserView.Browser.LoadURL("http://www.teamdev.com/services");
        }

        void printButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "document";
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF documents (.pdf)|*.pdf";
            dlg.Title = "Save page as PDF document";
            var res = dlg.ShowDialog();
            if (res.HasValue && res.Value)
            {
                browserView.Browser.PrintHandler = new MyPDFPrintHandler((printSettings) =>
                {
                    printSettings.PDFFilePath = dlg.FileName;
                    return printSettings;
                });
                browserView.Browser.Print();
            }
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
