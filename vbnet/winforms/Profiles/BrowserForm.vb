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

Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.WinForms

Namespace Profiles.WinForms
    Partial Public Class BrowserForm
        Inherits Form

        Private ReadOnly browserView As BrowserView
        Private _browser As IBrowser

        Public Property Browser As IBrowser
            Get
                Return _browser
            End Get

            Set
                _browser = value
                If _browser IsNot Nothing Then
                    browserView.InitializeFrom(_browser)
                    LoadUrl(AddressBar.Text)
                End If
            End Set
        End Property

        Public Sub New()
            browserView = New BrowserView With {.Dock = DockStyle.Fill}
            InitializeComponent()
            Controls.Add(browserView)
        End Sub

        Private Sub AddressBar_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                LoadUrl(AddressBar.Text)
            End If
        End Sub

        Private Sub BrowserForm_FormClosed(sender As Object, e As FormClosedEventArgs)
            _browser?.Dispose()
        End Sub

        Private Sub LoadUrl(address As String)
            browser?.Navigation?.LoadUrl(address).ContinueWith(Sub(t)
                UpdateControlsStates()
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        End Sub

        Private Sub UpdateControlsStates()
            If Not Browser.IsDisposed Then
                AddressBar.Text = Browser.Url
                Text = Browser.Title
            End If
        End Sub

    End Class
End Namespace
