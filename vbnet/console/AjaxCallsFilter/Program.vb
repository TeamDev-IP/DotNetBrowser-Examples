#Region "Copyright"

' Copyright © 2023, TeamDev. All rights reserved.
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
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     The sample demonstrates how to suppress Ajax calls by registering the LoadResourceHandler.
''' </summary>
Friend Class Program
    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                engine.Profiles.Default.Network.SendUrlRequestHandler =
                    New Handler(Of SendUrlRequestParameters, SendUrlRequestResponse)(AddressOf CanLoadResource)

                browser.Navigation.LoadUrl("https://www.w3schools.com/xml/tryit.asp?filename=tryajax_first").Wait()

                Dim demoFrame As IFrame =
                        browser.AllFrames.FirstOrDefault(
                            Function(f) f.Document.GetElementById("demo") IsNot Nothing)

                If demoFrame IsNot Nothing Then
                    Console.WriteLine("Demo frame found")
                    demoFrame.Document.GetElementByTagName("button").Click()
                End If

                Thread.Sleep(5000)
                Console.WriteLine("Demo HTML: " & demoFrame?.Document.GetElementById("demo").InnerHtml)

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Function CanLoadResource(arg As SendUrlRequestParameters) As SendUrlRequestResponse
        If arg.UrlRequest.ResourceType = ResourceType.Xhr Then
            Console.WriteLine("Suppress ajax call - " & arg.UrlRequest.Url)
            Return SendUrlRequestResponse.Cancel()
        End If
        Return SendUrlRequestResponse.Continue()
    End Function
End Class
