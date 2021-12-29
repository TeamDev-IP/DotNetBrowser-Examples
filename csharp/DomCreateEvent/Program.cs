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
using DotNetBrowser.Dom.Events;
using DotNetBrowser.Engine;

namespace DomCreateEvent
{
    /// <summary>
    ///     This example demonstrates how to create and dispatch a DOM event.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    byte[] htmlBytes =
                        Encoding.UTF8.GetBytes("<html><body><div id='root'></div></body></html>");

                    browser.Navigation
                           .LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}")
                           .Wait();

                    IDocument document = browser.MainFrame.Document;
                    INode root = document.GetElementById("root");

                    EventType eventType = new EventType("MyEvent");
                    var myEvent =
                        document.CreateEvent(eventType, new EventParameters.Builder().Build());

                    EventHandler<DomEventArgs> domEventHandler = (s, e) =>
                    {
                        if (e.Event.Type == eventType)
                        {
                            Console.WriteLine($"DOM event received: {eventType.Value}");
                            INode textNode = document.CreateTextNode("Some text");
                            IElement paragraph = document.CreateElement("p");
                            paragraph.Children.Append(textNode);
                            root.Children.Append(paragraph);
                        }
                    };

                    root.Events[eventType] += domEventHandler;
                    Console.WriteLine($"Dispatch custom DOM event: {eventType.Value}");
                    root.DispatchEvent(myEvent);
                    Thread.Sleep(3000);
                    Console.WriteLine($"Updated HTML: {browser.MainFrame.Html}");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }
}