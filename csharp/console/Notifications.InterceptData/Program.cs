#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;
using DotNetBrowser.Permissions;
using DotNetBrowser.Permissions.Handlers;

namespace Notifications.InterceptData
{
    /// <summary>
    ///     This example demonstrates how to intercept web notification data
    ///     by using JS-.NET bridge capabilities.
    /// </summary>
    internal class Program
    {
        private const string InjectedScript = @"function notifyCallback(title, opt) {
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
                                            ";

        public const string DemoUrl = "https://davidwalsh.name/demo/notifications-api.php";
        private static readonly NotificationCallback notificationCallback = new NotificationCallback();

        private static void Main(string[] args)
        {
            using (IEngine engine = EngineFactory.Create())
            {
                // Grant a permission to display notifications
                engine.Profiles.Default.Permissions.RequestPermissionHandler
                    = new Handler<RequestPermissionParameters,
                        RequestPermissionResponse>(OnRequestPermission);

                using (IBrowser browser = engine.CreateBrowser())
                {
                    browser.Size = new Size(640, 480);

                    //Configure JavaScript injection
                    browser.InjectJsHandler = new Handler<InjectJsParameters>(OnInjectJs);
                    //Load web page for testing
                    browser.Navigation.LoadUrl(DemoUrl).Wait();

                    //Create a notification by clicking the button on the web page
                    browser.MainFrame.Document
                           .GetElementByCssSelector(".demo-wrapper > p:nth-child(5) > button:nth-child(1)")
                          ?.Click();

                    Thread.Sleep(5000);
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void OnInjectJs(InjectJsParameters parameters)
        {
            IJsObject window = parameters.Frame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["notificationCallback"] = notificationCallback;

            parameters.Frame.ExecuteJavaScript(InjectedScript);
        }

        /// <summary>
        ///     Grants a permission to display notifications on the website.
        /// </summary>
        private static RequestPermissionResponse OnRequestPermission(RequestPermissionParameters arg)
            => arg.Type == PermissionType.Notifications
                   ? RequestPermissionResponse.Grant()
                   : RequestPermissionResponse.Deny();
    }

    internal class NotificationCallback
    {
        /// <summary>
        ///     Corresponds to the Notification constructor.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="options">The object containing any custom settings that will be applied to the notification.</param>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/API/Notification/Notification" />
        public void NewNotification(string title, IJsObject options)
        {
            Console.WriteLine($"New notification: {title}: {options.Properties["body"]}");
        }
    }
}
