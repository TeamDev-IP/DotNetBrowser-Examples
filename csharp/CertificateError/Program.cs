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
using DotNetBrowser.Browser;
using DotNetBrowser.Certificates;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;

namespace CertificateErrorSample
{
    /// <summary>
    ///     Demonstrates how to handle SSL certificate errors.
    /// </summary>
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
                        engine.NetworkService.VerifyCertificateHandler =
                            new Handler<CertificateVerifyHandlerParameters, CertificateVerifyResult>(HandleCertError);
                        browser.Navigation.LoadUrl("https://untrusted-root.badssl.com/")
                               .Wait();
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

        private static CertificateVerifyResult HandleCertError(CertificateVerifyHandlerParameters errorParams)
        {
            Certificate certificate = errorParams.Certificate;
            foreach (CertificateVerifyStatus status in errorParams.VerifyStatuses)
            {
                Console.WriteLine("CertificateVerifyStatus = " + status);
            }
            
            Console.WriteLine("SerialNumber = " + certificate.SerialNumber);
            Console.WriteLine("FingerPrint = " + certificate.Fingerprint);
            Console.WriteLine("CAFingerPrint = " + certificate.CaFingerPrint);

            string subject = certificate.Subject;
            Console.WriteLine("Subject = " + subject);

            string issuer = certificate.Issuer;
            Console.WriteLine("Issuer = " + issuer);

            Console.WriteLine("KeyUsages = " + string.Join(", ", certificate.KeyUsages));
            Console.WriteLine("ExtendedKeyUsages = " + string.Join(", ", certificate.ExtendedKeyUsages));

            Console.WriteLine("Expired = " + certificate.Expired);

            // Return Valid to ignore certificate error.
            return CertificateVerifyResult.Valid;
        }

        #endregion
    }
}