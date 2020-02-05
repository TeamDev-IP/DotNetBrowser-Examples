#Region "Copyright"

' Copyright 2020, TeamDev. All rights reserved.
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
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers

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
            engine =
                    EngineFactory.Create(
                        New EngineOptions.Builder With {.RenderingMode = RenderingMode.OffScreen}.Build())
            browser = engine.CreateBrowser()
        End Sub).ContinueWith(Sub(t)
            WebView.InitializeFrom(browser)
            browser.ShowContextMenuHandler =
                                 New AsyncHandler(Of ShowContextMenuParameters, ShowContextMenuResponse)(
                                     AddressOf ShowMenu)
            browser.Navigation.LoadUrl("https://www.google.com/")
        End Sub, TaskScheduler.FromCurrentSynchronizationContext())

        InitializeComponent()
    End Sub

#End Region

#Region "Methods"

    Private Function BuildMenuItem(item As String, isEnabled As Boolean, IsVisible As Visibility,
                                   clickHandler As RoutedEventHandler) As MenuItem
        Dim result As New MenuItem With {
                .Header = item,
                .Visibility = Visibility.Collapsed
                }
        result.Visibility = IsVisible
        result.IsEnabled = isEnabled
        AddHandler result.Click, clickHandler

        Return result
    End Function

    Private Function ShowMenu(parameters As ShowContextMenuParameters) As Task(Of ShowContextMenuResponse)
        Dim tcs As New TaskCompletionSource(Of ShowContextMenuResponse)()
        WebView.Dispatcher?.BeginInvoke(New Action(Sub()
            Dim popupMenu As New Controls.ContextMenu()

            If Not String.IsNullOrEmpty(parameters.LinkText) Then
                popupMenu.Items.Add(BuildMenuItem("Open link in new window", True, Visibility.Visible, Sub()
                    Dim linkURL As String = parameters.LinkUrl
                    Console.WriteLine($"linkURL = {linkURL}")
                    tcs.TrySetResult(ShowContextMenuResponse.Close())
                End Sub))
            End If

            popupMenu.Items.Add(BuildMenuItem("Reload", True, Visibility.Visible, Sub()
                Console.WriteLine("Reload current web page")
                browser.Navigation.Reload()
                tcs.TrySetResult(ShowContextMenuResponse.Close())
            End Sub))
            AddHandler popupMenu.Closed, Sub(sender, args)
                tcs.TrySetResult(ShowContextMenuResponse.Close())
            End Sub
            popupMenu.IsOpen = True
        End Sub))
        Return tcs.Task
    End Function

    Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
        browser.Dispose()
        engine.Dispose()
    End Sub

#End Region
End Class