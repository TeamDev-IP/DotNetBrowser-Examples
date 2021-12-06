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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     The sample demonstrates how to accept/reject SSL certificates using
'''     custom SSL certificate verifier.
''' </summary>
Public Class Program
    Public Shared Sub Main()
        Try
            Using engine As IEngine = EngineFactory.Create(New EngineOptions.Builder().Build())
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    engine.Profiles.Default.Network.VerifyCertificateHandler =
                        New Handler(Of VerifyCertificateParameters, VerifyCertificateResponse)(AddressOf VerifyCert)
                    Dim result As LoadResult = browser.Navigation.LoadUrl("https://google.com").Result
                    ' The certificate for google.com is correct, however, it is rejected in the handler.
                    ' As a result, the navigation fails.
                    Console.WriteLine("Load page result: " & result.ToString())
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try
        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Function VerifyCert(parameters As VerifyCertificateParameters) As VerifyCertificateResponse
        ' Reject SSL certificate for all "google.com" hosts.
        If parameters.HostName.Contains("google.com") Then
            Console.WriteLine("Rejected certificate for " & parameters.HostName)
            Return VerifyCertificateResponse.Invalid()
        End If
        Return VerifyCertificateResponse.Default()
    End Function
End Class