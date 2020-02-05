#Region "Copyright"

' Copyright © 2020, TeamDev. All rights reserved.
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
Imports DotNetBrowser.Net
Imports DotNetBrowser.Net.Handlers

Namespace NetworkHandlers
    Friend Class Program

#Region "Methods"

        Public Shared Sub Main()
            Try
                Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
                    Console.WriteLine("Engine created")

                    Using browser As IBrowser = engine.CreateBrowser()
                        engine.Network.SendUrlRequestHandler =
                            New Handler(Of SendUrlRequestParameters, SendUrlRequestResponse)(AddressOf OnSendUrlRequest)
                        engine.Network.SendHeadersHandler =
                            New Handler(Of SendHeadersParameters, SendHeadersResponse)(AddressOf OnSendHeaders)

                        Console.WriteLine("Loading http://www.teamdev.com/")
                        browser.Navigation.LoadUrl("http://www.teamdev.com/").Wait()
                        Console.WriteLine($"Loaded URL: {browser.Url}")
                    End Using
                End Using
            Catch e As Exception
                Console.WriteLine(e)
            End Try
            Console.WriteLine("Press any key to terminate...")
            Console.ReadKey()
        End Sub

        Public Shared Function OnSendHeaders(parameters As SendHeadersParameters) As SendHeadersResponse
            ' If navigate to google.com, then print User-Agent header value.
            If parameters.UrlRequest.Url = "http://www.google.com/" Then
                Dim headers As IEnumerable(Of IHttpHeader) = parameters.Headers
                Console.WriteLine(
                    "User-Agent: " &
                    headers.FirstOrDefault(Function(h) h.Name.Equals("User-Agent"))?.Values.FirstOrDefault())
            End If
            Return SendHeadersResponse.Continue()
        End Function

        Public Shared Function OnSendUrlRequest(parameters As SendUrlRequestParameters) As SendUrlRequestResponse
            ' If navigate to teamdev.com, then change URL to google.com.
            If parameters.UrlRequest.Url = "http://www.teamdev.com/" Then
                Console.WriteLine("Redirecting to  http://www.google.com/")
                Return SendUrlRequestResponse.Override("http://www.google.com")
            End If
            Return SendUrlRequestResponse.Continue()
        End Function

#End Region
    End Class
End Namespace
