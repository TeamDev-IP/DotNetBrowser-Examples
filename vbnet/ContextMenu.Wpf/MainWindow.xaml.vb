#Region "Copyright"

' Copyright © 2022, TeamDev. All rights reserved.
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
'''     The sample demonstrates how to customize a context menu
'''     for an IBrowser instance.
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Private browser As IBrowser
    Private engine As IEngine


    Public Sub New()
        Task.Run(Sub()
            engine = EngineFactory.Create(
                New EngineOptions.Builder _
                                    With {.RenderingMode = RenderingMode.OffScreen}.
                                    Build())

                     browser = engine.CreateBrowser()
                 End Sub).ContinueWith(Sub(t)
            WebView.InitializeFrom(browser)
            ConfigureContextMenu()
            browser.Navigation.LoadUrl("https://www.google.com/")
        End Sub, TaskScheduler.FromCurrentSynchronizationContext())

        InitializeComponent()
    End Sub

    Private Function BuildMenuItem(item As String, isEnabled As Boolean,
                                   isVisible As Visibility,
                                   clickHandler As RoutedEventHandler) As MenuItem
        Dim result As New MenuItem With {
                .Header = item,
                .Visibility = Visibility.Collapsed
                }
        result.Visibility = isVisible
        result.IsEnabled = isEnabled
        AddHandler result.Click, clickHandler

        Return result
    End Function

    Private Sub ConfigureContextMenu()
        ' #docfragment "ContextMenu.Configuration"
        browser.ShowContextMenuHandler =
            New AsyncHandler(Of ShowContextMenuParameters, ShowContextMenuResponse )(
                AddressOf ShowContextMenu)
        ' #enddocfragment "ContextMenu.Configuration"
    End Sub

    ' #docfragment "ContextMenu.Implementation"
    Private Function ShowContextMenu(parameters As ShowContextMenuParameters) _
        As Task(Of ShowContextMenuResponse)
        Dim tcs As New TaskCompletionSource(Of ShowContextMenuResponse)()
        WebView.Dispatcher?.BeginInvoke(New Action(Sub()
            Dim popupMenu As New ContextMenu()

            If Not String.IsNullOrEmpty(parameters.LinkText) Then
                Dim linkMenuItem As MenuItem =
                        BuildMenuItem("Show the URL link", True,
                                      Visibility.Visible,
                                      Sub(sender, args)
                                          Dim linkUrl As String = parameters.LinkUrl
                                          Debug.WriteLine( $"linkURL = {linkUrl}")
                                          MessageBox.Show(linkUrl,"URL")
                                          tcs.TrySetResult(ShowContextMenuResponse.Close())
                                      End Sub)
                popupMenu.Items.Add(linkMenuItem)
            End If

            Dim reloadMenuItem As MenuItem =
                    BuildMenuItem("Reload", True, Visibility.Visible,
                                  Sub(sender, args)
                                      Debug.WriteLine("Reload current web page")
                                      browser.Navigation.Reload()
                                      tcs.TrySetResult(ShowContextMenuResponse.Close())
                                  End Sub)
            popupMenu.Items.Add(reloadMenuItem)
            AddHandler popupMenu.Closed, Sub(sender, args)
                tcs.TrySetResult(ShowContextMenuResponse.Close())
            End Sub
            popupMenu.IsOpen = True
        End Sub))
        Return tcs.Task
    End Function
    ' #enddocfragment "ContextMenu.Implementation"

    Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
        browser.Dispose()
        engine.Dispose()
    End Sub
End Class
