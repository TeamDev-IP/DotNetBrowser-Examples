
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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

''' <summary>
'''     This example demonstrates how to find text on the loaded web page.
''' </summary>
Public Class Form1
    Private Browser As IBrowser
    Private Engine As IEngine

    Sub New()
        EngineFactory.CreateAsync(New EngineOptions.Builder With {
                                     .RenderingMode = RenderingMode.OffScreen
                                     }.Build()).ContinueWith(
                                         Sub(t)
                                             Engine = t.Result
                                             Browser = engine.CreateBrowser()
                                             BrowserView.InitializeFrom(Browser)
                                             Browser.Navigation.LoadUrl("https://teamdev.com/dotnetbrowser")
                                         End Sub, TaskScheduler.FromCurrentSynchronizationContext())

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        Browser.TextFinder.StopFinding()
        TextBox.Text = ""
    End Sub

    Private Sub FindButton_Click(sender As Object, e As EventArgs) Handles FindButton.Click
        If Not String.IsNullOrEmpty(TextBox.Text) Then
            ' Set the text which should be found
            Browser.TextFinder.Find(TextBox.Text).ContinueWith(Sub(t)
                ' Check that on the webpage exists one at least
                If t.Result.NumberOfMatches = 0 Then
                    MessageBox.Show("No matches!")
                End If
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        End If
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Browser.Dispose()
        Engine.Dispose()
    End Sub
End Class
