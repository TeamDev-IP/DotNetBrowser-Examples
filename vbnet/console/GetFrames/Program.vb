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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Frames
Imports DotNetBrowser.Geometry

''' <summary>
'''     The example demonstrates how to obtain the hierarchy of frames
'''     on the web page.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(700, 500)
                browser.Navigation.LoadUrl(
                    "https://www.w3schools.com/tags/tryit.asp?filename=tryhtml_frame_cols").Wait()

                PrintFrameHierarhy(browser.MainFrame)
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Public Shared Sub PrintFrameHierarhy(frame As IFrame, Optional ByVal padding As Integer = 0)
        If frame IsNot Nothing Then
            Dim indent As String = String.Empty.PadLeft(padding)
            Console.WriteLine($"{indent}Frame '{frame.Name}'" & (If(frame.IsMain, "(main)", String.Empty)))
            For Each childFrame In frame.Children
                PrintFrameHierarhy(childFrame, padding + 4)
            Next childFrame
        End If
    End Sub

End Class
