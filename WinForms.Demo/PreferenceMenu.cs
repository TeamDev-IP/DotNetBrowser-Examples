using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinForms.Demo;
using WinForms.Demo.Properties;

namespace DotNetBrowser.WinForms
{
    /// <summary>
    /// Represents default WinForms preference menu implementation.
    /// </summary>
    public class PreferenceMenu
    {
        private Browser browser;
        private BrowserView browserView;
        private bool JSConsoleEnable;
        JSConsole jsConsole;

        /// <summary>
        /// Constructs PreferenceMenu instance.
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
            System.Windows.Forms.ContextMenu cm = new System.Windows.Forms.ContextMenu();

            control.Click += delegate
            {
                if (cm.MenuItems.Count > 0)
                {
                    cm.MenuItems.Clear();
                }

                JSConsoleEnable = jsConsole.consoleStatus;
                if (JSConsoleEnable)
                {
                    cm.MenuItems.Add("Run JavaScript...", delegate
                    {                        
                        JSConsoleEnable = jsConsole.runJSConsole(JSConsoleEnable);
                    });
                }
                else if (!JSConsoleEnable)
                {
                    cm.MenuItems.Add("Close JavaScript Console", delegate
                {
                    JSConsoleEnable = jsConsole.runJSConsole(JSConsoleEnable);                    
                });
                }

                cm.MenuItems.Add("Get HTML", delegate
                {
                    RichTextBox richTextBox = new RichTextBox();
                    richTextBox.Dock = DockStyle.Fill;
                    richTextBox.BackColor = Color.White;
                    richTextBox.SelectionFont = new Font("Arial", 14);
                    richTextBox.ReadOnly = true;
                    richTextBox.Multiline = true;
                    richTextBox.Text = browser.GetHTML();
                    richTextBox.SelectionStart = 0;

                    Form form = new Form();
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.Text = "Get HTML";
                    form.Size = new Size(850, 600);
                    form.Controls.Add(richTextBox);
                    form.ShowDialog();
                });
                cm.MenuItems.Add("Popup Windows", delegate
                {
                    browser.LoadURL("http://www.popuptest.com/");
                });
                cm.MenuItems.Add("Upload File", delegate
                {
                    browser.LoadURL("http://www.cs.tut.fi/~jkorpela/forms/file.html#example");
                });
                cm.MenuItems.Add("Download File", delegate
                {
                    browser.LoadURL("http://cloud.teamdev.com/downloads/dotnetbrowser/dotnetbrowser-1.8.2.zip");
                });
                cm.MenuItems.Add("JavaScript Dialogs", delegate
                {
                    browser.LoadURL("http://www.javascripter.net/faq/alert.htm");
                });
                cm.MenuItems.Add("PDF Viewer", delegate
                {
                    browser.LoadURL("http://www.orimi.com/pdf-test.pdf");
                });
                cm.MenuItems.Add("Adobe Flash", delegate
                {
                    browser.LoadURL("https://helpx.adobe.com/flash-player.html");
                });
                cm.MenuItems.Add("Google Maps", delegate
                {
                    browser.LoadURL("https://www.google.com.ua/maps");
                });
                cm.MenuItems.Add("HTML5 Video", delegate
                {
                    browser.LoadURL("https://www.w3.org/2010/05/video/mediaevents.html");
                });

                cm.MenuItems.Add("Zoom In", delegate
                {
                    browser.ZoomIn();
                });
                cm.MenuItems.Add("Zoom Out", delegate
                {
                    browser.ZoomOut();
                });
                cm.MenuItems.Add("Actual size", delegate
                {
                    browser.ZoomReset();
                });

                cm.MenuItems.Add("Proxy Settings", delegate
                {
                    ProxyConfigForm proxyConfigForm = new ProxyConfigForm(browserView);
                    proxyConfigForm.Show();
                });

                cm.MenuItems.Add("Save Web Page...", delegate
                {
                    SaveWebPage();
                });
                cm.MenuItems.Add("Clear Cache", delegate
                {
                    if (!browser.URL.Contains("http://refreshyourcache.com/en/cache-test/"))
                    {
                        browser.LoadURL("http://refreshyourcache.com/en/cache-test/");
                        InfoMessageBox.Show((Control)browserView, "Test page loaded. Press\n\"OK\" button to clear the cache", "Test Page");
                    }
                    browser.CacheStorage.ClearCache();
                    browser.ReloadIgnoringCache();
                    InfoMessageBox.Show((Control)browserView, "Cache is cleared successfully", "Clear Cache");
                });

                //Child menu for MenuItem Preferences
                BrowserPreferencesMenu browserPreferencesMenu = new BrowserPreferencesMenu(browserView);
                cm.MenuItems.Add(browserPreferencesMenu.PreferencesItem());

                //Child menu for MenuItem Execute Command
                BrowserExecuteCommandMenu browserExecuteCommandMenu = new BrowserExecuteCommandMenu(browserView);
                cm.MenuItems.Add(browserExecuteCommandMenu.ExecuteCommandItem());                

                cm.MenuItems.Add("Print...", delegate
                {
                    PrintDialog dlgPrint = new PrintDialog();
                    bool print = (dlgPrint.ShowDialog(((Control)browserView).Parent) == DialogResult.OK);
                    if (print)
                    {
                        browser.PrintHandler = new MyPDFPrintHandler((printSettings) =>
                        {
                            printSettings.PrintToPDF = false;

                            if (dlgPrint.PrinterSettings.PrintToFile || dlgPrint.PrinterSettings.PrinterName.Contains("PDF"))
                            {
                                string fileName = String.Empty;
                                if (SaveToPDF(browser.Title, out fileName))
                                {
                                    printSettings.PrintToPDF = true;
                                    printSettings.PDFFilePath = fileName;
                                }
                            }

                            else
                            {
                                printSettings.Copies = dlgPrint.PrinterSettings.Copies;
                                printSettings.PrinterName = dlgPrint.PrinterSettings.PrinterName;
                                printSettings.DuplexMode = (DuplexMode)dlgPrint.PrinterSettings.DefaultPageSettings.PrinterSettings.Duplex;
                                printSettings.PrintToPDF = false;
                            }

                            return printSettings;
                        });
                        browser.Print();
                    }
                });

                cm.MenuItems.Add("-");

                cm.MenuItems.Add("More Features...", delegate
                {
                    browser.LoadURL("http://dotnetbrowser-support.teamdev.com/documentation");
                });

                cm.MenuItems.Add("-");

                cm.MenuItems.Add("About WinFormsDotNetBrowser Demo", delegate
                {
                    AboutDemo();
                });
                Point p = new Point(0, 28);
                cm.Show(control, p);
            };
        }

