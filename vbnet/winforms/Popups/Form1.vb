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

Imports System.IO
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

Namespace Popups.WinForms
    ''' <summary>
    '''     This example demonstrates how to implement and configure a custom
    '''     OpenPopupHandler to customize displaying the pop-ups.
    ''' </summary>
    Partial Public Class Form1
        Inherits Form

        Private browser As IBrowser
        Private engine As IEngine

        Public Sub New()
            EngineFactory.CreateAsync().ContinueWith(Sub(t)
                engine = t.Result
                browser = engine.CreateBrowser()
                browserView.InitializeFrom(browser)
                browser.OpenPopupHandler = New OpenPopupHandler(browserView)
                browser?.Navigation.LoadUrl(Path.GetFullPath("popup.html"))
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
            InitializeComponent()
        End Sub

        Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            engine?.Dispose()
        End Sub
    End Class
End Namespace
