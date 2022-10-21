#Region "Copyright"

' Copyright © 2022, TeamDev. All rights reserved.
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
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Dom.Events
Imports DotNetBrowser.Engine

''' <summary>
'''     This example demonstrates how to create and dispatch a DOM event.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<html><body><div id='root'></div></body></html>")
                browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait()
                Dim document As IDocument = browser.MainFrame.Document

                Dim eventType As New EventType("MyEvent")
                Dim myEvent = document.CreateEvent(eventType, (New EventParameters.Builder()).Build())

                Dim root As INode = document.GetElementById("root")

                Dim domEventHandler As EventHandler(Of DomEventArgs) = Sub(s, e)
                    If e.Event.Type Is eventType Then
                        Console.WriteLine($"DOM event received: {eventType.Value}")
                        Dim textNode As INode = document.CreateTextNode("Some text")
                        Dim paragraph As IElement = document.CreateElement("p")
                        paragraph.Children.Append(textNode)
                        root.Children.Append(paragraph)
                    End If
                End Sub

                AddHandler root.Events(eventType).EventReceived, domEventHandler
                Console.WriteLine($"Dispatch custom DOM event: {eventType.Value}")
                root.DispatchEvent(myEvent)
                Thread.Sleep(3000)
                Console.WriteLine($"Updated HTML: {browser.MainFrame.Html}")

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class
