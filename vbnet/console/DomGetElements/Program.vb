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
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry

''' <summary>
'''     This example demonstrates how to get the collection of the &lt;div&gt; elements
'''     on the web page.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Navigation.LoadUrl("https://www.google.com").Wait()
                Dim document As IDocument = browser.MainFrame.Document
                Dim divs As IEnumerable(Of INode) = document.GetElementsByTagName("div")

                For Each div As INode In divs
                    Dim tempVar As Boolean = TypeOf div Is IElement
                    Dim divElement As IElement = If(tempVar, CType(div, IElement), Nothing)
                    If tempVar Then
                        Dim boundingClientRect As Rectangle = divElement.BoundingClientRect
                        Console.Out.WriteLine(
                            "class = {0};" &
                            " boundingClientRect.Top = {1};" &
                            " boundingClientRect.Left = {2};" &
                            " boundingClientRect.Width = {3};" &
                            " boundingClientRect.Height = {4}",
                            divElement.Attributes("class"), boundingClientRect.Origin.Y,
                            boundingClientRect.Origin.X, boundingClientRect.Size.Width,
                            boundingClientRect.Size.Height)
                    End If
                Next div

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class
