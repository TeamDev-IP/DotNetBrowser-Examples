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

Imports System.IO
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

''' <summary>
'''     The sample demonstrates how to create several Chromium engines.
''' </summary>
Friend Class Program

    Public Shared Sub Main(args() As String)
        Try
            Dim userDataDir1 As String = Path.GetFullPath("user-data-dir-one")
            Directory.CreateDirectory(userDataDir1)
            Dim engine1 As IEngine =
                    EngineFactory.Create(New EngineOptions.Builder With {.UserDataDirectory = userDataDir1}.Build())
            Console.WriteLine("Engine1 created")

            Dim userDataDir2 As String = Path.GetFullPath("user-data-dir-two")
            Directory.CreateDirectory(userDataDir2)
            Dim engine2 As IEngine =
                    EngineFactory.Create(New EngineOptions.Builder With {.UserDataDirectory = userDataDir2}.Build())
            Console.WriteLine("Engine2 created")

            ' This Browser instance will store cookies and user data files in "user-data-dir-one" dir.
            Dim browser1 As IBrowser = engine1.CreateBrowser()
            Console.WriteLine("browser1 created")

            ' This Browser instance will store cookies and user data files in "user-data-dir-two" dir.
            Dim browser2 As IBrowser = engine2.CreateBrowser()
            Console.WriteLine("browser2 created")

            ' The browser1 and browser2 instances will not see the cookies and cache data files of each other.

            engine2.Dispose()
            engine1.Dispose()
        Catch e As Exception
            Console.WriteLine(e)
        End Try

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

End Class
