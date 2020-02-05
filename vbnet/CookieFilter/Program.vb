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
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     The sample demonstrates how to suppress/filter incoming and outgoing cookies.
''' </summary>
Friend Class Program

#Region "Methods"

    Public Shared Sub Main()
        Try
            Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    engine.Network.CanGetCookiesHandler =
                        New Handler(Of CanGetCookiesParameters, CanGetCookiesResponse)(AddressOf CanGetCookies)
                    engine.Network.CanSetCookieHandler =
                        New Handler(Of CanSetCookieParameters, CanSetCookieResponse)(AddressOf CanSetCookie)
                    Dim result As LoadResult = browser.Navigation.LoadUrl("http://google.com").Result
                    Console.WriteLine("LoadResult: " & result)
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try
        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Function CanGetCookies(arg As CanGetCookiesParameters) As CanGetCookiesResponse
        Dim cookies As String = arg.Cookies.Aggregate(String.Empty,
                                                      Function(current, cookie) current + (cookie.ToString() & vbLf))
        Console.WriteLine("CanGetCookies: " & cookies)
        Return CanGetCookiesResponse.Deny()
    End Function

    Private Shared Function CanSetCookie(arg As CanSetCookieParameters) As CanSetCookieResponse
        Console.WriteLine("CanSetCookie: " & arg.Cookie.ToString())
        Return CanSetCookieResponse.Deny()
    End Function

#End Region
End Class