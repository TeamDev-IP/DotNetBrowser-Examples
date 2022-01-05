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
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Zoom;
using DotNetBrowser.Zoom.Events;

namespace Zoom
{
    /// <summary>
    ///     The sample demonstrates how to set the zoom level using DotNetBrowser API.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            try
            {
                using (IEngine engine = EngineFactory.Create())
                {
                    Console.WriteLine("Engine created");

                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");
                        engine.Profiles.Default.ZoomLevels.LevelChanged +=
                            delegate(object sender, LevelChangedEventArgs e)
                            {
                                Console.Out.WriteLine("e.Host = " + e.Host);
                                Console.Out.WriteLine("e.ZoomLevel = " + e.Level.Value);
                            };

                        browser.Navigation.LoadUrl("https://www.teamdev.com").Wait();
                        Console.WriteLine("Updating zoom level");
                        browser.Zoom.Level = Level.P200;
                        Thread.Sleep(3000);
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
    }
}
