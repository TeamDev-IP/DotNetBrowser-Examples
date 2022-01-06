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

Imports System
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports ChromiumBinariesResolver.Wpf
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

''' <summary>
'''     Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Implements INotifyPropertyChanged

    Private ReadOnly chromiumDirectory As String
    Private browser As IBrowser
    Private engine As IEngine
'INSTANT VB NOTE: The field initializationStatus was renamed since Visual Basic does not allow fields to have the same name as other class members:
    Private initStatus As String = "Initializing"

    Public ReadOnly Property BinariesResolver() As BinariesResolver

    Public Property InitializationStatus() As String
        Get
            Return initStatus
        End Get
        Private Set(ByVal value As String)
            initStatus = value
            OnPropertyChanged()
        End Set
    End Property

    Private privateIsInitializationInProgress As Boolean
    Public Property IsInitializationInProgress() As Boolean
        Get
            Return privateIsInitializationInProgress
        End Get
        Private Set(ByVal value As Boolean)
            privateIsInitializationInProgress = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New()
        chromiumDirectory = Path.GetFullPath("chromium")

        'Delete the Chromium directory it it exists - this will force downloading the binaries over network.
        If Directory.Exists(chromiumDirectory) Then
            Directory.Delete(chromiumDirectory, True)
        End If

        Directory.CreateDirectory(chromiumDirectory)

        'Create and initialize the BinariesResolver
        BinariesResolver = New BinariesResolver()
        'Subscribe to the StatusUpdated event to update the UI accordingly.
        AddHandler BinariesResolver.StatusUpdated, Sub(sender, e) InitializationStatus = e.Message

        DataContext = Me

        Task.Run(Sub()
            IsInitializationInProgress = True
            Dim engineOptions As EngineOptions = New DotNetBrowser.Engine.EngineOptions.Builder With {
                    .RenderingMode = RenderingMode.HardwareAccelerated,
                    .ChromiumDirectory = chromiumDirectory
                    } .Build()
            InitializationStatus = "Creating DotNetBrowser engine"
            engine = EngineFactory.Create(engineOptions)
            InitializationStatus = "DotNetBrowser engine created"
            browser = engine.CreateBrowser()
        End Sub).ContinueWith(Sub(t)
            BrowserView.InitializeFrom(browser)
            IsInitializationInProgress = False
            browser.Navigation.LoadUrl("https://www.teamdev.com/")
        End Sub, TaskScheduler.FromCurrentSynchronizationContext())

        InitializeComponent()
    End Sub

    Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional ByVal propertyName As String = Nothing)
        PropertyChangedEvent?.Invoke(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private Sub MainWindow_OnClosed(ByVal sender As Object, ByVal e As EventArgs)
        engine?.Dispose()
    End Sub

End Class
