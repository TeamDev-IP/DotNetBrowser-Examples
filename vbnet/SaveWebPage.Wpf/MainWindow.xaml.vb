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
Imports System.IO
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Wpf

''' <summary>
'''     Demonstrates how to embed WPF BrowserView component into WPF Application,
'''     load and display HTML content from a string.
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Private browser As IBrowser
    Private browserView As BrowserView
    Private engine As IEngine

    Public Sub New()
        Task.Run(Sub()
            engine = EngineFactory.Create(
                        New EngineOptions.Builder With {
                                            .RenderingMode = RenderingMode.OffScreen
                                            }.Build())

            browser = engine.CreateBrowser()
        End Sub).ContinueWith(Sub(t)
            ' Create WPF BrowserView component.
            browserView = New BrowserView()
            ' Embed BrowserView component into main layout.
            mainLayout.Children.Add(browserView)

            browserView.InitializeFrom(browser)

            browser.Navigation.LoadUrl("https://www.teamdev.com/").ContinueWith(AddressOf SaveWebPage)
        End Sub, TaskScheduler.FromCurrentSynchronizationContext())
        ' Initialize WPF Application UI.
        InitializeComponent()
    End Sub

    Private Sub SaveWebPage(obj As Task(Of LoadResult))
        Dim filePath As String = Path.GetFullPath("SavedPages\index.html")
        Dim dirPath As String = Path.GetFullPath("SavedPages\resources")
        Directory.CreateDirectory(dirPath)
        browser.SaveWebPage(filePath, dirPath, SavePageType.CompletePage)
    End Sub

    Private Sub Window_Closing(sender As Object, e As CancelEventArgs)
        ' Dispose browser and engine when close app window.
        browser.Dispose()
        engine.Dispose()
    End Sub

End Class
