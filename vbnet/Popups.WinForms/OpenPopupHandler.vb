#Region "Copyright"

' Copyright © 2021, TeamDev. All rights reserved.
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

' #docfragment "OpenPopupHandler.WinForms"
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.WinForms

Public Class OpenPopupHandler
    Implements IHandler(Of OpenPopupParameters)

    Private ReadOnly parent As Control

    Public Sub New(parent As Control)
        Me.parent = parent
    End Sub

    Public Sub Handle(p As OpenPopupParameters) _
        Implements IHandler(Of OpenPopupParameters).Handle
        Dim showPopupAction As Action = Sub()
            ShowPopup(p.PopupBrowser, p.Rectangle)
        End Sub
        parent.BeginInvoke(showPopupAction)
    End Sub

    Private Sub ShowPopup(popupBrowser As IBrowser, rectangle As Rectangle)
        Dim browserView As New BrowserView With {.Dock = DockStyle.Fill}

        browserView.InitializeFrom(popupBrowser)
        ' Set the same popup handler for the popup browser itself.
        popupBrowser.OpenPopupHandler = New OpenPopupHandler(browserView)

        Dim form As New Form()

        If Not rectangle.IsEmpty Then
            form.StartPosition = FormStartPosition.Manual

            form.Location =
                New Drawing.Point(rectangle.Origin.X, rectangle.Origin.Y)

            form.ClientSize =
                New Drawing.Size(rectangle.Size.Width, rectangle.Size.Height)

            browserView.Width = CInt(rectangle.Size.Width)
            browserView.Height = CInt(rectangle.Size.Height)
        Else
            form.Width = 800
            form.Height = 600
        End If

        AddHandler form.Closed, Sub()
            form.Controls.Clear()

            If Not popupBrowser.IsDisposed Then
                popupBrowser.Dispose()
            End If
        End Sub

        AddHandler popupBrowser.TitleChanged, Sub(sender, e)
            form.BeginInvoke(CType(Sub()
                form.Text = e.Title
            End Sub, Action))
        End Sub

        AddHandler popupBrowser.Disposed, Sub()
            Dim formCloseAction As Action = Sub()
                form.Controls.Clear()
                form.Hide()
                form.Close()
                form.Dispose()
            End Sub
            form.BeginInvoke(formCloseAction)
        End Sub

        form.Controls.Add(browserView)
        form.Show()
    End Sub
End Class

' #enddocfragment "OpenPopupHandler.WinForms"