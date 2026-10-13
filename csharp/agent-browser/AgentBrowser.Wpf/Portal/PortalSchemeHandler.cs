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
using System.Reflection;
using System.Text;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace AgentBrowser.Portal
{
    /// <summary>
    ///     Serves the mock supplier portal from embedded resources at
    ///     dnb://portal/. Nothing is read from disk, so there is no path to escape.
    /// </summary>
    internal sealed class PortalSchemeHandler : IHandler<InterceptRequestParameters, InterceptRequestResponse>
    {
        public const string Origin = "dnb://portal/";
        public static readonly Scheme Scheme = Scheme.Create("dnb");

        private const string ResourcePrefix = "AgentBrowser.Portal.";
        private static readonly Dictionary<string, string> OrderLabels = new()
        {
            ["part"] = "Part number", ["quantity"] = "Quantity", ["warehouse"] = "Deliver to", ["notes"] = "Notes"
        };
        private static readonly Assembly Assembly = typeof(PortalSchemeHandler).Assembly;

        // The demo "session": set by a successful login, held in memory only.
        private volatile bool signedIn;

        public InterceptRequestResponse Handle(InterceptRequestParameters parameters)
        {
            string url = parameters.UrlRequest.Url;
            if (!url.StartsWith(Origin, StringComparison.Ordinal))
            {
                return InterceptRequestResponse.Proceed();
            }

            string path = new Uri(url).AbsolutePath.TrimStart('/');
            string method = parameters.UrlRequest.Method;

            if (method == "POST" && path == "login")
            {
                IReadOnlyDictionary<string, string> form = ReadForm(parameters.UploadData);
                signedIn = form.GetValueOrDefault("user") == "demo" && form.GetValueOrDefault("password") == "demo";
                return signedIn
                    ? Redirect(parameters, "order.html")
                    : Html(parameters, Resource("login.html").Replace("<!--notice-->",
                        "<p class=\"notice\">Wrong user name or password.</p>"));
            }
            if (method == "POST" && path == "order")
            {
                IReadOnlyDictionary<string, string> form = ReadForm(parameters.UploadData);
                string rows = "";
                foreach (var pair in form)
                {
                    string label = OrderLabels.GetValueOrDefault(pair.Key, pair.Key);
                    rows += $"<tr><th>{WebUtility.HtmlEncode(label)}</th><td>{WebUtility.HtmlEncode(pair.Value)}</td></tr>";
                }
                return Html(parameters, Resource("confirmation.html").Replace("<!--rows-->", rows));
            }
            if (path == "order.html" && !signedIn)
            {
                return Html(parameters, Resource("login.html").Replace("<!--notice-->",
                    "<p class=\"notice\">Sign in to place orders.</p>"));
            }
            if (path == "spec-x42.pdf")
            {
                return Content(parameters, Encoding.ASCII.GetBytes("%PDF-1.4\n% demo spec sheet\n%%EOF\n"),
                               "application/pdf", "attachment; filename=\"spec-x42.pdf\"");
            }

            if (path.Length == 0)
            {
                path = "catalogue.html";
            }
            string? body = TryResource(path);
            if (body == null)
            {
                return Status(parameters, HttpStatusCode.NotFound);
            }
            return Content(parameters, Encoding.UTF8.GetBytes(body), MimeTypeOf(path));
        }

        private static InterceptRequestResponse Html(InterceptRequestParameters p, string html) =>
            Content(p, Encoding.UTF8.GetBytes(html), "text/html");

        private static InterceptRequestResponse Content(InterceptRequestParameters p, byte[] bytes,
                                                        string contentType, string? disposition = null)
        {
            UrlRequestJobOptions options = new()
            {
                HttpStatusCode = HttpStatusCode.OK,
                Headers = { new HttpHeader("Content-Type", contentType) }
            };
            if (disposition != null)
            {
                options.Headers.Add(new HttpHeader("Content-Disposition", disposition));
            }
            UrlRequestJob job = p.Network.CreateUrlRequestJob(p.UrlRequest, options);
            job.Write(bytes);
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }

        private static InterceptRequestResponse Redirect(InterceptRequestParameters p, string location)
        {
            UrlRequestJob job = p.Network.CreateUrlRequestJob(p.UrlRequest, new UrlRequestJobOptions
            {
                HttpStatusCode = HttpStatusCode.Found,
                Headers = { new HttpHeader("Location", Origin + location) }
            });
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }

        private static InterceptRequestResponse Status(InterceptRequestParameters p, HttpStatusCode code)
        {
            UrlRequestJob job = p.Network.CreateUrlRequestJob(p.UrlRequest,
                                                               new UrlRequestJobOptions { HttpStatusCode = code });
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }

        private static IReadOnlyDictionary<string, string> ReadForm(IUploadData? data)
        {
            Dictionary<string, string> form = new();
            switch (data)
            {
                case FormData formData:
                    foreach (var pair in formData.Data) form[pair.Key] = pair.Value;
                    break;
                case TextData text:
                    foreach (string part in text.Data.Split('&', StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] kv = part.Split('=', 2);
                        form[WebUtility.UrlDecode(kv[0])] = kv.Length > 1 ? WebUtility.UrlDecode(kv[1]) : "";
                    }
                    break;
            }
            return form;
        }

        private static string Resource(string name) => TryResource(name)
            ?? throw new FileNotFoundException($"Missing portal resource {name}");

        private static string? TryResource(string name)
        {
            using Stream? stream = Assembly.GetManifestResourceStream(ResourcePrefix + name);
            if (stream == null)
            {
                return null;
            }
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }

        private static string MimeTypeOf(string path) => Path.GetExtension(path) switch
        {
            ".html" => "text/html",
            ".css" => "text/css",
            ".js" => "text/javascript",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
}
