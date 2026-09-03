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
Imports Microsoft.Playwright
Imports BrowserViewExtensions = DotNetBrowser.Browser.BrowserViewExtensions

Public Partial Class MainWindow
    Inherits Window

    Private Const Url As String = "https://html5test.teamdev.com/"
    Private Const RemoteDebuggingPort As Integer = 9223
    Private Const LocationUrl As String = "https://www.infobyip.com/browsergeolocation.php"
    Private ReadOnly browser As DotNetBrowser.Browser.IBrowser
    Private ReadOnly engine As IEngine

    Public Sub New()
        ' Create and initialize the IEngine instance
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

        browser.Navigation.LoadUrl(Url).ContinueWith(Async Function(t) 
            Await ConnectAsync()
            Return Task.CompletedTask
        End Function)
    End Sub

    Private Async Function ConnectAsync() As Task
        Try
            ' #docfragment "Playwright.Connect"
            Using _
                playwright As IPlaywright =
                    Await Microsoft.Playwright.Playwright.CreateAsync()
                ' Connect to the browser using CDP
                Dim playwrightBrowser As IBrowser =
                        Await _
                        playwright.Chromium.ConnectOverCDPAsync(
                            $"http://localhost:{RemoteDebuggingPort}")

                Dim browserContext As IBrowserContext = playwrightBrowser.Contexts(0)
                Await browserContext.GrantPermissionsAsync({"geolocation"})
                Await browserContext.SetGeolocationAsync(New Geolocation With {
                                                            .Latitude = 42.746635F,
                                                            .Longitude = - 75.770045F
                                                            })

                Dim page As IPage = browserContext.Pages(0)
                Await page.GotoAsync(LocationUrl)
                Await page.WaitForSelectorAsync("title")

                ' Scroll the map into view
                Await page.Locator("#map").ScrollIntoViewIfNeededAsync()
            End Using
            ' #enddocfragment "Playwright.Connect"
        Catch e As Exception
            Debug.WriteLine(e)
            MessageBox.Show($"Failed to connect: {e.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Function

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        If browser IsNot Nothing Then
            browser.Dispose()
        End If
        If engine IsNot Nothing Then
            engine.Dispose()
        End If
    End Sub
End Class