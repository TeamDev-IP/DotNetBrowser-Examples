#Region "Copyright"
' Copyright © 2026, TeamDev. All rights reserved.
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


Imports DotNetBrowser.Engine
Imports BrowserViewExtensions = DotNetBrowser.Browser.BrowserViewExtensions

Public Partial Class MainWindow
    Inherits Window

    Private Const RemoteDebuggingPort As Integer = 9224
    Private ReadOnly browser As DotNetBrowser.Browser.IBrowser
    Private ReadOnly engine As IEngine

    Public Sub New()
        ' Create and initialize the IEngine instance. An MCP server
        ' attaches to the engine through the remote debugging port.
        Dim engineOptions As EngineOptions = New EngineOptions.Builder With {
                .RenderingMode = RenderingMode.HardwareAccelerated,
                .LicenseKey = "",
                .RemoteDebuggingPort = RemoteDebuggingPort
                }.Build()
        engine = EngineFactory.Create(engineOptions)

        ' Create the IBrowser instance
        browser = engine.CreateBrowser()

        InitializeComponent()

        ' Initialize the WPF BrowserView control
        BrowserViewExtensions.InitializeFrom(browserView, browser)

        ' Load the bundled page the agent works with
        browser.Navigation.LoadUrl(IO.Path.GetFullPath("orders.html"))
    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        If browser IsNot Nothing Then
            browser.Dispose()
        End If
        If engine IsNot Nothing Then
            engine.Dispose()
        End If
    End Sub
End Class
