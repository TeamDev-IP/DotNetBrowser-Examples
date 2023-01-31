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
using System.Text;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Events;

namespace AccessingHttpResponseData
{
    /// <summary>
    ///     The sample demonstrates how to access HTTP response data.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    engine.Profiles.Default.Network.ResponseBytesReceived += OnResponseBytesReceived;
                    browser.Navigation.LoadUrl("https://teamdev.com").Wait();

                    Console.WriteLine("URL loaded");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void OnResponseBytesReceived(object sender, ResponseBytesReceivedEventArgs eventArgs)
        {
            if (eventArgs.MimeType.Equals(MimeType.TextHtml))
            {
                Console.WriteLine($"MimeType = {eventArgs.MimeType}");
                Console.WriteLine($"The HTTP method = {eventArgs.UrlRequest.Method}");

                if(eventArgs.Data != null)
                {
                    string data = Encoding.UTF8.GetString(eventArgs.Data);
                    Console.WriteLine($"Data = {data}\n");
                }
            }
        }
    }
}
