using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using DotNetBrowser.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;
using Microsoft.Extensions.Logging;

namespace ReactExample.Desktop;

public interface ISchemeHandler : IHandler<InterceptRequestParameters, InterceptRequestResponse>
{
    Scheme Scheme { get; }
}

public class ResourceRequestHandler : ISchemeHandler
{
    public const string Domain = "dnb://internal.host/";
    private const string PrefixTemplate = "{0}.web.";
    private readonly ILogger<ResourceRequestHandler> _logger;
    private readonly string prefix;

    public ResourceRequestHandler(ILogger<ResourceRequestHandler> logger)
    {
        _logger = logger;
        prefix = string.Format(PrefixTemplate, Assembly.GetExecutingAssembly().GetName().Name);
    }

    public InterceptRequestResponse Handle(InterceptRequestParameters parameters)
    {
        string url = parameters.UrlRequest.Url;
        if (!url.StartsWith(Domain)) return InterceptRequestResponse.Proceed();

        UrlRequestJob urlRequestJob;
        try
        {
            string resourcePath = ConvertToResourcePath(url);
            byte[]? content = FindResource(resourcePath);
            if (content != null)
            {
                MimeType mimeType = GetMimeType(resourcePath);
                urlRequestJob = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                    new UrlRequestJobOptions
                    {
                        HttpStatusCode = HttpStatusCode.OK,
                        Headers = new List<HttpHeader>
                        {
                            new HttpHeader("Content-Type",
                                mimeType.Value)
                        }
                    });
                urlRequestJob.Write(content);
            }
            else
            {
                _logger.LogWarning("Resource was not found.");
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
            _logger.LogError(e, "Exception occurred while handling request.");

            urlRequestJob = parameters.Network.CreateUrlRequestJob(parameters.UrlRequest,
                new UrlRequestJobOptions
                {
                    HttpStatusCode =
                        HttpStatusCode.InternalServerError
                });
        }

        return InterceptRequestResponse.Intercept(urlRequestJob);
    }

    public Scheme Scheme { get; } = Scheme.Create("dnb");

    private string ConvertToResourcePath(string url)
    {
        string path = url.Replace(Domain, string.Empty);
        if (string.IsNullOrWhiteSpace(path) || Equals(path, "/")) path = "index.html";

        string resourcePath = path.Replace("/", ".");
        resourcePath = prefix + resourcePath;
        _logger.LogInformation("URL: {Url}", url);
        _logger.LogInformation("Resource: {ResourcePath}", resourcePath);

        return resourcePath;
    }

    private byte[]? FindResource(string url)
    {
        try
        {
            using Stream? resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(url);
            if (resourceStream == null) return null;

            using MemoryStream ms = new MemoryStream();
            resourceStream.CopyTo(ms);
            return ms.ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception occurred while finding resource.");
            return null;
        }
    }

    private MimeType GetMimeType(string url)
    {
        string extension = Path.GetExtension(url);
        if (extension.StartsWith(".")) extension = extension.Substring(1);

        switch (extension.ToLower())
        {
            case "css":
                return MimeType.TextCss;
            case "htm":
            case "html":
                return MimeType.TextHtml;
            case "ico":
                return MimeType.Create("image/x-icon");
            case "js":
                return MimeType.TextJavascript;
            case "json":
                return MimeType.ApplicationJson;
            case "svg":
                return MimeType.Create("image/svg+xml");
            default:
                return MimeType.ApplicationOctetStream;
        }
    }
}