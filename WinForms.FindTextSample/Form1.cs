using DotNetBrowser;
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

namespace WinForms.FindTextSample
{
    public partial class Form1 : Form
    {        
        private Browser browser;
        private BrowserView browserView;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            browserView = new WinFormsBrowserView()
            {
                Dock = DockStyle.Fill
            };
            browser = browserView.Browser;
            browser.LoadURL("google.com");
            panel1.Controls.Add((Control)browserView);
        }

        private void find_Click(object sender, EventArgs e)
        {
            if (findTextBox.Text != String.Empty)
            {
                SearchParams searchParams = new SearchParams(findTextBox.Text);

                if (browser.FindText(searchParams).NumberOfMatches == 0)
                {
                    MessageBox.Show("No matches!");
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!browser.IsDisposed())
            {
                browserView.Dispose();
                browserView.Browser.Dispose();
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            browser.StopFindingText(StopFindAction.CLEAR_SELECTION);
            findTextBox.Text = "";
        }
    }
}
