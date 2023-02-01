#Region "Copyright"

' Copyright © 2023, TeamDev. All rights reserved.
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

Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.WinForms

Public Class Form1
    Private Const RemoteDebuggingPort As Integer = 9222

    Private engine As IEngine
    Private browser As IBrowser
    Private browserView As BrowserView
    Private seleniumInstance As SeleniumInstance

    Delegate Sub SeleniumInstanceConnectedDelegate()

    Public Sub New()

        InitializeComponent()

        AddHandler Closed, AddressOf Form1_Closed
        AddHandler Load, AddressOf Form1_Load

        InitializeBrowser()

        seleniumInstance = new SeleniumInstance(RemoteDebuggingPort)
        AddHandler seleniumInstance.Connected, AddressOf SeleniumInstance_Connected
    End Sub

    Private Sub SeleniumInstance_Connected()
        If InvokeRequired Then
            Dim connectedDelegate As SeleniumInstanceConnectedDelegate = AddressOf SeleniumInstance_Connected
            Invoke(connectedDelegate)
            Return
        End If

        Activate()
    End Sub

    Private Sub InitializeBrowser()
        Dim engineOptionsBuilder As EngineOptions.Builder = new EngineOptions.Builder
        With engineOptionsBuilder
            .WebSecurityDisabled = True
            .RemoteDebuggingPort = RemoteDebuggingPort
            .ChromiumSwitches.Add("--enable-automation")
        End With

        Dim engineOptions = engineOptionsBuilder.Build()

        engine = EngineFactory.Create(engineOptions)
        browser = engine.CreateBrowser()

        Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<h1>Waiting for Selenium...</h1>")
        browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes))

        browserView = new BrowserView()
        browserView.Dock = DockStyle.Fill
        browserView.InitializeFrom(browser)
        Controls.Add(browserView)
    End Sub

    Private Async Sub Form1_Load(sender As Object, e As EventArgs)
        Await seleniumInstance.ConnectAndRun()
    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs)
        browser?.Dispose()
        engine?.Dispose()
    End Sub
End Class
