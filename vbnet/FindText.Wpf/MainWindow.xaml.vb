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
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

''' <summary>
'''     Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Private browser As IBrowser
    Private engine As IEngine

#Region "Constructors"

    Public Sub New()
        Task.Run(Sub()
            engine =
                    EngineFactory.Create(
                        New EngineOptions.Builder With {.RenderingMode = RenderingMode.OffScreen}.Build())
            browser = engine.CreateBrowser()
        End Sub).ContinueWith(Sub(t)
            browserView.InitializeFrom(browser)

            browser.Navigation.LoadUrl("https://teamdev.com/dotnetbrowser")
        End Sub, TaskScheduler.FromCurrentSynchronizationContext())

        InitializeComponent()
    End Sub

#End Region

#Region "Methods"

    Private Sub clearButton_Click(sender As Object, e As RoutedEventArgs)
        browser.TextFinder.StopFinding()
        textBox.Text = ""
    End Sub

    Private Sub findButton_Click(sender As Object, e As RoutedEventArgs)
        If textBox.Text <> String.Empty Then
            browser.TextFinder.Find(textBox.Text).ContinueWith(Sub(t)
                If t.Result.NumberOfMatches = 0 Then
                    MessageBox.Show("No matches!")
                End If
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        End If
    End Sub

    Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
        browser.Dispose()
        engine.Dispose()
    End Sub

#End Region
End Class