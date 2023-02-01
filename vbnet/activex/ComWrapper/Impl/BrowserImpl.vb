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
Imports DotNetBrowser.Browser

Namespace ComWrapper.WinForms.Impl
	Friend Class BrowserImpl
		Implements IComBrowser

		Friend ReadOnly Property Browser() As IBrowser

		Public ReadOnly Property Document() As IComDocument Implements IComBrowser.Document
			Get
				CheckInitialized()
				Return New DocumentImpl(Browser.MainFrame.Document)
			End Get
		End Property

		Public ReadOnly Property Html() As String Implements IComBrowser.Html
			Get
				CheckInitialized()
				Return Browser.MainFrame.Html
			End Get
		End Property

		Public Sub New(ByVal browserInstance As IBrowser)
			Me.Browser = browserInstance
		End Sub

		Public Sub Dispose() Implements IComBrowser.Dispose
			Browser.Dispose()
		End Sub

		Public Sub LoadUrl(ByVal url As String) Implements IComBrowser.LoadUrl
			CheckInitialized()
			Browser.Navigation.LoadUrl(url)
		End Sub

		Public Sub LoadUrlAndWait(ByVal url As String) Implements IComBrowser.LoadUrlAndWait
			CheckInitialized()
			Browser.Navigation.LoadUrl(url).Wait()
		End Sub

		Private Sub CheckInitialized()
			If Browser.IsDisposed Then
				Throw New InvalidOperationException("Browser is already disposed.")
			End If
		End Sub

	End Class
End Namespace
