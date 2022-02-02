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
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Js
Imports DotNetBrowser.Logging

''' <summary>
'''     This example demonstrates how to inject a .NET object into JavaScript and
'''     invoke its public methods from the JavaScript side.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        LoggerProvider.Instance.Level = SourceLevels.Information
        LoggerProvider.Instance.FileLoggingEnabled = True
        LoggerProvider.Instance.OutputFile = "dnb.log"

        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(700, 500)
                Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<html>
                                 <body>
                                    <script type='text/javascript'>
                                        var ShowData = function (a) 
                                        {
                                             document.title = a.FullName 
                                             + ' ' + a.Age + '. ' + a.Walk(a.Children.get_Item(1))
                                             + ' ' + a.Children.get_Item(1).FullName 
                                             + ' ' + a.Children.get_Item(1).Age;
                                        };
                                    </script>
                                 </body>
                               </html>")
                browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)) _
                .Wait()
                Dim person = New Person("Jack", 30, True)
                person.Children = New Dictionary(Of Double, Person)()
                person.Children.Add(1.0, New Person("Oliver", 10, True))
                Dim value As IJsObject = browser.MainFrame.ExecuteJavaScript (Of IJsObject)("window").Result
                value.Invoke("ShowData", person)

                Console.WriteLine($"{vbTab}Browser title: {browser.Title}")
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Class Person

        Public ReadOnly Property Age As Double

        Public Property Children As IDictionary(Of Double, Person)
        Public ReadOnly Property FullName As String

        Public ReadOnly Property Gender As Boolean

        Public Sub New(fullName As String, age As Integer, gender As Boolean)
            Me.Gender = gender
            Me.FullName = fullName
            Me.Age = age
        End Sub

        Public Function Walk(withPerson As Person) As String
            Return String.Format("{0} is walking with {1}!", If(Gender, "He", "She"), withPerson.FullName)
        End Function

    End Class
End Class
