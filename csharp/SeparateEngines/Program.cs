#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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
using DotNetBrowser.Engine;

namespace SeparateEngines
{
    /// <summary>
    ///     The sample demonstrates how to create several Chromium engines.
    /// </summary>
    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            try
            {
                string userDataDir1 = Path.GetFullPath("user-data-dir-one");
                Directory.CreateDirectory(userDataDir1);
                IEngine engine1 = EngineFactory.Create(new EngineOptions.Builder
                {
                    UserDataDirectory = userDataDir1
                }.Build());
                Console.WriteLine("Engine1 created");

                string userDataDir2 = Path.GetFullPath("user-data-dir-two");
                Directory.CreateDirectory(userDataDir2);
                IEngine engine2 = EngineFactory.Create(new EngineOptions.Builder
                {
                    UserDataDirectory = userDataDir2
                }.Build());
                Console.WriteLine("Engine2 created");

                // This Browser instance will store cookies and user data files in "user-data-dir-one" dir.
                IBrowser browser1 = engine1.CreateBrowser();
                Console.WriteLine("browser1 created");

                // This Browser instance will store cookies and user data files in "user-data-dir-two" dir.
                IBrowser browser2 = engine2.CreateBrowser();
                Console.WriteLine("browser2 created");

                // The browser1 and browser2 instances will not see the cookies and cache data files of each other.

                engine2.Dispose();
                engine1.Dispose();
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