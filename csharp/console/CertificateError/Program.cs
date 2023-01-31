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
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net.Certificates;
using DotNetBrowser.Net.Handlers;

namespace CertificateError
{
    /// <summary>
    ///     The sample demonstrates how to handle SSL certificate errors.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    engine.Profiles.Default.Network.VerifyCertificateHandler =
                        new Handler<VerifyCertificateParameters,
                            VerifyCertificateResponse>(HandleCertError);

                    browser.Navigation.LoadUrl("https://untrusted-root.badssl.com/").Wait();
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static VerifyCertificateResponse HandleCertError(VerifyCertificateParameters errorParams)
        {
            PrintCertificateAndErrorDetails(errorParams);

            // Return Valid to ignore certificate error.
            return VerifyCertificateResponse.Valid();
        }

        private static void PrintCertificateAndErrorDetails(VerifyCertificateParameters errorParams)
        {
            foreach (CertificateVerificationError certificateVerificationError in errorParams.VerifyErrors)
            {
                Console.WriteLine($"CertificateVerificationStatus = {certificateVerificationError.VerifyStatus}");
                Console.WriteLine($"Short description = {certificateVerificationError.ShortDescription}");
                Console.WriteLine($"Detailed description = {certificateVerificationError.DetailedDescription}");
            }

            Certificate certificate = errorParams.Certificate;

            Console.WriteLine($"SerialNumber = {certificate.SerialNumber}");
            Console.WriteLine($"FingerPrint = {certificate.Fingerprint}");
            Console.WriteLine($"CAFingerPrint = {certificate.CaFingerPrint}");
            Console.WriteLine($"Subject = {certificate.Subject}");
            Console.WriteLine($"Issuer = {certificate.Issuer}");
            Console.WriteLine($"KeyUsages = {string.Join(", ", certificate.KeyUsages)}");
            Console.WriteLine($"ExtendedKeyUsages = {string.Join(", ", certificate.ExtendedKeyUsages)}");
            Console.WriteLine($"Expired = {certificate.Expired}");
        }
    }
}
