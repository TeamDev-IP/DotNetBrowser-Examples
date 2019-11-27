using DotNetBrowser;
using DotNetBrowser.DOM;
using DotNetBrowser.DOM.Events;
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DOMFocusSample
{
    public partial class Form1 : Form
    {
        Browser browser;

        public Form1()
        {
            InitializeComponent();

            browser = BrowserFactory.Create();
            BrowserView browserView = new WinFormsBrowserView(browser)
            {
                Dock = DockStyle.Fill
            };
            browser.DialogHandler = new WinFormsDefaultDialogHandler((Control)browserView);

            this.Controls.Add((Control)browserView.GetComponent());

            browserView.Browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    DOMDocument document = e.Browser.GetDocument();

                    DOMElement link3Element = document.GetElementById("link3");
                    DOMEventHandler link3Event = delegate(object domEventSender, DOMEventArgs domEventArgs)
                    {
                        if (domEventArgs.Type == DOMEventType.OnFocus)
                        {
                            Console.Out.WriteLine("link3 gets focus");
                        }
                        if (domEventArgs.Type == DOMEventType.OnBlur)
                        {
                            Console.Out.WriteLine("Remove focus from link3");
                        }
                    };
                    link3Element.AddEventListener(DOMEventType.OnFocus | DOMEventType.OnBlur, link3Event, false);


                    DOMElement elementFocused = null;
                    foreach(DOMElement element in document.GetElementsByTagName("a"))
                    {
                        Thread.Sleep(2000);
                        element.Focus();
                        elementFocused = element;
                    }

                    if (elementFocused != null)
                    {
                        Thread.Sleep(2000);
                        elementFocused.Blur();
                    }
                }
            };
            browserView.Browser.LoadHTML(@"<html>
                                                <body>
                                                    <a id='link1' href='#'>The link 1</a>
                                                    <a id='link2' href='#'>The link 2</a>
                                                    <a id='link3' href='#'>The link 3</a>
                                                    <script type='text/javascript'>
                                                        var link2 = document.getElementById('link2');
                                                        link2.addEventListener('focus', function () { console.log('link2 gets focus'); });
                                                        link2.addEventListener('blur', function () { console.log('Remove focus from link2'); });
                                                    </script>
                                                </body>
                                            </html>");
        }
    }
}
