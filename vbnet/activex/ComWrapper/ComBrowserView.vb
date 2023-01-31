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
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ComWrapper.WinForms.Impl
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Logging

Namespace ComWrapper.WinForms
	<Guid("5D300D12-E721-4711-8262-215E75894FE5")>
	<ClassInterface(ClassInterfaceType.AutoDual)>
	<ProgId("DotNetBrowser.ComWrapper.ComBrowserView")>
	Partial Public Class ComBrowserView
		Inherits UserControl
		Implements IComBrowserView

		Private ReadOnly _engineWrapper As EngineWrapper

		Public ReadOnly Property Browser() As IComBrowser Implements IComBrowserView.Browser

		Public ReadOnly Property Engine() As IComEngine Implements IComBrowserView.Engine
			Get
				Return _engineWrapper
			End Get
		End Property

		Public Sub New()
			Try
				ConfigureLogging()
				InitializeComponent()
				_engineWrapper = New EngineWrapper()
				_engineWrapper.Initialize()
				Browser = _engineWrapper.CreateBrowser()
				InitializeFrom(Browser)
				Browser.LoadUrl("teamdev.com/dotnetbrowser")
				EventLogWrapper.Log("ComBrowserView initialized", EventLogEntryType.Information, 201)
			Catch e As Exception
				EventLogWrapper.Log(e.ToString(), EventLogEntryType.Error, 500)
				Throw
			End Try
		End Sub

		Private Sub IComBrowserView_Dispose() Implements IComBrowserView.Dispose
			_engineWrapper.Dispose()
			EventLogWrapper.Log("ComBrowserView disposed", EventLogEntryType.Information, 201)
		End Sub

		Public Sub InitializeFrom(ByVal comBrowser As IComBrowser)
			If InvokeRequired Then
				BeginInvoke(New Action(Sub()
					browserView1.InitializeFrom(TryCast(comBrowser, BrowserImpl)?.Browser)
				End Sub))
			Else
				browserView1.InitializeFrom(TryCast(comBrowser, BrowserImpl)?.Browser)
			End If
		End Sub

		Private Shared Sub ConfigureLogging()
			LoggerProvider.Instance.Level = SourceLevels.Information
			LoggerProvider.Instance.FileLoggingEnabled = True
			Dim outputFile As String = Path.GetFullPath("dotnetbrowser.log")
			LoggerProvider.Instance.OutputFile = outputFile
			EventLogWrapper.Log($"DotNetBrowser logs can be found at {outputFile}", EventLogEntryType.Information, 202)
		End Sub


		<EditorBrowsable(EditorBrowsableState.Never)>
		<ComRegisterFunction>
		Private Shared Sub Register(ByVal t As Type)
			ControlRegistration.RegisterControl(t)
		End Sub

		<EditorBrowsable(EditorBrowsableState.Never)>
		<ComUnregisterFunction>
		Private Shared Sub Unregister(ByVal t As Type)
			ControlRegistration.UnregisterControl(t)
		End Sub

	End Class
End Namespace
