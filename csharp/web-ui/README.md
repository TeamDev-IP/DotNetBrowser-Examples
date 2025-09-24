# .NET desktop app with Shadcn UI

This folder contains a cross-platform desktop and web application combining a React + TypeScript frontend (using Vite) and a .NET Avalonia UI desktop app wrapping this React frontend as its UI.

## Prerequisites

- [Node.js](https://nodejs.org/) (v16+ recommended)
- [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)
- [.NET SDK](https://dotnet.microsoft.com/download) (v8.0+ recommended)

## Project Structure

- `reactexample/` — React + TypeScript web frontend.
- `ReactExample.Desktop/` — .NET Avalonia desktop application that utilizes the web frontend as its UI.
- `proto` — shared Protobuf declarations for communicating between JavaScript and .NET via gRPC.

## Build Instructions

### Setup the DotNetBrowser license key

You must have a license key to make everything work. [Get the evaluation key](https://teamdev.com/dotnetbrowser#evaluate)

You can edit the [dotnetbrowser.license](./../../dotnetbrowser.license) file with any text editor and put your license key there.

As an alternative, you can specify the license directly [in the `EngineService` code](./ReactExample.Desktop/EngineService.cs#L74):

```csharp
  EngineOptions engineOptions = new EngineOptions.Builder
  {
      RenderingMode = RenderingMode.HardwareAccelerated,
      LicenseKey = "your_license_key",
  }.Build();
```

More details on installing the license can be found in the [official documentation](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html#installing-license).

### Build Projects

Run the following command from the `ReactWebUi.sln` solution location:

```
dotnet build
```

This will build both frontend and desktop parts, restoring dependencies using `npm` if needed.

## Launch Desktop Application

You can launch the desktop application from the source code with one command.

```bash
dotnet run --project ReactExample.Desktop
```

This will build both frontend and desktop parts, restoring dependencies using `npm` if needed. After that, the desktop application will be launched.

## Packaging

- **Web:** After building, static files are in `reactexample/dist/`.
- **Desktop:** Use `dotnet publish -c Release` in `ReactExample.Desktop` for a distributable package.
