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
Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.SpellCheck

''' <summary>
'''     The example demonstrates how to create a context menu with the SpellChecker functionality.
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Private browser As IBrowser
    Private engine As IEngine


    Public Sub New()
        Task.Run(Sub()
            engine = EngineFactory.Create(New EngineOptions.Builder With {
                                            .RenderingMode = RenderingMode.OffScreen
                                        }.Build())
            browser = engine.CreateBrowser()
        End Sub).ContinueWith(Sub(t)
            WebView.InitializeFrom(browser)
            ConfigureContextMenu()
            Dim htmlBytes() As Byte =
                    Encoding.UTF8.GetBytes(
                        "<html>
                        <head>
                          <meta charset='UTF-8'>
                        </head>
                        <body>
                        <textarea autofocus cols='30' rows='20'>Simpple mistakee</textarea>
                        </body>
                        </html>")
            browser.Navigation.LoadUrl(
                "data:text/html;base64," & Convert.ToBase64String(htmlBytes))
        End Sub, TaskScheduler.FromCurrentSynchronizationContext())

        InitializeComponent()
    End Sub

    Private Function BuildMenuItem(item As String, isEnabled As Boolean,
                                   clickHandler As RoutedEventHandler) As MenuItem
        Dim result As New MenuItem With {
            .Header = item,
            .Visibility = Visibility.Collapsed
        }
        result.Visibility = Visibility.Visible
        result.IsEnabled = isEnabled
        AddHandler result.Click, clickHandler

        Return result
    End Function

    Private Sub ConfigureContextMenu()
        ' #docfragment "ContextMenu.Configuration"
        browser.ShowContextMenuHandler =
            New AsyncHandler(Of ShowContextMenuParameters, ShowContextMenuResponse)(
                AddressOf ShowContextMenu)
        ' #enddocfragment "ContextMenu.Configuration"
    End Sub

    ' #docfragment "ContextMenu.Implementation"
    Private Function ShowContextMenu(parameters As ShowContextMenuParameters) _
        As Task(Of ShowContextMenuResponse)

        Dim tcs As New TaskCompletionSource(Of ShowContextMenuResponse)()
        Dim spellCheckMenu As SpellCheckMenu = parameters.SpellCheckMenu
        WebView.Dispatcher?.BeginInvoke(New Action(Sub()
            Dim popupMenu As New Controls.ContextMenu()

            Dim suggestions As IEnumerable(Of String) = spellCheckMenu.DictionarySuggestions
            If suggestions IsNot Nothing Then
                ' Add menu items with suggestions.
                For Each suggestion As String In suggestions
                    Dim menuItem As MenuItem = BuildMenuItem(suggestion, True, Sub()
                        browser.ReplaceMisspelledWord(suggestion)
                        tcs.TrySetResult(ShowContextMenuResponse.Close())
                    End Sub)
                    popupMenu.Items.Add(menuItem)
                Next suggestion
            End If

            ' Add "Add to Dictionary" menu item.
            Dim addToDictionary As String =
                    If(spellCheckMenu.AddToDictionaryMenuItemText, "Add to Dictionary")

            popupMenu.Items.Add(BuildMenuItem(addToDictionary, True, Sub()
                If Not String.IsNullOrWhiteSpace(spellCheckMenu.MisspelledWord) Then
                    engine.Profiles.Default.SpellChecker?.CustomDictionary?.Add(
                        spellCheckMenu.MisspelledWord)
                End If

                tcs.TrySetResult(ShowContextMenuResponse.Close())
            End Sub))
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
