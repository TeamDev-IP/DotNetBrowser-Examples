#Region "Copyright"

' Copyright 2020, TeamDev. All rights reserved.
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
Imports DotNetBrowser.Navigation.Events

''' <summary>
'''     The sample demonstrates how to receive notifications about
'''     web page loading progress.
''' </summary>
Friend Class Program

#Region "Methods"

    Public Shared Sub Main()
        Try
            Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    AddHandler browser.Navigation.FrameLoadFinished,
                        Sub(sender As Object, e As FrameLoadFinishedEventArgs)
                            Console.Out.WriteLine(
                                $"FrameLoadFinished: URL = {e.ValidatedUrl}, IsMainFrame = {e.Frame.IsMain}")
                        End Sub
                    AddHandler browser.Navigation.LoadStarted, Sub()
                        Console.Out.WriteLine("LoadStarted")
                    End Sub
                    AddHandler browser.Navigation.NavigationStarted,
                        Sub(sender As Object, e As NavigationStartedEventArgs)
                            Console.Out.WriteLine($"NavigationStarted: Url = {e.Url}")
                        End Sub
                    AddHandler browser.Navigation.FrameDocumentLoadFinished,
                        Sub(sender As Object, e As FrameDocumentLoadFinishedEventArgs)
                            Console.Out.WriteLine($"FrameDocumentLoadFinished: IsMainFrame = {e.Frame.IsMain}")
                        End Sub
                    browser.Navigation.LoadUrl("http://www.google.com").Wait()
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