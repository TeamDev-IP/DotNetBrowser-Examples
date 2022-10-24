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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Print;
using DotNetBrowser.Print.Handlers;
using DotNetBrowser.Print.Settings;

namespace Printing.WebPageToPdf
{
    /// <summary>
    ///     This example demonstrates how to load a web page and print it to PDF.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            string url =
                "https://dotnetbrowser-support.teamdev.com/docs/guides/gs/printing.html";
            string pdfFilePath = Path.GetFullPath("result.pdf");

            EngineOptions engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.OffScreen,
                GpuDisabled = true
            }.Build();

            using (IEngine engine = EngineFactory.Create(engineOptions))
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    // #docfragment "PrintToPdf"
                    // Resize browser to the required dimension.
                    browser.Size = new Size(1024, 768);

                    // Load the required web page and wait until it is loaded completely.
                    Console.WriteLine($"Loading {url}");
                    browser.Navigation.LoadUrl(url).Wait();


                    // Configure print handlers.
                    browser.RequestPrintHandler =
                        new Handler<RequestPrintParameters, RequestPrintResponse
                        >(p => RequestPrintResponse.Print());

                    TaskCompletionSource<string> printCompletedTcs =
                        new TaskCompletionSource<string>();
                    browser.PrintHtmlContentHandler
                        = new Handler<PrintHtmlContentParameters, PrintHtmlContentResponse
                        >(p =>
                        {
                            try
                            {
                                // Get the print job for the built-in PDF printer.
                                PdfPrinter<PdfPrinter.IHtmlSettings> pdfPrinter =
                                    p.Printers.Pdf;
                                IPrintJob<PdfPrinter.IHtmlSettings> printJob =
                                    pdfPrinter.PrintJob;

                                // Apply the necessary print settings.
                                printJob.Settings.Apply(s =>
                                {
                                    IReadOnlyCollection<PaperSize> paperSizes =
                                        pdfPrinter.Capabilities.PaperSizes;
                                    s.PaperSize =
                                        paperSizes.First(size => size.Name.Contains("A4"));

                                    s.PrintingHeaderFooterEnabled = true;
                                    // Specify the path to save the result.
                                    s.PdfFilePath = pdfFilePath;
                                });

                                string browserUrl = p.Browser.Url;
                                printJob.PrintCompleted += (sender, args) =>
                                {
                                    // Set the task result when the printing is completed.
                                    printCompletedTcs.TrySetResult(browserUrl);
                                };

                                // Tell Chromium to use the built-in PDF printer
                                // for printing.
                                return PrintHtmlContentResponse.Print(pdfPrinter);
                            }
                            catch (Exception e)
                            {
                                printCompletedTcs.TrySetException(e);
                                throw;
                            }
                        });

                    // Initiate printing and wait until it is completed.
                    Console.WriteLine("URL loaded. Initiate printing");
                    browser.MainFrame.Print();
                    string printedUrl = printCompletedTcs.Task.Result;
                    Console.WriteLine($"Printing completed for the URL: {printedUrl}");
                    // #enddocfragment "PrintToPdf"
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }
    }
}
