# MCP servers

This example shows how to let an AI agent operate the web page displayed by
DotNetBrowser. The application creates the engine with a remote debugging
port and loads a bundled page with an order form and a table. An MCP server
then attaches to the running engine over the Chrome DevTools Protocol, and the
agent works with that page.

The example covers two MCP servers:

- [Playwright MCP](https://github.com/microsoft/playwright-mcp)
  (`@playwright/mcp`)
- [Chrome DevTools MCP](https://github.com/ChromeDevTools/chrome-devtools-mcp)
  (`chrome-devtools-mcp`)

The tutorial with the details and the limitations is available at
[teamdev.com](https://teamdev.com/dotnetbrowser/docs/tutorials/automation/mcp-servers/).

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)
- [Node.js](https://nodejs.org/). Chrome DevTools MCP requires Node.js
  20.19, 22.12, or newer.
- An MCP client, such as Claude Code or VS Code

## Project structure

- `MainWindow.xaml.vb` creates the engine with `RemoteDebuggingPort = 9224`
  and loads `orders.html`.
- `orders.html` contains the "Recent orders" table and the "New order" form
  the agent works with.

## Set the license key

Open `MainWindow.xaml.vb` and set your license key:

```vb
Dim engineOptions As EngineOptions = New EngineOptions.Builder With {
        .LicenseKey = "<your_license_key>",
        ...
        }.Build()
```

## Run the example

From this directory:

```bash
dotnet run
```

The window shows the "Orders" page. Keep it open while the agent works.

## Connect an MCP server

Each MCP server connects to `http://127.0.0.1:9224`, the endpoint the
application opens.

### Claude Code

Playwright MCP:

```bash
claude mcp add playwright -- npx @playwright/mcp@latest --cdp-endpoint http://127.0.0.1:9224
```

Chrome DevTools MCP:

```bash
claude mcp add chrome-devtools -- npx chrome-devtools-mcp@latest --browser-url=http://127.0.0.1:9224
```

To share the configuration with the project instead, put it in `.mcp.json`
at the project root:

```json
{
  "mcpServers": {
    "playwright": {
      "command": "npx",
      "args": ["@playwright/mcp@latest", "--cdp-endpoint", "http://127.0.0.1:9224"]
    },
    "chrome-devtools": {
      "command": "npx",
      "args": ["chrome-devtools-mcp@latest", "--browser-url=http://127.0.0.1:9224"]
    }
  }
}
```

### VS Code

Put the configuration in `.vscode/mcp.json`. VS Code uses the `servers` key
and starts workspace servers only after you trust the workspace:

```json
{
  "servers": {
    "playwright": {
      "command": "npx",
      "args": ["@playwright/mcp@latest", "--cdp-endpoint", "http://127.0.0.1:9224"]
    },
    "chrome-devtools": {
      "command": "npx",
      "args": ["chrome-devtools-mcp@latest", "--browser-url=http://127.0.0.1:9224"]
    }
  }
}
```

## Try it

With the application running, ask the agent:

```text
The embedded browser already shows an Orders page. Read the "Recent orders"
table and tell me the total quantity ordered. Then add an order for Dana Lee,
dana@example.com, product Headset, quantity 4, and confirm that the new row
appears in the table.
```

The new row appears in the application window as the agent submits the form.

## Limitations

- The agent works with the page the application displays. DotNetBrowser does
  not support opening a new tab from either server, because browsers are
  created in other ways: the application creates them, and pages open
  pop-ups. A page can still open a pop-up with `window.open`: the
  `BrowserView` shows it in a separate window, and both servers list it as a
  new tab.
- Playwright MCP blocks navigation to `file://` URLs unless it is started with
  `--allow-unrestricted-file-access`. The page the application loads is
  available without it.
- The Playwright MCP `browser_tabs` tool with the `close` action closes the
  page the application displays and disposes its `IBrowser`; the window goes
  blank.
