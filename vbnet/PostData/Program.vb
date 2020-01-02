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

Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Net
Imports DotNetBrowser.Net.Handlers

Namespace PostData
    ''' <summary>
    '''     This sample demonstrates how to read and modify POST parameters using BeforeSendUploadDataHandler.
    ''' </summary>
    Friend Class Program
#Region "Methods"

        Public Shared Sub Main()
            Try
                Using engine = EngineFactory.Create((New EngineOptions.Builder()).Build())
                    Console.WriteLine("Engine created")

                    Using browser = engine.CreateBrowser()
                        Console.WriteLine("Browser created")
                        engine.NetworkService.BeforeSendUploadDataHandler = New Handler(Of BeforeSendUploadDataParameters, BeforeSendUploadDataResponse)(AddressOf OnBeforeSendUploadData)

                        Dim parameters = New LoadUrlParameters("https://postman-echo.com/post") With {
                            .PostData = "key=value",
                            .HttpHeaders = {New HttpHeader("Content-Type", "text/plain")}
                        }
                        browser.Navigation.LoadUrl(parameters).Wait()
                        Console.WriteLine(browser.MainFrame.Document.DocumentElement.InnerText)
                    End Using
                End Using
            Catch e As Exception
                Console.WriteLine(e)
            End Try
            Console.WriteLine("Press any key to terminate...")
            Console.ReadKey()
        End Sub

        Public Shared Function OnBeforeSendUploadData(ByVal parameters As BeforeSendUploadDataParameters) As BeforeSendUploadDataResponse
            If "POST" = parameters.UrlRequest.Method Then
                Dim uploadData = parameters.UploadData
                If uploadData.Type = UploadDataType.TextData Then
                    Console.WriteLine($"Text data intercepted: {uploadData.TextData}")
                    Return BeforeSendUploadDataResponse.Override(New FormData From {
                        {"fname", "MyName"},
                        {"lname", "MyLastName"}
                    })
                End If
            End If
            Return BeforeSendUploadDataResponse.Ignore()
        End Function

#End Region
    End Class
End Namespace
