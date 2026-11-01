# Price and stock monitor

This example shows how to watch product pages for price and stock changes
with DotNetBrowser. A .NET Worker Service loads each page from
`appsettings.json` into an off-screen browser on a schedule, reads the price
and the availability, and compares them with the last values it saw. When
something changes, it posts an alert to a Discord channel through a webhook.

The price can come from two places, and the example shows both:

- **The rendered page.** The monitor waits until the page's script has
  filled in the price, then reads the price element and the state of the
  "Add to cart" button.
- **The JSON response the page loads.** The monitor listens to the
  network and captures the body of the request that returns the product
  data.

The example comes with a demo sneaker shop, served from the app's
resources at `https://shop.example/`. Its prices and stock change every 90
seconds, so you see alerts within minutes. Like many real stores, its
product pages arrive without a price: a script requests the product data
as JSON and fills it in.

The same code runs in a console, as a Windows service, and as a systemd
service on Linux.

The tutorial is available on the
[DotNetBrowser blog](https://teamdev.com/dotnetbrowser/blog/monitor-product-prices/).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
- An operating system from the DotNetBrowser
  [system requirements](https://teamdev.com/dotnetbrowser/docs/guides/requirements.html).
  The example was tested on Windows 11 and Ubuntu 24.04.
- A DotNetBrowser
  [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html).
- Optionally, a Discord server where you can manage webhooks. Without
  a webhook, the monitor writes the alerts only to the log.
- To install the service: administrator rights on Windows, or `sudo` on
  Linux.

## Project structure

- `PriceMonitor` is a .NET Worker Service:
  - `Program.cs` sets up the host: Windows service and systemd support, and
    the places the webhook URL can come from.
  - `BrowserEngineHost.cs` creates one off-screen engine when the monitor
    starts, creates a new one if Chromium exits on its own, and disposes it
    when the monitor stops.
  - `MonitorWorker.cs` checks every product on a schedule, retries failed
    pages, and decides what changed.
  - `ProductReader.cs` loads a product page and reads the price and the
    availability, from the rendered page or from the JSON response.
  - `PriceHistory.cs` appends every saved reading to `history.jsonl` and remembers
    the last values, so a restart doesn't report the same change twice.
  - `ProductSnapshot.cs` holds the price and the availability read at one
    moment.
  - `DiscordNotifier.cs` posts the alerts to the Discord webhook.
  - `MonitorOptions.cs` reads the `Monitor` section of `appsettings.json`.
  - `MockShop` is the demo shop: the request handler and the page, script,
    style, and pictures it serves.

The monitor keeps everything it writes in the `Data` folder next to the
executable: the Chromium binaries, the Chromium profile, crash dumps, and
`history.jsonl`. To change the folder, the interval, or the products, edit
`appsettings.json`.

## Set the license key

Put your DotNetBrowser license key into the `dotnetbrowser.license` file in
the root directory of this repository. The build copies it to the output
and publish directories. A service runs under its own account, so a license
set only in your user environment won't reach it: keep the file next to
the executable.

## Create a Discord webhook

1. In Discord, open **Server Settings** > **Integrations** > **Webhooks**.
2. Click **New Webhook**, choose the channel for the alerts, and click
   **Copy Webhook URL**.

Keep the URL secret: anyone who has it can post to the channel. Don't put
it into `appsettings.json` or the code. The monitor reads it from the
`Discord:WebhookUrl` setting, which can come from user secrets, an
environment variable named `Discord__WebhookUrl`, or a `secrets.json` file
in the `Data` folder. The sections below use each of them.

## Run the example in the console

From this directory, in a terminal that isn't running as administrator:

```
cd PriceMonitor
dotnet user-secrets set "Discord:WebhookUrl" "<your webhook URL>"
dotnet run
```

Or open `PriceMonitor.sln` in your IDE and run `PriceMonitor`.

The first check logs the current price of each product. After that, the
monitor checks the shop every 60 seconds and posts an alert to Discord when
a product sells out, comes back in stock, or changes its price. Press
**Ctrl+C** to stop.

On Windows, a Chromium process that starts with administrator rights drops
them and restarts itself. DotNetBrowser doesn't support that restart, so in
a terminal or IDE running as administrator, the engine fails with
`Failed to establish RPC connection to the Chromium engine.` If you need
administrator rights, add
`ChromiumSwitches = { "--do-not-de-elevate" }` to the `EngineOptions.Builder`
in `BrowserEngineHost.cs`, as the
[troubleshooting guide](https://teamdev.com/dotnetbrowser/docs/guides/troubleshoot/common-issues/#running-as-admin-on-windows)
shows.

## Watch a real product

1. Set `Monitor:MockShop:Enabled` to `false` in `appsettings.json`.
2. Replace the entries in `Monitor:Products` with your pages:
   - For `"Source": "Dom"`, set `PriceSelector` to a CSS selector of the
     price element and `CartButtonSelector` to the "Add to cart" button.
     The product counts as sold out when the button is disabled. If no
     element matches, the check fails and is retried.
   - For `"Source": "Json"`, set `ApiUrl` to the address of the request
     that returns the product data; its query part is ignored. To find it,
     open the page in a browser, open the developer tools, and look at the
     **Fetch/XHR** requests on the **Network** tab.
     `ProductReader.ReadFromJsonAsync()` reads the `price` and `inStock`
     fields; change it to match the shop's JSON. If the **Size** column shows
     `(ServiceWorker)` for that request, the store's service worker answers
     it from its own cache, and the response may not reach the monitor; use
     `"Source": "Dom"` for that store.
3. Raise `IntervalSeconds`. Checking a real shop every few minutes is
   plenty.

Check the site's terms of use and `robots.txt` first, and use the shop's
official API or feed if it has one. Many shops block automated browsers;
this example doesn't try to get around that.

## Run the monitor as a Windows service

Run these commands in Windows PowerShell 5.1 as administrator, from this
directory, in one window: the steps share variables. PowerShell 7 has no
`*-EventLog` commands. The
[windows-service](../windows-service) example explains each step and the
choice of the account in detail.

1. Publish the monitor and copy it to the install folder. Only
   administrators can change files in `Program Files`:

   ```powershell
   dotnet publish PriceMonitor -c Release -r win-x64 --self-contained `
       -o publish
   $installDir = "$env:ProgramFiles\PriceMonitor"
   New-Item -ItemType Directory -Force $installDir | Out-Null
   Copy-Item -Recurse -Force publish\* $installDir
   ```

2. Register the Event Log source and create the service under the virtual
   account `NT SERVICE\PriceMonitor`:

   ```powershell
   New-EventLog -LogName Application -Source PriceMonitor
   New-Service -Name PriceMonitor -DisplayName PriceMonitor `
       -BinaryPathName "`"$installDir\PriceMonitor.exe`"" `
       -StartupType Automatic
   sc.exe config PriceMonitor obj= "NT SERVICE\PriceMonitor"
   ```

   `New-Service` creates the service under LocalSystem, with full rights on
   the machine; the `sc.exe config` line switches it to the virtual account.
   Type `sc.exe`, not `sc`: in Windows PowerShell, `sc` means `Set-Content`.

3. Create the data folder. Only administrators, SYSTEM, and the service
   account get access to it, so other users can't read the webhook URL or
   replace the Chromium binaries that the service runs:

   ```powershell
   $account = "NT SERVICE\PriceMonitor"
   $dataDir = "$installDir\Data"
   New-Item -ItemType Directory -Force $dataDir | Out-Null
   icacls $dataDir /inheritance:r /grant:r "*S-1-5-32-544:(OI)(CI)F" `
       "*S-1-5-18:(OI)(CI)F" "${account}:(OI)(CI)M"
   icacls $installDir /grant "${account}:(OI)(CI)RX"
   ```

4. Save the webhook URL to `secrets.json` in the data folder:

   ```powershell
   $secrets = @{ Discord = @{ WebhookUrl = "<your webhook URL>" } }
   $secrets | ConvertTo-Json | Set-Content "$dataDir\secrets.json"
   ```

5. Start the service and read its log:

   ```powershell
   Start-Service PriceMonitor
   Get-EventLog -LogName Application -Source PriceMonitor -Newest 20
   ```

   The first start takes a few seconds longer, because Chromium is
   extracted into `Data\Chromium`. The Event Log gets the start, the stop,
   the alerts, warnings, and errors; the console shows every check.

To remove the service:

```powershell
Stop-Service PriceMonitor
sc.exe delete PriceMonitor
Remove-EventLog -Source PriceMonitor
Remove-Item -Recurse -Force "$env:ProgramFiles\PriceMonitor"
```

## Run the monitor as a systemd service on Linux

DotNetBrowser on Linux needs an X server, even in off-screen mode. On
a server without a display, the monitor uses Xvfb, a virtual X server, as
the
[headless Linux guide](https://teamdev.com/dotnetbrowser/docs/guides/headless-linux.html)
describes. Here, Xvfb runs as its own service, so it keeps running while the
monitor shuts down. Its display socket lives in a private `/tmp` that only
the two services share, so other users on the server can't connect to it.

The commands below are for Ubuntu 24.04.

1. Install the .NET runtime, the libraries Chromium needs, and Xvfb:

   ```bash
   sudo apt-get update
   sudo apt-get install -y dotnet-runtime-10.0 libgtk-3-0t64 libgbm1 libnss3 \
       libxss1 libasound2t64 libxtst6 xvfb
   ```

2. Publish the monitor on a machine with the .NET SDK, from this directory:

   ```bash
   dotnet publish PriceMonitor -c Release -r linux-x64 --self-contained false \
       -o publish
   ```

   Copy the `publish` folder to your home folder on the server, not to
   `/tmp`, where other users could change the files before you install them.
   Then copy it to `/opt/price-monitor`. The files belong to root, so the
   service account can't change them:

   ```bash
   sudo mkdir -p /opt/price-monitor
   sudo cp -r publish/. /opt/price-monitor/
   ```

3. Create the service account. Don't run the monitor as root: Chromium
   doesn't start as root with its sandbox on, and the sandbox is what
   protects the server from the pages the monitor loads:

   ```bash
   sudo useradd --system --no-create-home --shell /usr/sbin/nologin price-monitor
   ```

4. Save the webhook URL to a file that only root can read. The first
   command creates the file with those permissions, so the URL is never in
   a file that others can read. systemd reads the file before it starts the
   monitor:

   ```bash
   sudo install -m 600 /dev/null /etc/price-monitor.env
   echo 'Discord__WebhookUrl=<your webhook URL>' | sudo tee /etc/price-monitor.env > /dev/null
   ```

5. Create `/etc/systemd/system/price-monitor-xvfb.service`:

   ```ini
   [Unit]
   Description=Virtual display for the price monitor

   [Service]
   User=price-monitor
   PrivateTmp=yes
   ExecStart=/usr/bin/Xvfb :99 -screen 0 1280x800x24 -nolisten tcp -nolisten local
   Restart=always
   ```

   `PrivateTmp` gives the service its own `/tmp`, where Xvfb creates its
   socket. `-nolisten local` turns off the second socket that Linux would
   otherwise make visible to every user.

6. Create `/etc/systemd/system/price-monitor.service`:

   ```ini
   [Unit]
   Description=Price monitor
   Requires=price-monitor-xvfb.service
   After=price-monitor-xvfb.service network-online.target
   Wants=network-online.target
   JoinsNamespaceOf=price-monitor-xvfb.service

   [Service]
   Type=notify
   User=price-monitor
   PrivateTmp=yes
   Environment=DISPLAY=:99
   Environment=Monitor__DataDirectory=/var/lib/price-monitor
   EnvironmentFile=/etc/price-monitor.env
   ExecStart=/usr/bin/dotnet /opt/price-monitor/PriceMonitor.dll
   StateDirectory=price-monitor
   StateDirectoryMode=0700
   Restart=on-failure
   RestartSec=10

   [Install]
   WantedBy=multi-user.target
   ```

   `JoinsNamespaceOf` and `PrivateTmp` put the monitor into the same private
   `/tmp` as Xvfb, so it finds the display socket. `StateDirectory` makes
   systemd create `/var/lib/price-monitor` for the service account, with
   access for that account only.

7. Start the service and read its log:

   ```bash
   sudo systemctl daemon-reload
   sudo systemctl enable --now price-monitor
   journalctl -u price-monitor -f
   ```

On Ubuntu 23.10 and later, AppArmor may stop Chromium from creating the
user namespaces its sandbox needs. If the engine fails to start, see
[Linux](https://teamdev.com/dotnetbrowser/docs/guides/gs/chromium.html#linux)
in the Chromium guide. Don't disable the sandbox: the monitor loads pages
from the internet.

A local user who starts their own X server on display `:99` before the
monitor does can still make Chromium connect to it: X clients on Linux try
an abstract socket first, and `PrivateTmp` doesn't cover those. Such
a server can't change the pages the monitor reads, but it can keep the
engine from starting or attack the X client libraries in Chromium's browser
process, which runs as `price-monitor`. Run the monitor on a server where
you trust the other local users, or in a container, which has its own
network namespace and so its own abstract sockets.

To remove the service:

```bash
sudo systemctl disable --now price-monitor
sudo systemctl stop price-monitor-xvfb
sudo rm /etc/systemd/system/price-monitor.service \
    /etc/systemd/system/price-monitor-xvfb.service
sudo rm -r /opt/price-monitor /var/lib/price-monitor /etc/price-monitor.env
sudo userdel price-monitor
```
