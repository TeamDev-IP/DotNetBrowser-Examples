using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CertificateErrorSample
{
    /// <summary>
    /// Demonstrates how to handle SSL certificate errors.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Browser browser = BrowserFactory.Create();

            browser.LoadHandler = new SampleLoadHandler();

            browser.LoadURL("<https-url-with-invalid-ssl-certificate>");

        }

        private class SampleLoadHandler : DefaultLoadHandler
        {
            public override bool OnCertificateError(CertificateErrorParams errorParams)
            {
                Certificate certificate = errorParams.Certificate;

                Console.WriteLine("ErrorCode = " + errorParams.CertificateError);
                Console.WriteLine("SerialNumber = " + certificate.SerialNumber);
                Console.WriteLine("FingerPrint = " + certificate.FingerPrint);
                Console.WriteLine("CAFingerPrint = " + certificate.CAFingerPrint);

                string subject = certificate.Subject;
                Console.WriteLine("Subject = " + subject);

                string issuer = certificate.Issuer;
                Console.WriteLine("Issuer = " + issuer);

                Console.WriteLine("KeyUsages = " + String.Join(", ", certificate.KeyUsages));
                Console.WriteLine("ExtendedKeyUsages = " + String.Join(", ", certificate.ExtendedKeyUsages));

                Console.WriteLine("HasExpired = " + certificate.HasExpired);

                // Return false to ignore certificate error.
                return false;
            }
        }
    }
}
