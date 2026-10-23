# Chromium in a Windows service

This example shows how to run DotNetBrowser in a Windows service. The service
renders HTML pages on a schedule, with no desktop and no signed-in user. Every
60 seconds it loads each page from `appsettings.json` into an off-screen
browser and saves:

- a PDF file, printed with the built-in PDF printer;
- a PNG screenshot;
- a JSON file with the page title and text read from the DOM.

There are two pages: `report.html`, a local report whose table, chart, and
summary are drawn by JavaScript, and `https://example.com/`. One Chromium
engine serves the whole service. It starts with the service and stops with
it; if Chromium crashes, the next job starts a new one.

The tutorial is available on the
[DotNetBrowser blog](https://teamdev.com/dotnetbrowser/blog/chromium-in-windows-service/).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0). For
  .NET 8, change `TargetFramework` in `DnbReportService.csproj` to `net8.0`.
- Windows 10 or later, or Windows Server 2016 or later, as listed in the
  DotNetBrowser
  [system requirements](https://teamdev.com/dotnetbrowser/docs/guides/requirements.html).
  The example was tested on Windows 11 and Windows Server 2025.
- A DotNetBrowser
  [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html).
- Administrator rights to install the service.

## Project structure

- `DnbReportService` is a .NET Worker Service:
  - `Program.cs` sets up the host: Windows service support, the app folder
    as the content root, and logging to the Event Log and to a text file.
  - `BrowserEngineHost.cs` creates one off-screen engine when the service
    starts, creates a new one if Chromium exits on its own, and disposes it
    when the service stops. It logs the account, the session, and the
    folders Chromium uses.
  - `ReportWorker.cs` runs the jobs: loads each page, waits until it is
    ready, and saves the PDF, PNG, and JSON files.
  - `PdfExport.cs` prints the page to a PDF file without a print preview.
  - `PngEncoder.cs` saves a browser screenshot as PNG. The service
    references neither WPF nor WinForms, so it encodes the PNG itself.
  - `ReportServiceOptions.cs` reads the `ReportService` section of
    `appsettings.json`.
  - `FileLoggerProvider.cs` writes the log to a text file.
  - `report.html` is the local report page.

By default, the service keeps everything under
`%ProgramData%\DnbReportService`:

| Folder      | Contents                                                |
|-------------|---------------------------------------------------------|
| `Chromium`  | The Chromium binaries, extracted on the first start     |
| `UserData`  | The Chromium profile                                    |
| `Output`    | The PDF, PNG, and JSON files and `DnbReportService.log` |

To change the folders, the interval, or the pages, edit `appsettings.json`.

The service keeps every file it writes: about 150 KB per cycle, or about
200 MB a day at the default interval. Raise `IntervalSeconds` or clean up
the `Output` folder if you leave the service running.

## Set the license key

Put your DotNetBrowser license key into the `dotnetbrowser.license` file in the
root directory of this repository. The build copies it to the output and
publish directories. A service runs under its own account, so a license set
only in your user environment will not reach it: keep the file next to the
exe.

## Run the example in the console

Try the example as a console application first. From this directory, in
a terminal that is not running as administrator:

```powershell
dotnet run --project DnbReportService
```

Or open `WindowsService.sln` in your IDE and run `DnbReportService`.

After the first cycle, the console shows `Cycle finished: 2 ok, 0 failed.`
and the files appear in `%ProgramData%\DnbReportService\Output`. Press
**Ctrl+C** to stop. Stop the console instance before you start the service:
the two would share one Chromium profile, and a profile can be used by one
engine at a time.

Since Chromium 138, a browser that starts with administrator rights drops
them and restarts itself. DotNetBrowser does not support that restart, so in
a terminal or IDE running as administrator, the engine fails with `Failed to
establish RPC connection to the Chromium engine.`

If you need administrator rights, add
`ChromiumSwitches = { "--do-not-de-elevate" }` to the `EngineOptions.Builder`
in `BrowserEngineHost.cs`, as the
[troubleshooting guide](https://teamdev.com/dotnetbrowser/docs/guides/troubleshoot/common-issues/#running-as-admin-on-windows)
shows. The service is not affected: in our tests, it ran under LocalSystem and
under an administrator account.

To write to other folders, override the settings on the command line:

```powershell
dotnet run --project DnbReportService -- `
    --ReportService:OutputDirectory=C:\Temp\DnbReportService\Output
```

## Publish the service

From this directory:

```powershell
dotnet publish DnbReportService -c Release -r win-x64 --self-contained `
    -o publish
```

The `publish` folder contains `DnbReportService.exe`,
`DotNetBrowser.Chromium.Win-x64.dll` with the Chromium binaries,
`appsettings.json`, `report.html`, and `dotnetbrowser.license`.

## Install and start the service

Run these commands in Windows PowerShell 5.1 as administrator, from this
directory, in one window: the steps share variables. PowerShell 7 has no
`*-EventLog` commands.

1. Copy the published files to the install folder:

   ```powershell
   $installDir = "C:\Services\DnbReportService"
   New-Item -ItemType Directory -Force $installDir | Out-Null
   Copy-Item -Recurse -Force publish\* $installDir
   ```

2. Register the Event Log source. The service account is not allowed to
   create it:

   ```powershell
   New-EventLog -LogName Application -Source DnbReportService
   ```

3. Create the service:

   ```powershell
   New-Service -Name DnbReportService -DisplayName DnbReportService `
       -BinaryPathName "$installDir\DnbReportService.exe" `
       -StartupType Automatic
   sc.exe config DnbReportService obj= "NT SERVICE\DnbReportService"
   ```

   `New-Service` creates the service under LocalSystem. The `sc.exe config`
   line switches it to the virtual account `NT SERVICE\DnbReportService`.
   `sc.exe` needs the space after `obj=`. Type `sc.exe`, not `sc`: in
   Windows PowerShell, `sc` means `Set-Content`.
   You can do both in one `sc.exe` command:

   ```powershell
   sc.exe create DnbReportService start= auto `
       binPath= "C:\Services\DnbReportService\DnbReportService.exe" `
       obj= "NT SERVICE\DnbReportService"
   ```

4. Give the service account access to its folders: Modify on the data
   folder, Read & execute on the install folder:

   ```powershell
   $account = "NT SERVICE\DnbReportService"
   $dataDir = "$env:ProgramData\DnbReportService"
   New-Item -ItemType Directory -Force $dataDir | Out-Null
   icacls $dataDir /grant "${account}:(OI)(CI)M"
   icacls $installDir /grant "${account}:(OI)(CI)RX"
   ```

5. Start the service:

   ```powershell
   Start-Service DnbReportService
   Get-Service DnbReportService
   ```

The first start takes a few seconds longer, because Chromium is extracted
into `%ProgramData%\DnbReportService\Chromium`. Later starts, under any
account, reuse that folder.

### Choose the account

We recommend a low-privileged account:

- **A virtual account**, `NT SERVICE\DnbReportService`, as in the commands
  above. Windows creates it with the service. It needs no password and has
  its own profile.
- **LocalService.** Use `obj= "NT AUTHORITY\LocalService"` with `sc.exe`,
  and grant the folder permissions to `NT AUTHORITY\LOCAL SERVICE`.

Whichever account you choose, it needs Modify on
`%ProgramData%\DnbReportService` and Read & execute on the install folder.

LocalSystem also works and needs no folder permissions, but the service then
runs with full rights on the machine.

## Find the logs and the output

The service logs to the Application event log, source `DnbReportService`, and
to `DnbReportService.log` in the output folder:

```powershell
Get-EventLog -LogName Application -Source DnbReportService -Newest 20
Get-Content `
    "$env:ProgramData\DnbReportService\Output\DnbReportService.log" -Tail 20
Get-ChildItem "$env:ProgramData\DnbReportService\Output"
```

The first lines after a start show the account, the session, whether the
process runs as a service, and the Chromium and user data folders. Check them
first when the service does not behave as in the console.

## Stop and delete the service

In Windows PowerShell as administrator:

```powershell
Stop-Service DnbReportService
sc.exe delete DnbReportService
Remove-EventLog -Source DnbReportService
```

To remove the files as well:

```powershell
Remove-Item -Recurse -Force "C:\Services\DnbReportService"
Remove-Item -Recurse -Force "$env:ProgramData\DnbReportService"
```
