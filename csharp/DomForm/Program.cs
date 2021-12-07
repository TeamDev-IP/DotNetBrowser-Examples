#region Copyright

// Copyright © 2021, TeamDev. All rights reserved.
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
using System.Text;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;

namespace DomForm
{
    /// <summary>
    ///     This example demonstrates how to fill HTML Form fields using DotNetBrowser DOM API.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            try
            {
                using (IEngine engine = EngineFactory.Create())
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");

                        byte[] htmlBytes = Encoding.UTF8.GetBytes("<html><body><form name=\"myForm\">"
                                                                  + "First name: <input type=\"text\" id=\"firstName\" name=\"firstName\"/><br/>"
                                                                  + "Last name: <input type=\"text\" id=\"lastName\" name=\"lastName\"/><br/>"
                                                                  + "<input type='checkbox' id='agreement' name='agreement' value='agreed'>I agree<br>"
                                                                  + "<input type='button' id='saveButton' value=\"Save\" onclick=\""
                                                                  + "if(document.getElementById('agreement').checked){"
                                                                  + "    console.log(document.getElementById('firstName').value +' '+"
                                                                  + "document.getElementById('lastName').value);}"
                                                                  + "\"/>"
                                                                  + "</form></body></html>");
                        browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait();

                        IDocument document = browser.MainFrame.Document;
                        IInputElement firstName = (IInputElement) document.GetElementByName("firstName");
                        IInputElement lastName = (IInputElement) document.GetElementByName("lastName");
                        IInputElement agreement = (IInputElement) document.GetElementByName("agreement");

                        firstName.Value = "John";
                        lastName.Value = "Doe";
                        agreement.Checked = true;

                        browser.ConsoleMessageReceived += (sender, args) =>
                        {
                            Console.WriteLine("JS Console: < " + args.Message);
                        };
                        document.GetElementById("saveButton").Click();
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }
}