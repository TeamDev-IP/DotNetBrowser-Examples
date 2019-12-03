Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports DotNetBrowser.Logging
Imports Outlook = Microsoft.Office.Interop.Outlook
Imports Office = Microsoft.Office.Core

Namespace MyOutlookAddIn
	Partial Public Class ThisAddIn
		Private Sub ThisAddIn_Startup(ByVal sender As Object, ByVal e As System.EventArgs)
			LoggerProvider.Instance.Level = SourceLevels.Verbose
			LoggerProvider.Instance.FileLoggingEnabled = True
			LoggerProvider.Instance.OutputFile = "D:\Projects\MyOutlookAddIn\DotNetBrowser.log"
		End Sub

		Private Sub ThisAddIn_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs)
			' Note: Outlook no longer raises this event. If you have code that 
			'    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
		End Sub

		#Region "VSTO generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InternalStartup()
			AddHandler Me.Startup, AddressOf ThisAddIn_Startup
			AddHandler Me.Shutdown, AddressOf ThisAddIn_Shutdown
		End Sub

		#End Region
	End Class
End Namespace
