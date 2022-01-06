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
Imports System.Diagnostics
Imports System.IO
Imports System.Net.Http
Imports System.Reflection
Imports System.Threading.Tasks

''' <summary>
'''     The abstract class for implementing the binaries resolver.
''' </summary>
Public MustInherit Class BinariesResolverBase
    Private ReadOnly client As HttpClient

    Protected ReadOnly Property RequestUri() As String

    ''' <summary>
    '''     Occurs when the status update messages are sent by the implementation.
    '''     These messages can be used to update the application UI and indicate the progress.
    ''' </summary>
    Public Event StatusUpdated As EventHandler(Of BinariesResolverStatusEventArgs)

    Protected Sub New(ByVal uri As String, Optional ByVal domain As AppDomain = Nothing)
        If domain Is Nothing Then
            domain = AppDomain.CurrentDomain
        End If

        Me.RequestUri = uri
        client = New HttpClient()
        AddHandler domain.AssemblyResolve, AddressOf Resolve
    End Sub

    ''' <summary>
    '''     The AssemblyResolve event handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <returns></returns>
    Public Function Resolve(ByVal sender As Object, ByVal args As ResolveEventArgs) As System.Reflection.Assembly
        Return If(args.Name.StartsWith("DotNetBrowser.Chromium"), Resolve(args.Name).Result, Nothing)
    End Function

    Protected Sub OnStatusUpdated(ByVal message As String, Optional ByVal completed As Boolean = False)
        StatusUpdatedEvent?.Invoke(Me, New BinariesResolverStatusEventArgs(message, completed))
    End Sub

    Protected MustOverride Function PrepareRequest(ByVal assemblyName As AssemblyName) As String
    Protected MustOverride Function ProcessResponse(ByVal responseBody As Stream, ByVal assemblyName As AssemblyName) As System.Reflection.Assembly

    Private Async Function Resolve(ByVal binariesAssemblyName As String) As Task(Of System.Reflection.Assembly)
        'Note: assemblies are usually resolved in the background thread of the UI application.
        Try
            'Construct a request using the fully-qualified assembly name.
            Dim assemblyName As New AssemblyName(binariesAssemblyName)
            Dim request As String = PrepareRequest(assemblyName)

            'Perform the request and download the response.
            OnStatusUpdated("Downloading Chromium binaries...")
            Debug.WriteLine($"Downloading {request}")
            Dim response As HttpResponseMessage = Await client.GetAsync(request)

            response.EnsureSuccessStatusCode()
            OnStatusUpdated("Chromium binaries package downloaded")
            Dim responseBody As Stream = Await response.Content.ReadAsStreamAsync()

            'Process the response bytes and load the assembly.
            Return ProcessResponse(responseBody, assemblyName)
        Catch e As Exception
            Debug.WriteLine("Exception caught: {0} ", e)
        End Try

        Return Nothing
    End Function

End Class
