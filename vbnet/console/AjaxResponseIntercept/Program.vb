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

Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Frames
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Net.Events
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     This example demonstrates how to intercept the response data
'''     for the AJAX requests.
''' </summary>
Friend Class Program
    Private Shared ReadOnly AjaxRequests As New Dictionary(Of String, HttpRequest)()

    Public Shared Sub Main(ByVal args() As String)
        Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
            Using browser As IBrowser = engine.CreateBrowser()

                engine.Profiles.Default.Network.SendUrlRequestHandler = New Handler(Of SendUrlRequestParameters, SendUrlRequestResponse)(AddressOf OnSendUrlRequest)
                AddHandler engine.Profiles.Default.Network.ResponseBytesReceived, AddressOf OnResponseBytesReceived
                AddHandler engine.Profiles.Default.Network.RequestCompleted, AddressOf OnRequestCompleted

                browser.Navigation.LoadUrl("https://www.w3schools.com/xml/tryit.asp?filename=tryajax_first").Wait()

                Dim demoFrame As IFrame = browser.AllFrames.FirstOrDefault(Function(f) f.Document.GetElementById("demo") IsNot Nothing)

                If demoFrame IsNot Nothing Then
                    'Click the button in the demo frame to make an AJAX request.
                    Console.WriteLine("Demo frame found")
                    demoFrame.Document.GetElementByTagName("button").Click()
                End If

                'Wait for 15 seconds to be sure that at least some requests are completed.
                Thread.Sleep(15000)

                ' The dictionary will contain some requests, including the one we sent by clicking the button.
                Dim key As String = AjaxRequests.Keys.FirstOrDefault(Function(k) k.Contains("ajax_info.txt"))
                If Not String.IsNullOrEmpty(key) Then
                    Dim ajaxRequest As HttpRequest = AjaxRequests(key)
                    Console.WriteLine($"Response intercepted: " & vbLf & ajaxRequest.Response)
                End If

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Sub OnRequestCompleted(ByVal s As Object, ByVal e As RequestCompletedEventArgs)
        'Here, we mark the requests as completed for the previously filtered URLs.
        Dim url As String = e.UrlRequest.Url
        If AjaxRequests.ContainsKey(url) Then
            AjaxRequests(url).Complete()
        End If
    End Sub

    Private Shared Sub OnResponseBytesReceived(ByVal s As Object, ByVal e As ResponseBytesReceivedEventArgs)
        'Here, we collect the response data for the previously filtered URLs.
        Dim url As String = e.UrlRequest.Url
        If AjaxRequests.ContainsKey(url) Then
            Dim httpRequest As HttpRequest = AjaxRequests(url)
            If httpRequest.MimeType Is Nothing Then
                httpRequest.MimeType = e.MimeType
            End If

            httpRequest.AppendResponseBytes(e.Data)
        End If
    End Sub

    Private Shared Function OnSendUrlRequest(ByVal arg As SendUrlRequestParameters) As SendUrlRequestResponse
        ' Here, we check the URL request and decide if we want to intercept a response for it.
        ' For instance, we can check the resource type. This works for AJAX requests.
        ' We can also check the arg.UrlRequest.Url to determine if this is a URL we need.
        If arg.UrlRequest.ResourceType = ResourceType.Xhr Then
            AjaxRequests(arg.UrlRequest.Url) = New HttpRequest(arg.UrlRequest.Url, arg.UrlRequest.Method)
        End If

        Return SendUrlRequestResponse.Continue()
    End Function
End Class
