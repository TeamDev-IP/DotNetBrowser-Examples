#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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

using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Dialogs;
using DotNetBrowser.Browser.Dialogs.Handlers;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Downloads.Handlers;
using DotNetBrowser.Handlers;
using DotNetBrowser.Navigation;
using DotNetBrowser.Navigation.Handlers;
using DotNetBrowser.Net;
using DotNetBrowser.Net.Handlers;
using DotNetBrowser.Permissions.Handlers;

namespace GuardedBrowser
{
    /// <summary>
    ///     Wires the <see cref="BrowserPolicy" /> into DotNetBrowser handlers.
    ///     These limits hold whatever the model or the page tries.
    /// </summary>
    public sealed class PolicyEnforcer
    {
        private readonly IBrowser browser;
        private readonly BrowserPolicy policy;
        private readonly InputGate gate;
        private readonly SubmitGuard submitGuard;
        private readonly AuditLog log;

        public PolicyEnforcer(IBrowser browser, BrowserPolicy policy, InputGate gate,
                              SubmitGuard submitGuard, AuditLog log)
        {
            this.browser = browser;
            this.policy = policy;
            this.gate = gate;
            this.submitGuard = submitGuard;
            this.log = log;
        }

        public void Attach()
        {
            INavigation navigation = browser.Navigation;

            // Site allowlist, first layer: the navigation never starts.
            navigation.StartNavigationHandler =
                new Handler<StartNavigationParameters, StartNavigationResponse>(p =>
                {
                    if (!policy.CanRead(p.Url))
                    {
                        Block("navigation", p.Url);
                        if (p.IsMainFrame)
                        {
                            ShowBlockedPage(p.Url);
                        }
                        return StartNavigationResponse.Ignore();
                    }
                    if (p.IsPost && !gate.HumanInControl && !submitGuard.AllowNext)
                    {
                        Block("form POST without approval", p.Url);
                        return StartNavigationResponse.Ignore();
                    }
                    return StartNavigationResponse.Start();
                });

            // Site allowlist, second layer: every request the page makes, such as
            // frames, scripts, images, and XHR.
            browser.Profile.Network.SendUrlRequestHandler =
                new Handler<SendUrlRequestParameters, SendUrlRequestResponse>(p =>
                {
                    if (policy.CanRead(p.UrlRequest.Url))
                    {
                        return SendUrlRequestResponse.Continue();
                    }
                    Block($"{p.UrlRequest.ResourceType} request", p.UrlRequest.Url);
                    return SendUrlRequestResponse.Cancel();
                });

            // A cancelled main-frame request would show Chromium's error page.
            // Other network errors on allowed sites keep it.
            navigation.ShowNetErrorPageHandler =
                new Handler<ShowNetErrorPageParameters, ShowNetErrorPageResponse>(p =>
                {
                    if (!policy.CanRead(p.Url))
                    {
                        ShowBlockedPage(p.Url);
                    }
                    return ShowNetErrorPageResponse.ShowDefault();
                });

            // Closed escape hatches.
            browser.StartDownloadHandler =
                new Handler<StartDownloadParameters, StartDownloadResponse>(p =>
                {
                    Block("download", p.Download.Info.Url);
                    return StartDownloadResponse.Cancel();
                });
            browser.CreatePopupHandler =
                new Handler<CreatePopupParameters, CreatePopupResponse>(p =>
                {
                    Block("popup", p.TargetUrl);
                    return CreatePopupResponse.Suppress();
                });
            browser.RequestPrintHandler =
                new Handler<RequestPrintParameters, RequestPrintResponse>(_ =>
                {
                    Block("print", browser.Url);
                    return RequestPrintResponse.Cancel();
                });
            browser.Profile.Permissions.RequestPermissionHandler =
                new Handler<RequestPermissionParameters, RequestPermissionResponse>(p =>
                {
                    Block($"permission {p.Type}", p.Url);
                    return RequestPermissionResponse.Deny();
                });

            IDialogs dialogs = browser.Dialogs;
            dialogs.OpenFileHandler =
                new Handler<OpenFileParameters, OpenFileResponse>(_ =>
                {
                    Block("file dialog", browser.Url);
                    return OpenFileResponse.Cancel();
                });
            dialogs.OpenMultipleFilesHandler =
                new Handler<OpenMultipleFilesParameters, OpenMultipleFilesResponse>(_ =>
                {
                    Block("file dialog", browser.Url);
                    return OpenMultipleFilesResponse.Cancel();
                });
            dialogs.OpenDirectoryHandler =
                new Handler<OpenDirectoryParameters, OpenDirectoryResponse>(_ =>
                {
                    Block("directory dialog", browser.Url);
                    return OpenDirectoryResponse.Cancel();
                });
            dialogs.SaveFileHandler =
                new Handler<SaveFileParameters, SaveFileResponse>(_ =>
                {
                    Block("save dialog", browser.Url);
                    return SaveFileResponse.Cancel();
                });
            dialogs.OpenExternalAppHandler =
                new Handler<OpenExternalAppParameters, OpenExternalAppResponse>(p =>
                {
                    Block("external app", p.Url);
                    return OpenExternalAppResponse.Cancel();
                });

            IJsDialogs jsDialogs = browser.JsDialogs;
            jsDialogs.AlertHandler =
                new Handler<AlertParameters>(p => Block("alert", p.Message));
            jsDialogs.ConfirmHandler =
                new Handler<ConfirmParameters, ConfirmResponse>(p =>
                {
                    Block("confirm dialog", p.Message);
                    return ConfirmResponse.Cancel();
                });
            jsDialogs.PromptHandler =
                new Handler<PromptParameters, PromptResponse>(p =>
                {
                    Block("prompt dialog", p.Message);
                    return PromptResponse.Cancel();
                });
            jsDialogs.BeforeUnloadHandler =
                new Handler<BeforeUnloadParameters, BeforeUnloadResponse>(_ =>
                    BeforeUnloadResponse.Leave());
        }

        private void Block(string what, string detail) =>
            log.Record("blocked", $"{what}: {detail}");

        private void ShowBlockedPage(string blockedUrl)
        {
            // A blocked page outside the allowlist would be blocked in turn.
            if (policy.BlockedPageUrl == null || !policy.CanRead(policy.BlockedPageUrl))
            {
                return;
            }
            // Handlers run synchronously, so the navigation is started elsewhere;
            // the handler returns its response before this navigation begins.
            string url = policy.BlockedPageUrl + "?url="
                         + System.Uri.EscapeDataString(blockedUrl);
            Task.Run(() => browser.Navigation.LoadUrl(url));
        }
    }
}
