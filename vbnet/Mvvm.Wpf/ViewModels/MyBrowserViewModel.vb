#Region "Copyright"

' Copyright 2021, TeamDev. All rights reserved.
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

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Navigation.Events

Namespace Mvvm.Wpf.ViewModels
	Public Class MyBrowserViewModel
		Implements INotifyPropertyChanged

		Public ReadOnly Property Browser() As IBrowser

		Public Property Url() As String
			Get
				Return Browser.Url
			End Get
			Set(ByVal value As String)
				If Not String.IsNullOrWhiteSpace(value) Then
					Browser.Navigation.LoadUrl(value).Wait()
				End If

				OnPropertyChanged()
			End Set
		End Property

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

		Public Sub New(ByVal browserInstance As IBrowser)
			Me.Browser = browserInstance
			AddHandler Me.Browser.Navigation.FrameLoadFinished, AddressOf NavigationOnFrameLoadFinished
		End Sub

		Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional ByVal propertyName As String = Nothing)
			PropertyChangedEvent?.Invoke(Me, New PropertyChangedEventArgs(propertyName))
		End Sub

		Private Sub NavigationOnFrameLoadFinished(ByVal sender As Object, ByVal e As FrameLoadFinishedEventArgs)
			If e.Frame.IsMain Then
				'Navigation is finished, notify that the URL is possibly updated.
				OnPropertyChanged(NameOf(Url))
			End If
		End Sub

	End Class
End Namespace