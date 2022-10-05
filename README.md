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
* Embed into Windows Forms application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/csharp/Embedding.WinForms), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/vbnet/Embedding.WinForms) 
* Embed into WPF application: [C#](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/csharp/Embedding.Wpf), [VB.NET](https://github.com/TeamDev-IP/DotNetBrowser-QuickStart/vbnet/Embedding.Wpf) 

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
* Fill in and submit a multipage form: [C#](csharp/SimulateUserInput.WinForms), [VB.NET](vbnet/SimulateUserInput.WinForms)
* Work with attributes of the specific DOM element: [C#](csharp/DomGetAttributes/Program.cs), [VB.NET](vbnet/DomGetAttributes/Program.vb)
* Get DOM elements by tag name: [C#](csharp/DomGetElements/Program.cs), [VB.NET](vbnet/DomGetElements/Program.vb)
* Find element by CSS Selector: [C#](csharp/DomQuerySelector/Program.cs), [VB.NET](vbnet/DomQuerySelector/Program.vb)
* Intercept DOM Drag & Drop events: [C#](csharp/Dom.DragAndDrop.WinForms), [VB.NET](vbnet/Dom.DragAndDrop.WinForms)
* Get DOM node at a specific point on the web page: [C#](csharp/Inspect/Program.cs), [VB.NET](vbnet/Inspect/Program.vb)
* Get DOM node by mouse location (WinForms): [C#](csharp/Inspect.WinForms), [VB.NET](vbnet/Inspect.WinForms)
* Get DOM node by mouse location (WPF): [C#](csharp/Inspect.Wpf), [VB.NET](vbnet/Inspect.Wpf)
* Evaluate an XPath expression. Work with the evaluation result.: [C#](csharp/XPath/Program.cs), [VB.NET](vbnet/XPath/Program.vb)
* Work with Shadow DOM: [C#](csharp/ShadowDom/Program.cs), [VB.NET](vbnet/ShadowDom/Program.vb)
* Modify DOM element CSS style. Change DOM element visibility: [C#](csharp/ElementVisibility.WinForms), [VB.NET](vbnet/ElementVisibility.WinForms)

#### JS-.NET bridge

* Execute any JavaScript on the web page. Get JavaScript return value.: [C#](csharp/JavaScript/Program.cs), [VB.NET](vbnet/JavaScript/Program.vb)
* Work with JavaScript objects, update their properties and <br/>invoke methods.: [C#](csharp/JavaScriptObjects/Program.cs), [VB.NET](vbnet/JavaScriptObjects/Program.vb)
* Work with JavaScript arrays: [C#](csharp/JavaScriptBridge.Arrays/Program.cs), [VB.NET](vbnet/JavaScriptBridge.Arrays/Program.vb)
* Work with JavaScript name converting: [C#](csharp/JavaScriptBridge.NameConverter/Program.cs), [VB.NET](vbnet/JavaScriptBridge.NameConverter/Program.vb)
* Work with JavaScript Promises: [C#](csharp/JavaScriptBridge.Promises/Program.cs), [VB.NET](vbnet/JavaScriptBridge.Promises/Program.vb)
* Inject a .NET object into JavaScript. Get its properties and invoke <br/>public methods from the JavaScript side.: [C#](csharp/JavaScriptBridge/Program.cs), [VB.NET](vbnet/JavaScriptBridge/Program.vb)
* Execute JavaScript on UI event. Call back from JavaScript to <br/>.NET UI (WinForms): [C#](csharp/JavaScriptBridge.WinForms), [VB.NET](vbnet/JavaScriptBridge.WinForms)
* Inject object for scripting (`window.external`): [C#](csharp/InjectObjectForScripting/Program.cs), [VB.NET](vbnet/InjectObjectForScripting/Program.vb)
* Observe web page content changes on .NET side <br/>using `MutationObserver`: [C#](csharp/ObservePageChanges.WinForms), [VB.NET](vbnet/ObservePageChanges.WinForms)
* Intercept Notification title and message: [C#](csharp/Notifications.InterceptData/Program.cs), [VB.NET](vbnet/Notifications.InterceptData/Program.vb)

#### Network

* Redirect a URL request to another website. Access the URL <br/>request headers.: [C#](csharp/NetworkHandlers/Program.cs), [VB.NET](vbnet/NetworkHandlers/Program.vb)
* Intercept and handle URL requests: [C#](csharp/CustomRequestHandling/Program.cs), [VB.NET](vbnet/CustomRequestHandling/Program.vb)
* Handle `mailto` or any other URI scheme to open external application: [C#](csharp/MailToHandling.WinForms), [VB.NET](vbnet/MailToHandling.WinForms)
* Access HTTP response data: [C#](csharp/AccessingHttpResponseData/Program.cs), [VB.NET](vbnet/AccessingHttpResponseData/Program.vb)
* Intercept the response data for AJAX requests: [C#](csharp/AjaxResponseIntercept/Program.cs), [VB.NET](vbnet/AjaxResponseIntercept/Program.vb) 
* Suppress AJAX requests: [C#](csharp/AjaxCallsFilter/Program.cs), [VB.NET](vbnet/AjaxCallsFilter/Program.vb) 
* Read and modify POST data : [C#](csharp/PostData/Program.cs), [VB.NET](vbnet/PostData/Program.vb)
* Intercept WebSocket data: [C#](csharp/WebSockets.InterceptData/Program.cs), [VB.NET](vbnet/WebSockets.InterceptData/Program.vb)
* Handle SSL certificate errors: [C#](csharp/CertificateError/Program.cs), [VB.NET](vbnet/CertificateError/Program.vb) 
* Accept or reject SSL certificates: [C#](csharp/CertificateVerifier/Program.cs), [VB.NET](vbnet/CertificateVerifier/Program.vb) 
* Filter out incoming and outgoing cookies: [C#](csharp/CookieFilter/Program.cs), [VB.NET](vbnet/CookieFilter/Program.vb) 
* Get all stored cookies by URL: [C#](csharp/Cookies/Program.cs), [VB.NET](vbnet/Cookies/Program.vb) 
* Share cookies between two IEngine instances with different user data directories (WinForms): [C#](csharp/CookiesSharing.WinForms), [VB.NET](vbnet/CookiesSharing.WinForms)

#### Media

* List available audio and video devices. Select default media devices for <br/>the web page: [C#](csharp/DefaultMediaStreamDevice/Program.cs), [VB.NET](vbnet/DefaultMediaStreamDevice/Program.vb)

#### UI
* Embed into Windows Forms ElementHost: [C#](csharp/ElementHostEmbedding.WinForms), [VB.NET](vbnet/ElementHostEmbedding.WinForms) 
* Customize context menu in WPF: [C#](csharp/ContextMenu.Wpf), [VB.NET](vbnet/ContextMenu.Wpf) 
* Customize context menu with WinForms: [C#](csharp/ContextMenu.WinForms), [VB.NET](vbnet/ContextMenu.WinForms) 
* Spell checker context menu in WPF: [C#](csharp/ContextMenu.SpellCheck.Wpf), [VB.NET](vbnet/ContextMenu.SpellCheck.Wpf) 
* Spell checker context menu in WinForms: [C#](csharp/ContextMenu.SpellCheck.WinForms), [VB.NET](vbnet/ContextMenu.SpellCheck.WinForms)
* Customize popup windows in WPF: [C#](csharp/Popups.Wpf), [VB.NET](vbnet/Popups.Wpf) 
* Customize popup windows in WinForms: [C#](csharp/Popups.WinForms), [VB.NET](vbnet/Popups.WinForms) 
* Configure custom keyboard shortcuts: [C#](csharp/CustomShortcuts.WinForms), [VB.NET](vbnet/CustomShortcuts.WinForms) 
* Simulate keyboard input in WPF: [C#](csharp/KeyboardEventSimulation.Wpf), [VB.NET](vbnet/KeyboardEventSimulation.Wpf)
* Simulate keyboard input in WinForms: [C#](csharp/KeyboardEventSimulation.WinForms), [VB.NET](vbnet/KeyboardEventSimulation.WinForms)
* Work with Chromium DevTools: [C#](csharp/DevTools.WinForms), [VB.NET](vbnet/DevTools.WinForms)
* Handle Full Screen mode: [C#](csharp/FullScreen.WinForms), [VB.NET](vbnet/FullScreen.WinForms)
* Zoom web page on `Ctrl+Scroll` : [C#](csharp/Zoom.Wpf), [VB.NET](vbnet/Zoom.Wpf)
* WPF Demo application with tabs: [C#](csharp/Demo.Wpf)
* Windows Forms Demo application with tabs: [C#](csharp/Demo.WinForms)
* Work with the web page via UI Automation (MSAA/UIA): [C#](csharp/UiAutomation.Wpf), [VB.NET](vbnet/UiAutomation.Wpf)
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
