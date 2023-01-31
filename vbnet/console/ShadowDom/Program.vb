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

Imports System
Imports System.IO
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Js

''' <summary>
'''     This example demonstrates how to work with Shadow DOM.
''' </summary>
Friend Class Program

    Public Shared Sub Main(ByVal args() As String)
        Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(1024, 768)
                browser.Navigation.LoadUrl(Path.GetFullPath("example.html")).Wait()

                Console.WriteLine("URL loaded")

                Dim document As IDocument = browser.MainFrame.Document
                Dim container As IJsObject = TryCast(document.GetElementById("container"), IJsObject)

                'Create shadow root.
                Dim shadowRoot As INode = container?.Invoke(Of INode)("attachShadow", browser.MainFrame.ParseJsonString("{""mode"": ""open""}"))
                Console.WriteLine($"Shadow root created: {(shadowRoot IsNot Nothing)}")

                'Fetch shadow root.
                shadowRoot = TryCast(container?.Properties("shadowRoot"), INode)
                Console.WriteLine($"Shadow root fetched: {(shadowRoot IsNot Nothing)}")

                'Add node to shadow root.
                Dim inside As IElement = document.CreateElement("h1")
                inside.InnerText = "Inside Shadow DOM"
                inside.Attributes("id") = "inside"
                shadowRoot?.Children.Append(inside)

                'Find new node in shadow root.
                Dim element As IElement = shadowRoot?.GetElementById("inside")
                Console.WriteLine($"Inside element inner text: {element?.InnerText}")

                'Try finding the same node from the main document.
                element = document.GetElementById("inside")
                Console.WriteLine($"Inside element found in the document: {(element IsNot Nothing)}")
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class
