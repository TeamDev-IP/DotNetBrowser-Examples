#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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
using DotNetBrowser.Downloads.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;

namespace DownloadPdf
{
    /// <summary>
    ///     This example demonstrates how to download the PDF from the given URL.
    /// </summary>
    internal class Program
    {
        #region Methods

        public static void Main()
        {
            string url = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf";

            try
            {
                using (IEngine engine = EngineFactory.Create(new EngineOptions.Builder
                {
                    RenderingMode = RenderingMode.OffScreen
                }.Build()))
                {
                    Console.WriteLine("Engine created");

                    //1. Disable PDF viewer to trigger PDF file download on loading the URL.
                    engine.Plugins.Settings.PdfViewerEnabled = false;
                    using (IBrowser browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");

                        // 2. Configure the download handler.
                        TaskCompletionSource<string> downloadFinishedTcs = new TaskCompletionSource<string>();
                        browser.StartDownloadHandler = new Handler<StartDownloadParameters, StartDownloadResponse>(p =>
                        {
                            try
                            {
                                Console.WriteLine("Starting download for: " + p.Download.Info.Url);
                                string suggestedFileName = p.Download.Info.SuggestedFileName;
                                string targetPath = Path.GetFullPath(suggestedFileName);
                                p.Download.Finished += (sender, args) =>
                                {
                                    downloadFinishedTcs.TrySetResult(targetPath);
                                };
                                return StartDownloadResponse.DownloadTo(targetPath);
                            }
                            catch (Exception e)
                            {
                                downloadFinishedTcs.TrySetException(e);
                                throw;
                            }
                        });

                        // 2. Load the required web page and wait until it is loaded completely.
                        Console.WriteLine("Loading " + url);
                        browser.Navigation.LoadUrl(url).Wait();
                        Console.WriteLine("URL loaded.");

                        // 3. Wait until the download is finished.
                        string downloadedUrl = downloadFinishedTcs.Task.Result;
                        Console.WriteLine("Download completed for the URL: " + downloadedUrl);
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

        #endregion
    }
}