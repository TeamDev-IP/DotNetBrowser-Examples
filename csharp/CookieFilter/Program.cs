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
using System.Linq;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net.Handlers;

namespace CookieFilter
{
    /// <summary>
    ///     The sample demonstrates how to suppress/filter incoming and outgoing cookies.
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
                        engine.Profiles.Default.Network.CanGetCookiesHandler =
                            new Handler<CanGetCookiesParameters, CanGetCookiesResponse>(CanGetCookies);
                        engine.Profiles.Default.Network.CanSetCookieHandler =
                            new Handler<CanSetCookieParameters, CanSetCookieResponse>(CanSetCookie);
                        LoadResult result = browser.Navigation.LoadUrl("https://www.google.com").Result;
                        Console.WriteLine("LoadResult: " + result);
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

        private static CanGetCookiesResponse CanGetCookies(CanGetCookiesParameters arg)
        {
            string cookies = arg.Cookies.Aggregate(string.Empty, (current, cookie) => current + (cookie + "\n"));
            Console.WriteLine("CanGetCookies: " + cookies);
            return CanGetCookiesResponse.Deny();
        }

        private static CanSetCookieResponse CanSetCookie(CanSetCookieParameters arg)
        {
            Console.WriteLine("CanSetCookie: " + arg.Cookie);
            return CanSetCookieResponse.Deny();
        }
    }
}