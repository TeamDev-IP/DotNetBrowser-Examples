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
using System.IO;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Js;

namespace ShadowDom
{
    /// <summary>
    ///     This example demonstrates how to work with Shadow DOM.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = new Size(1024, 768);
                    browser.Navigation.LoadUrl(Path.GetFullPath("example.html")).Wait();

                    Console.WriteLine("URL loaded");

                    IDocument document = browser.MainFrame.Document;
                    IJsObject container = document.GetElementById("container") as IJsObject;

                    //Create shadow root.
                    INode shadowRoot = container?.Invoke<INode>("attachShadow",
                     browser
                        .MainFrame
                        .ParseJsonString("{\"mode\": \"open\"}"));
                    Console.WriteLine($"Shadow root created: {(shadowRoot != null)}");

                    //Fetch shadow root.
                    shadowRoot = container?.Properties["shadowRoot"] as INode;
                    Console.WriteLine($"Shadow root fetched: {(shadowRoot != null)}");

                    //Add node to shadow root.
                    IElement inside = document.CreateElement("h1");
                    inside.InnerText = "Inside Shadow DOM";
                    inside.Attributes["id"] = "inside";
                    shadowRoot?.Children.Append(inside);

                    //Find new node in shadow root.
                    IElement element = shadowRoot?.GetElementById("inside");
                    Console.WriteLine($"Inside element inner text: {element?.InnerText}");

                    //Try finding the same node from the main document.
                    element = document.GetElementById("inside");
                    Console.WriteLine($"Inside element found in the document: {(element != null)}");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }
}
