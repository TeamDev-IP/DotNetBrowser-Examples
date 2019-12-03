Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports Office = Microsoft.Office.Core
Imports Outlook = Microsoft.Office.Interop.Outlook

Namespace MyOutlookAddIn
	Partial Friend Class BrowserFormRegion
		#Region "Form Region Factory "

		<Microsoft.Office.Tools.Outlook.FormRegionMessageClass(Microsoft.Office.Tools.Outlook.FormRegionMessageClassAttribute.Note)>
		<Microsoft.Office.Tools.Outlook.FormRegionName("MyOutlookAddIn.BrowserFormRegion")>
		Partial Public Class BrowserFormRegionFactory
			' Occurs before the form region is initialized.
			' To prevent the form region from appearing, set e.Cancel to true.
			' Use e.OutlookItem to get a reference to the current Outlook item.
			Private Sub BrowserFormRegionFactory_FormRegionInitializing(ByVal sender As Object, ByVal e As Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs)
				e.Cancel = False
			End Sub
		End Class

		#End Region

		Private engine As IEngine
		Private browser As IBrowser

		' Occurs before the form region is displayed.
		' Use this.OutlookItem to get a reference to the current Outlook item.
		' Use this.OutlookFormRegion to get a reference to the form region.
		Private Sub BrowserFormRegion_FormRegionShowing(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.FormRegionShowing
			Task.Run(Sub()
				engine = EngineFactory.Create(New EngineOptions.Builder() With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())
				browser = engine.CreateBrowser()
			End Sub).ContinueWith(Sub(t)
				browserView1.InitializeFrom(browser)
				browser?.Navigation.LoadUrl("https://www.teamdev.com/dotnetbrowser")
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())

		End Sub

		' Occurs when the form region is closed.
		' Use this.OutlookItem to get a reference to the current Outlook item.
		' Use this.OutlookFormRegion to get a reference to the form region.
		Private Sub BrowserFormRegion_FormRegionClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.FormRegionClosed
			engine?.Dispose()
		End Sub
	End Class
End Namespace
