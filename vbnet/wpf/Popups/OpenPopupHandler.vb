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

' #docfragment "OpenPopupHandler.Wpf"
Imports System.Windows.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Wpf

Public Class OpenPopupHandler
    Implements IHandler(Of OpenPopupParameters)

    Private ReadOnly parent As FrameworkElement

    Private ReadOnly Property Dispatcher As Dispatcher
        Get
            Return If(parent?.Dispatcher, Application.Current.Dispatcher)
        End Get
    End Property

    Public Sub New(parentElement As FrameworkElement)
        parent = parentElement
    End Sub

    Public Sub Handle(p As OpenPopupParameters) _
        Implements IHandler(Of OpenPopupParameters).Handle
        Dim showPopupAction As Action =
                Sub()
                    ShowPopup(p.PopupBrowser, p.Rectangle)
                End Sub
        Dispatcher.BeginInvoke(showPopupAction)
    End Sub

    Private Sub ShowPopup(popupBrowser As IBrowser, rectangle As Rectangle)
        Dim browserView As New BrowserView()
        browserView.InitializeFrom(popupBrowser)
        ' Set the same popup handler for the popup browser itself.
        popupBrowser.OpenPopupHandler = New OpenPopupHandler(browserView)

        Dim window As New Window With {.Owner = Window.GetWindow(parent)}

        If Not rectangle.IsEmpty Then
            window.Top = rectangle.Origin.Y
            window.Left = rectangle.Origin.X
            window.SizeToContent = SizeToContent.WidthAndHeight

            browserView.Width = rectangle.Size.Width
            browserView.Height = rectangle.Size.Height
        Else
            window.Width = 800
            window.Height = 600
        End If

        AddHandler window.Closed, Sub(sender, args)
            window.Content = Nothing
            If Not popupBrowser.IsDisposed Then
                popupBrowser.Dispose()
            End If
        End Sub

        AddHandler popupBrowser.TitleChanged, Sub(sender, e)
            Dispatcher?.BeginInvoke(CType(Sub() window.Title = e.Title, Action))
        End Sub

        AddHandler popupBrowser.Disposed, Sub()
            Dispatcher?.Invoke(Sub()
                window.Content = Nothing
                window.Hide()
                window.Close()
            End Sub)
        End Sub

        window.Content = browserView
        window.Show()
    End Sub
End Class

' #enddocfragment "OpenPopupHandler.Wpf"
