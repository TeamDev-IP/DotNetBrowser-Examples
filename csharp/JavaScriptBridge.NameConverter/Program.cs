#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Reflection;
using System.Text;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Frames.Handlers;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;

namespace JavaScriptBridge.NameConverter
{
    /// <summary>
    ///     This example demonstrates how to handle JavaScript name converting.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
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
                                                 document.title = a.fullName 
                                                                + ' ' + a.FullYears + '. ' + a.Walk(a.Children[1])
                                                                + ' ' + a.Children[1].fullName 
                                                                + ' ' + a.Children[1].FullYears;
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

                    browser.ConvertJsNameHandler =
                        new Handler<ConvertJsNameParameters,
                            ConvertJsNameResponse>(OnConvertJsName);

                    value.Invoke("ShowData", person);

                    Console.WriteLine($"\tBrowser title: {browser.Title}");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static ConvertJsNameResponse OnConvertJsName(ConvertJsNameParameters arg)
        {
            if (arg.MemberInfo.Name.Equals("FullName"))
            {
                return ConvertJsNameResponse.CamelCase;
            }

            if (arg.MemberInfo.MemberType == MemberTypes.Property
                && arg.MemberInfo.Name.Equals("Age"))
            {
                return ConvertJsNameResponse.ConvertTo("FullYears");
            }

            return ConvertJsNameResponse.NoConversion;
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