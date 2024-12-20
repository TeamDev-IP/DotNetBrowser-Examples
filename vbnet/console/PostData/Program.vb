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

Imports System.Collections.ObjectModel
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Net
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     This sample demonstrates how to read and modify POST parameters using SendUploadDataHandler.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine = EngineFactory.Create()
            Using browser = engine.CreateBrowser()

                engine.Profiles.Default.Network.SendUploadDataHandler =
                    New Handler(Of SendUploadDataParameters, SendUploadDataResponse)(AddressOf OnSendUploadData)

                Dim parameters = New LoadUrlParameters("https://postman-echo.com/post") With {
                        .UploadData = New TextData("key=value"),
                        .HttpHeaders = {New HttpHeader("Content-Type", "text/plain")}
                        }

                browser.Navigation.LoadUrl(parameters).Wait()
                Console.WriteLine(browser.MainFrame.Document.DocumentElement.InnerText)
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Public Shared Function OnSendUploadData(parameters As SendUploadDataParameters) As SendUploadDataResponse
        If "POST" = parameters.UrlRequest.Method Then
            Dim uploadData = parameters.UploadData
            Dim textData = TryCast(uploadData, TextData)

            If textData IsNot Nothing Then
                Console.WriteLine($"Text data intercepted: {textData.Data}")
                Return _
                    SendUploadDataResponse.Override(
                        New FormData(
                            New ReadOnlyCollection(Of KeyValuePair(Of String, String))(
                                New List(Of KeyValuePair(Of String, String))() From {
                                      New KeyValuePair (Of String, String)("fname", "MyName"),
                                      New KeyValuePair (Of String, String)("lname", "MyLastName")
                                      })))
            End If
        End If
        Return SendUploadDataResponse.Continue()
    End Function

End Class
