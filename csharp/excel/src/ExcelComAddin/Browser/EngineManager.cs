using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;

namespace ExcelComAddin.Browser
{
    /// <summary>
    /// Creates and owns the DotNetBrowser <see cref="IEngine"/> instance. The engine is started
    /// lazily on the first call to <see cref="Start"/> and stopped via <see cref="Stop"/> or
    /// <see cref="Dispose"/>. A custom <c>app://</c> URL scheme is registered so the browser
    /// can load <c>index.html</c>, which is embedded directly in the assembly.
    /// </summary>
    public sealed class EngineManager : IDisposable
    {
        private const string AppScheme = "app";
        private const string IndexHtmlResource = "ExcelComAddin.Web.index.html";
        private readonly Func<IEngine> _engineFactory;
        private IEngine _engine;

        public EngineManager()
            : this(null)
        {
        }

        public EngineManager(Func<IEngine> engineFactory)
        {
            _engineFactory = engineFactory ?? CreateEngine;
        }

        /// <summary>
        /// Starts the Chromium engine if it is not already running and returns it.
        /// Subsequent calls return the same instance.
        /// </summary>
        public IEngine Start()
        {
            if (_engine == null) _engine = _engineFactory();
            return _engine;
        }

        /// <summary>Disposes the running engine and releases all Chromium resources.</summary>
        public void Stop()
        {
            if (_engine is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _engine = null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Stop();
        }

        private IEngine CreateEngine()
        {
            return EngineFactory.Create(BuildEngineOptions());
        }

        private EngineOptions BuildEngineOptions()
        {
            var builder = new EngineOptions.Builder
            {
                LicenseKey = "",
                RenderingMode = RenderingMode.HardwareAccelerated
            };

            builder.Schemes[Scheme.Create(AppScheme)] =
                new Handler<InterceptRequestParameters, InterceptRequestResponse>(HandleAppSchemeRequest);

            return builder.Build();
        }

        private static InterceptRequestResponse HandleAppSchemeRequest(InterceptRequestParameters parameters)
        {
            try
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(IndexHtmlResource))
                {
                    if (stream == null)
                        return CreateResponse(parameters, HttpStatusCode.NotFound, Encoding.UTF8.GetBytes("<h1>Not Found</h1>"));

                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return CreateResponse(parameters, HttpStatusCode.OK, ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                var body = "<h1>Error</h1><pre>" + WebUtility.HtmlEncode(ex.Message) + "</pre>";
                return CreateResponse(parameters, HttpStatusCode.InternalServerError, Encoding.UTF8.GetBytes(body));
            }
        }

        private static InterceptRequestResponse CreateResponse(
            InterceptRequestParameters parameters,
            HttpStatusCode statusCode,
            byte[] body)
        {
            var options = new UrlRequestJobOptions
            {
                HttpStatusCode = statusCode,
                Headers = new List<HttpHeader>
                {
                    new HttpHeader("Content-Type",  "text/html; charset=utf-8")
                }
            };

            var job = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest, options);
            job.Write(body ?? Array.Empty<byte>());
            job.Complete();
            return InterceptRequestResponse.Intercept(job);
        }
    }
}
