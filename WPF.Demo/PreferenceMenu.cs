using Demo.WPF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DotNetBrowser.WPF
{
    /// <summary>
    /// Represents default WPF preference menu implementation.
    /// </summary>
    public class PreferenceMenu
    {
        private Browser browser;
        private BrowserView browserView;
        private static bool JSConsoleEnable;
        JSConsole jsConsole;

        /// <summary>
        /// Constructs WPFDefaultPreferenceMenuHandler instance.
        /// </summary>
        /// <param name="browserView">Owner UI component.</param>       
        public PreferenceMenu(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;
            jsConsole = new JSConsole(browserView);
        }

        /// <summary>
        /// Invoked when preference menu should be displayed.
        /// </summary>
        /// <param name="control">open menu on click.</param>
        public void AddPreferenceMenu(Control control)
        {
            System.Windows.Controls.ContextMenu cm = new System.Windows.Controls.ContextMenu();
            
            control.MouseDown += delegate
            {
                if (cm.Items.Count > 0)
                {
                    cm.Items.Clear();
                }
                JSConsoleEnable = jsConsole.consoleStatus;
                if (JSConsoleEnable)
                {
                    cm.Items.Add(BuildMenuItem.Build("Run JavaScript...", true, false, delegate
                    {
                        JSConsoleEnable = jsConsole.runJSConsole(JSConsoleEnable);                        
                    }));
                }
                else if (!JSConsoleEnable)
                {
                    cm.Items.Add(BuildMenuItem.Build("Close JavaScript Console", true, false, delegate
                    {
                        JSConsoleEnable = jsConsole.runJSConsole(JSConsoleEnable);
                    }));
                }
                cm.Items.Add(BuildMenuItem.Build("Get HTML", true, false, delegate
                    {
                        TextBox textBox = new TextBox();
                        textBox.IsReadOnly = true;
                        textBox.Text = browser.GetHTML();
                        textBox.SelectionStart = 0;
                        textBox.TextWrapping = TextWrapping.WrapWithOverflow;
                        textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                        textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                        Window window = new Window();
                        window.Owner = Window.GetWindow((FrameworkElement)browserView);
                        window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        window.Title = "Get HTML";
                        window.Height = 600;
                        window.Width = 850;
                        window.Content = textBox;
                        window.ShowDialog();
                        
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Popup Windows", true, false, delegate
                    {
                        browser.LoadURL("http://www.popuptest.com/");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Upload File", true, false, delegate
                    {
                        browser.LoadURL("http://www.cs.tut.fi/~jkorpela/forms/file.html#example");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Download File", true, false, delegate
                    {
                        browser.LoadURL("http://cloud.teamdev.com/downloads/dotnetbrowser/dotnetbrowser-1.8.2.zip");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("JavaScript Dialogs", true, false, delegate
                    {
                        browser.LoadURL("http://www.javascripter.net/faq/alert.htm");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("PDF Viewer", true, false, delegate
                    {
                        browser.LoadURL("http://www.orimi.com/pdf-test.pdf");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Adobe Flash", true, false, delegate
                    {
                        browser.LoadURL("https://helpx.adobe.com/flash-player.html");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Google Maps", true, false, delegate
                    {
                        browser.LoadURL("https://www.google.com.ua/maps");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("HTML5 Video", true, false, delegate
                    {
                        browser.LoadURL("https://www.w3.org/2010/05/video/mediaevents.html");
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Zoom In", true, false, delegate
                    {
                        browser.ZoomIn();
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Zoom Out", true, false, delegate
                    {
                        browser.ZoomOut();
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Actual size", true, false, delegate
                    {
                        browser.ZoomReset();
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Proxy Settings", true, false, delegate
                    {
                        ProxyConfigForm proxyConfigForm = new ProxyConfigForm(browserView);
                        proxyConfigForm.Show();
                    }));

                    cm.Items.Add(BuildMenuItem.Build("Save Web Page...", true, false, delegate
                    {
                        SaveWebPage();
                    }));
                    cm.Items.Add(BuildMenuItem.Build("Clear Cache", true, false, delegate
                    {
                        if (!browser.URL.Contains("http://refreshyourcache.com/en/cache-test/"))
                        {
                            browser.LoadURL("http://refreshyourcache.com/en/cache-test/");
                            InfoMessageBox.Show((FrameworkElement)browserView, "Test page loaded. Press\n\"OK\" to clear the cache.", "Test page");
                        }
                        browser.CacheStorage.ClearCache();
                        browser.ReloadIgnoringCache();
                        InfoMessageBox.Show((FrameworkElement)browserView,  "Cache is cleared successfully", "Clear Cache");
                    }));

                    //Child menu for MenuItem Preferences
                    BrowserPreferencesMenu browserPreferencesMenu = new BrowserPreferencesMenu(browserView);
                    cm.Items.Add(browserPreferencesMenu.PreferencesItem());

                    //Child menu for MenuItem Execute Command
                    BrowserExecuteCommandMenu browserExecuteCommandMenu = new BrowserExecuteCommandMenu(browserView);
                    cm.Items.Add(browserExecuteCommandMenu.ExecuteCommandItem());

                    cm.Items.Add(BuildMenuItem.Build("Print...", true, false, delegate
                    {
                        Nullable<Boolean> print = false;
                        PrintDialog dlgPrint = new PrintDialog();
                        print = dlgPrint.ShowDialog();
                        string printerName = dlgPrint.PrintQueue.Name;
                        if (print == true)
                        {
                            browser.PrintHandler = new MyPDFPrintHandler((printSettings) =>
                            {
                                printSettings.PrinterName = printerName;
                                dlgPrint.UserPageRangeEnabled = true;

                                if (printSettings.PrinterName.Contains("PDF"))
                                {
                                    string fileName = String.Empty;
                                    if (SaveToPDF(browser.Title, out fileName))
                                    {
                                        printSettings.PDFFilePath = fileName;
                                        printSettings.PrintToPDF = true;
                                    }
                                }
                                else
                                {
                                    printSettings.PrintToPDF = false;
                                    printSettings.PaperSize = PaperSize.DEFAULT;
                                }
                                printSettings.PrintBackgrounds = true;
                                return printSettings;
                            });
                            browser.Print();
                        }
                    }));                    

                    cm.Items.Add(new Separator());

                    cm.Items.Add(BuildMenuItem.Build("More Features...", true, false, delegate
                    {
                        browser.LoadURL("http://dotnetbrowser-support.teamdev.com/documentation");
                    }));

                    cm.Items.Add(new Separator());

                    cm.Items.Add(BuildMenuItem.Build("About WPFDotNetBrowser Demo", true, false, delegate
                    {
                        AboutDemo();
                    }));
                cm.IsOpen = true;
            };
        }

        private void AboutDemo()
        {
            Window aboutDemo = new Window();
            String strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            BrowserView browserViewDemo = new WPFBrowserView(BrowserFactory.Create(BrowserType.HEAVYWEIGHT));

            aboutDemo.Width = 380;
            aboutDemo.Height = 300;
            aboutDemo.Title = "About Demo";
            aboutDemo.ResizeMode = ResizeMode.NoResize;
            aboutDemo.WindowStyle = WindowStyle.SingleBorderWindow;
            aboutDemo.Topmost = true;
            aboutDemo.Content = browserViewDemo;

            string textAboutDemo = "<br>" + "<html><font face='Arial' size='2'>" +
            "<font size='5'>DotNetBrowser Demo</font><br><br>" +
            "<b>Version " + strVersion + "</b><br><br>" +
            "<base target='_blank'>" +

            "This application is created for demonstration purposes only.<br>" +
            "&copy; 2017 TeamDev Ltd. All rights reserved.<br><br>" +

            "Powered by <a color='#3d82f8' href='https://www.teamdev.com/dotnetbrowser' " +
            "style='text-decoration:none'>DotNetBrowser</a>. See " +
            "<a color='#3d82f8' href='https://www.teamdev.com/dotnetbrowser-licence-agreement' " +
            "style='text-decoration:none'>terms of use.</a><br>" +

            "Based on <a color='#3d82f8' href='http://www.chromium.org/' " +
            "style='text-decoration:none'>Chromium project</a>. " +
            "See <a color='#3d82f8' " +
            "href='http://dotnetbrowser-support.teamdev.com/documentation/open-source-components-licences' " +
            "style='text-decoration:none'>full list</a> of Chromium<br>components, " +
            "used in the current DotNetBrowser version.<br><br>" +

            "This demo uses WebKit project under LGPL.<br>" +

            "See licence text " +
            "<a color='#3d82f8' href='https://www.gnu.org/licenses/old-licenses/lgpl-2.0.html' " +
            "style='text-decoration:none'>LGPL v.2</a> and " +
            "<a color='#3d82f8' href='https://www.gnu.org/licenses/old-licenses/lgpl-2.1.en.html' " +
            "style='text-decoration:none'>LGPL v.2.1</a></font></html>";

            browserViewDemo.Browser.LoadHTML(textAboutDemo);
            aboutDemo.Owner = Window.GetWindow((FrameworkElement)browserView);
            aboutDemo.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            aboutDemo.Closing += delegate
            {
                if (!browserViewDemo.Browser.IsDisposed())
                {
                    browserViewDemo.Browser.Dispose();
                    browserViewDemo.Dispose();
                }
            };

            aboutDemo.ShowDialog();
        }

        private void SaveWebPage()
        {
            string extHtml = "*.html";
            string extMhtml = "*.mht";

            SaveFileDialog fileChooser = new SaveFileDialog();
            fileChooser.FileName = "my-web-page.html";
            fileChooser.Filter = String.Format("Webpage, HTML only|{0}|Webpage, Complete|{0}|Web Archive, single file|{1}", extHtml, extMhtml);
            fileChooser.Title = "Save As";
            fileChooser.AddExtension = true;
            SavePageType savePageType;

            fileChooser.FileOk += delegate
            {
                string filePath = fileChooser.FileName;
                string resourcePath = Path.Combine(Path.GetDirectoryName(fileChooser.FileName), Path.GetFileNameWithoutExtension(fileChooser.FileName));
                savePageType = SavePageType.ONLY_HTML;

                if (fileChooser.FilterIndex == 2)
                {
                    savePageType = SavePageType.COMPLETE_HTML;
                }
                else if (fileChooser.FilterIndex == 3)
                {
                    savePageType = SavePageType.MHTML;
                }

                browser.SaveWebPage(filePath, resourcePath, savePageType);
            };
            fileChooser.ShowDialog(Window.GetWindow((FrameworkElement)browserView));
        }

        private static bool SaveToPDF(string name, out string fileName)
        {
            string destinationFile = String.Empty;
            ManualResetEvent endUIThread = new ManualResetEvent(false);

            CloseStatus returnValue = CloseStatus.CANCEL;
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    string ext = "*.pdf";
                    SaveFileDialog fileChooser = new SaveFileDialog();
                    fileChooser.FileName = name;
                    fileChooser.Filter = String.Format("PDF document|{0}", ext);
                    fileChooser.Title = "Save";
                    fileChooser.AddExtension = true;

                    fileChooser.FileOk += delegate
                    {
                        destinationFile = fileChooser.FileName;
                        returnValue = CloseStatus.OK;
                    };                   
                        fileChooser.ShowDialog();                   
                }
                finally
                {
                    endUIThread.Set();
                }
            }));
            
            fileName = destinationFile;
            return returnValue == CloseStatus.OK;
        }       
    
        private class MyPDFPrintHandler : PrintHandler
        {
            Func<PrintSettings, PrintSettings> func;

            public MyPDFPrintHandler(Func<PrintSettings, PrintSettings> func)
            {
                this.func = func;
            }

            public PrintStatus OnPrint(PrintJob printJob)
            {
                PrintSettings printSettings = func(printJob.PrintSettings);
                printSettings.PrintBackgrounds = true;
                printSettings.PageMargins = new PageMargins(20, 40, 40, 20);
                return PrintStatus.CONTINUE;
            }
        }
    }
}