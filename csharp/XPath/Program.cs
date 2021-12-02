﻿#region Copyright

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
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Dom.XPath;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;

namespace XPath
{
    /// <summary>
    ///     The sample demonstrates how to evaluate an XPath expression and work
    ///     with the evaluation result.
    /// </summary>
    internal class Program
    {
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
                        browser.Size = new Size(1024, 768);

                        browser.Navigation.LoadUrl("https://www.teamdev.com/dotnetbrowser").Wait();
                        IDocument document = browser.MainFrame.Document;

                        string expression = "count(//div)";
                        Console.WriteLine($"Evaluating \'{expression}\'");
                        IXPathResult result = document.Evaluate(expression);

                        // Make sure that result is a number.
                        if (result.Type == XPathResultType.Number)
                        {
                            Console.WriteLine("Result: " + result.Numeric);
                        }
                    }
                }
            }
            // If the expression is not a valid XPath expression or the document
            // element is not available, we'll get an error.
            catch (XPathException e)
            {
                Console.WriteLine("Error message: " + e.Message);
                return;
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