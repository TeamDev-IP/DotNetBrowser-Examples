#Region "Copyright"

' Copyright © 2020, TeamDev. All rights reserved.
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
Imports System.Threading.Tasks
Imports System.Windows
Imports System.Windows.Controls
Imports DotNetBrowser.Browser
Imports DotNetBrowser.ContextMenu
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers

Namespace ContextMenu.Wpf
	''' <summary>
	'''     Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

		Private browser As IBrowser
		Private engine As IEngine

		#Region "Constructors"

		Public Sub New()
			Task.Run(Sub()
					engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.OffScreen} .Build())
					browser = engine.CreateBrowser()
			End Sub).ContinueWith(Sub(t)
					webView.InitializeFrom(browser)
					browser.ContextMenuHandler = New AsyncHandler(Of ContextMenuParameters, ContextMenuResponse)(AddressOf ShowMenu)
					browser.Navigation.LoadUrl("https://www.google.com/")
			End Sub, TaskScheduler.FromCurrentSynchronizationContext())

			InitializeComponent()
		End Sub

		#End Region

		#Region "Methods"

		Private Function BuildMenuItem(ByVal item As String, ByVal isEnabled As Boolean, ByVal IsVisible As Visibility, ByVal clickHandler As RoutedEventHandler) As MenuItem
			Dim result As New MenuItem()
			result.Header = item
			result.Visibility = Visibility.Collapsed
			result.Visibility = IsVisible
			result.IsEnabled = isEnabled
			AddHandler result.Click, clickHandler

			Return result
		End Function

		Private Function ShowMenu(ByVal parameters As ContextMenuParameters) As Task(Of ContextMenuResponse)
			Dim tcs As New TaskCompletionSource(Of ContextMenuResponse)()
			webView.Dispatcher?.BeginInvoke(New Action(Sub()
                                                           Dim popupMenu As New Controls.ContextMenu()

                                                           If Not String.IsNullOrEmpty(parameters.LinkText) Then
					popupMenu.Items.Add(BuildMenuItem("Open link in new window", True, Visibility.Visible, Sub()
						Dim linkURL As String = parameters.LinkUrl
						Console.WriteLine($"linkURL = {linkURL}")
						tcs.TrySetResult(ContextMenuResponse.Close())
					End Sub))
				End If

				popupMenu.Items.Add(BuildMenuItem("Reload", True, Visibility.Visible, Sub()
					Console.WriteLine("Reload current web page")
					browser.Navigation.Reload()
					tcs.TrySetResult(ContextMenuResponse.Close())
				End Sub))
				AddHandler popupMenu.Closed, Sub(sender, args)
					tcs.TrySetResult(ContextMenuResponse.Close())
				End Sub
				popupMenu.IsOpen = True
			End Sub))
			Return tcs.Task
		End Function

		Private Sub Window_Closing(ByVal sender As Object, ByVal e As CancelEventArgs)
			browser.Dispose()
			engine.Dispose()
		End Sub

		#End Region
	End Class
End Namespace
