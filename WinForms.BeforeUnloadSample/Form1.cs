using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.WinForms;

namespace WinForms.BeforeUnloadSample
{
    public partial class Form1 : Form
    {
        private readonly WinFormsBrowserView browserView;

        public Form1()
        {
            InitializeComponent();
            FormClosing += Form1_FormClosing;

            browserView = new WinFormsBrowserView() {Dock = DockStyle.Fill};
            Controls.Add(browserView);

            browserView.Browser.LoadHTML("<html><body onbeforeunload='return myFunction()'>" +
                                         "<a href='http://www.google.com'>Click here to leave</a>" +
                                         "<script>function myFunction() { return 'Leave this web page?'; }" +
                                         "</script></body></html>");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Task<bool> disposeTask = browserView.Browser.Dispose(true);

                if (disposeTask.IsCompleted)
                {
                    e.Cancel = !disposeTask.Result;
                    return;
                }

                e.Cancel = true;

                disposeTask.ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        BeginInvoke((Action) (() =>
                        {
                            FormClosing -= Form1_FormClosing;
                            Close();
                        }));
                    }
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}