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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Dom.XPath
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry

''' <summary>
'''     The sample demonstrates how to evaluate an XPath expression and work
'''     with the evaluation result.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(1024, 768)

                browser.Navigation.LoadUrl("https://html5test.teamdev.com").Wait()
                Dim document As IDocument = browser.MainFrame.Document

                Try
                    Dim expression = "count(//div)"
                    Console.WriteLine($"Evaluating '{expression}'")
                    Dim result As IXPathResult = document.Evaluate(expression)

                    ' Make sure that result is a number.
                    If result.Type = XPathResultType.Number Then
                        Console.WriteLine($"Result: {result.Numeric}")
                    End If

                ' If the expression is not a valid XPath expression or the document
                ' element is not available, we'll get an error.
                Catch e As XPathException
                    Console.WriteLine("Error message: " + e.Message)
                    Return
                End Try
                
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub
End Class
