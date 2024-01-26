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

' #docfragment "ElementHostEmbedding.WinForms"
Imports System.Windows.Forms.Integration
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Wpf

Namespace ElementHostEmbedding.WinForms
    Partial Public Class Form1
        Inherits Form

        Private Const Url As String = "https://html5test.teamdev.com"
        Private ReadOnly browser As IBrowser
        Private ReadOnly engine As IEngine
        Private ReadOnly host As ElementHost

        Public Sub New()
            ' Create and initialize the IEngine instance.
            Dim engineOptions As EngineOptions = New EngineOptions.Builder With {
                .RenderingMode = RenderingMode.OffScreen,
                .LicenseKey = "your_license_key_goes_here"
            }.Build()
            engine = EngineFactory.Create(engineOptions)

            ' Create the IBrowser instance.
            browser = engine.CreateBrowser()
            ' Create the WPF BrowserView control.
            Dim browserView As New BrowserView()

            InitializeComponent()
            AddHandler FormClosed, AddressOf Form1_FormClosed

            ' Create and initialize the ElementHost control.
            host = New ElementHost With {
                .Dock = DockStyle.Fill,
                .Child = browserView
            }
            Controls.Add(host)

            ' Initialize the WPF BrowserView control.
            browserView.InitializeFrom(browser)
            browser.Navigation.LoadUrl(Url)
        End Sub

        Private Sub Form1_FormClosed(sender As Object, e As EventArgs)
            browser?.Dispose()
            engine?.Dispose()
        End Sub
    End Class
End Namespace
' #enddocfragment "ElementHostEmbedding.WinForms"
