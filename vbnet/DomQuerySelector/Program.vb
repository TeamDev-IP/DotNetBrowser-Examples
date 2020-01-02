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
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine

Namespace DomQuerySelector
    Friend Class Program
#Region "Methods"

        Public Shared Sub Main()
            Try
                Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
                    Console.WriteLine("Engine created")

                    Using browser As IBrowser = engine.CreateBrowser()
                        Console.WriteLine("Browser created")

                        browser.MainFrame.LoadHtml("<html><body><div id='root'>" & "<p>paragraph1</p>" & "<p>paragraph2</p>" & "<p>paragraph3</p>" & "</div></body></html>").Wait()
                        Dim document As IDocument = browser.MainFrame.Document
                        Dim documentElement As IElement = document.DocumentElement
                        ' Get the div with id = "root".
                        Dim divRoot As INode = documentElement.GetElementByCssSelector("#root")
                        ' Get all paragraphs.
                        Dim paragraphs As IEnumerable(Of INode) = divRoot.GetElementsByCssSelector("p")

                        For Each paragraph In paragraphs
                            Console.Out.WriteLine("paragraph.InnerText = " & TryCast(paragraph, IElement)?.InnerText)
                        Next paragraph
                    End Using
                End Using
            Catch e As Exception
                Console.WriteLine(e)
            End Try
            Console.WriteLine("Press any key to terminate...")
            Console.ReadKey()
        End Sub

#End Region
    End Class
End Namespace
