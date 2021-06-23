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

* Create two separate Chromium engines: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SeparateEngines), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SeparateEngines)
* Create two separate Chromium engines in different AppDomains: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SeparateEngines.AppDomains), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SeparateEngines.AppDomains)
* Load URL and listen to the navigation events: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/LoadEvents), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/LoadEvents)
* Embed into .NET Core WPF applications: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/NETCore30.Wpf)
* Embed into .NET Core WinForms applications: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/NETCore30.WinForms)

#### Web page content

* Get page HTML: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GetHtml), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GetHtml)
* Walk through the hierarchy of frames on the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GetFrames), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GetFrames)
* Get selected text on the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GetSelectedText), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GetSelectedText)
* Find text on the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/FindText), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/FindText)
* Text search (WPF): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/FindText.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/FindText.Wpf)
* Save web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SaveWebPage.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SaveWebPage.Wpf)
* Save an image from the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SaveImageFromPage), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SaveImageFromPage)
* Create web page screenshot: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/HtmlToImage), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/HtmlToImage)
* Execute editor commands (Cut, Copy, Paste, Undo, Select All,<br/> Insert Text etc.): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ExecuteCommand), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/ExecuteCommand)
* Access local storage of the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/WebStorage), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/WebStorage)
* Update the zoom level on the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Zoom), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Zoom)
* Print web page to PDF: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Printing.WebPageToPdf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Printing.WebPageToPdf)
* Download a PDF file by its URL: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DownloadPdf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DownloadPdf)

#### DOM 

* Create new DOM element: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomCreateElement), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomCreateElement)
* Create and dispatch any DOM event (including custom events): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomCreateEvent), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomCreateEvent)
* Fill in and submit a form: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomForm), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomForm)
* Work with attributes of the specific DOM element: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomGetAttributes), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomGetAttributes)
* Get DOM elements by tag name: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomGetElements), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomGetElements)
* Find element by CSS Selector: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DomQuerySelector), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DomQuerySelector)
* Intercept DOM Drag & Drop events: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Dom.DragAndDrop.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Dom.DragAndDrop.WinForms)
* Get DOM node at a specific point on the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Inspect), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Inspect)
* Get DOM node by mouse location (WinForms): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Inspect.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Inspect.WinForms)
* Get DOM node by mouse location (WPF): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Inspect.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Inspect.Wpf)
* Evaluate an XPath expression. Work with the evaluation result.: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/XPath), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/XPath)
* Work with Shadow DOM: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ShadowDom), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/ShadowDom)

#### JS-.NET bridge

* Execute any JavaScript on the web page. Get JavaScript return value.: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScript), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScript)
* Work with JavaScript objects, update their properties and <br/>invoke methods.: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptObjects), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptObjects)
* Work with JavaScript arrays: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptBridge.Arrays), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptBridge.Arrays)
* Work with JavaScript Promises: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptBridge.Promises), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptBridge.Promises)
* Inject a .NET object into JavaScript. Get its properties and invoke <br/>public methods from the JavaScript side.: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptBridge), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptBridge)
* Execute JavaScript on UI event. Call back from JavaScript to <br/>.NET UI (WinForms): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/JavaScriptBridge.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/JavaScriptBridge.WinForms)
* Inject object for scripting (`window.external`): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/InjectObjectForScripting), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/InjectObjectForScripting)
* Observe web page content changes on .NET side <br/>using `MutationObserver`: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ObservePageChanges.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/ObservePageChanges.WinForms)
* Intercept Notification title and message: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Notifications.InterceptData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Notifications.InterceptData)

#### Network

* Redirect a URL request to another website. Access the URL <br/>request headers.: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/NetworkHandlers), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/NetworkHandlers)
* Intercept and handle URL requests: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CustomRequestHandling), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CustomRequestHandling)
* Handle `mailto` or any other URI scheme to open external application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/MailToHandling.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/MailToHandling.WinForms)
* Access HTTP response data: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/AccessingHttpResponseData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/AccessingHttpResponseData)
* Intercept the response data for AJAX requests: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/AjaxResponseIntercept), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/AjaxResponseIntercept) 
* Suppress AJAX requests: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/AjaxCallsFilter), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/AjaxCallsFilter) 
* Read and modify POST data : [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/PostData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/PostData)
* Intercept WebSocket data: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/WebSockets.InterceptData), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/WebSockets.InterceptData)
* Handle SSL certificate errors: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CertificateError), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CertificateError) 
* Accept or reject SSL certificates: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CertificateVerifier), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CertificateVerifier) 
* Filter out incoming and outgoing cookies: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CookieFilter), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CookieFilter) 
* Get all stored cookies by URL: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Cookies), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Cookies) 

#### Media

* List available audio and video devices. Select default media devices for <br/>the web page: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DefaultMediaStreamDevice), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DefaultMediaStreamDevice)

#### UI

* Customize context menu in WPF: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ContextMenu.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ContextMenu.Wpf) 
* Customize context menu with WinForms: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ContextMenu.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/ContextMenu.WinForms) 
* Configure custom keyboard shortcuts: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CustomShortcuts.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CustomShortcuts.WinForms) 
* Simulate keyboard input in WPF: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/KeyboardEventSimulation.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/KeyboardEventSimulation.Wpf)
* Simulate keyboard input in WinForms: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/KeyboardEventSimulation.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/KeyboardEventSimulation.WinForms)
* Work with Chromium DevTools: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DevTools.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DevTools.WinForms)
* Handle Full Screen mode: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/FullScreen.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/FullScreen.WinForms)
* Zoom web page on `Ctrl+Scroll` : [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Zoom.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Zoom.Wpf)
* WPF Demo application with tabs: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Demo.Wpf)
* Windows Forms Demo application with tabs: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Demo.WinForms)
* Work with the web page via UI Automation (MSAA/UIA): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/blob/master/csharp/UiAutomation.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/blob/master/vbnet/UiAutomation.Wpf)
* Display web page with transparent background in <br/>a transparent WPF window: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/TransparentWebPage.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/TransparentWebPage.Wpf)
* Intercept Drag & Drop events. Access `IDataObject`: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/DragAndDrop.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/DragAndDrop.Wpf)
* Use DotNetBrowser with WPF data binding (MVVM): [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Mvvm.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Mvvm.Wpf)

#### Specific use-cases

* Create custom HTML UI: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/CreateHtmlUi.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/CreateHtmlUi.Wpf)
* WPF kiosk application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Kiosk.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Kiosk.Wpf)
* WinForms kiosk application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/Kiosk.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/Kiosk.WinForms)
* Integrate with Google Maps: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GoogleMaps.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GoogleMaps.WinForms)
* Integrate with Google Street View: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/GoogleStreetView.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/GoogleStreetView.WinForms)
* Integrate with Selenium Chrome Driver: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/SeleniumChromeDriver), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/SeleniumChromeDriver)
* VSTO Add-In for Microsoft Outlook: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/MyOutlookAddIn), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/MyOutlookAddIn)

#### Examples for tutorials
* Нow to deploy Chromium binaries over network: [C#](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/csharp/ChromiumBinariesResolver.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-Examples/tree/master/vbnet/ChromiumBinariesResolver.Wpf)

### Contact us
Feel free to [submit request](https://dotnetbrowser.support.teamdev.com/support/tickets/new) to our support team if you own a commercial license with an active support subscription.

---

The information in this repository is provided on the following terms: https://www.teamdev.com/terms-and-privacy
