#region Copyright

// Copyright 2019, TeamDev. All rights reserved.
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

namespace DOMGetAttributesSample
{
    internal class Program
    {
        #region Methods

        public static void Main()
        {
            try
            {
                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder().Build()))
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");

                        browser.MainFrame
                               .LoadHtml("<html><body><a href='#' id='link' title='link title'></a></body></html>")
                               .Wait();
                        IDocument document = browser.MainFrame.Document;
                        IElement link = document.GetElementById("link");
                        IDictionary<string, string> attributes = link.Attributes;
                        Console.WriteLine("Link attributes: ");
                        foreach (var attribute in attributes)
                        {
                            Console.WriteLine($"- {attribute.Key} = {attribute.Value}");
                        }
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

        #endregion
    }
}