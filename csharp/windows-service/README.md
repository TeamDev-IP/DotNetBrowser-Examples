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
  .NET 8, change `TargetFramework` in `DnbReportService.csproj` to `net8.0-windows`.
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
  - `BitmapConverter.cs` converts a browser screenshot to
    a `System.Drawing.Bitmap`, which is saved as PNG.
  - `ReportServiceOptions.cs` reads the `ReportService` section of
    `appsettings.json`.
  - `FileLoggerProvider.cs` writes the log to a text file.
  - `report.html` is the local report page.

By default, the service keeps everything in the `Data` folder next to
the exe:

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
and the files appear in `DnbReportService\bin\Debug\net10.0-windows\win-x64\Data\Output`.
Press **Ctrl+C** to stop.

Since Chromium 138, a Chromium process that starts with administrator rights
drops them and restarts itself. DotNetBrowser does not support that restart,
so in a terminal or IDE running as administrator, the engine fails with
`Failed to establish RPC connection to the Chromium engine.`

If you need administrator rights, add
`ChromiumSwitches = { "--do-not-de-elevate" }` to the `EngineOptions.Builder`
in `BrowserEngineHost.cs`, as the
[troubleshooting guide](https://teamdev.com/dotnetbrowser/docs/guides/troubleshoot/common-issues/#running-as-admin-on-windows)
shows.

The service is a different case: in our tests, it ran under LocalSystem and
under an administrator account without this switch. Chromium restarts only
when UAC split the administrator's rights at sign-in into a full set and
a standard one. A service does not sign in that way, so Chromium starts
normally.

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

1. Copy the published files to the install folder. Only administrators can
   change files in `Program Files`, so no other user can replace the exe that
   the service runs:

   ```powershell
   $installDir = "$env:ProgramFiles\DnbReportService"
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
       -BinaryPathName "`"$installDir\DnbReportService.exe`"" `
       -StartupType Automatic
   sc.exe config DnbReportService obj= "NT SERVICE\DnbReportService"
   ```

   The path is in quotes because it contains a space; without them, Windows
   could try to start `C:\Program.exe`. `New-Service` creates the service
   under LocalSystem. The `sc.exe config` line switches it to the virtual
   account `NT SERVICE\DnbReportService`. `sc.exe` needs the space after
   `obj=`. Type `sc.exe`, not `sc`: in Windows PowerShell, `sc` means
   `Set-Content`.

4. Give the service account access to its folders: Modify on the data
   folder, Read & execute on the rest of the install folder:

   ```powershell
   $account = "NT SERVICE\DnbReportService"
   $dataDir = "$installDir\Data"
   New-Item -ItemType Directory -Force $dataDir | Out-Null
   icacls $dataDir /inheritance:r /grant:r "*S-1-5-32-544:(OI)(CI)F" `
       "*S-1-5-18:(OI)(CI)F" "${account}:(OI)(CI)M"
   icacls $installDir /grant "${account}:(OI)(CI)RX"
   ```

   The service runs the Chromium binaries from the data folder. Other users
   can't create or change files in `Program Files`, so they can't replace
   them. The first `icacls` line also replaces the permissions the folder
   inherits: only administrators (`*S-1-5-32-544`), SYSTEM (`*S-1-5-18`), and
   the service account keep access, so other users can't read the Chromium
   profile or the output. The SIDs work on every Windows language, where
   the group names differ.

5. Start the service:

   ```powershell
   Start-Service DnbReportService
   Get-Service DnbReportService
   ```

The first start takes a few seconds longer, because Chromium is extracted
into `Data\Chromium` in the install folder. Later starts reuse that folder.

### Choose the account

We recommend a low-privileged account:

- **A virtual account**, `NT SERVICE\DnbReportService`, as in the commands
  above. Windows creates it with the service. It needs no password and has
  its own profile.
- **LocalService.** Use `obj= "NT AUTHORITY\LocalService"` with `sc.exe`,
  and grant the folder permissions to `NT AUTHORITY\LOCAL SERVICE`.

Whichever account you choose, it needs Modify on the `Data` folder and
Read & execute on the install folder.

LocalSystem also works and needs no folder permissions, but the service then
runs with full rights on the machine.

## Find the logs and the output

The service writes its start, its stop, warnings, and errors to the
Application event log, source `DnbReportService`, and every line to
`DnbReportService.log` in the output folder. Only administrators can read
the output folder, so use the administrator window:

```powershell
Get-EventLog -LogName Application -Source DnbReportService -Newest 20
$output = "$env:ProgramFiles\DnbReportService\Data\Output"
Get-Content "$output\DnbReportService.log" -Tail 20
Get-ChildItem $output
```

The first lines of the log file after a start show the account, the session,
whether the process runs as a service, and the Chromium and user data folders.
Check them first when the service does not behave as in the console.

## Stop and delete the service

In Windows PowerShell as administrator:

```powershell
Stop-Service DnbReportService
sc.exe delete DnbReportService
Remove-EventLog -Source DnbReportService
```

To remove the files as well:

```powershell
Remove-Item -Recurse -Force "$env:ProgramFiles\DnbReportService"
```
