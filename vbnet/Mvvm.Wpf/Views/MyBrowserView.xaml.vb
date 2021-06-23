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


Imports DotNetBrowser.Browser
Imports Mvvm.Wpf.ViewModels

Namespace Mvvm.Wpf.Views
	''' <summary>
	'''     Interaction logic for MyBrowserView.xaml
	''' </summary>
	Partial Public Class MyBrowserView
		Inherits UserControl

		#Region "Constructors"

		Public Sub New()
			InitializeComponent()
			AddHandler Me.DataContextChanged, AddressOf OnDataContextChanged
		End Sub

		#End Region

		#Region "Methods"

		Private Sub AddressBox_OnKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			If e.Key = Key.Enter Then
				AddressBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource()
			End If
		End Sub

		Private Sub OnDataContextChanged(ByVal sender As Object, ByVal e As DependencyPropertyChangedEventArgs)
			Dim viewModel = TryCast(e.NewValue, MyBrowserViewModel)

			If viewModel IsNot Nothing Then
				BrowserView.InitializeFrom(viewModel.Browser)
			End If
		End Sub

		#End Region
	End Class
End Namespace