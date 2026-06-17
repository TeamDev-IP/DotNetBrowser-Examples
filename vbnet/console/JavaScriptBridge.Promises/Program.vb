#Region "Copyright"

'' Copyright © 2026, TeamDev. All rights reserved.
'' 
'' Redistribution and use in source and/or binary forms, with or without
'' modification, must retain the above copyright notice and the following
'' disclaimer.
'' 
'' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
'' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
'' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
'' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
'' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
'' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
'' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
'' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
'' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
'' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
'' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#End Region

Imports System.Text
Imports System.Threading.Tasks
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
                                    <script type=''text/javascript''>
                                        function CreatePromise(success) 
                                        {
                                             return new Promise(function(resolve, reject) {
                                                if(success) {
                                                    resolve(''Promise fulfilled.'');
                                                }
                                                else {
                                                    reject(''Promise rejected.'');
                                                }
                                             });
                                        };
                                    </script>
                                 </body>
                               </html>")

                browser.Navigation.LoadUrl("data:text/html;base64," & Convert.ToBase64String(htmlBytes)).Wait()
                Dim window As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("window").Result

                '' Prepare promise handlers.
                Dim promiseResolvedHandler As Action(Of Object) = Sub(o) Console.WriteLine($"Success: {o}")
                Dim promiseRejectedHandler As Action(Of Object) = Sub(o) Console.Error.WriteLine($"Error: {o}")

                '' Create a promise that is fulfilled.
                Console.WriteLine("Create a promise that is fulfilled...")
                Dim promise1 = window.Invoke(Of IJsObject)("CreatePromise", True)
                '' Append fulfillment and rejection handlers to the promise.
                promise1.Invoke("then", promiseResolvedHandler, promiseRejectedHandler)

                '' Create a promise that is rejected.
                Console.WriteLine("Create a promise that is rejected...")
                Dim promise2 = window.Invoke(Of IJsObject)("CreatePromise", False)
                '' Append fulfillment and rejection handlers to the promise.
                promise2.Invoke("then", promiseResolvedHandler, promiseRejectedHandler)

                CreatePromiseAsync(window).Wait()
            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    '' #docfragment "JavaScriptBridge.Promises.Async"
    Private Shared Async Function CreatePromiseAsync(ByVal window As IJsObject) As Task
        '' IJsPromise can be integrated with async/await using TaskCompletionSource.

        '' Create a promise that is fulfilled.
        Console.WriteLine($"{vbLf}Create another promise that is fulfilled...")
        Dim tcs As New TaskCompletionSource(Of Object)(TaskCreationOptions.RunContinuationsAsynchronously)
        Dim promise3 As IJsPromise = window.Invoke(Of IJsPromise)("CreatePromise", True)
        promise3.Then(
            Sub(o)
                Console.WriteLine($"Callback:Success: {o}")
                tcs.SetResult(o)
            End Sub,
            Sub(e) tcs.SetException(New Exception(e?.ToString())))
        Dim result = Await tcs.Task
        Console.WriteLine($"Result: {result}")

        '' Create a promise that is rejected.
        Console.WriteLine($"{vbLf}Create another promise that is rejected...")
        Dim tcs2 As New TaskCompletionSource(Of Object)(TaskCreationOptions.RunContinuationsAsynchronously)
        Dim promise4 As IJsPromise = window.Invoke(Of IJsPromise)("CreatePromise", False)
        promise4.Then(
            Sub(o) tcs2.SetResult(o),
            Sub(e)
                Console.WriteLine($"Callback:Error: {e}")
                tcs2.SetResult(Nothing)
            End Sub)
        Await tcs2.Task
    End Function
    '' #enddocfragment "JavaScriptBridge.Promises.Async"

End Class