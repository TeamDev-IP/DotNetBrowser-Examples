# Screen sharing

This example shows how to handle WebRTC screen sharing requests in a WPF
application. When the page calls `navigator.mediaDevices.getDisplayMedia()`,
the application shows its own source picker with the screens, application
windows, and browsers that the page can share. The picker lets the user share
audio, if the page asked for it, and hide the sharing notification. It can
also hand the request over to the standard Chromium dialog. The application
then shows the active session and stops it.

The page only shows the captured video. A real application would send the
stream to another peer over WebRTC, which is the same in any browser.

The [Media guide](https://teamdev.com/dotnetbrowser/docs/guides/gs/media/#screen-sharing)
describes the API.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows
- A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html)

## Project structure

- `ScreenSharing/share.html` is the page. Its **Share screen** button calls
  `getDisplayMedia()` and shows the stream in a `<video>` element.
- `ScreenSharing/MainWindow.xaml.cs` handles the request in
  `ICapture.StartSessionHandler`, listens to `ICapture.SessionStarted` and
  `ISession.Stopped`, and stops the session with `ISession.Stop()`. In
  DotNetBrowser 4.5.0, `Stopped` isn't raised when the user stops sharing in
  the Chromium notification, so the page also reports the end of its video
  track with a console message.
- `ScreenSharing/SourcePicker.xaml.cs` lists the sources from
  `StartSessionParameters.Sources` with their thumbnails and builds the
  `StartSessionResponse`.

## Set the license key

Put your license key into the `dotnetbrowser.license` file in the root
directory of this repository. The build copies it to the output directory.

## Run the example

From this directory:

```bash
dotnet run --project ScreenSharing
```

Or open `ScreenSharing.sln` in your IDE and run the project.

In the window, click **Share screen**, choose a source in the picker, and
click **Share**. The page shows the captured video. Click **Stop sharing** at
the bottom of the window to stop the session. The example shares one source at
a time.

To share audio, select **With audio** in the page before you click
**Share screen**. Otherwise, **Share audio** in the picker is unavailable,
because the page didn't ask for audio.
