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
Imports Avalonia.Controls
Imports Avalonia.Markup.Xaml
Imports DotNetBrowser.AvaloniaUi
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

'''<summary>
'''    The sample demonstrates how to enable transparent background
'''    on the web page.
'''</summary>
Partial Public Class MainWindow
    Inherits Window

    Private ReadOnly Url As String = Path.Combine(Directory.GetCurrentDirectory(), "transparent.html")
    Private ReadOnly browser As IBrowser
    Private ReadOnly engine As IEngine

    Private Window As Window
    Private BrowserView As BrowserView

    Public Sub New()
        ' Create and initialize the IEngine instance.
        Dim engineOptions As EngineOptions = New EngineOptions.Builder With {
            .RenderingMode = RenderingMode.OffScreen
        }.Build()

        engine = EngineFactory.Create(engineOptions)

        ' Create the IBrowser instance.
        browser = engine.CreateBrowser()
        browser.Settings.TransparentBackgroundEnabled = true

        InitializeComponent()

        ' Initialize the Avalonia UI BrowserView control.
        BrowserView.InitializeFrom(browser)
        browser.Navigation.LoadUrl(Url)
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As EventArgs)
        browser?.Dispose()
        engine?.Dispose()
    End Sub

    ' Auto-wiring does not work for VB, so do it manually
    ' Wires up the controls and optionally loads XAML markup and attaches dev tools
    ' (if Avalonia.Diagnostics package is referenced)
    Private Sub InitializeComponent(Optional loadXaml As Boolean = True)

        If loadXaml Then
            AvaloniaXamlLoader.Load(Me)
        End If

        ' An example of manually getting the named BrowserView component
        BrowserView = FindNameScope().Find("BrowserView")

        ' An example of manually getting the named Window
        Window = FindNameScope().Find("Window")

    End Sub

End Class
