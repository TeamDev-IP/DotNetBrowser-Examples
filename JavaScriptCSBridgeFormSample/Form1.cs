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

namespace JavaScriptCSBridgeSample
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
                    JSValue value = browser.ExecuteJavaScriptAndReturnValue("window");
                    value.AsObject().SetProperty("Account", new Account());
                }
            };

            browser.LoadHTML(@"<!DOCTYPE html>
                                    <html>
                                    <head>
                                        <script type='text/javascript'>
                                              function sendFormData() {
                                                var firstNameValue = myForm.elements['firstName'].value;
                                                var lastNameValue = myForm.elements['lastName'].value;
                                                // Invoke the 'save' method of the 'Account' Java object.
                                                Account.Save(firstNameValue, lastNameValue);
                                              }
                                            </script>
                                    </head>
                                    <body>
                                    <form name='myForm'>
                                        First name: <input type='text' name='firstName'/>
                                        <br/>
                                        Last name: <input type='text' name='lastName'/>
                                        <br/>
                                        <input type='button' value='Save' onclick='sendFormData();'/>
                                    </form>
                                    </body>
                                    </html>");


            BrowserView browserView = new WinFormsBrowserView(browser)
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add((Control)browserView.GetComponent());
        }

        /**
        * c# class used for binding Java to JavaScript must be public.
        */
        public class Account
        {
            public void Save(String firstName, String lastName)
            {
                Console.Out.WriteLine("firstName = " + firstName);
                Console.Out.WriteLine("lastName = " + lastName);
            }
        }
    }
}
