# Local AI

This example shows how to build a small Avalonia desktop application that embeds
DotNetBrowser and runs a local browser-based AI feature with Transformers.js.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)

## Project structure

- `MainWindow.axaml.cs` initializes DotNetBrowser and loads the local app URL.
- `LocalAppSchemeHandler.cs` serves bundled files from the `web/` directory.
- `web/index.html` contains the browser UI.
- `web/app.js` loads the local model and handles the user actions.
- `web/styles.css` styles the sample UI.

## Set the license key

Open `MainWindow.axaml.cs` and set your license key:

```csharp
engine = EngineFactory.Create(new EngineOptions.Builder
{
    LicenseKey = "<your_license_key>",
    ...
}.Build());
```

## Run the example

From this directory:

```bash
dotnet run
```

Or open `LocalAiAssistant.sln` in your IDE and run the `LocalAiAssistant` project.

When the window opens, wait for the model to load. Then type some text into
the input field and click one of the actions. The app runs the model locally
and shows the generated result in the same window.
