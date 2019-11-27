using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.ApplicationCacheSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            browserView.Browser.LoadURL("http://www.w3schools.com/html/tryhtml5_html_manifest.htm");

            browserView.Dock = DockStyle.None;
        }

        private void OriginURLs_Click(object sender, EventArgs e)
        {
            string message = String.Empty;
            IEnumerable<string> originURLs = browserView.Browser.AppCacheStorage.OriginURLs;                 

            foreach (var originURL in originURLs)
            {
                message += originURL + "\n";
            }

            MessageBox.Show(message);
        }

        private void getByOriginURL_Click(object sender, EventArgs e)
        {
            string message = String.Empty;
            IEnumerable<string> originURLs = browserView.Browser.AppCacheStorage.OriginURLs;


            foreach (var originURL in originURLs)
            {                
                IAppCache originInfo = browserView.Browser.AppCacheStorage.GetInfoByOriginURL(originURL);
                IEnumerable<IAppCacheInfo> manifests = originInfo.Manifests;

                foreach (var manifest in manifests)
                {
                    message += manifest.ManifestURL + "\n";
                }
            }
            MessageBox.Show(message);
        }

        private void removalForManifestURL_Click(object sender, EventArgs e)
        {
            browserView.Browser.AppCacheStorage.RemoveInfoForManifestURL("http://www.w3schools.com/html/demo_html.appcache");

            MessageBox.Show("Manifest 'demo_html.appcache' has been successfully removed");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!browserView.IsDisposed)
            {
                browserView.Browser.Dispose();
                browserView.Dispose();
            }
        }
    }
}
