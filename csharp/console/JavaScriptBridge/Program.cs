#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Js;
using DotNetBrowser.Logging;

namespace JavaScriptBridge
{
    /// <summary>
    ///     This example demonstrates how to inject a .NET object into JavaScript and
    ///     invoke its public methods from the JavaScript side.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            LoggerProvider.Instance.Level = SourceLevels.Information;
            LoggerProvider.Instance.FileLoggingEnabled = true;
            LoggerProvider.Instance.OutputFile = "dnb.log";

            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = new Size(700, 500);
                    byte[] htmlBytes = Encoding.UTF8.GetBytes(@"<html>
                                     <body>
                                        <script type='text/javascript'>
                                            var ShowData = function (a) 
                                            {
                                                 document.title = a.FullName 
                                                                + ' ' + a.Age + '. ' + a.Walk(a.Children[1])
                                                                + ' ' + a.Children[1].FullName 
                                                                + ' ' + a.Children[1].Age;
                                            };
                                        </script>
                                     </body>
                                   </html>");

                    browser.Navigation
                           .LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}")
                           .Wait();

                    Person person = new Person("Jack", 30, true)
                    {
                        Children = new Dictionary<int, Person>()
                    };

                    person.Children.Add(1, new Person("Oliver", 10, true));
                    IJsObject value = browser.MainFrame
                                             .ExecuteJavaScript<IJsObject>("window")
                                             .Result;
                    value.Invoke("ShowData", person);

                    Console.WriteLine($"\tBrowser title: {browser.Title}");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private class Person
        {
            public double Age { get; }

            public IDictionary<int, Person> Children { get; set; }

            public string FullName { get; }

            public bool Gender { get; }

            public Person(string fullName, int age, bool gender)
            {
                Gender = gender;
                FullName = fullName;
                Age = age;
            }

            public string Walk(Person withPerson)
                => $"{(Gender ? "He" : "She")} is walking with {withPerson.FullName}!";
        }
    }
}
