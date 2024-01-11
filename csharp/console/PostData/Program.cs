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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace PostData
{
    /// <summary>
    ///     This sample demonstrates how to read and modify POST parameters using SendUploadDataHandler.
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    engine.Profiles.Default.Network.SendUploadDataHandler =
                        new Handler<SendUploadDataParameters,
                            SendUploadDataResponse>(OnSendUploadData);

                    LoadUrlParameters parameters =
                        new LoadUrlParameters("https://postman-echo.com/post")
                        {
                            PostData = "key=value",
                            HttpHeaders = new[]
                            {
                                new HttpHeader("Content-Type", "text/plain")
                            }
                        };

                    browser.Navigation.LoadUrl(parameters).Wait();
                    Console.WriteLine(browser.MainFrame.Document.DocumentElement.InnerText);
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        public static SendUploadDataResponse OnSendUploadData(SendUploadDataParameters parameters)
        {
            if ("POST" == parameters.UrlRequest.Method)
            {
                IUploadData uploadData = parameters.UploadData;
                TextData textData = uploadData as TextData;
                if (textData != null)
                {
                    Console.WriteLine($"Text data intercepted: {textData.Data}");
                    return SendUploadDataResponse
                       .Override(new FormData(new ReadOnlyCollection<KeyValuePair<string, string>>
                                                  (new List<KeyValuePair<string, string>>
                                                  {
                                                      new KeyValuePair<string, string>("fname", "MyName"),
                                                      new KeyValuePair<string, string>("lname", "MyLastName")
                                                  })));
                }
            }

            return SendUploadDataResponse.Continue();
        }
    }
}
