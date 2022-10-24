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
Imports JavaScriptBridge.Promises

''' <summary>
'''     This example demonstrates how to work with JavaScript Promises
'''     via JS-.NET bridge.
''' </summary>
Friend Class Program

    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                browser.Size = New Size(700, 500)
                Dim htmlBytes() As Byte = Encoding.UTF8.GetBytes("<html>
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
                               </html>")

                browser.Navigation.LoadUrl("data:text/html;base64," + Convert.ToBase64String(htmlBytes)).Wait()
                Dim window As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("window").Result
                'Prepare promise handlers
                Dim promiseResolvedHandler As Action(Of Object) = Sub(o) Console.WriteLine(
                    $"Success: {o}")
                Dim promiseRejectedHandler As Action(Of Object) = Sub(o) Console.Error.WriteLine(
                    $"Error: {o}")

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

                CreatePromiseAsync(window).Wait()
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Async Function CreatePromiseAsync(ByVal window As IJsObject) As Task
        'It is also possible to create a wrapper class for IJsObject that simplifies appending the
        'handlers and type checks. Such approach can be used to integrate JavaScript promises
        'with async/await in the .NET application.

        'Create a promise that is fulfilled and wrap this promise
        Console.WriteLine($"{vbLf}Create another promise that is fulfilled...")
        Dim promise3 As JsPromise = window.Invoke(Of IJsObject)("CreatePromise", True).AsPromise()
        Dim result As JsPromise.Result = Await promise3.Then(Function(o)
            Console.WriteLine($"Callback:Success: {o}")
            Return o
        End Function).ResolveAsync()
        Console.WriteLine($"Result state:{result?.State}")
        Console.WriteLine($"Result type:{(If(result?.Data?.GetType().ToString(), "null"))}")

        'Create a promise that is rejected and wrap this promise
        Console.WriteLine($"{vbLf}Create another promise that is rejected...")
        Dim promise4 As JsPromise = window.Invoke(Of IJsObject)("CreatePromise", False).AsPromise()
        result = Await promise4.ResolveAsync()

        Console.WriteLine($"Result state:{result?.State}")
        Console.WriteLine($"Result type:{(If(result?.Data?.GetType().ToString(), "null"))}")
    End Function

End Class
