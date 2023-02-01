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
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;

namespace SeparateEngines.AppDomains
{
    /// <summary>
    ///     This example demonstrates how to create and use separate IEngine instances
    ///     in different application domains.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create two separate application domains.
            AppDomain domain1 = AppDomain.CreateDomain("Domain1");
            AppDomain domain2 = AppDomain.CreateDomain("Domain2");

            // Create an instance of EngineWrapper in the first AppDomain.
            // A proxy to the object is returned.
            Console.WriteLine("Create wrapper1");
            EngineWrapper wrapper1 = (EngineWrapper)
                domain1.CreateInstanceAndUnwrap(typeof(EngineWrapper).Assembly.FullName,
                                                typeof(EngineWrapper).FullName);

            // Create an instance of EngineWrapper in the second AppDomain.
            // A proxy to the object is returned.
            Console.WriteLine("Create wrapper2");
            EngineWrapper wrapper2 = (EngineWrapper)
                domain2.CreateInstanceAndUnwrap(typeof(EngineWrapper).Assembly.FullName,
                                                typeof(EngineWrapper).FullName);

            //Execute an action in the first application domain.
            string title1 = wrapper1.LoadAndGetTitle("teamdev.com");
            Console.WriteLine("Title 1: {0}", title1);

            //Dispose the wrapper and unload the first application domain.
            Console.WriteLine("Dispose wrapper1");
            wrapper1.Dispose();
            AppDomain.Unload(domain1);

            //After unloading the first domain, the engine in the second domain is alive, and we can execute actions.
            string title2 = wrapper2.LoadAndGetTitle("teamdev.com/dotnetbrowser");
            Console.WriteLine("Title 2: {0}", title2);

            //Dispose the wrapper and unload the second application domain.
            Console.WriteLine("Dispose wrapper2");
            wrapper2.Dispose();
            AppDomain.Unload(domain2);

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }

    /// <summary>
    ///     This class serves as a wrapper for DotNetBrowser objects and the logic related to these objects,
    ///     since they cannot be marshaled directly.
    /// </summary>
    internal class EngineWrapper : MarshalByRefObject, IDisposable
    {
        private IEngine Engine { get; }

        public EngineWrapper()
        {
            //Perform complex engine initialization here if necessary.
            Engine = EngineFactory.Create();
        }

        public void Dispose()
        {
            Engine?.Dispose();
        }

        /// <summary>
        ///     This method demonstrates how to implement a particular scenario that
        ///     should be performed in a separate application domain.
        ///     <para>
        ///         For instance, this method returns a web page title after loading that web page completely
        ///         in a separate browser instance.
        ///     </para>
        /// </summary>
        /// <param name="url">The URL to load.</param>
        /// <returns>The title of the loaded web page.</returns>
        public string LoadAndGetTitle(string url)
        {
            string result = null;
            Console.WriteLine("Loading URL '{0}' in '{1}'.", url,
                              Thread.GetDomain().FriendlyName);
            using (IBrowser browser = Engine.CreateBrowser())
            {
                Console.WriteLine("Browser created");
                browser.Size = new Size(700, 500);
                browser.Navigation.LoadUrl(url).Wait();
                Console.WriteLine("URL loaded");
                result = browser.Title;
            }

            return result;
        }
    }
}
