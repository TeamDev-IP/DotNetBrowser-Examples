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
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Js

''' <summary>
'''     This example demonstrates how to work with JavaScript Promises
'''     via JS-.NET bridge.
''' </summary>
Friend Class Program
#Region "Methods"

    Public Shared Sub Main()
        Try
            Using engine As IEngine = EngineFactory.Create()
                Console.WriteLine("Engine created")

                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    browser.Size = New Size(700, 500)
                    browser.MainFrame.LoadHtml("<html>
                                     <body>
                                        <script type='text/javascript'>
                                            function CreatePromise(success) 
                                            {
                                                 return new Promise(function(resolve, reject) {
                                                    if(success) {
                                                        resolve('Promise fulfilled.');
                                                    }
                                                    else {
                                                        reject('Promise rejected.');
                                                    }
                                                 });
                                            };
                                        </script>
                                     </body>
                                   </html>").Wait()
                    Dim window As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("window").Result
                    'Prepare promise handlers
                    Dim promiseResolvedHandler As Action(Of Object) = Sub(o) Console.WriteLine("Success: " & o.ToString())
                    Dim promiseRejectedHandler As Action(Of Object) = Sub(o) Console.Error.WriteLine("Error: " & o.ToString())

                    'Create a promise that is fulfilled
                    Console.WriteLine("Create a promise that is fulfilled...")
                    Dim promise1 = window.Invoke(Of IJsObject)("CreatePromise", True)
                    'Append fulfillment and rejection handlers to the promise
                    promise1.Invoke("then", promiseResolvedHandler, promiseRejectedHandler)

                    'Create a promise that is rejected
                    Console.WriteLine("Create a promise that is rejected...")
                    Dim promise2 = window.Invoke(Of IJsObject)("CreatePromise", False)
                    'Append fulfillment and rejection handlers to the promise
                    promise2.Invoke("then", promiseResolvedHandler, promiseRejectedHandler)
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