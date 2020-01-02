#region Copyright

// Copyright © 2020, TeamDev. All rights reserved.
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
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace PostData
{
    /// <summary>
    ///     This sample demonstrates how to read and modify POST parameters using BeforeSendUploadDataHandler.
    /// </summary>
    internal class Program
    {
        #region Methods

        public static void Main()
        {
            try
            {
                using (var engine = EngineFactory.Create(new EngineOptions.Builder().Build()))
                {
                    Console.WriteLine("Engine created");

                    using (var browser = engine.CreateBrowser())
                    {
                        Console.WriteLine("Browser created");
                        engine.NetworkService.BeforeSendUploadDataHandler =
                            new Handler<BeforeSendUploadDataParameters, BeforeSendUploadDataResponse
                            >(OnBeforeSendUploadData);

                        var parameters =
                            new LoadUrlParameters("https://postman-echo.com/post")
                            {
                                PostData = "key=value",
                                HttpHeaders = new[] {new HttpHeader("Content-Type", "text/plain")}
                            };
                        browser.Navigation.LoadUrl(parameters).Wait();
                        Console.WriteLine(browser.MainFrame.Document.DocumentElement.InnerText);
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

        public static BeforeSendUploadDataResponse OnBeforeSendUploadData(BeforeSendUploadDataParameters parameters)
        {
            if ("POST" == parameters.UrlRequest.Method)
            {
                var uploadData = parameters.UploadData;
                if (uploadData.Type == UploadDataType.TextData)
                {
                    Console.WriteLine($"Text data intercepted: {uploadData.TextData}");
                    return BeforeSendUploadDataResponse.Override(new FormData
                    {
                        {"fname", "MyName"},
                        {"lname", "MyLastName"}
                    });
                }
            }
            return BeforeSendUploadDataResponse.Ignore();
        }

        #endregion
    }
}