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
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Js;
using DotNetBrowser.Js.Collections;

namespace JavaScriptBridge.Arrays
{
    /// <summary>
    ///     This example demonstrates how to use IJsArray to
    ///     simplify work with the common JavaScript arrays.
    /// </summary>
    internal class Program
    {
        private const string JsArray = "['Cabbage', 'Turnip', 'Radish', 'Carrot']";

        private static void Main(string[] args)
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    IJsArray arrayObject = browser.MainFrame
                                                   .ExecuteJavaScript<IJsArray>(JsArray)
                                                   .Result;
                    Console.Out.WriteLine($"Item count: {arrayObject.Count}");
                    IReadOnlyList<string> list = arrayObject.ToReadOnlyList<string>();
                    foreach(string item in list)
                    {
                        Console.Out.WriteLine($"Item: {item}");
                    }
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }
}
