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
using System.IO;
using System.Net;
using System.Reflection;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace RichTextEditor
{
    /// <summary>
    ///     Serves the editor page and Quill from the app's embedded resources
    ///     at app://editor/, so the editor works without a network connection.
    /// </summary>
    public sealed class EditorScheme
        : IHandler<InterceptRequestParameters, InterceptRequestResponse>
    {
        public const string PageUrl = "app://editor/editor.html";
        public static readonly Scheme Scheme = Scheme.Create("app");

        private const string Host = "editor";
        private static readonly Assembly Assembly = typeof(EditorScheme).Assembly;

        public InterceptRequestResponse Handle(InterceptRequestParameters parameters)
        {
            Uri uri = new Uri(parameters.UrlRequest.Url);
            string name = Path.GetFileName(uri.AbsolutePath);
            Stream? resource = uri.Host == Host
                                   ? Assembly.GetManifestResourceStream(
                                       "RichTextEditor.Editor." + name)
                                   : null;

            UrlRequestJobOptions options = new UrlRequestJobOptions
            {
                HttpStatusCode = resource == null
                                     ? HttpStatusCode.NotFound
                                     : HttpStatusCode.OK,
                Headers = { new HttpHeader("Content-Type", MimeTypeOf(name)) }
            };
            UrlRequestJob job =
                parameters.Network.CreateUrlRequestJob(parameters.UrlRequest, options);
            if (resource != null)
            {
                using (resource)
                using (MemoryStream bytes = new MemoryStream())
                {
                    resource.CopyTo(bytes);
                    job.Write(bytes.ToArray());
                }
            }
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }

        private static string MimeTypeOf(string name)
        {
            switch (Path.GetExtension(name))
            {
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "text/javascript";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
