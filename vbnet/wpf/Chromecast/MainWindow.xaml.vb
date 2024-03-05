#Region "Copyright"

' Copyright © 2024, TeamDev. All rights reserved.
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

Imports System.Collections.ObjectModel
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Cast
Imports DotNetBrowser.Engine

'''<summary>
'''    The sample demonstrates how to use Chromecast functionality in DotNetBrowser.
'''</summary>
Partial Public Class MainWindow
    Inherits Window

    Private ReadOnly browser As IBrowser
    Private ReadOnly engine As IEngine
    Private castSession As ICastSession

    Public Property Items As New ObservableCollection(Of Receiver)()

    Public Sub New()
        InitializeComponent()

        Dim engineOptions As EngineOptions =
                New EngineOptions.Builder With {.MediaRoutingEnabled = True}.Build()

        engine = EngineFactory.Create(engineOptions)
        browser = engine.CreateBrowser()
        browserView.InitializeFrom(browser)

        AddHandler browser.Navigation.NavigationFinished, Sub(sender, args)
            Dispatcher.BeginInvoke(New Action(Sub() navigationBar.Text = browser.Url))
        End Sub

        browser.Navigation.LoadUrl("https://youtube.com")
    End Sub

    Private Sub Navigate_Click(sender As Object, e As RoutedEventArgs)
        If castSession IsNot Nothing Then
            castSession.Stop()
            castSession = Nothing
            receiversBox.SelectedItem = Nothing
        End If

        browser.Navigation.LoadUrl(navigationBar.Text)
    End Sub

    Private Sub Receivers_Click(sender As Object, e As RoutedEventArgs)
        Dispatcher.BeginInvoke(New Action(Sub()
            receiversBox.ItemsSource = Nothing
            Items.Clear()
            browser.Profile.MediaCasting.Receivers.Refresh()

            Dim mediaReceivers As IReadOnlyList(Of IMediaReceiver) =
                    browser.Profile.MediaCasting.Receivers.AllAvailable

            For Each receiver As IMediaReceiver In mediaReceivers
                Items.Add(New Receiver With {
                             .MediaReceiver = receiver,
                             .Name = receiver.Name
                             })
            Next receiver

            receiversBox.ItemsSource = Items
        End Sub))
    End Sub

    Private Async Sub ReceiversBox_SelectionChanged(sender As Object,
                                                    e As SelectionChangedEventArgs)
        If castSession IsNot Nothing Then
            Try
                castSession.Stop()
            Catch e1 As ObjectDisposedException
            End Try

            castSession = Nothing
        End If

        If receiversBox.SelectedItem IsNot Nothing Then
            Dim selectedReceiver = TryCast(receiversBox.SelectedItem, Receiver)
            Try
                castSession = Await browser.Cast.CastContent(selectedReceiver?.MediaReceiver)
            Catch ex As CastSessionStartFailedException
                MessageBox.Show(Me, ex.Message, "CastSessionStartFailedException")
            End Try
        End If
    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        engine.Dispose()
    End Sub
End Class

