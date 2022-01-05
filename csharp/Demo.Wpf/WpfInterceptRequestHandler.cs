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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Resources;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace Demo.Wpf
{
    internal class WpfInterceptRequestHandler : IHandler<InterceptRequestParameters, InterceptRequestResponse>
    {
        private const string Domain = "http://www.popuptest.com/";
        private const string Prefix = "Resources\\popups\\";
        private static readonly TraceSource Log = new TraceSource("DotNetBrowser.Demo.Wpf");

        public InterceptRequestResponse Handle(InterceptRequestParameters parameters)
        {
            string url = parameters.UrlRequest.Url;
            if (!url.StartsWith(Domain))
            {
                return InterceptRequestResponse.Proceed();
            }

            UrlRequestJob urlRequestJob;
            try
            {
                StreamResourceInfo content = FindResource(url);
                if (content != null)
                {
                    urlRequestJob = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                                                                           new UrlRequestJobOptions
                                                                           {
                                                                               HttpStatusCode = HttpStatusCode.OK,
                                                                               Headers = new List<HttpHeader>
                                                                               {
                                                                                   new HttpHeader("Content-Type",
                                                                                                  content.ContentType)
                                                                               }
                                                                           });
                    urlRequestJob.Write(GetBytes(content.Stream));
                }
                else
                {
                    Debug.WriteLine("Resource was not found.");
                    urlRequestJob = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                                                                           new UrlRequestJobOptions
                                                                           {
                                                                               HttpStatusCode = HttpStatusCode.NotFound
                                                                           });
                }


                urlRequestJob.Complete();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Log.TraceEvent(TraceEventType.Error, 1, e.ToString());

                urlRequestJob = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                                                                       new UrlRequestJobOptions
                                                                       {
                                                                           HttpStatusCode =
                                                                               HttpStatusCode.InternalServerError
                                                                       });
            }

            return InterceptRequestResponse.Intercept(urlRequestJob);
        }

        private string ConvertToLocalPath(string url)
        {
            string path = url.Replace(Domain, string.Empty);
            if (string.IsNullOrWhiteSpace(path) || Equals(path, "/"))
            {
                path = "index.html";
            }

            string resourcePath = path.Replace("/", "\\");
            resourcePath = Prefix + resourcePath;
            Debug.WriteLine("URL: " + url);
            Debug.WriteLine("Resource: " + resourcePath);

            return resourcePath;
        }

        private StreamResourceInfo FindResource(string url)
        {
            string localPath = ConvertToLocalPath(url);
            Uri uri = new Uri(localPath, UriKind.Relative);
            try
            {
                StreamResourceInfo streamResourceInfo = Application.GetResourceStream(uri);
                return streamResourceInfo;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        private byte[] GetBytes(Stream contentStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                contentStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
