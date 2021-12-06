#Region "Copyright"

' Copyright 2021, TeamDev. All rights reserved.
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
Imports DotNetBrowser.WinForms

Namespace GoogleMaps.WinForms
    ''' <summary>
    '''     This example demonstrates how to use Google Maps with DotNetBrowser.
    '''     To make this sample work, please configure the valid Google API key in map.html(line 11)
    ''' </summary>
    Partial Public Class MainForm
        Inherits Form

        Private Const MinZoomLevel As Integer = 0
        Private Const MaxZoomLevel As Integer = 21

        Private zoomLevel As Integer = 4 'The default value for Google Maps zoom

        Private ReadOnly Property Browser() As IBrowser
        Private ReadOnly Property BrowserView() As BrowserView

        Private Property CurrentZoomLevel() As Integer
            Get
                Return zoomLevel
            End Get

            Set
                If value <> zoomLevel AndAlso value > MinZoomLevel AndAlso value < MaxZoomLevel Then
                    If Not Browser.IsDisposed Then
                        zoomLevel = value
                        Browser.MainFrame.ExecuteJavaScript($"map.setZoom({zoomLevel})")
                    End If
                End If
            End Set
        End Property

        Private ReadOnly Property Engine() As IEngine

        Private ReadOnly Property PathToMapFile() As String
            Get
                Return Path.GetFullPath("map.html")
            End Get
        End Property

        Public Sub New()
            InitializeComponent()

            Engine = EngineFactory.Create()
            Browser = Engine.CreateBrowser()
            BrowserView = New BrowserView With {.Dock = DockStyle.Fill}

            BrowserView.InitializeFrom(Browser)
            Controls.Add(BrowserView)

            Browser.Navigation.LoadUrl(PathToMapFile)

            AddHandler Me.Closed, AddressOf MainForm_Closed
        End Sub

        Private Sub MainForm_Closed(ByVal sender As Object, ByVal e As EventArgs)
            Browser.Dispose()
            Engine.Dispose()
        End Sub

        Private Sub ZoomInBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ZoomInBtn.Click
            CurrentZoomLevel += 1
        End Sub

        Private Sub ZoomOutBtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ZoomOutBtn.Click
            CurrentZoomLevel -= 1
        End Sub

    End Class
End Namespace