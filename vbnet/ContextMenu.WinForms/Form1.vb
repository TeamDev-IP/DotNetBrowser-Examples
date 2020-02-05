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

Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Logging
Imports DotNetBrowser.SpellCheck
Imports DotNetBrowser.WinForms

Namespace ContextMenu.WinForms
    Partial Public Class Form1
        Inherits Form

        Private browser As IBrowser
        Private engine As IEngine
        Private ReadOnly webView As BrowserView

#Region "Constructors"

        Public Sub New()
            LoggerProvider.Instance.Level = SourceLevels.Verbose
            LoggerProvider.Instance.FileLoggingEnabled = True
            LoggerProvider.Instance.OutputFile = "log.txt"
            webView = New BrowserView With {.Dock = DockStyle.Fill}
            Task.Run(Sub()
                engine =
                        EngineFactory.Create(
                            New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())
                browser = engine.CreateBrowser()
            End Sub).ContinueWith(Sub(t)
                webView.InitializeFrom(browser)
                browser.ShowContextMenuHandler =
                                     New AsyncHandler(Of ShowContextMenuParameters, ShowContextMenuResponse)(
                                         AddressOf ShowMenu)
                browser.MainFrame.LoadHtml(
                    "<html>
<head>
  <meta charset='UTF-8'>
</head>
<body>
<textarea autofocus cols='30' rows='20'>Simpple mistakee</textarea>
</body>
</html>")
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
            InitializeComponent()
            AddHandler Me.FormClosing, AddressOf Form1_FormClosing
            Controls.Add(webView)
        End Sub

#End Region

#Region "Methods"

        Private Function BuildMenuItem(item As String, isEnabled As Boolean, clickHandler As EventHandler) As MenuItem
            Dim result As New MenuItem()
            result.Text = item
            result.Enabled = isEnabled
            AddHandler result.Click, clickHandler

            Return result
        End Function

        Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs)
            browser?.Dispose()
            engine?.Dispose()
        End Sub

        Private Function ShowMenu(parameters As ShowContextMenuParameters) As Task(Of ShowContextMenuResponse)
            Dim tcs As New TaskCompletionSource(Of ShowContextMenuResponse)()
            Dim spellCheckMenu As SpellCheckMenu = parameters.SpellCheckMenu
            If spellCheckMenu IsNot Nothing Then
                BeginInvoke(New Action(Sub()
                    Dim popupMenu As New System.Windows.Forms.ContextMenu()
                    Dim suggestions As IEnumerable(Of String) = spellCheckMenu.DictionarySuggestions
                    If suggestions IsNot Nothing Then
                        For Each suggestion As String In suggestions
                            popupMenu.MenuItems.Add(BuildMenuItem(suggestion, True, Sub()
                                browser.ReplaceMisspelledWord(suggestion)
                                tcs.TrySetResult(ShowContextMenuResponse.Close())
                            End Sub))
                        Next suggestion
                    End If

                    Dim addToDictionary As String = If(spellCheckMenu.AddToDictionaryMenuItemText, "Add to Dictionary")
                    popupMenu.MenuItems.Add(BuildMenuItem(addToDictionary, True, Sub()
                        engine.SpellChecker?.CustomDictionary?.Add(spellCheckMenu.MisspelledWord)
                        tcs.TrySetResult(ShowContextMenuResponse.Close())
                    End Sub))
                    AddHandler popupMenu.Collapse, Sub(sender, args)
                        tcs.TrySetResult(ShowContextMenuResponse.Close())
                    End Sub

                    Dim location_Conflict As New Point(parameters.Location.X, parameters.Location.Y)
                    popupMenu.Show(Me, location_Conflict)
                    tcs.TrySetResult(ShowContextMenuResponse.Close())
                End Sub))
            Else
                tcs.TrySetResult(ShowContextMenuResponse.Close())
            End If

            Return tcs.Task
        End Function

#End Region
    End Class
End Namespace
