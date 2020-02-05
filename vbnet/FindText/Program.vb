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

Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Search
Imports DotNetBrowser.Search.Handlers

Friend Class Program

#Region "Methods"

    Public Shared Sub Main()
        Try
            Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    browser.Size = New Size(700, 500)
                    browser.MainFrame.LoadHtml("<html><body><p>Find me</p><p>Find me</p></body></html>").Wait()

                    Thread.Sleep(2000)
                    ' Find text from the beginning of the loaded web page.
                    Dim searchText = "find me"
                    Dim requestId = 0
                    Dim intermediateResultsHandler As IHandler(Of FindResultReceivedParameters) =
                            New Handler(Of FindResultReceivedParameters)(AddressOf ProcessSearchResults)
                    Console.WriteLine("Find text (1/2)")
                    Dim findResult As FindResult =
                            browser.TextFinder.Find(searchText, Nothing, intermediateResultsHandler).Result
                    Console.Out.WriteLine($"Find Result: {findResult.SelectedMatch}/{findResult.NumberOfMatches}")
                    Console.WriteLine("Find text (2/2)")
                    findResult = browser.TextFinder.Find(searchText, Nothing, intermediateResultsHandler).Result
                    Console.Out.WriteLine($"Find Result: {findResult.SelectedMatch}/{findResult.NumberOfMatches}")
                    browser.TextFinder.StopFinding()
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try
        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Sub ProcessSearchResults(args As FindResultReceivedParameters)
        Dim result As FindResult = args.FindResult

        If args.IsSearchFinished Then
            Console.Out.WriteLine("Found: " & result.SelectedMatch & "/" & result.NumberOfMatches)
        Else
            Console.Out.WriteLine(
                "Search in progress... Found " & result.SelectedMatch & "/" & result.NumberOfMatches)
        End If
    End Sub

#End Region
End Class