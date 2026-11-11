# HTML rich text editor

This example shows how to add an HTML rich text editor to an Avalonia UI
desktop app with DotNetBrowser. The editor is
[Quill](https://quilljs.com/), a JavaScript editor, running in a
`BrowserView`. The app talks to it in both directions:

- A native toolbar formats the selected text through Quill's API.
- **Open** loads an HTML file into the editor, **Save** writes the edited
  document back as HTML, and **Export PDF** prints it to a PDF file.
- The editor tells the app when the user changes the text, and the status
  bar shows the word count.
- Chromium checks the spelling, and the context menu offers suggestions,
  **Add to dictionary**, and **Cut**, **Copy**, and **Paste**.

The editor page and Quill ship inside the app as embedded resources, so the
editor works without a network connection.

The tutorial is available on the
[DotNetBrowser blog](https://teamdev.com/dotnetbrowser/blog/html-rich-text-editor/).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
- An operating system from the DotNetBrowser
  [system requirements](https://teamdev.com/dotnetbrowser/docs/guides/requirements/).
  The example was tested on Windows 11.
- A DotNetBrowser
  [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license/).

## Project structure

- `RichTextEditor` is an Avalonia UI 12 app:
  - `MainWindow.axaml` and `MainWindow.axaml.cs` create the engine and the
    browser, set up the spell checker, and handle the toolbar, the files,
    and the PDF export.
  - `EditorScheme.cs` serves the files of the `Editor` folder from the
    app's resources at `app://editor/`.
  - `EditorHost.cs` is the .NET object that the editor page calls when the
    text changes.
  - `EditorContextMenu.cs` builds the context menu with the spelling
    suggestions.
  - `Editor` holds the editor page, its script and style, and Quill 2.0.3
    with its license. The page's Content Security Policy allows only these
    files, so an opened document can't load remote images, scripts, or
    frames.

## Set the license key

Put your DotNetBrowser license key into the `dotnetbrowser.license` file in
the root directory of this repository. The build copies it to the output
directory.

## Run the example

From this directory, in a terminal that isn't running as
administrator:

```
dotnet run --project RichTextEditor
```

Or open `RichTextEditor.sln` in your IDE and run `RichTextEditor`.

Right-click a word with a red underline to see the suggestions.

One copy of the app runs at a time. A second one fails with an unhandled
`UserDataInUseException`, because the first one uses the user data
directory.

On Windows, Chromium doesn't start in an app that runs with administrator
rights. If you need them, see
[Running as admin on Windows](https://teamdev.com/dotnetbrowser/docs/guides/troubleshoot/common-issues/#running-as-admin-on-windows).

## Spell checking without a network connection

On Windows and Linux, Chromium downloads the dictionary the first time the
app adds the language, and keeps it in the user data directory:
`%LOCALAPPDATA%\RichTextEditor` on Windows, `~/.local/share/RichTextEditor`
on Linux. Without a network connection on the first run, spell checking
stays off. On macOS, Chromium uses the system's dictionaries.

To ship the dictionary with the app:

1. Run the example once with a network connection.
2. Copy `Dictionaries/en-US-10-2.bdic` from the user data directory to
   `RichTextEditor/Dictionaries`, and add it to the project so the build
   copies it to the output directory:

   ```xml
   <ItemGroup>
     <None Include="Dictionaries\*.bdic" CopyToOutputDirectory="PreserveNewest" />
   </ItemGroup>
   ```

At startup, the app copies the dictionaries from its `Dictionaries` folder
to the user data directory. The file name contains the dictionary version,
so take the file from a run with the same DotNetBrowser version.
