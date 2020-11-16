
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
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Input.Keyboard
Imports DotNetBrowser.Input.Keyboard.Events
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.WinForms

''' <summary>
'''     This example demonstrates how to simulate keypress.
''' </summary>
Public Class Form1

    Private browser As IBrowser
    Private browserView As BrowserView
    Private engine As IEngine

    #Region "Constructors"

    Public Sub New()
        InitializeComponent()
        AddHandler Closing, AddressOf Form_Closing

        Try
            Task.Run(Sub()
                engine = EngineFactory.Create(
                            New EngineOptions.Builder With {.RenderingMode = RenderingMode.OffScreen}.Build())
                browser = engine.CreateBrowser()
            End Sub).ContinueWith(Sub(t)
                browserView = New BrowserView()
                ' Embed BrowserView component into main layout.
                Controls.Add(browserView)

                browserView.InitializeFrom(browser)
                browserView.Dock = DockStyle.Fill

                browser.MainFrame.LoadHtml("<html>
                                                <body>
                                                    <input type='text' autofocus></input>
                                                </body>
                                            </html>") _
                                     .ContinueWith(AddressOf SimulateInput)
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
            
        Catch exception As Exception
            Debug.WriteLine(exception)
        End Try
    End Sub

    #End Region

    #Region "Methods"

    Private Async Sub SimulateInput(e As Task(Of LoadResult))
        If e.Result = LoadResult.Completed Then
            Await Task.Delay(2000)
            Dim keyboard As IKeyboard = browser.Keyboard
            SimulateKey(keyboard, KeyCode.VkH, "H")
            SimulateKey(keyboard, KeyCode.VkE, "e")
            SimulateKey(keyboard, KeyCode.VkL, "l")
            SimulateKey(keyboard, KeyCode.VkL, "l")
            SimulateKey(keyboard, KeyCode.VkO, "o")
            SimulateKey(keyboard, KeyCode.Space, " ")
            'Simulate input of some non-letter characters
            SimulateKey(keyboard, KeyCode.Vk5, "%", New KeyModifiers() With {.ShiftDown = True})
            SimulateKey(keyboard, KeyCode.Vk2, "@", New KeyModifiers() With {.ShiftDown = True})
        End If
    End Sub

    Private Shared Sub SimulateKey(keyboard As IKeyboard, key As KeyCode, keyChar As String, Optional ByVal modifiers As KeyModifiers = Nothing)
        modifiers = If(modifiers, New KeyModifiers())
        Dim keyPressedEventArgs = New KeyPressedEventArgs With {
                .KeyChar = keyChar,
                .VirtualKey = key,
                .Modifiers = modifiers
                }

        Dim keyTypedEventArgs = New KeyTypedEventArgs With {
                .KeyChar = keyChar,
                .VirtualKey = key,
                .Modifiers = modifiers
                }
        Dim keyReleasedEventArgs = New KeyReleasedEventArgs With {
                .VirtualKey = key,
                .Modifiers = modifiers
                }

        keyboard.KeyPressed.Raise(keyPressedEventArgs)
        keyboard.KeyTyped.Raise(keyTypedEventArgs)
        keyboard.KeyReleased.Raise(keyReleasedEventArgs)
    End Sub


    Private Sub Form_Closing(sender As Object, e As CancelEventArgs)
        ' Dispose browser and engine when close app window.
        browser.Dispose()
        engine.Dispose()
    End Sub

    #End Region

End Class
