Imports System.Reflection
Imports System.Text
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Frames.Handlers
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Js

''' <summary>
'''     This example demonstrates how to handle JavaScript name converting.
''' </summary>
Friend Class Program
    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(700, 500)
                Dim htmlBytes() As Byte =
                        Encoding.UTF8.GetBytes(
                            "<html>
                                 <body>
                                    <script type='text/javascript'>
                                        var ShowData = function (a) 
                                        {
                                             document.title = a.fullName 
                                             + ' ' + a.FullYears + '. ' + a.Walk(a.Children[1])
                                             + ' ' + a.Children[1].fullName 
                                             + ' ' + a.Children[1].FullYears;
                                        };
                                    </script>
                                 </body>
                               </html>")
                browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)) _
                    .Wait()
                Dim person = New Person("Jack", 30, True)
                person.Children = New Dictionary(Of Integer, Person)()
                person.Children.Add(1, New Person("Oliver", 10, True))
                Dim value As IJsObject =
                        browser.MainFrame.ExecuteJavaScript (Of IJsObject)("window").Result

                browser.ConvertJsNameHandler =
                    New Handler(Of ConvertJsNameParameters,ConvertJsNameResponse)(
                        AddressOf OnConvertJsName)

                value.Invoke("ShowData", person)

                Console.WriteLine($"{vbTab}Browser title: {browser.Title}")
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Function OnConvertJsName(arg As ConvertJsNameParameters) As ConvertJsNameResponse
        If arg.MemberInfo.Name.Equals("FullName") Then
            Return ConvertJsNameResponse.CamelCase
        End If

        If arg.MemberInfo.MemberType = MemberTypes.Property AndAlso
            arg.MemberInfo.Name.Equals("Age") Then
            Return ConvertJsNameResponse.ConvertTo("FullYears")
        End If

        Return ConvertJsNameResponse.NoConversion
    End Function

    Private Class Person
        Public ReadOnly Property Age As Double

        Public Property Children As IDictionary(Of Integer, Person)
        Public ReadOnly Property FullName As String

        Public ReadOnly Property Gender As Boolean

        Public Sub New(fullName As String, age As Integer, gender As Boolean)
            Me.Gender = gender
            Me.FullName = fullName
            Me.Age = age
        End Sub

        Public Function Walk(withPerson As Person) As String
            Return _
                String.Format("{0} is walking with {1}!", If(Gender, "He", "She"),
                              withPerson.FullName)
        End Function
    End Class
End Class
