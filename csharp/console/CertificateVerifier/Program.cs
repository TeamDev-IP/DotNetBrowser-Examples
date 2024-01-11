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
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net.Handlers;

namespace CertificateVerifier
{
    /// <summary>
    ///     The sample demonstrates how to accept/reject SSL certificates using
    ///     custom SSL certificate verifier.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    engine.Profiles.Default.Network.VerifyCertificateHandler =
                        new Handler<VerifyCertificateParameters,
                            VerifyCertificateResponse>(VerifyCert);

                    NavigationResult result = browser.Navigation.LoadUrl("https://google.com").Result;

                    Console.WriteLine($"LoadResult: {result.LoadResult}");
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static VerifyCertificateResponse VerifyCert(VerifyCertificateParameters parameters)
        {
            // Reject SSL certificate for all "google.com" hosts.
            if (parameters.HostName.Contains("google.com"))
            {
                Console.WriteLine($"Rejected certificate for {parameters.HostName}");
                return VerifyCertificateResponse.Invalid();
            }

            return VerifyCertificateResponse.Default();
        }
    }
}
