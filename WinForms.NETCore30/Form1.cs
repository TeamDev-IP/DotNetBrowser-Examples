using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.NETCore30
{
    public partial class Form1 : Form
    {
        BrowserView webView;

        private IBrowser browser;
        private IEngine engine;

        public Form1()
        {
            webView = new BrowserView() { Dock = DockStyle.Fill };
            Task.Run(() =>
            {
                engine = EngineFactory.Create(new EngineOptions.Builder { RenderingMode = RenderingMode.HardwareAccelerated }
                                                  .Build());
                browser = engine.CreateBrowser();
            })
            .ContinueWith(t =>
            {
                webView.InitializeFrom(browser);
                browser.Navigation.LoadUrl("https://www.teamdev.com/");
            }, TaskScheduler.FromCurrentSynchronizationContext());
            InitializeComponent();
            FormClosing += Form1_FormClosing;
            Controls.Add(webView);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }
    }
}
