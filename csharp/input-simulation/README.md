# Input simulation

This example shows how to simulate keyboard and mouse input in a web page from
C# code. It contains two WPF applications that do the same thing, one with
DotNetBrowser and one with CefSharp, so that you can compare the code. Each
application loads a bucket list page, types a task into the text field, and
clicks the **Add** button.

The tutorial is available on the
[DotNetBrowser blog](https://teamdev.com/dotnetbrowser/blog/simulate-mouse-and-keyboard-input/).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)

## Project structure

- `shared/bucket-list.html` is the page both applications load. Its **Add**
  button is enabled only by `input` events.
- `DotNetBrowserApp/MainWindow.xaml.cs` raises keyboard events on
  `IBrowser.Keyboard` and clicks the button with `IMouse.SimulateClick()`.
- `CefSharpApp/MainWindow.xaml.cs` sends key and mouse events through
  `IBrowserHost` and gets the button position with JavaScript.

## Set the license key

Put your license key into the `dotnetbrowser.license` file in the root
directory of this repository. The build copies it to the output directory of
`DotNetBrowserApp`.

## Run the example

From this directory:

```bash
dotnet run --project DotNetBrowserApp
dotnet run --project CefSharpApp
```

Or open `InputSimulation.sln` in your IDE and run either project.

In the window, click **Type task**, then **Click Add**. The task appears in
the list.
