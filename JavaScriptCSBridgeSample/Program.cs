using DotNetBrowser;
using DotNetBrowser.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JavaScriptCSBridgeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Browser browser = BrowserFactory.Create())
            {
                ManualResetEvent waitEvent = new ManualResetEvent(false);
                ManualResetEvent waitEvent2 = new ManualResetEvent(false);

                // Modifies document.title value via JavaScript-Java Bridge API
                JSValue document = browser.ExecuteJavaScriptAndReturnValue("document");
                if (document.IsObject())
                {
                    document.AsObject().SetProperty("title", "My Title");
                }

                bool first = true;
                browser.TitleChangedEvent += delegate(object sender, TitleEventArgs e)
                {
                    Console.Out.WriteLine("\"document.title\" \"" + e.Title + "\"");
                    waitEvent.Set();
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        waitEvent2.Set();
                    }
                };


                browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
                {
                    if (e.IsMainFrame)
                    {
                        var person = new Person("Jack", 30, true);
                        person.Children = new Dictionary<int, Person>();
                        person.Children.Add(1, new Person("Oliver", 10, true));
                        JSValue value = browser.ExecuteJavaScriptAndReturnValue("ShowData");
                        ((JSFunction)value).Invoke(null, person);

                    }
                };


                browser.LoadHTML(@"<html>
                                     <body>
                                        <script type='text/javascript'>
                                            var ShowData = function (a) 
                                            {
                                                 document.title = a.FullName + ' ' + a.Age + '. ' + a.Walk(a.Children[1])
                                                                + ' ' + a.Children[1].FullName + ' ' + a.Children[1].Age;
                                            };
                                        </script>
                                     </body>
                                   </html>");


                waitEvent.WaitOne();
                waitEvent2.WaitOne();
                Thread.Sleep(3000);
            }
         }

        private class Person
        {
            public string FullName { get; private set; }

            public int Age {get; private set;}

            public bool Gender { get; set; }

            public IDictionary<int, Person> Children { get; set; }

            public Person(string fullName, int age, bool gender)
            {
                this.Gender = gender;
                this.FullName = fullName;
                this.Age = age;
            }

            public string Walk(Person withPerson)
            {
                return String.Format("{0} is walking with {1}!", this.Gender ? "He" : "She", withPerson.FullName);
            }

        }

    }
}
