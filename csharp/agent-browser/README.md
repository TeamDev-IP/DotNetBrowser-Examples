# Agent browser

This example shows how to give an AI assistant in a WPF application a browser
that cannot leave the limits the application sets. The assistant, Claude,
drives a visible `BrowserView` while the user watches. The application enforces
the limits through DotNetBrowser handlers, whatever the model or the page tries:

- a site allowlist with read-only and read-write sites, covering redirects,
  frames, and every request;
- no downloads, pop-ups, printing, file dialogs, external apps, or permissions;
- human input suppressed while the assistant acts, with **Take over** and
  **Hand back** buttons;
- no typing into password fields: the assistant hands the page to the user to
  sign in;
- a dialog that shows the exact form data before anything is submitted;
- an audit log of navigations, blocked actions, tool calls, and token usage;
- an incognito profile, so nothing is inherited or kept.

The demo runs against a mock supplier portal served from the application's
resources, so the scenario is the same on every machine.

The tutorial is available on the
[DotNetBrowser blog](https://teamdev.com/dotnetbrowser/blog/guard-railed-browser-for-ai-assistant/).

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10 or later
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)
- An [Anthropic API key](https://console.anthropic.com/)

## Project structure

- `GuardedBrowser` is a class library with no UI and no model code:
  - `BrowserPolicy.cs` holds the allowed sites and schemes.
  - `PolicyEnforcer.cs` sets the navigation, network, download, pop-up,
    permission, print, and dialog handlers.
  - `InputGate.cs` decides whether the user or the assistant holds the
    keyboard and mouse.
  - `SubmitGuard.cs` cancels form submissions the user has not approved.
  - `PageSnapshot.cs` and `snapshot.js` describe the page as a numbered list
    of interactive elements.
  - `BrowserActions.cs` performs the assistant's actions with simulated input.
  - `AuditLog.cs` records what happens.
- `AssistantAgent` is a class library that connects Claude through the
  [Anthropic C# SDK](https://github.com/anthropics/anthropic-sdk-csharp):
  - `BrowserTools.cs` defines the tools the model can call.
  - `AssistantSession.cs` runs the tool loop, pauses while the user holds the
    page, and asks for approval before a form is submitted.
  - `SystemPrompt.md` tells the model about the limits.
- `AgentBrowser.Wpf` is the application:
  - `MainWindow.xaml` shows the browser, the task box, the buttons, and the
    audit log.
  - `ApprovalDialog.xaml` shows what the assistant wants to submit.
  - `Portal/` contains the mock supplier portal and the scheme handler that
    serves it at `dnb://portal/`.

## Set the license key

Put your DotNetBrowser license key into the `dotnetbrowser.license` file in the
root directory of this repository. The build copies it to the output directory
of the application.

## Set the Anthropic API key

The Anthropic SDK reads the key from the `ANTHROPIC_API_KEY` environment
variable. Set it in the terminal you run the example from, or in the run
configuration of your IDE:

```powershell
$env:ANTHROPIC_API_KEY = "<your_api_key>"
```

## Run the example

From this directory:

```bash
dotnet run --project AgentBrowser.Wpf
```

Or open `AgentBrowser.sln` in your IDE and run `AgentBrowser.Wpf`.

Click **Run** to start the task shown in the task box. When the assistant
reaches the sign-in page, it hands the page over: sign in with user name
`demo` and password `demo`, then click **Hand back**. Before the order is
placed, the approval dialog shows the form data; click **Submit** to send it.