        private void AboutDemo()
        {
            Form aboutDemo = new Form();

            aboutDemo.ShowIcon = false;
            aboutDemo.Size = new Size(380, 300);
            aboutDemo.Text = "About Demo";
            aboutDemo.FormBorderStyle = FormBorderStyle.FixedDialog;
            aboutDemo.MaximizeBox = false;
            aboutDemo.MinimizeBox = false;
            aboutDemo.StartPosition = FormStartPosition.CenterParent;

            String strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            BrowserView browserView = new WinFormsBrowserView(BrowserFactory.Create(BrowserType.HEAVYWEIGHT));
            aboutDemo.Controls.Add((Control)browserView);

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

            browserView.Browser.LoadHTML(textAboutDemo);

            aboutDemo.FormClosing += delegate
            {
                if (!browserView.Browser.IsDisposed())
                {
                    browserView.Browser.Dispose();
                    browserView.Dispose();
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

            fileChooser.ShowDialog(((Control)browserView).Parent);
        }

        private static bool SaveToPDF(string name, out string fileName)
        {
            string destinationFile = String.Empty;
            ManualResetEvent endUIThread = new ManualResetEvent(false);
            CloseStatus returnValue = CloseStatus.CANCEL;
            Form mainForm = Form.ActiveForm;

            mainForm.Invoke((Action)(() =>
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

