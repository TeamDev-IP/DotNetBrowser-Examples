using DotNetBrowser;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinForms.Demo.Properties;

namespace WinForms.Demo
{
    public partial class MainForm : Form
    {
        [DllImport("User32.dll")]
        public static extern int SetProcessDPIAware();

        private static readonly String DBG_PORT_PARAM = "--remote-debugging-port=";

        private static TabbedPane tabbedPane;

        public MainForm()
        {
            SetProcessDPIAware();
            InitializeComponent();
            WinFormsUIContext.Instance.Value = WindowsFormsSynchronizationContext.Current;

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
                    string logFile = String.Format("DotNetBrowser-WinForms-{0}.log", guid);
                    LoggerProvider.Instance.OutputFile = System.IO.Path.GetFullPath(logFile);
                }
                if (arg != null && arg.ToLower().Contains("lightweight"))
                {
                    TabFactory.BrowserType = BrowserType.LIGHTWEIGHT;
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
                BrowserPreferences.SetChromiumSwitches(switches.ToArray());
            }
            tabbedPane = new TabbedPane();
            InsertTab(TabFactory.CreateFirstTab());
            InsertNewTabButton();
            this.Controls.Add(tabbedPane);


            Application.ApplicationExit += delegate
            {
                tabbedPane.DisposeAllTabs();
            };

        }

        private static void InsertNewTabButton()
        {
            ImageButton newTab = new ImageButton();
            newTab.Icon = Resources.NewTab;
            newTab.ToolTip = Resources.NewTabButtonTooltip;

            newTab.Click += delegate
            {
                InsertTab(TabFactory.CreateTab());
            };
            tabbedPane.AddTabButton(newTab);

        }

        private static void InsertTab(Tab tab)
        {
            tabbedPane.AddTab(tab);
            tabbedPane.SelectTab(tab);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.T))
            {
                InsertTab(TabFactory.CreateTab());
            }
            if (keyData == (Keys.Control | Keys.W))
            {
                tabbedPane.RemoveSelectedTab();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}