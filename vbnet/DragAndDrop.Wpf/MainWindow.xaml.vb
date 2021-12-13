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

Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Input.DragAndDrop.Events
Imports DotNetBrowser.Input.DragAndDrop.Handlers
Imports DotNetBrowser.Net

''' <summary>
'''     This example demonstrates how to intercept drag and drop events
'''     and extract data from them.
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    Private ReadOnly browser As IBrowser
    Private ReadOnly engine As IEngine

    Public Sub New()
        Dim engineBuilder = New EngineOptions.Builder With {
                .RenderingMode = RenderingMode.HardwareAccelerated,
                .SandboxDisabled = True
                }
        engineBuilder.ChromiumSwitches.Add("--enable-com-in-drag-drop")
        engine = EngineFactory.Create(engineBuilder.Build())

        browser = engine.CreateBrowser()
        ' #docfragment "DragAndDrop.Configuration
        browser.DragAndDrop.EnterDragHandler =
            New Handler(Of EnterDragParameters)(AddressOf OnDragEnter)
        browser.DragAndDrop.DropHandler = New Handler(Of DropParameters)(AddressOf OnDrop)
        ' #enddocfragment "DragAndDrop.Configuration

        InitializeComponent()
        browserView.InitializeFrom(browser)
        browser.Navigation.LoadUrl("teamdev.com")
    End Sub

    Private Sub ExtractData(eventName As String, dataObject As Windows.IDataObject)
        If dataObject Is Nothing Then
            Debug.WriteLine("IDataObject is null")
            Return
        End If

        Dim sb As New StringBuilder("=====================================================")
        sb.AppendLine(vbLf & "Event name:" & eventName)
        sb.AppendLine(vbLf & "IDataObject:" & dataObject.ToString())
        sb.AppendLine("=====================================================")
        For Each format As String In dataObject.GetFormats()
            sb.AppendLine("Format:" & format)
            Try
                Dim data As Object = dataObject.GetData(format)
                sb.AppendLine(
                    "Type:" & (If(data Is Nothing, "[null]", data.GetType().ToString())))

                sb.AppendLine("Data:" & data.ToString())
                Dim strings = TryCast(data, IEnumerable(Of String))
                If strings IsNot Nothing Then
                    For Each s As String In strings
                        sb.AppendLine(vbTab & "Value: " & s)
                    Next s
                End If
            Catch ex As Exception
                sb.AppendLine("Exception thrown: " & ex.Message)
            End Try

            sb.AppendLine("=====================================================")
        Next format

        Dim message As String = sb.ToString()
        Dispatcher.BeginInvoke(CType(Sub()
            Output.Text = message
        End Sub,
                                     Action))

        Debug.WriteLine(message)
    End Sub

    Private Sub LogData(dropData As IDropData, evtName As String)
        Debug.WriteLine($"{evtName}:DropData is null? {dropData Is Nothing}")
        If dropData IsNot Nothing Then
            For Each file As IFileValue In dropData.Files
                Debug.WriteLine($"{evtName}:File = {file?.FileName}")
            Next file
        End If
    End Sub

    Private Sub MainWindow_OnClosed(sender As Object, e As EventArgs)
        browser?.Dispose()
        engine?.Dispose()
    End Sub

    ' #docfragment "DragAndDrop.Implementation
    Private Sub OnDragEnter(arg As EnterDragParameters)
        LogData(arg.Event.DropData, NameOf(OnDragEnter))
        Debug.WriteLine("Data is null? " & (arg.Event.DataObject Is Nothing))
        Dim dataObject As IDataObject = arg.Event.DataObject
        If dataObject IsNot Nothing Then
            'Process data in IDataObject
            ExtractData(NameOf(OnDragEnter), New DataObject(dataObject))
        End If
    End Sub

    Private Sub OnDrop(arg As DropParameters)
        LogData(arg.Event.DropData, NameOf(OnDrop))
        Debug.WriteLine("Data is null? " & (arg.Event.DataObject Is Nothing))
        Dim dataObject As IDataObject = arg.Event.DataObject
        If dataObject IsNot Nothing Then
            ' Process data in IDataObject
            ExtractData(NameOf(OnDrop), New DataObject(dataObject))
        End If
    End Sub
    ' #enddocfragment "DragAndDrop.Implementation
End Class