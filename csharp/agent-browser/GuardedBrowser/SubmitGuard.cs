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

using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Dom.Events;

namespace GuardedBrowser
{
    /// <summary>
    ///     Stops form submissions the assistant has not been approved for.
    ///     The DOM submit event is cancelled unless <see cref="AllowNext" />
    ///     was set; a POST that bypasses the event is caught by
    ///     <see cref="PolicyEnforcer" /> through the navigation handler.
    /// </summary>
    public sealed class SubmitGuard
    {
        private readonly IBrowser browser;
        private readonly InputGate gate;
        private readonly AuditLog log;
        private volatile bool allowNext;

        public SubmitGuard(IBrowser browser, InputGate gate, AuditLog log)
        {
            this.browser = browser;
            this.gate = gate;
            this.log = log;
        }

        /// <summary>
        ///     True when the next submission was approved by the user.
        /// </summary>
        public bool AllowNext
        {
            get => allowNext;
            set => allowNext = value;
        }

        public void Attach()
        {
            browser.Navigation.FrameLoadFinished += (_, e) =>
            {
                if (e.Frame.IsMain)
                {
                    Subscribe(e.Frame.Document);
                }
            };
        }

        private void Subscribe(IDocument document)
        {
            document.Events[EventType.Submit] += (_, e) =>
            {
                // The flag stays set until the approved POST has passed the
                // navigation check too; BrowserActions clears it afterwards.
                if (gate.HumanInControl || allowNext)
                {
                    return;
                }
                e.Event.PreventDefault();
                log.Record("blocked",
                           $"Form submission without approval on {browser.Url}");
            };
        }
    }
}
