#Region "Copyright"

' Copyright 2021, TeamDev. All rights reserved.
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

Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Js
Imports DotNetBrowser.Permissions
Imports DotNetBrowser.Permissions.Handlers

''' <summary>
'''     This example demonstrates how to intercept web notification data
'''     by using JS-.NET bridge capabilities.
''' </summary>
Friend Class Program
    Private Const InjectedScript As String = "function notifyCallback(title, opt) {
                                                notificationCallback.NewNotification(title, opt);
                                            }

                                            const handler = {
                                                construct(target, args) {
                                                    notifyCallback(...args);
                                                    return new target(...args);
                                                }
                                            };

                                            const ProxifiedNotification= new Proxy(Notification, handler);

                                            window.Notification = ProxifiedNotification;
                                            "

    Public Const DemoUrl As String = "https://davidwalsh.name/demo/notifications-api.php"
    Private Shared ReadOnly notificationCallback As New NotificationCallback()

#Region "Methods"

    Public Shared Sub Main(ByVal args() As String)
        Try
            Using engine As IEngine = EngineFactory.Create()
                Console.WriteLine("Engine created")
                ' Grant a permission to display notifications
                engine.Profiles.Default.Permissions.RequestPermissionHandler = New Handler(Of RequestPermissionParameters, RequestPermissionResponse)(AddressOf OnRequestPermission)
                Using browser As IBrowser = engine.CreateBrowser()
                    Console.WriteLine("Browser created")
                    browser.Size = New Size(640, 480)

                    'Configure JavaScript injection
                    browser.InjectJsHandler = New Handler(Of InjectJsParameters)(AddressOf OnInjectJs)
                    'Load web page for testing
                    browser.Navigation.LoadUrl(DemoUrl).Wait()

                    'Create a notification by clicking the button on the web page
                    browser.MainFrame.Document.GetElementByCssSelector(".demo-wrapper > p:nth-child(5) > button:nth-child(1)")? .Click()
                    Thread.Sleep(5000)
                End Using
            End Using
        Catch e As Exception
            Console.WriteLine(e)
        End Try

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Sub OnInjectJs(ByVal parameters As InjectJsParameters)
        Dim window As IJsObject = parameters.Frame.ExecuteJavaScript(Of IJsObject)("window").Result
        window.Properties("notificationCallback") = notificationCallback

        parameters.Frame.ExecuteJavaScript(InjectedScript)
    End Sub

    ''' <summary>
    '''     Grants a permission to display notifications on the website.
    ''' </summary>
    Private Shared Function OnRequestPermission(ByVal arg As RequestPermissionParameters) As RequestPermissionResponse
        Return If(arg.Type = PermissionType.Notifications, RequestPermissionResponse.Grant(), RequestPermissionResponse.Deny())
    End Function

#End Region
End Class

Friend Class NotificationCallback
#Region "Methods"

    ''' <summary>
    '''     Corresponds to the Notification constructor.
    ''' </summary>
    ''' <param name="title">The notification title.</param>
    ''' <param name="options">The object containing any custom settings that will be applied to the notification.</param>
    ''' <seealso cref="https://developer.mozilla.org/en-US/docs/Web/API/Notification/Notification"/>
    Public Sub NewNotification(ByVal title As String, ByVal options As IJsObject)
        Console.WriteLine($"New notification: {title}: {options.Properties("body")}")
    End Sub

#End Region
End Class