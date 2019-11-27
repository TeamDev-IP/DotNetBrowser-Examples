using DotNetBrowser;
using DotNetBrowser.Events;
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

namespace WinForms.KeyboardEventSimulateSample
{
    public partial class Form1 : Form
    {
        private Browser browser;
        public Form1()
        {
            InitializeComponent();

            browser = BrowserFactory.Create();
            browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    // Press TAB key to set focus to text field.
                    KeyParams paramers = new KeyParams(VirtualKeyCode.TAB, ' ');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    // Type 'Hello' text in the focused text field.
                    paramers = new KeyParams(VirtualKeyCode.VK_H, 'H');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    paramers = new KeyParams(VirtualKeyCode.VK_E, 'e');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    paramers = new KeyParams(VirtualKeyCode.VK_L, 'l');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    paramers = new KeyParams(VirtualKeyCode.VK_L, 'l');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);

                    paramers = new KeyParams(VirtualKeyCode.VK_O, 'o');
                    browser.KeyDown(paramers);
                    browser.KeyUp(paramers);
                }
            };

            BrowserView browserView = new WinFormsBrowserView(browser)
            {
                Dock = DockStyle.Fill
            };
            browserView.Browser.LoadHTML(@"<html><body><input type='text' autofocus></input></body></html>");

            this.Controls.Add((Control)browserView.GetComponent());
        }
    }
}
