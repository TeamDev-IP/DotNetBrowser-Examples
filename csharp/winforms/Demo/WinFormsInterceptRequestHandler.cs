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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace DotNetBrowser.WinForms.Demo
{
    internal class WinFormsInterceptRequestHandler : IHandler<InterceptRequestParameters, InterceptRequestResponse>
    {
        private const string Domain = "http://internal.host/";
        private const string PrefixTemplate = "{0}.Resources.";
        private static readonly TraceSource Log = new TraceSource("DotNetBrowser.WinForms.Demo");

        private readonly string prefix;

        public WinFormsInterceptRequestHandler()
        {
            prefix = string.Format(PrefixTemplate, typeof(WinFormsInterceptRequestHandler).Namespace);
        }

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
                string resourcePath = ConvertToResourcePath(url);
                byte[] content = FindResource(resourcePath);
                if (content != null)
                {
                    MimeType mimeType = GetMimeType(resourcePath);
                    List<HttpHeader> headers = new List<HttpHeader>
                    {
                        new HttpHeader("Content-Type", mimeType.Value),
                        new HttpHeader("Accept-Ranges", "bytes")
                    };
                    HttpStatusCode statusCode = HttpStatusCode.OK;
                    int offset = 0;
                    int count = content.Length;

                    RangeResult rangeResult = ParseRange(parameters.Headers, content.Length,
                                                         out int rangeStart, out int rangeEnd);
                    if (rangeResult == RangeResult.Satisfiable)
                    {
                        statusCode = HttpStatusCode.PartialContent;
                        offset = rangeStart;
                        count = rangeEnd - rangeStart + 1;
                        headers.Add(new HttpHeader("Content-Range",
                                                   $"bytes {rangeStart}-{rangeEnd}/{content.Length}"));
                    }
                    else if (rangeResult == RangeResult.NotSatisfiable)
                    {
                        statusCode = HttpStatusCode.RequestedRangeNotSatisfiable;
                        count = 0;
                        headers.Add(new HttpHeader("Content-Range", $"bytes */{content.Length}"));
                    }

                    headers.Add(new HttpHeader("Content-Length", count.ToString(CultureInfo.InvariantCulture)));
                    urlRequestJob = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                                                                           new UrlRequestJobOptions
                                                                           {
                                                                               HttpStatusCode = statusCode,
                                                                               Headers = headers
                                                                           });
                    if (count > 0)
                    {
                        urlRequestJob.Write(content, offset, count);
                    }
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

        private string ConvertToResourcePath(string url)
        {
            string path = url.Replace(Domain, string.Empty);
            if (string.IsNullOrWhiteSpace(path) || Equals(path, "/"))
            {
                path = "index.html";
            }

            string resourcePath = path.Replace("/", ".");
            resourcePath = prefix + resourcePath;
            Debug.WriteLine("URL: " + url);
            Debug.WriteLine("Resource: " + resourcePath);

            return resourcePath;
        }

        private RangeResult ParseRange(IEnumerable<IHttpHeader> headers, int contentLength,
                                       out int rangeStart, out int rangeEnd)
        {
            rangeStart = 0;
            rangeEnd = contentLength - 1;

            IHttpHeader rangeHeader = headers.FirstOrDefault(
                header => header.Name.Equals("Range", StringComparison.OrdinalIgnoreCase));
            string rangeValue = rangeHeader?.Values.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(rangeValue) ||
                !rangeValue.StartsWith("bytes=", StringComparison.OrdinalIgnoreCase))
            {
                return RangeResult.None;
            }

            string range = rangeValue.Substring("bytes=".Length).Trim();
            if (range.Contains(","))
            {
                return RangeResult.None;
            }

            string[] boundaries = range.Split('-');
            if (boundaries.Length != 2)
            {
                return RangeResult.None;
            }

            if (string.IsNullOrWhiteSpace(boundaries[0]))
            {
                if (!long.TryParse(boundaries[1], NumberStyles.None, CultureInfo.InvariantCulture,
                                   out long suffixLength))
                {
                    return RangeResult.None;
                }

                if (suffixLength <= 0 || contentLength == 0)
                {
                    return RangeResult.NotSatisfiable;
                }

                rangeStart = (int) Math.Max(0, contentLength - suffixLength);
                return RangeResult.Satisfiable;
            }

            if (!long.TryParse(boundaries[0], NumberStyles.None, CultureInfo.InvariantCulture,
                               out long requestedStart))
            {
                return RangeResult.None;
            }

            if (requestedStart >= contentLength)
            {
                return RangeResult.NotSatisfiable;
            }

            if (string.IsNullOrWhiteSpace(boundaries[1]))
            {
                rangeStart = (int) requestedStart;
                return RangeResult.Satisfiable;
            }

            if (!long.TryParse(boundaries[1], NumberStyles.None, CultureInfo.InvariantCulture,
                               out long requestedEnd) ||
                requestedStart > requestedEnd)
            {
                return RangeResult.None;
            }

            rangeStart = (int) requestedStart;
            rangeEnd = (int) Math.Min(requestedEnd, contentLength - 1);
            return RangeResult.Satisfiable;
        }

        private byte[] FindResource(string url)
        {
            try
            {
                using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(url))
                {
                    if (resourceStream == null)
                    {
                        return null;
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        resourceStream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }

        private MimeType GetMimeType(string url)
        {
            string extension = Path.GetExtension(url);
            if (extension.StartsWith("."))
            {
                extension = extension.Substring(1);
            }

            switch (extension.ToLower())
            {
                case "css":
                    return MimeType.TextCss;
                case "htm":
                case "html":
                    return MimeType.TextHtml;
                case "cur":
                case "ico":
                    return MimeType.Create("image/x-icon");
                case "gif":
                    return MimeType.ImageGif;
                case "js":
                    return MimeType.TextJavascript;
                case "json":
                    return MimeType.ApplicationJson;
                case "pdf":
                    return MimeType.ApplicationPdf;
                case "png":
                    return MimeType.ImagePng;
                case "webm":
                    return MimeType.Create("video/webm");
                default:
                    return MimeType.ApplicationOctetStream;
            }
        }

        private enum RangeResult
        {
            None,
            Satisfiable,
            NotSatisfiable
        }
    }
}
