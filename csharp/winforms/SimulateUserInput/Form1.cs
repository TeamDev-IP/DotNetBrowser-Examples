#region Copyright

// Copyright © 2023, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;

namespace SimulateUserInput.WinForms
{
    /// <summary>
    ///     This example demonstrates how to fill multipage HTML Form fields using DotNetBrowser DOM API.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly IBrowser browser;

        public Form1()
        {
            InitializeComponent();

            IEngine engine = EngineFactory.Create();
            browser = engine.CreateBrowser();

            Closed += (sender, args) => { engine.Dispose(); };

            BrowserView browserView = new BrowserView { Dock = DockStyle.Fill };
            browserView.InitializeFrom(browser);

            tableLayoutPanel1.Controls.Add(browserView, 0, 0);

            browser.Navigation.LoadUrl(Path.Combine(Directory.GetCurrentDirectory(),
                                                    "form1.html"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser.Focus();

            IDocument document = browser.MainFrame.Document;

            //Get necessary input elements on the form.
            IInputElement firstName =
                (IInputElement)document.GetElementById("firstname");
            IInputElement lastName =
                (IInputElement)document.GetElementById("lastname");
            IInputElement submitForm1 =
                (IInputElement)document.GetElementById("submitForm1");

            //Fill the input elements.
            firstName.Value = "John";
            lastName.Value = "Doe";

            ManualResetEvent waitEvent = new ManualResetEvent(false);

            //Subscribe to the navigation event which indicates that the new web page was loaded.
            browser.Navigation.FrameDocumentLoadFinished += (o, args) =>
            {
                if (args.Frame.IsMain)
                {
                    waitEvent.Set();
                }
            };

            //Simulate click.
            submitForm1.Click();
            waitEvent.WaitOne();
            Thread.Sleep(500);

            document = browser.MainFrame.Document;

            //Get the necessary elements of the new form. 
            ISelectElement gender =
                (ISelectElement)document.GetElementById("gender");
            IInputElement checkbox =
                (IInputElement)document.GetElementById("checkbox");
            IInputElement submitForm2 =
                (IInputElement)document.GetElementById("submitForm2");

            //Choose the necessary 'option' element from the 'select' element.
            foreach (IOptionElement item in gender.Options)
            {
                if (item.InnerText.StartsWith("Male"))
                {
                    item.Selected = true;
                }
            }

            //Put the flag on the 'checkbox' element.
            checkbox.Checked = true;
            Thread.Sleep(500);

            //Simulate click.
            submitForm2.Click();
        }
    }
}
