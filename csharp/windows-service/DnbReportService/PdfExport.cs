#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Handlers;
using DotNetBrowser.Print;
using DotNetBrowser.Print.Handlers;

namespace DnbReportService
{
    /// <summary>
    ///     Prints the loaded page to a PDF file with the built-in PDF
    ///     printer: no print preview, no system printer, no desktop.
    /// </summary>
    internal static class PdfExport
    {
        /// <summary>
        ///     Prints the page in <paramref name="browser" /> to
        ///     <paramref name="pdfPath" />, which must be a full path.
        /// </summary>
        /// <returns>A task that ends when the PDF file is written.</returns>
        public static Task SaveAsync(IBrowser browser, string pdfPath)
        {
            TaskCompletionSource done = new TaskCompletionSource(
                TaskCreationOptions.RunContinuationsAsynchronously);

            // Skip the print preview and print right away.
            browser.RequestPrintHandler =
                new Handler<RequestPrintParameters, RequestPrintResponse>(
                    p => RequestPrintResponse.Print());

            // Send HTML pages to the PDF printer instead of a real one.
            browser.PrintHtmlContentHandler =
                new Handler<PrintHtmlContentParameters,
                            PrintHtmlContentResponse>(p =>
                {
                    PdfPrinter<PdfPrinter.IHtmlSettings> pdf =
                        p.Printers.Pdf;
                    pdf.PrintJob.Settings.PdfFilePath = pdfPath;
                    pdf.PrintJob.Settings.PrintingHeaderFooterEnabled = false;
                    pdf.PrintJob.PrintCompleted += (s, e) =>
                    {
                        if (e.IsCompletedSuccessfully)
                        {
                            done.TrySetResult();
                        }
                        else
                        {
                            done.TrySetException(new InvalidOperationException(
                                "Printing to PDF failed."));
                        }
                    };
                    return PrintHtmlContentResponse.Print(pdf);
                });

            browser.MainFrame.Print();
            return done.Task;
        }
    }
}
