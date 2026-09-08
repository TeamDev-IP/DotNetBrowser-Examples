# DotNetBrowser-Examples
Examples of using [DotNetBrowser](https://teamdev.com/dotnetbrowser).

[![Nuget](https://img.shields.io/nuget/v/DotNetBrowser?color=%238f479b&style=for-the-badge)](https://www.nuget.org/packages/DotNetBrowser/) ![Downloads](https://img.shields.io/nuget/dt/DotNetBrowser?color=%238f479b&style=for-the-badge) [![Twitter Follow](https://img.shields.io/twitter/follow/DotNetBrowser?color=%238f479b&style=for-the-badge)](https://twitter.com/intent/follow?screen_name=DotNetBrowser)

DotNetBrowser is a .NET library which allows embedding a Chromium-based browser into .NET applications to load and display web pages built with HTML5, CSS3, JavaScript, etc. It provides UI controls for **WPF**, **WinForms**, **WinUI 3**, and **Avalonia UI** that you can embed into your desktop application, and an off-screen rendering mode for console, headless, and server-side scenarios. It runs on **Windows**, **macOS**, and **Linux**.

To learn more about the library please visit the [product page](https://teamdev.com/dotnetbrowser) or the [help center](https://teamdev.com/dotnetbrowser/docs).

### Requirements

* .NET 5 - 10, or .NET Framework 4.6.2 - 4.8.1 (Windows only)
* Windows, macOS, or Linux, on x64 or ARM64 (x86 is also supported on Windows)
* A DotNetBrowser [license key](https://teamdev.com/dotnetbrowser/docs/guides/installation/license.html). [Get the evaluation key](https://teamdev.com/dotnetbrowser#evaluate)

See the [system requirements](https://teamdev.com/dotnetbrowser/docs/guides/requirements/) for the full list of supported operating systems and .NET versions.

### How to run

1. Open the `dotnetbrowser.license` file in the root directory with any text editor, copy and paste your license key and save the changes. The file is copied to the output directory of every example by `csharp/Directory.Build.props` and `vbnet/Directory.Build.props`.

   A few examples set the key in code through `EngineOptions` instead and describe it in their own README: [Docker](csharp/docker), [Local AI](csharp/local-ai), [Excel add-in](csharp/excel), and [Unity3D](csharp/unity3d).

2. Each folder under `csharp/` and `vbnet/` has its own solution, such as `csharp/console/Console.sln`, `csharp/wpf/Wpf.sln`, or `csharp/avalonia/Avalonia.sln`. Open the one you need in Visual Studio 2022 or JetBrains Rider.

3. Most examples are SDK-style projects targeting .NET 6 or later, so you can also run them from the command line without an IDE:

   ```bash
   dotnet run --project csharp/console/GetHtml
   ```

   The console, Avalonia, Docker, and web UI examples build on macOS and Linux with the .NET SDK alone.

4. The WinForms, WPF, VSTO, and ActiveX examples are .NET Framework 4.6.2 projects that use `packages.config`. They need Visual Studio on Windows and an explicit restore: right-click the solution in "Solution Explorer" and select "Restore NuGet Packages", or run `nuget restore <solution>.sln`.

To build every solution for one language in a single pass, use the [`build.cake`](build.cake) script in the root directory with `--lang=csharp` or `--lang=vbnet`. It requires [Cake](https://cakebuild.net/) and MSBuild.

### List of examples

#### Basics

* Create two separate Chromium engines: [C#](csharp/console/SeparateEngines/Program.cs), [VB.NET](vbnet/console/SeparateEngines/Program.vb)
* Create two separate Chromium engines in different AppDomains: [C#](csharp/console/SeparateEngines.AppDomains/Program.cs), [VB.NET](vbnet/console/SeparateEngines.AppDomains/Program.vb)
* Create separate Chromium profiles: [C#](csharp/winforms/Profiles), [VB.NET](vbnet/winforms/Profiles)
* Load URL and listen to the navigation events: [C#](csharp/console/LoadEvents/Program.cs), [VB.NET](vbnet/console/LoadEvents/Program.vb)
* Embed into Windows Forms application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/tree/main/csharp/Embedding.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/tree/main/vbnet/Embedding.WinForms) 
* Embed into WPF application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/tree/main/csharp/Embedding.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/tree/main/vbnet/Embedding.Wpf)

#### Web page content

* Get page HTML: [C#](csharp/console/GetHtml/Program.cs), [VB.NET](vbnet/console/GetHtml/Program.vb)
* Walk through the hierarchy of frames on the web page: [C#](csharp/console/GetFrames/Program.cs), [VB.NET](vbnet/console/GetFrames/Program.vb)
* Get selected text on the web page: [C#](csharp/console/GetSelectedText/Program.cs), [VB.NET](vbnet/console/GetSelectedText/Program.vb)
* Find text on the web page: [C#](csharp/console/FindText/Program.cs), [VB.NET](vbnet/console/FindText/Program.vb)
* Text search (WinForms): [C#](csharp/winforms/FindText), [VB.NET](vbnet/winforms/FindText)
* Text search (WPF): [C#](csharp/wpf/FindText), [VB.NET](vbnet/wpf/FindText)
* Save web page: [C#](csharp/wpf/SaveWebPage), [VB.NET](vbnet/wpf/SaveWebPage)
* Save an image from the web page: [C#](csharp/console/SaveImageFromPage/Program.cs), [VB.NET](vbnet/console/SaveImageFromPage/Program.vb)
* Create web page screenshot: [C#](csharp/console/HtmlToImage/Program.cs), [VB.NET](vbnet/console/HtmlToImage/Program.vb)
* Create web page screenshot (cross-platform, uses SkiaSharp): [C#](csharp/console/HtmlToImage.SkiaSharp/Program.cs), [VB.NET](vbnet/console/HtmlToImage.SkiaSharp/Program.vb)
* Execute editor commands (Cut, Copy, Paste, Undo, Select All,<br/> Insert Text etc.): [C#](csharp/console/ExecuteCommand/Program.cs), [VB.NET](vbnet/console/ExecuteCommand/Program.vb)
* Access local storage of the web page: [C#](csharp/console/WebStorage), [VB.NET](vbnet/console/WebStorage/Program.vb)
* Update the zoom level on the web page: [C#](csharp/console/Zoom/Program.cs), [VB.NET](vbnet/console/Zoom/Program.vb)
* Print web page to PDF: [C#](csharp/console/Printing.WebPageToPdf/Program.cs), [VB.NET](vbnet/console/Printing.WebPageToPdf/Program.vb)
* Download a PDF file by its URL: [C#](csharp/console/DownloadPdf/Program.cs), [VB.NET](vbnet/console/DownloadPdf/Program.vb)

#### DOM 

* Create new DOM element: [C#](csharp/console/DomCreateElement/Program.cs), [VB.NET](vbnet/console/DomCreateElement/Program.vb)
* Create and dispatch any DOM event (including custom events): [C#](csharp/console/DomCreateEvent/Program.cs), [VB.NET](vbnet/console/DomCreateEvent/Program.vb)
* Fill in and submit a form: [C#](csharp/console/DomForm/Program.cs), [VB.NET](vbnet/console/DomForm/Program.vb)
* Fill in and submit a multipage form: [C#](csharp/winforms/SimulateUserInput), [VB.NET](vbnet/winforms/SimulateUserInput)
* Work with attributes of the specific DOM element: [C#](csharp/console/DomGetAttributes/Program.cs), [VB.NET](vbnet/console/DomGetAttributes/Program.vb)
* Get DOM elements by tag name: [C#](csharp/console/DomGetElements/Program.cs), [VB.NET](vbnet/console/DomGetElements/Program.vb)
* Find element by CSS Selector: [C#](csharp/console/DomQuerySelector/Program.cs), [VB.NET](vbnet/console/DomQuerySelector/Program.vb)
* Intercept DOM Drag & Drop events: [C#](csharp/winforms/Dom.DragAndDrop), [VB.NET](vbnet/winforms/Dom.DragAndDrop)
* Get DOM node at a specific point on the web page: [C#](csharp/console/Inspect/Program.cs), [VB.NET](vbnet/console/Inspect/Program.vb)
* Get DOM node by mouse location (WinForms): [C#](csharp/winforms/Inspect), [VB.NET](vbnet/winforms/Inspect)
* Get DOM node by mouse location (WPF): [C#](csharp/wpf/Inspect), [VB.NET](vbnet/wpf/Inspect)
* Evaluate an XPath expression. Work with the evaluation result.: [C#](csharp/console/XPath/Program.cs), [VB.NET](vbnet/console/XPath/Program.vb)
* Work with Shadow DOM: [C#](csharp/console/ShadowDom/Program.cs), [VB.NET](vbnet/console/ShadowDom/Program.vb)
* Modify DOM element CSS style. Change DOM element visibility: [C#](csharp/winforms/ElementVisibility), [VB.NET](vbnet/winforms/ElementVisibility)

#### JS-.NET bridge

* Execute any JavaScript on the web page. Get JavaScript return value.: [C#](csharp/console/JavaScript/Program.cs), [VB.NET](vbnet/console/JavaScript/Program.vb)
* Read a value from the loaded web page with JavaScript: [C#](csharp/console/ExecuteJavaScript/Program.cs), [VB.NET](vbnet/console/ExecuteJavaScript/Program.vb)
* Work with JavaScript objects, update their properties and <br/>invoke methods.: [C#](csharp/console/JavaScriptObjects/Program.cs), [VB.NET](vbnet/console/JavaScriptObjects/Program.vb)
* Work with JavaScript arrays: [C#](csharp/console/JavaScriptBridge.Arrays/Program.cs), [VB.NET](vbnet/console/JavaScriptBridge.Arrays/Program.vb)
* Work with JavaScript name converting: [C#](csharp/console/JavaScriptBridge.NameConverter/Program.cs), [VB.NET](vbnet/console/JavaScriptBridge.NameConverter/Program.vb)
* Work with JavaScript Promises: [C#](csharp/console/JavaScriptBridge.Promises/Program.cs), [VB.NET](vbnet/console/JavaScriptBridge.Promises/Program.vb)
* Inject a .NET object into JavaScript. Get its properties and invoke <br/>public methods from the JavaScript side.: [C#](csharp/console/JavaScriptBridge/Program.cs), [VB.NET](vbnet/console/JavaScriptBridge/Program.vb)
* Execute JavaScript on UI event. Call back from JavaScript to <br/>.NET UI (WinForms): [C#](csharp/winforms/JavaScriptBridge), [VB.NET](vbnet/winforms/JavaScriptBridge)
* Inject object for scripting (`window.external`): [C#](csharp/console/InjectObjectForScripting/Program.cs), [VB.NET](vbnet/console/InjectObjectForScripting/Program.vb)
* Observe web page content changes on .NET side <br/>using `MutationObserver`: [C#](csharp/winforms/ObservePageChanges), [VB.NET](vbnet/winforms/ObservePageChanges)
* Intercept Notification title and message: [C#](csharp/console/Notifications.InterceptData/Program.cs), [VB.NET](vbnet/console/Notifications.InterceptData/Program.vb)

#### Network

* Redirect a URL request to another website. Access the URL <br/>request headers.: [C#](csharp/console/NetworkHandlers/Program.cs), [VB.NET](vbnet/console/NetworkHandlers/Program.vb)
* Intercept and handle URL requests: [C#](csharp/console/CustomRequestHandling/Program.cs), [VB.NET](vbnet/console/CustomRequestHandling/Program.vb)
* Handle `mailto` or any other URI scheme to open external application: [C#](csharp/winforms/MailToHandling), [VB.NET](vbnet/winforms/MailToHandling)
* Access HTTP response data: [C#](csharp/console/AccessingHttpResponseData/Program.cs), [VB.NET](vbnet/console/AccessingHttpResponseData/Program.vb)
* Intercept the response data for AJAX requests: [C#](csharp/console/AjaxResponseIntercept/Program.cs), [VB.NET](vbnet/console/AjaxResponseIntercept/Program.vb) 
* Suppress AJAX requests: [C#](csharp/console/AjaxCallsFilter/Program.cs), [VB.NET](vbnet/console/AjaxCallsFilter/Program.vb) 
* Read and modify POST data : [C#](csharp/console/PostData/Program.cs), [VB.NET](vbnet/console/PostData/Program.vb)
* Intercept WebSocket data: [C#](csharp/console/WebSockets.InterceptData/Program.cs), [VB.NET](vbnet/console/WebSockets.InterceptData/Program.vb)
* Handle SSL certificate errors: [C#](csharp/console/CertificateError/Program.cs), [VB.NET](vbnet/console/CertificateError/Program.vb) 
* Accept or reject SSL certificates: [C#](csharp/console/CertificateVerifier/Program.cs), [VB.NET](vbnet/console/CertificateVerifier/Program.vb) 
* Filter out incoming and outgoing cookies: [C#](csharp/console/CookieFilter/Program.cs), [VB.NET](vbnet/console/CookieFilter/Program.vb) 
* Get all stored cookies by URL: [C#](csharp/console/Cookies/Program.cs), [VB.NET](vbnet/console/Cookies/Program.vb) 
* Share cookies between two IEngine instances with different user data directories (WinForms): [C#](csharp/winforms/CookiesSharing), [VB.NET](vbnet/winforms/CookiesSharing)

#### Media

* List available audio and video devices. Select default media devices for <br/>the web page: [C#](csharp/console/DefaultMediaStreamDevice/Program.cs), [VB.NET](vbnet/console/DefaultMediaStreamDevice/Program.vb)

#### UI
* Embed into Windows Forms ElementHost: [C#](csharp/winforms/ElementHostEmbedding), [VB.NET](vbnet/winforms/ElementHostEmbedding) 
* Customize context menu in WPF: [C#](csharp/wpf/ContextMenu), [VB.NET](vbnet/wpf/ContextMenu) 
* Customize context menu with WinForms: [C#](csharp/winforms/ContextMenu), [VB.NET](vbnet/winforms/ContextMenu) 
* Customize context menu with Avalonia: [C#](csharp/avalonia/ContextMenu)
* Spell checker context menu in WPF: [C#](csharp/wpf/ContextMenu.SpellCheck), [VB.NET](vbnet/wpf/ContextMenu.SpellCheck) 
* Spell checker context menu in WinForms: [C#](csharp/winforms/ContextMenu.SpellCheck), [VB.NET](vbnet/winforms/ContextMenu.SpellCheck)
* Spell checker context menu in Avalonia: [C#](csharp/avalonia/ContextMenu.SpellCheck)
* Customize popup windows in WPF: [C#](csharp/wpf/Popups), [VB.NET](vbnet/wpf/Popups) 
* Customize popup windows in WinForms: [C#](csharp/winforms/Popups), [VB.NET](vbnet/winforms/Popups) 
* Customize popup windows in Avalonia: [C#](csharp/avalonia/Popups)
* Configure custom keyboard shortcuts: [C#](csharp/winforms/CustomShortcuts), [VB.NET](vbnet/winforms/CustomShortcuts) 
* Simulate keyboard input in WPF: [C#](csharp/wpf/KeyboardEventSimulation), [VB.NET](vbnet/wpf/KeyboardEventSimulation)
* Simulate keyboard input in WinForms: [C#](csharp/winforms/KeyboardEventSimulation), [VB.NET](vbnet/winforms/KeyboardEventSimulation)
* Simulate keyboard input in Avalonia: [C#](csharp/avalonia/KeyboardEventSimulation)
* Work with Chromium DevTools: [C#](csharp/winforms/DevTools), [VB.NET](vbnet/winforms/DevTools)
* Install and manage Chrome extensions in WPF: [C#](csharp/wpf/Extensions)
* Install and manage Chrome extensions in Avalonia: [C#](csharp/avalonia/Extensions)
* Handle Full Screen mode: [C#](csharp/winforms/FullScreen), [VB.NET](vbnet/winforms/FullScreen)
* Zoom web page on `Ctrl+Scroll` : [C#](csharp/wpf/Zoom), [VB.NET](vbnet/wpf/Zoom)
* WPF Demo application with tabs: [C#](csharp/wpf/Demo)
* Windows Forms Demo application with tabs: [C#](csharp/winforms/Demo)
* Work with the web page via UI Automation (MSAA/UIA): [C#](csharp/wpf/UiAutomation), [VB.NET](vbnet/wpf/UiAutomation)
* Display web page with transparent background in <br/>a transparent WPF window: [C#](csharp/wpf/TransparentWebPage), [VB.NET](vbnet/wpf/TransparentWebPage)
* Display web page with transparent background in <br/>a transparent Avalonia window: [C#](csharp/avalonia/TransparentWebPage), [VB.NET](vbnet/avalonia/TransparentWebPage)
* Intercept Drag & Drop events. Access `IDataObject`: [C#](csharp/wpf/DragAndDrop), [VB.NET](vbnet/wpf/DragAndDrop)
* Use DotNetBrowser with WPF data binding (MVVM): [C#](csharp/wpf/Mvvm), [VB.NET](vbnet/wpf/Mvvm)
* Use DotNetBrowser with Avalonia data binding (MVVM): [C#](csharp/avalonia/Mvvm)
* Use Chromecast with DotNetBrowser in Avalonia: [C#](csharp/avalonia/Chromecast), [VB.NET](vbnet/avalonia/Chromecast)
* Use Chromecast with DotNetBrowser in WPF: [C#](csharp/wpf/Chromecast), [VB.NET](vbnet/wpf/Chromecast)

#### Specific use-cases

* Create a custom HTML UI: [C#](csharp/wpf/CreateHtmlUi), [VB.NET](vbnet/wpf/CreateHtmlUi)
* Create a custom web UI with TypeScript, React, and Shadcn UI: [C#](csharp/web-ui)
* Build a Local AI Assistant application: [C#](csharp/local-ai)
* WPF kiosk application: [C#](csharp/wpf/Kiosk), [VB.NET](vbnet/wpf/Kiosk)
* WinForms kiosk application: [C#](csharp/winforms/Kiosk), [VB.NET](vbnet/winforms/Kiosk)
* Avalonia kiosk application: [C#](csharp/avalonia/Kiosk)
* Integrate with Google Maps: [C#](csharp/winforms/GoogleMaps), [VB.NET](vbnet/winforms/GoogleMaps)
* Integrate with Google Street View: [C#](csharp/winforms/GoogleStreetView), [VB.NET](vbnet/winforms/GoogleStreetView)
* Integrate with Selenium Chrome Driver: [C#](csharp/devtools-protocol/SeleniumChromeDriver), [VB.NET](vbnet/devtools-protocol/SeleniumChromeDriver)
* Integrate with Playwright: [C#](csharp/devtools-protocol/Playwright), [VB.NET](vbnet/devtools-protocol/Playwright)
* Integrate with Puppeteer: [C#](csharp/devtools-protocol/Puppeteer), [VB.NET](vbnet/devtools-protocol/Puppeteer)
* VSTO Add-In for Microsoft Outlook: [C#](csharp/vsto/MyOutlookAddIn), [VB.NET](vbnet/vsto/MyOutlookAddIn)
* COM Add-In for Excel: [C#](csharp/excel)
* COM/ActiveX wrapper: [C#](csharp/activex/ComWrapper), [VB.NET](vbnet/activex/ComWrapper)
* Integrate with Unity3D: [C#](csharp/unity3d)
* Integrate with Docker: [C#](csharp/docker)

#### Examples for tutorials
* How to deploy Chromium binaries over network: [C#](csharp/wpf/ChromiumBinariesResolver), [VB.NET](vbnet/wpf/ChromiumBinariesResolver)

### Contact us
Feel free to [submit request](https://dotnetbrowser.support.teamdev.com/support/tickets/new) to our support team if you own a commercial license with an active support subscription. If you have any questions regarding using DotNetBrowser or the examples in this repository, [contact us via email](mailto:customer-care@teamdev.com?subject=[GitHub]%20Question%20on%20DotNetBrowser%20Examples).

---

The information in this repository is provided on the following terms: https://www.teamdev.com/terms-and-privacy
