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
using System.Collections.Generic;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;

namespace DomGetElements
{
    /// <summary>
    ///     This example demonstrates how to get the collection of the &lt;div&gt; elements
    ///     on the web page.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Navigation
                           .LoadUrl("https://www.teamdev.com")
                           .Wait();

                    IDocument document = browser.MainFrame.Document;
                    IEnumerable<INode> divs = document.GetElementsByTagName("div");

                    foreach (INode node in divs)
                    {
                        PrintBoundingClientRect(node);
                    }
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void PrintBoundingClientRect(INode node)
        {
            IElement div = node as IElement;

            if (div != null)
            {
                Rectangle rect = div.BoundingClientRect;

                Console
                   .Out.WriteLine($"class = {div.Attributes["class"]};\n"
                                  + $" boundingClientRect.Top = {rect.Origin.Y};\n"
                                  + $" boundingClientRect.Left = {rect.Origin.X};\n"
                                  + $" boundingClientRect.Width = {rect.Size.Width};\n"
                                  + $" boundingClientRect.Height = {rect.Size.Height}");
            }
        }
    }
}
