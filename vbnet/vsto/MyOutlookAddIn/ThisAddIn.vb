#Region "Copyright"

' Copyright © 2023, TeamDev. All rights reserved.
' 
' Redistribution and use in source and/or binary forms, with or without
' modification, must retain the above copyright notice and the following
' disclaimer.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#End Region

Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
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
			LoggerProvider.Instance.OutputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"DotNetBrowser.log")
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
