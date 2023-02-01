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

Imports System.Text
Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Search
Imports DotNetBrowser.Search.Handlers

''' <summary>
'''     This example demonstrates how to perform text search on the loaded web page.
''' </summary>
Friend Class Program
    ' #docfragment "FindText"
    Private Const Html As String = "<html><body><p>Find me</p><p>Find me</p></body></html>"

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(700, 500)

                Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes(Html)
                browser.Navigation.
                        LoadUrl($"data:text/html;base64,{Convert.ToBase64String(htmlBytes)}").
                        Wait()

                ' Add a timeout to make sure the web page is rendered completely.
                Thread.Sleep(2000)

                ' Find text from the beginning of the loaded web page.
                Dim searchText = "find me"

                Dim intermediateResultsHandler As IHandler(Of FindResultReceivedParameters) =
                        New Handler(Of FindResultReceivedParameters)(
                            AddressOf ProcessSearchResults)
                Console.WriteLine("Find text (1/2)")

                Dim textFinder As ITextFinder = browser.TextFinder
                Dim findResult As FindResult =
                        textFinder.Find(searchText, Nothing, intermediateResultsHandler).
                        Result

                Dim selectedMatch = findResult.SelectedMatch
                Dim count = findResult.NumberOfMatches
                Console.WriteLine($"Find Result: {selectedMatch}/{count}")

                Console.WriteLine("Find text (2/2)")
                findResult = textFinder.
                    Find(searchText, Nothing, intermediateResultsHandler).
                    Result

                selectedMatch = findResult.SelectedMatch
                count = findResult.NumberOfMatches
                Console.WriteLine($"Find Result: {selectedMatch}/{count}")

                textFinder.StopFinding()
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Sub ProcessSearchResults(args As FindResultReceivedParameters)
        Dim result As FindResult = args.FindResult

        If args.IsSearchFinished Then
            Console.WriteLine(
                $"Found: {result.SelectedMatch}/{result.NumberOfMatches}")
        Else
            Console.WriteLine(
                $"Search in progress... Found {result.SelectedMatch}/{result.NumberOfMatches}")
        End If
    End Sub
    ' #enddocfragment "FindText"
End Class
