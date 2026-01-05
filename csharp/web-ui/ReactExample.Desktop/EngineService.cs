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
    private readonly Lazy<IEngine> engineLazy;
    private readonly ILogger<EngineService> logger;
    private readonly ISchemeHandler schemeHandler;

    public EngineService(ILogger<EngineService> logger, ISchemeHandler schemeHandler)
    {
        this.logger = logger;
        this.schemeHandler = schemeHandler;
        engineLazy = new Lazy<IEngine>(InitializeEngine);
        logger.LogInformation("EngineService created");
    }

    public IBrowser CreateBrowser() => engineLazy.Value.CreateBrowser();

    public void Dispose()
    {
        logger.LogInformation("Disposing EngineService");
        if (engineLazy.IsValueCreated)
        {
            engineLazy.Value.Dispose();
        }
    }

    public ValueTask DisposeAsync()
    {
        logger.LogInformation("Disposing EngineService asynchronously");
        Dispose();
        return ValueTask.CompletedTask;
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
}