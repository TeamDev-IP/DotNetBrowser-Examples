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

Imports System.Text
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Navigation
Imports DotNetBrowser.Net
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     This example demonstrates how to intercept all URL requests and handle a custom protocol.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Try
            Dim interceptRequestHandler =
                    New Handler(Of InterceptRequestParameters, InterceptRequestResponse)(Function(p)
                        Dim options = New UrlRequestJobOptions With {
                                .Headers = New List(Of HttpHeader) From {
                                New HttpHeader("Content-Type", "text/html", "charset=utf-8")
                                }
                                }
                        Dim job As UrlRequestJob = p.Network.CreateUrlRequestJob(p.UrlRequest, options)
                        Task.Run(Sub()
                            ' The request processing is performed in a background thread
                            ' in order to avoid freezing the web page.
                            job.Write(Encoding.UTF8.GetBytes("Hello world!"))
                            job.Complete()
                        End Sub)
                        Return InterceptRequestResponse.Intercept(job)
                    End Function)


            Dim engineOptionsBuilder = new EngineOptions.Builder
            With engineOptionsBuilder
                .Schemes.Add(Scheme.Create("myscheme"), interceptRequestHandler)
            End With

            Dim engineOptions = engineOptionsBuilder.Build()
            Using engine As IEngine = EngineFactory.Create(engineOptions)
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    Dim loadResult As LoadResult = browser.Navigation.LoadUrl("myscheme://test1").Result
                    Console.WriteLine("Load result: " & loadResult.ToString())
                    Console.WriteLine("HTML: " & browser.MainFrame.Html)
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try
        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class