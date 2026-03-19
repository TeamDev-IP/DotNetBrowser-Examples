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
using System.IO;
using System.Net;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace LocalAiAssistant;

internal sealed class LocalAppSchemeHandler
    : IHandler<InterceptRequestParameters, InterceptRequestResponse>
{
    public const string Domain = "dnb://app/";
    public static readonly Scheme Scheme = Scheme.Create("dnb");

    public InterceptRequestResponse Handle(InterceptRequestParameters parameters)
    {
        string url = parameters.UrlRequest.Url;
        if (!url.StartsWith(Domain, StringComparison.Ordinal))
        {
            return InterceptRequestResponse.Proceed();
        }

        string relativePath = GetRelativePath(url);
        string fullPath = Path.Combine(AppContext.BaseDirectory, "web", relativePath);

        UrlRequestJob job;
        if (!File.Exists(fullPath))
        {
            job = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                new UrlRequestJobOptions
                {
                    HttpStatusCode = HttpStatusCode.NotFound
                });
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }

        job = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
            new UrlRequestJobOptions
            {
                HttpStatusCode = HttpStatusCode.OK,
                Headers = new List<HttpHeader>
                {
                    new HttpHeader("Content-Type", GetMimeType(fullPath).Value)
                }
            });

        job.Write(File.ReadAllBytes(fullPath));
        job.Complete();
        return InterceptRequestResponse.Intercept(job);
    }

    private static string GetRelativePath(string url)
    {
        string path = url[Domain.Length..];
        if (string.IsNullOrWhiteSpace(path) || path == "/")
        {
            return "index.html";
        }

        return path.TrimStart('/');
    }

    private static MimeType GetMimeType(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".css" => MimeType.TextCss,
            ".html" => MimeType.TextHtml,
            ".js" => MimeType.TextJavascript,
            _ => MimeType.ApplicationOctetStream
        };
    }
}
