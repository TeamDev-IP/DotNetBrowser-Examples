# DotNetBrowser-Examples
Examples of using [DotNetBrowser](https://www.teamdev.com/dotnetbrowser).

To learn about the library please visit the [product page](https://www.teamdev.com/dotnetbrowser) or the [help center](https://dotnetbrowser-support.teamdev.com/).

### How to run
1. Open the `dotnetbrowser.license` file in the root directory with any text editor, copy and paste your license key and save the changes.
2. Open the solution in Visual Studio 2019.
3. Right-click the solution in "Solution Explorer" and select "Restore NuGet Packages"
4. Build the solution and run any of the example projects.

### List of examples

#### Basics

| Example | Links  |
|:--------------------------- |:-----|
| Create two separate Chromium engines | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SeparateEngines), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SeparateEngines) |
| Create two separate Chromium engines in different AppDomains | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SeparateEngines.AppDomains), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SeparateEngines.AppDomains) |
| Load URL and listen to the navigation events | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/LoadEvents), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/LoadEvents) |
| Embed into .NET Core WPF applications | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/NETCore30.Wpf) |
| Embed into .NET Core WinForms applications | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/NETCore30.WinForms) |

#### Web page content

| Example | Links  |
|:--------------------------- |:-----|
| Get page HTML | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GetHtml), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GetHtml) |
| Walk through the hierarchy of frames on the web page | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GetFrames), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GetFrames) |
| Save web page | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SaveWebPage.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SaveWebPage.Wpf) |
| Save an image from the web page | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SaveImageFromPage), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SaveImageFromPage) |
| Create web page screenshot | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/HtmlToImage), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/HtmlToImage) |

#### DOM API

| Example | Links  |
|:--------------------------- |:-----|
| Create new DOM element | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomCreateElement), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomCreateElement) |
| Create and dispatch custom DOM event | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomCreateEvent), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomCreateEvent) |
| Intercept DOM Drag & Drop events | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Dom.DragAndDrop.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Dom.DragAndDrop.WinForms) |

#### JS-.NET bridge

| Example | Links  |
|:--------------------------- |:-----|
| Work with JavaScript arrays | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptBridge.Arrays), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptBridge.Arrays) |
| Work with JavaScript Promises | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptBridge.Promises), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptBridge.Promises)) |
| Inject object for scripting (`window.external`) | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/InjectObjectForScripting), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/InjectObjectForScripting) |

#### Network

| Example | Links  |
|:--------------------------- |:-----|
| Intercept and handle URL requests | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CustomRequestHandling), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CustomRequestHandling) |
| Handle `mailto` URI scheme to open external application | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/MailToHandling.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/MailToHandling.WinForms) |
| Access HTTP response data | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/AccessingHttpResponseData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/AccessingHttpResponseData)|
| Intercept the response data for AJAX requests | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/AjaxResponseIntercept), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/AjaxResponseIntercept) | 
| Suppress AJAX requests | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/AjaxCallsFilter), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/AjaxCallsFilter) | 
| Read and modify POST data |  [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/PostData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/PostData) |
| Intercept WebSocket data | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/WebSockets.InterceptData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/WebSockets.InterceptData) |
| Handle SSL certificate errors | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CertificateError), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CertificateError) | 
| Accept or reject SSL certificates | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CertificateVerifier), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CertificateVerifier) | 
| Filter out incoming and outgoing cookies | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CookieFilter), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CookieFilter) | 
| Get all stored cookies by URL | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Cookies), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Cookies) | 

#### Media
| Example | Links  |
|:--------------------------- |:-----|
| List available audio and video devices, select default media devices for the web page | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DefaultMediaStreamDevice), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DefaultMediaStreamDevice) |

#### UI

| Example | Links  |
|:--------------------------- |:-----|
| Customize context menu in WPF | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ContextMenu.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ContextMenu.Wpf) | 
| Customize context menu with WinForms | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ContextMenu.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/ContextMenu.WinForms) | 
| Configure custom keyboard shortcuts | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CustomShortcuts.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CustomShortcuts.WinForms) | 
| Simulate keyboard input in WPF | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/KeyboardEventSimulation.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/KeyboardEventSimulation.Wpf) |
| Simulate keyboard input in WinForms | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/KeyboardEventSimulation.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/KeyboardEventSimulation.WinForms) |
| Work with Chromium DevTools | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DevTools.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DevTools.WinForms) |
| Handle Full Screen mode | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/FullScreen.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/FullScreen.WinForms) |
| Zoom web page on `Ctrl+Scroll` |  [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Zoom.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Zoom.Wpf) |
| WPF Demo application with tabs | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Demo.Wpf) |
| Windows Forms Demo application with tabs | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Demo.WinForms) |
| Work with the web page via UI Automation (MSAA/UIA) | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/blob/master/csharp/UiAutomation.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/blob/master/vbnet/UiAutomation.Wpf) |

#### Specific use-cases

| Example | Links  |
|:--------------------------- |:-----|
| Create custom HTML UI | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CreateHtmlUi.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CreateHtmlUi.Wpf) |
| WPF kiosk application | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Kiosk.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Kiosk.Wpf) |
| WinForms kiosk application | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Kiosk.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Kiosk.WinForms) |
| Integrate with Google Maps | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GoogleMaps.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GoogleMaps.WinForms) |
| Integrate with Google Street View | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GoogleStreetView.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GoogleStreetView.WinForms) |
| Integrate with Selenium Chrome Driver | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SeleniumChromeDriver), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SeleniumChromeDriver) |
| VSTO Add-In for Microsoft Outlook | [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/MyOutlookAddIn), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/MyOutlookAddIn) |

### Contact us
Feel free to [submit request](https://dotnetbrowser.support.teamdev.com/support/tickets/new) to our support team if you own a commercial license with an active support subscription.

---

The information in this repository is provided on the following terms: https://www.teamdev.com/terms-and-privacy
