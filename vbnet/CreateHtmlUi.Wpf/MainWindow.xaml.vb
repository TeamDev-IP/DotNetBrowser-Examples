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

Imports System.IO
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Dom.Events
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Navigation.Events

Namespace CreateHtmlUi.Wpf
    ''' <summary>
    '''    The sample demonstrates how to create a custom HTML UI using DotNetBrowser
    '''    and debug it using DevTools.
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Private browser1 As IBrowser
        Private browser2 As IBrowser
        Private engine As IEngine

        Public Sub New()
            Task.Run(Sub()
                         engine = EngineFactory.Create(New EngineOptions.Builder With {
                             .RenderingMode = RenderingMode.HardwareAccelerated,
                             .RemoteDebuggingPort = 9222
                         }.Build())
                         browser1 = engine.CreateBrowser()
                         browser2 = engine.CreateBrowser()
                     End Sub).ContinueWith(Sub(t)
                                               browserView1.InitializeFrom(browser1)
                                               browserView2.InitializeFrom(browser2)

                                               AddHandler browser1.Navigation.FrameLoadFinished, AddressOf browser1_FrameLoadFinished
                                               browser1.Navigation.LoadUrl(Path.GetFullPath("UI.html"))
                                               browser2.Navigation.LoadUrl(browser1.DevTools.RemoteDebuggingUrl)
                                           End Sub, TaskScheduler.FromCurrentSynchronizationContext())

            InitializeComponent()
        End Sub

        Private Sub browser1_FrameLoadFinished(ByVal sender As Object, ByVal e As FrameLoadFinishedEventArgs)
            If Not e.Frame.IsDisposed AndAlso e.Frame.IsMain Then

                Dim document As IDocument = browser1.MainFrame.Document
                Dim inputs As IEnumerable(Of IElement) = document.GetElementsByTagName("input")

                For Each element As IElement In inputs
                    If element.Attributes("type").ToLower().Equals("submit") Then
                        AddHandler element.Events.Click, AddressOf OnSubmitClicked
                    End If
                Next element

            End If
        End Sub

        Private Sub MainWindow_OnClosed(ByVal sender As Object, ByVal e As EventArgs)
            engine.Dispose()
        End Sub

        Private Sub OnSubmitClicked(ByVal sender As Object, ByVal e As DomEventArgs)
            Task.Run(Sub()
                         Dim login As String = String.Empty
                         Dim password As String = String.Empty

                         Dim document As IDocument = browser1.MainFrame.Document

                         login = DirectCast(document.GetElementById("login"), IInputElement).Value
                         password = DirectCast(document.GetElementById("password"), IInputElement).Value

                         Application.Current.Dispatcher.BeginInvoke(New Action(
                             Sub()
                                 MessageBox.Show(Me, $"Login: {login}{vbLf}Password: {password}", "Data")
                             End Sub))
                     End Sub)
        End Sub

    End Class
End Namespace
