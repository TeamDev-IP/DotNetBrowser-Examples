'This example demonstrates how to interact with DotNetBrowser COM wrapper from VBScript.

Dim html 
Set engine = CreateObject("DotNetBrowser.ComWrapper.Engine")
engine.Initialize()
Set browser = engine.CreateBrowser()
browser.LoadUrlAndWait("google.com")
html = browser.Html
engine.Dispose()

MsgBox html