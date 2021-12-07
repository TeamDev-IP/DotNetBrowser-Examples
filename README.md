# DotNetBrowser-Examples
Examples of using [DotNetBrowser](https://www.teamdev.com/dotnetbrowser).

[![Nuget](https://img.shields.io/nuget/v/DotNetBrowser?color=%238f479b&style=for-the-badge)](https://www.nuget.org/packages/DotNetBrowser/) ![Downloads](https://img.shields.io/nuget/dt/DotNetBrowser?color=%238f479b&style=for-the-badge) [![Twitter Follow](https://img.shields.io/twitter/follow/DotNetBrowser?color=%238f479b&style=for-the-badge)](https://twitter.com/intent/follow?screen_name=DotNetBrowser)

DotNetBrowser is a .NET library which allows embedding a Chromium-based browser into .NET applications to load and display web pages built with HTML5, CSS3, JavaScript, etc. DotNetBrowser supports both WPF and WinForms and provides UI controls which you can embed into your desktop application to display web pages. 

To learn more about the library please visit the [product page](https://www.teamdev.com/dotnetbrowser) or the [help center](https://dotnetbrowser-support.teamdev.com/).

### How to run
1. Open the `dotnetbrowser.license` file in the root directory with any text editor, copy and paste your license key and save the changes. [Get the evaluation key](https://www.teamdev.com/dotnetbrowser#evaluate)
2. Open the solution in Visual Studio 2019.
3. Right-click the solution in "Solution Explorer" and select "Restore NuGet Packages"
4. Build the solution and run any of the example projects.

### List of examples

#### Basics

* Create two separate Chromium engines: [C#](csharp/SeparateEngines/Program.cs), [VB.NET](vbnet/SeparateEngines/Program.vb)
* Create two separate Chromium engines in different AppDomains: [C#](csharp/SeparateEngines.AppDomains/Program.cs), [VB.NET](vbnet/SeparateEngines.AppDomains/Program.vb)
* Create separate Chromium profiles: [C#](csharp/Profiles.WinForms), [VB.NET](vbnet/Profiles.WinForms)
* Load URL and listen to the navigation events: [C#](csharp/LoadEvents/Program.cs), [VB.NET](vbnet/LoadEvents/Program.vb)
* Embed into .NET Core WPF applications: [C#](csharp/NETCore30.Wpf)
* Embed into .NET Core WinForms applications: [C#](csharp/NETCore30.WinForms)

#### Web page content

* Get page HTML: [C#](csharp/GetHtml/Program.cs), [VB.NET](vbnet/GetHtml/Program.vb)
* Walk through the hierarchy of frames on the web page: [C#](csharp/GetFrames/Program.cs), [VB.NET](vbnet/GetFrames/Program.vb)
* Get selected text on the web page: [C#](csharp/GetSelectedText/Program.cs), [VB.NET](vbnet/GetSelectedText/Program.vb)
* Find text on the web page: [C#](csharp/FindText/Program.cs), [VB.NET](vbnet/FindText/Program.vb)
* Text search (WPF): [C#](csharp/FindText.Wpf), [VB.NET](vbnet/FindText.Wpf)
* Save web page: [C#](csharp/SaveWebPage.Wpf), [VB.NET](vbnet/SaveWebPage.Wpf)
* Save an image from the web page: [C#](csharp/SaveImageFromPage/Program.cs), [VB.NET](vbnet/SaveImageFromPage/Program.vb)
* Create web page screenshot: [C#](csharp/HtmlToImage/Program.cs), [VB.NET](vbnet/HtmlToImage/Program.vb)
* Execute editor commands (Cut, Copy, Paste, Undo, Select All,<br/> Insert Text etc.): [C#](csharp/ExecuteCommand/Program.cs), [VB.NET](vbnet/ExecuteCommand/Program.vb)
* Access local storage of the web page: [C#](csharp/WebStorage), [VB.NET](vbnet/WebStorage/Program.vb)
* Update the zoom level on the web page: [C#](csharp/Zoom/Program.cs), [VB.NET](vbnet/Zoom/Program.vb)
* Print web page to PDF: [C#](csharp/Printing.WebPageToPdf/Program.cs), [VB.NET](vbnet/Printing.WebPageToPdf/Program.vb)
* Download a PDF file by its URL: [C#](csharp/DownloadPdf/Program.cs), [VB.NET](vbnet/DownloadPdf/Program.vb)

#### DOM 

* Create new DOM element: [C#](csharp/DomCreateElement/Program.cs), [VB.NET](vbnet/DomCreateElement/Program.vb)
* Create and dispatch any DOM event (including custom events): [C#](csharp/DomCreateEvent/Program.cs), [VB.NET](vbnet/DomCreateEvent/Program.vb)
* Fill in and submit a form: [C#](csharp/DomForm/Program.cs), [VB.NET](vbnet/DomForm/Program.vb)
* Work with attributes of the specific DOM element: [C#](csharp/DomGetAttributes/Program.cs), [VB.NET](vbnet/DomGetAttributes/Program.vb)
* Get DOM elements by tag name: [C#](csharp/DomGetElements/Program.cs), [VB.NET](vbnet/DomGetElements/Program.vb)
* Find element by CSS Selector: [C#](csharp/DomQuerySelector/Program.cs), [VB.NET](vbnet/DomQuerySelector/Program.vb)
* Intercept DOM Drag & Drop events: [C#](csharp/Dom.DragAndDrop.WinForms), [VB.NET](vbnet/Dom.DragAndDrop.WinForms)
* Get DOM node at a specific point on the web page: [C#](csharp/Inspect/Program.cs), [VB.NET](vbnet/Inspect/Program.vb)
* Get DOM node by mouse location (WinForms): [C#](csharp/Inspect.WinForms), [VB.NET](vbnet/Inspect.WinForms)
* Get DOM node by mouse location (WPF): [C#](csharp/Inspect.Wpf), [VB.NET](vbnet/Inspect.Wpf)
* Evaluate an XPath expression. Work with the evaluation result.: [C#](csharp/XPath/Program.cs), [VB.NET](vbnet/XPath/Program.vb)
* Work with Shadow DOM: [C#](csharp/ShadowDom/Program.cs), [VB.NET](vbnet/ShadowDom/Program.vb)

#### JS-.NET bridge

* Execute any JavaScript on the web page. Get JavaScript return value.: [C#](csharp/JavaScript), [VB.NET](vbnet/JavaScript)
* Work with JavaScript objects, update their properties and <br/>invoke methods.: [C#](csharp/JavaScriptObjects), [VB.NET](vbnet/JavaScriptObjects)
* Work with JavaScript arrays: [C#](csharp/JavaScriptBridge.Arrays), [VB.NET](vbnet/JavaScriptBridge.Arrays)
* Work with JavaScript Promises: [C#](csharp/JavaScriptBridge.Promises), [VB.NET](vbnet/JavaScriptBridge.Promises)
* Inject a .NET object into JavaScript. Get its properties and invoke <br/>public methods from the JavaScript side.: [C#](csharp/JavaScriptBridge), [VB.NET](vbnet/JavaScriptBridge)
* Execute JavaScript on UI event. Call back from JavaScript to <br/>.NET UI (WinForms): [C#](csharp/JavaScriptBridge.WinForms), [VB.NET](vbnet/JavaScriptBridge.WinForms)
* Inject object for scripting (`window.external`): [C#](csharp/InjectObjectForScripting), [VB.NET](vbnet/InjectObjectForScripting)
* Observe web page content changes on .NET side <br/>using `MutationObserver`: [C#](csharp/ObservePageChanges.WinForms), [VB.NET](vbnet/ObservePageChanges.WinForms)
* Intercept Notification title and message: [C#](csharp/Notifications.InterceptData), [VB.NET](vbnet/Notifications.InterceptData)

#### Network

* Redirect a URL request to another website. Access the URL <br/>request headers.: [C#](csharp/NetworkHandlers), [VB.NET](vbnet/NetworkHandlers)
* Intercept and handle URL requests: [C#](csharp/CustomRequestHandling), [VB.NET](vbnet/CustomRequestHandling)
* Handle `mailto` or any other URI scheme to open external application: [C#](csharp/MailToHandling.WinForms), [VB.NET](vbnet/MailToHandling.WinForms)
* Access HTTP response data: [C#](csharp/AccessingHttpResponseData), [VB.NET](vbnet/AccessingHttpResponseData)
* Intercept the response data for AJAX requests: [C#](csharp/AjaxResponseIntercept), [VB.NET](vbnet/AjaxResponseIntercept) 
* Suppress AJAX requests: [C#](csharp/AjaxCallsFilter), [VB.NET](vbnet/AjaxCallsFilter) 
* Read and modify POST data : [C#](csharp/PostData), [VB.NET](vbnet/PostData)
* Intercept WebSocket data: [C#](csharp/WebSockets.InterceptData), [VB.NET](vbnet/WebSockets.InterceptData)
* Handle SSL certificate errors: [C#](csharp/CertificateError), [VB.NET](vbnet/CertificateError) 
* Accept or reject SSL certificates: [C#](csharp/CertificateVerifier), [VB.NET](vbnet/CertificateVerifier) 
* Filter out incoming and outgoing cookies: [C#](csharp/CookieFilter), [VB.NET](vbnet/CookieFilter) 
* Get all stored cookies by URL: [C#](csharp/Cookies), [VB.NET](vbnet/Cookies) 

#### Media

* List available audio and video devices. Select default media devices for <br/>the web page: [C#](csharp/DefaultMediaStreamDevice), [VB.NET](vbnet/DefaultMediaStreamDevice)

#### UI

* Customize context menu in WPF: [C#](csharp/ContextMenu.Wpf), [VB.NET](csharp/ContextMenu.Wpf) 
* Customize context menu with WinForms: [C#](csharp/ContextMenu.WinForms), [VB.NET](vbnet/ContextMenu.WinForms) 
* Configure custom keyboard shortcuts: [C#](csharp/CustomShortcuts.WinForms), [VB.NET](vbnet/CustomShortcuts.WinForms) 
* Simulate keyboard input in WPF: [C#](csharp/KeyboardEventSimulation.Wpf), [VB.NET](vbnet/KeyboardEventSimulation.Wpf)
* Simulate keyboard input in WinForms: [C#](csharp/KeyboardEventSimulation.WinForms), [VB.NET](vbnet/KeyboardEventSimulation.WinForms)
* Work with Chromium DevTools: [C#](csharp/DevTools.WinForms), [VB.NET](vbnet/DevTools.WinForms)
* Handle Full Screen mode: [C#](csharp/FullScreen.WinForms), [VB.NET](vbnet/FullScreen.WinForms)
* Zoom web page on `Ctrl+Scroll` : [C#](csharp/Zoom.Wpf), [VB.NET](vbnet/Zoom.Wpf)
* WPF Demo application with tabs: [C#](csharp/Demo.Wpf)
* Windows Forms Demo application with tabs: [C#](csharp/Demo.WinForms)
* Work with the web page via UI Automation (MSAA/UIA): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/blob/master/csharp/UiAutomation.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/blob/master/vbnet/UiAutomation.Wpf)
* Display web page with transparent background in <br/>a transparent WPF window: [C#](csharp/TransparentWebPage.Wpf), [VB.NET](vbnet/TransparentWebPage.Wpf)
* Intercept Drag & Drop events. Access `IDataObject`: [C#](csharp/DragAndDrop.Wpf), [VB.NET](vbnet/DragAndDrop.Wpf)
* Use DotNetBrowser with WPF data binding (MVVM): [C#](csharp/Mvvm.Wpf), [VB.NET](vbnet/Mvvm.Wpf)

#### Specific use-cases

* Create custom HTML UI: [C#](csharp/CreateHtmlUi.Wpf), [VB.NET](vbnet/CreateHtmlUi.Wpf)
* WPF kiosk application: [C#](csharp/Kiosk.Wpf), [VB.NET](vbnet/Kiosk.Wpf)
* WinForms kiosk application: [C#](csharp/Kiosk.WinForms), [VB.NET](vbnet/Kiosk.WinForms)
* Integrate with Google Maps: [C#](csharp/GoogleMaps.WinForms), [VB.NET](vbnet/GoogleMaps.WinForms)
* Integrate with Google Street View: [C#](csharp/GoogleStreetView.WinForms), [VB.NET](vbnet/GoogleStreetView.WinForms)
* Integrate with Selenium Chrome Driver: [C#](csharp/SeleniumChromeDriver), [VB.NET](vbnet/SeleniumChromeDriver)
* VSTO Add-In for Microsoft Outlook: [C#](csharp/MyOutlookAddIn), [VB.NET](vbnet/MyOutlookAddIn)
* COM/ActiveX wrapper: [C#](csharp/ComWrapper.WinForms), [VB.NET](vbnet/ComWrapper.WinForms)

#### Examples for tutorials
* Нow to deploy Chromium binaries over network: [C#](csharp/ChromiumBinariesResolver.Wpf), [VB.NET](vbnet/ChromiumBinariesResolver.Wpf)

### Contact us
Feel free to [submit request](https://dotnetbrowser.support.teamdev.com/support/tickets/new) to our support team if you own a commercial license with an active support subscription.

---

The information in this repository is provided on the following terms: https://www.teamdev.com/terms-and-privacy
