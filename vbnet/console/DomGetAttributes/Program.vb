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

Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine

''' <summary>
'''     This example demonstrates how to get the list of existing attributes of a specified HTML element.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes(
                    "<html><body><a href='#' id='link' title='link title'></a></body></html>")
                browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait()

                Dim document As IDocument = browser.MainFrame.Document
                Dim link As IElement = document.GetElementById("link")
                Dim attributes As IDictionary(Of String, String) = link.Attributes

                Console.WriteLine("Link attributes: ")
                For Each attribute In attributes
                    Console.WriteLine($"- {attribute.Key} = {attribute.Value}")
                Next attribute

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class
