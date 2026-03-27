# Excel COM Add-in with DotNetBrowser

This example shows how to build a .NET Framework COM add-in that embeds
DotNetBrowser in an Excel task pane and lets a web page read and write
spreadsheet data through a JavaScript bridge.

## Prerequisites

- Windows
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)
- Microsoft Excel
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)

## Project structure

- `AddIn/Connect.cs` is the COM entry point Excel loads at startup. It implements the add-in lifecycle, supplies the ribbon XML, and creates the task pane.
- `AddIn/ComRegistration.cs` writes and removes the registry keys that register the add-in with Excel.
- `Interop/ComConstants.cs` holds the ProgId and GUID that identify the COM class.
- `Ribbon/RibbonController.cs` owns the ribbon XML and handles the "Open Panel" button callback.
- `Browser/EngineManager.cs` creates the DotNetBrowser engine and registers the `app://` scheme used to serve the bundled web page.
- `TaskPane/BrowserHostControl.cs` is the COM-visible WinForms control that hosts the browser inside the task pane. It exposes `readFromExcel` and `writeToExcel` to JavaScript as `window.excelBridge`.
- `TaskPane/TaskPaneManager.cs` coordinates the task pane and engine lifecycle and implements the Excel cell read/write logic.
- `Web/index.html` is the web UI bundled with the add-in.

## Set the license key

Open `Browser/EngineManager.cs` and set your license key:

```csharp
var builder = new EngineOptions.Builder
{
    LicenseKey = "<your_license_key>",
    ...
};
```

## Build and register

Build the project with MSBuild:

```bash
msbuild src/ExcelComAddin/ExcelComAddin.csproj /p:Configuration=Debug
```

Or open `ExcelComAddin.sln` in Visual Studio and build from there.

Then register the add-in using the provided script (requires an elevated PowerShell prompt):

```powershell
.\scripts\com-registration.ps1 -Action register
```

To unregister:

```powershell
.\scripts\com-registration.ps1 -Action unregister
```

To check the current registration status without making any changes:

```powershell
.\scripts\com-registration.ps1 -Action status
```

If you use Visual Studio, you can alternatively enable **Register for COM Interop**
in **Project Properties → Build** for the Debug configuration and the registration
will run automatically on each build.

## Run the example

After a successful build, open Excel. A **Sales Lead Add-in** tab appears in the
ribbon. Click **Open Panel** to open the task pane with the browser UI loaded.

- Click **Read from Excel** to display the current value of cell A1.
- Type a value and click **Write to Excel** to write it directly to the cell.
