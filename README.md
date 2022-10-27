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

* Create two separate Chromium engines: [C#](csharp/console/SeparateEngines/Program.cs), [VB.NET](vbnet/console/SeparateEngines/Program.vb)
* Create two separate Chromium engines in different AppDomains: [C#](csharp/console/SeparateEngines.AppDomains/Program.cs), [VB.NET](vbnet/console/SeparateEngines.AppDomains/Program.vb)
* Create separate Chromium profiles: [C#](csharp/winforms/Profiles), [VB.NET](vbnet/winforms/Profiles)
* Load URL and listen to the navigation events: [C#](csharp/console/LoadEvents/Program.cs), [VB.NET](vbnet/console/LoadEvents/Program.vb)
* Embed into Windows Forms application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/csharp/Embedding.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/vbnet/Embedding.WinForms) 
* Embed into WPF application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/csharp/Embedding.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/vbnet/Embedding.Wpf) 

#### Web page content

* Get page HTML: [C#](csharp/console/GetHtml/Program.cs), [VB.NET](vbnet/console/GetHtml/Program.vb)
* Walk through the hierarchy of frames on the web page: [C#](csharp/console/GetFrames/Program.cs), [VB.NET](vbnet/console/GetFrames/Program.vb)
* Get selected text on the web page: [C#](csharp/console/GetSelectedText/Program.cs), [VB.NET](vbnet/console/GetSelectedText/Program.vb)
* Find text on the web page: [C#](csharp/console/FindText/Program.cs), [VB.NET](vbnet/console/FindText/Program.vb)
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
* Spell checker context menu in WPF: [C#](csharp/wpf/ContextMenu.SpellCheck), [VB.NET](vbnet/wpf/ContextMenu.SpellCheck) 
* Spell checker context menu in WinForms: [C#](csharp/winforms/ContextMenu.SpellCheck), [VB.NET](vbnet/winforms/ContextMenu.SpellCheck)
* Customize popup windows in WPF: [C#](csharp/wpf/Popups), [VB.NET](vbnet/wpf/Popups) 
* Customize popup windows in WinForms: [C#](csharp/winforms/Popups), [VB.NET](vbnet/winforms/Popups) 
* Configure custom keyboard shortcuts: [C#](csharp/winforms/CustomShortcuts), [VB.NET](vbnet/winforms/CustomShortcuts) 
* Simulate keyboard input in WPF: [C#](csharp/wpf/KeyboardEventSimulation), [VB.NET](vbnet/wpf/KeyboardEventSimulation)
* Simulate keyboard input in WinForms: [C#](csharp/winforms/KeyboardEventSimulation), [VB.NET](vbnet/winforms/KeyboardEventSimulation)
* Work with Chromium DevTools: [C#](csharp/winforms/DevTools), [VB.NET](vbnet/winforms/DevTools)
* Handle Full Screen mode: [C#](csharp/winforms/FullScreen), [VB.NET](vbnet/winforms/FullScreen)
* Zoom web page on `Ctrl+Scroll` : [C#](csharp/wpf/Zoom), [VB.NET](vbnet/wpf/Zoom)
* WPF Demo application with tabs: [C#](csharp/wpf/Demo)
* Windows Forms Demo application with tabs: [C#](csharp/winforms/Demo)
* Work with the web page via UI Automation (MSAA/UIA): [C#](csharp/wpf/UiAutomation), [VB.NET](vbnet/wpf/UiAutomation)
* Display web page with transparent background in <br/>a transparent WPF window: [C#](csharp/wpf/TransparentWebPage), [VB.NET](vbnet/wpf/TransparentWebPage)
* Intercept Drag & Drop events. Access `IDataObject`: [C#](csharp/wpf/DragAndDrop), [VB.NET](vbnet/wpf/DragAndDrop)
* Use DotNetBrowser with WPF data binding (MVVM): [C#](csharp/wpf/Mvvm), [VB.NET](vbnet/wpf/Mvvm)

#### Specific use-cases

* Create custom HTML UI: [C#](csharp/wpf/CreateHtmlUi), [VB.NET](vbnet/wpf/CreateHtmlUi)
* WPF kiosk application: [C#](csharp/wpf/Kiosk), [VB.NET](vbnet/wpf/Kiosk)
* WinForms kiosk application: [C#](csharp/winforms/Kiosk), [VB.NET](vbnet/winforms/Kiosk)
* Integrate with Google Maps: [C#](csharp/winforms/GoogleMaps), [VB.NET](vbnet/winforms/GoogleMaps)
* Integrate with Google Street View: [C#](csharp/winforms/GoogleStreetView), [VB.NET](vbnet/winforms/GoogleStreetView)
* Integrate with Selenium Chrome Driver: [C#](csharp/selenium/SeleniumChromeDriver), [VB.NET](vbnet/selenium/SeleniumChromeDriver)
* VSTO Add-In for Microsoft Outlook: [C#](csharp/vsto/MyOutlookAddIn), [VB.NET](vbnet/vsto/MyOutlookAddIn)
* COM/ActiveX wrapper: [C#](csharp/activex/ComWrapper), [VB.NET](vbnet/activex/ComWrapper)
* Integration with Unity3D: [C#](csharp/unity3d)

#### Examples for tutorials
* Нow to deploy Chromium binaries over network: [C#](csharp/wpf/ChromiumBinariesResolver), [VB.NET](vbnet/wpf/ChromiumBinariesResolver)

### Contact us
Feel free to [submit request](https://dotnetbrowser.support.teamdev.com/support/tickets/new) to our support team if you own a commercial license with an active support subscription.

---

The information in this repository is provided on the following terms: https://www.teamdev.com/terms-and-privacy
