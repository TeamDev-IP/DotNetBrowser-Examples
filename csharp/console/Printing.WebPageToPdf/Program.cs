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
using System.IO;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Print;
using DotNetBrowser.Print.Handlers;

namespace Printing.WebPageToPdf
{
    /// <summary>
    ///     This example demonstrates how to load a web page and print it to PDF.
    /// </summary>
    internal class Program
    {
        public static async Task Main()
        {
            var engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.OffScreen,
                LicenseKey = "your license key"
            }.Build();

            using var engine = EngineFactory.Create(engineOptions);
            using var browser = engine.CreateBrowser();

            await browser.Navigation.LoadUrl(Path.GetFullPath("template.html"));
            FillInData(browser);

            var whenPrintCompleted = ConfigurePrinting(browser);
            browser.MainFrame.Print();
            var resultPath = await whenPrintCompleted.Task;
            Console.WriteLine($"PDF is generated: {resultPath}");
            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void FillInData(IBrowser browser)
        {
            var accountNumber = "123-4567";
            var name = "Dr. Emmett Brown";
            var address = "1640 Riverside Drive";
            var reportingPeriod = "Oct 25 — November 25, 1985";

            // This JavaScript function is embedded into the template HTML page.
            // Since this is a regular web page, you can use any JavaScript library,
            // WebGL, SVG, and other technologies available in Chromium.
            browser.MainFrame.ExecuteJavaScript(
                $"setBillInfo('{accountNumber}', '{name}', '{address}', '{reportingPeriod}')"
            );

            var dayCost = 500;
            var nightCost = 312;
            var dayUsage = 1.212;
            var nightUsage = 88;

            browser.MainFrame.ExecuteJavaScript(
                $"addCharge('Day Tariff', {dayUsage}, {dayCost});" +
                $"addCharge('Night Tariff', {nightUsage}, {nightCost});"
            );
        }

        private static TaskCompletionSource<string> ConfigurePrinting(IBrowser browser)
        {
            // Tell the browser to print automatically instead of showing the print preview.
            browser.RequestPrintHandler = new Handler<RequestPrintParameters, RequestPrintResponse>(
               p => RequestPrintResponse.Print()
            );

            TaskCompletionSource<string> whenCompleted = new();
            // When the browser prints an HTML page.
            browser.PrintHtmlContentHandler = new Handler<PrintHtmlContentParameters, PrintHtmlContentResponse>(
                parameters =>
                {
                    // Use the PDF printer.
                    var printer = parameters.Printers.Pdf;
                    var job = printer.PrintJob;

                    // Generate a random name for PDF file.
                    var guid = Guid.NewGuid();
                    var path = Path.GetFullPath($"{guid}.pdf");
                    job.Settings.PdfFilePath = path;

                    // Remove white areas on the sides.
                    job.Settings.PageMargins = PageMargins.None;
                    // Remove default browser headers and footers.
                    job.Settings.PrintingHeaderFooterEnabled = false;
                    job.PrintCompleted += (_, _) => whenCompleted.SetResult(path);

                    // Proceed with printing using the PDF printer.
                    return PrintHtmlContentResponse.Print(printer);
                });
            return whenCompleted;
        }
    }
}
