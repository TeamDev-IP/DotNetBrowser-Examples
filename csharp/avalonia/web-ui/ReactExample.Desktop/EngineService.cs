using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Microsoft.Extensions.Logging;

namespace ReactExample.Desktop;

public interface IEngineService : IDisposable, IAsyncDisposable
{
    IBrowser CreateBrowser();
}

public class EngineService : IEngineService
{
    private readonly ILogger<EngineService> logger;
    private readonly ISchemeHandler schemeHandler;

    private readonly Lazy<IEngine> engineLazy;

    public EngineService(ILogger<EngineService> logger, ISchemeHandler schemeHandler)
    {
        this.logger = logger;
        this.schemeHandler = schemeHandler;
        this.engineLazy = new Lazy<IEngine>(InitializeEngine);
        logger.LogInformation("EngineService created");
    }

    private IEngine InitializeEngine()
    {
        // Create and initialize the IEngine instance.
        EngineOptions engineOptions = new EngineOptions.Builder
        {
            RenderingMode = RenderingMode.HardwareAccelerated,
            LicenseKey = "",
            Schemes =
            {
                { schemeHandler.Scheme, schemeHandler }
            }
        }.Build(); 
        return EngineFactory.Create(engineOptions);
    }

    public void Dispose()
    {
        logger.LogInformation("Disposing EngineService");
        if(engineLazy.IsValueCreated)
        {
            engineLazy.Value.Dispose();
        }
    }

    public IBrowser CreateBrowser()
    {
        return engineLazy.Value.CreateBrowser();
    }

    public ValueTask DisposeAsync()
    {
        logger.LogInformation("Disposing EngineService asynchronously");
        Dispose();
        return ValueTask.CompletedTask;
    }
}