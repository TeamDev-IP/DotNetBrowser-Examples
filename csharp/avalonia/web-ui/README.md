# ReactExample

A cross-platform desktop and web application combining a React + TypeScript frontend (using Vite) and a .NET Avalonia UI desktop app wrapping this React frontend as its UI.

## Prerequisites

- [Node.js](https://nodejs.org/) (v16+ recommended)
- [npm](https://www.npmjs.com/) or [yarn](https://yarnpkg.com/)
- [.NET SDK](https://dotnet.microsoft.com/download) (v8.0+ recommended)

## Project Structure

- `reactexample/` — React + TypeScript web frontend.
- `ReactExample.Desktop/` — .NET Avalonia desktop application that utilizes the web frontend as its UI.
- `proto` — shared Protobuf declarations for communicating between JavaScript and .NET via gRPC.

## Build Instructions

### Setting up DotNetBrowser license key

TBD
/ReactExample.Desktop/EngineService.cs, line 36

### Build Projects

Run the following command from the repository root:

```
dotnet build
```

This will build both frontend and desktop parts, restoring dependencies using `npm` if needed.

## Launch Desktop Application

```bash
dotnet run --project ReactExample.Desktop
```

## Packaging

- **Web:** After building, static files are in `reactexample/dist/`.
- **Desktop:** Use `dotnet publish -c Release` in `ReactExample.Desktop` for a distributable package.
