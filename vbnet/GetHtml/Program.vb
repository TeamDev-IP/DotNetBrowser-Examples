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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry

''' <summary>
'''     This example demonstrates how to obtain HTML code of the loaded web page.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Try
            Using engine As IEngine = EngineFactory.Create()
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    browser.Size = New Size(700, 500)
                    browser.Navigation.LoadUrl("https://www.teamdev.com").Wait()
                    ' Get HTML of the loaded web page and write it to Console.
                    Console.WriteLine(browser.MainFrame.Html)
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try
        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class
