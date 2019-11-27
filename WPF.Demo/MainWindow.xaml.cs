using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using DotNetBrowser;

namespace Demo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("User32.dll")]
        public static extern int SetProcessDPIAware();

       RoutedCommand newCmd = new RoutedCommand();
       RoutedCommand delCmd = new RoutedCommand();

       public MainWindow()
       {
           SetProcessDPIAware();
           InitializeComponent();
           String[] arguments = Environment.GetCommandLineArgs();
           System.Collections.Generic.List<string> switches = new System.Collections.Generic.List<string>();
           foreach (string arg in arguments)
           {
               if (arg != null && arg.ToLower().Contains("enable-file-log"))
               {
                   LoggerProvider.Instance.LoggingEnabled = true;
                   LoggerProvider.Instance.FileLoggingEnabled = true;
                   Guid guid = Guid.NewGuid();
                   string logFile = String.Format("DotNetBrowser-WPF-{0}.log", guid);
                   LoggerProvider.Instance.OutputFile = System.IO.Path.GetFullPath(logFile);
               }
               if (arg != null && arg.ToLower().Contains("lightweight"))
               {
                   TabFactory.BrowserType = DotNetBrowser.BrowserType.LIGHTWEIGHT;
               }
               if (arg != null && arg.ToLower().Contains("npapi"))
               {
                   switches.Add("--enable-npapi");
               }
               if (arg != null && arg.StartsWith("--remote-debugging-port="))
               {
                   switches.Add(arg);
               }
			   if (arg != null && arg.ToLower().Contains("silent-download"))
               {
                   DefaultDownloadHandler.DownloadMode = DownloadMode.Silent;
               }
           }
           if (switches.Count > 0)
           {
               DotNetBrowser.BrowserPreferences.SetChromiumSwitches(switches.ToArray());
           }
           TabbedPane tabbedPane = new TabbedPane();
           Application.Current.Dispatcher.Invoke((Action)(() =>
           {
               InsertTab(tabbedPane, TabFactory.CreateFirstTab());
               InsertNewTabButton(tabbedPane);
               mainContent.Children.Add(tabbedPane);
           }));
           Application.Current.Exit += delegate
           {
               tabbedPane.DisposeAllTabs();
           };

           newCmd.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));
           CommandBindings.Add(new CommandBinding(newCmd, delegate
           {
               InsertTab(tabbedPane, TabFactory.CreateTab());
           }));

           delCmd.InputGestures.Add(new KeyGesture(Key.W, ModifierKeys.Control));
           CommandBindings.Add(new CommandBinding(delCmd, delegate
           {
               tabbedPane.RemoveSelectedTab();
           }));
       }

        private static void InsertNewTabButton(TabbedPane tabbedPane)
        {
            ImageButton newTab = new ImageButton();
            newTab.Icon = Demo.WPF.Resources.NewTab;
            newTab.Width = newTab.Height = 26;

            newTab.Click += delegate
            {
                InsertTab(tabbedPane, TabFactory.CreateTab());
            };

            tabbedPane.AddNewTabButton(newTab);

        }

        private static void InsertTab(TabbedPane tabbedPane, Tab tab)
        {
            tabbedPane.AddTab(tab);
        }

    }
}