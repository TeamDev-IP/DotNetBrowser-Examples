# DotNetBrowser COM/ActiveX wrapper example 

This example demonstrates the possible way to wrap DotNetBrowser functionality and expose it to the applications that support interacting with COM components or embedding ActiveX controls. As a result, the Chromium functionality provided by DotNetBrowser can be used in VB6, VBA, VBScript, etc.

During the project build, the TLB is generated and registered automatically. Cleaning the project leads to unregistering the components.

After the wrapper is registered, you can reuse it, for example, in VBScript:

```vbs
'This example demonstrates how to interact with DotNetBrowser COM wrapper from VBScript.

Dim html 
Set engine = CreateObject("DotNetBrowser.ComWrapper.Engine")
engine.Initialize()
Set browser = engine.CreateBrowser()
browser.LoadUrlAndWait("google.com")
html = browser.Html
engine.Dispose()

MsgBox html
```

The example also logs registration and initialization events to the system event logger. These logged events can then be found in the Event Viewer -> Windows Logs -> Application.

_Note:_ please check the bitness of the application to are trying to integrate with. if your application is 64-bit and the control is registered for 32-bit applications only (the registry entries are created under `HKEY_CLASSES_ROOT\WOW6432Node\` in this case), the control will be inaccessible for this application. The same can be observed if the application is 32-bit and the control is registered for 64-bit applications only.