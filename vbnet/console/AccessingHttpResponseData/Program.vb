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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Net
Imports DotNetBrowser.Net.Events

''' <summary>
'''     The sample demonstrates how to access HTTP response data.
''' </summary>
Friend Class Program
    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                AddHandler engine.Profiles.Default.Network.ResponseBytesReceived, AddressOf OnResponseBytesReceived
                browser.Navigation.LoadUrl("https://teamdev.com").Wait()
                Console.WriteLine("URL loaded")

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub


    Private Shared Sub OnResponseBytesReceived(sender As Object, eventArgs As ResponseBytesReceivedEventArgs)
        If eventArgs.MimeType.Equals(MimeType.TextHtml) Then

            Console.WriteLine($"MimeType = {eventArgs.MimeType}")
            Console.WriteLine($"The HTTP method = {eventArgs.UrlRequest.Method}")

            If eventArgs.Data IsNot Nothing Then
                Dim data As String = eventArgs.Data.Aggregate(Of String)(Nothing, Function(current, t) current + ChrW(t))
                Console.WriteLine($"Data = {data}" & vbLf)
            End If

        End If
    End Sub
End Class
