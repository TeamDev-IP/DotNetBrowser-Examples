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

Imports System.Diagnostics

Namespace ComWrapper.WinForms
	''' <summary>
	'''     Logs events to the system event logger.
	'''     These logged events can then be found in the Event Viewer -> Windows Logs -> Application
	''' </summary>
	Friend Class EventLogWrapper
		Private Const Source As String = "DotNetBrowser.ComWrapper"

		Private Const LogName As String = "Application"

		Public Shared Sub Log(ByVal message As String, ByVal level As EventLogEntryType, ByVal eventId As Integer)
			If Not EventLog.SourceExists(Source) Then
				EventLog.CreateEventSource(Source, LogName)
			End If

			EventLog.WriteEntry(Source, message, level, eventId)
		End Sub

	End Class
End Namespace
