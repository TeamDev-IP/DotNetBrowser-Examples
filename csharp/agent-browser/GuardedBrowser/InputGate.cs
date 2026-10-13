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

using System;
using System.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Input.Mouse;
using DotNetBrowser.Input.Mouse.Events;
using DotNetBrowser.Input.Touch;
using DotNetBrowser.Input.Touch.Events;

namespace GuardedBrowser
{
    /// <summary>
    ///     Decides who holds the mouse and the keyboard. While the assistant
    ///     works, human input into the page is suppressed. "Take over" gives
    ///     the page back to the human, "Hand back" returns it to the assistant.
    /// </summary>
    public sealed class InputGate
    {
        private static readonly TimeSpan AgentTurnGrace = TimeSpan.FromMilliseconds(500);

        private readonly IBrowser browser;
        private readonly AuditLog log;
        private volatile bool agentTurn;
        private long graceEndsAt;
        private volatile bool humanInControl = true;

        public InputGate(IBrowser browser, AuditLog log)
        {
            this.browser = browser;
            this.log = log;
        }

        /// <summary>
        ///     True while the human has taken over the page.
        /// </summary>
        public bool HumanInControl => humanInControl;

        public void Attach()
        {
            IKeyboard keyboard = browser.Keyboard;
            keyboard.KeyPressed.Handler = Gate<IKeyPressedEventArgs>();
            keyboard.KeyTyped.Handler = Gate<IKeyTypedEventArgs>();
            keyboard.KeyReleased.Handler = Gate<IKeyReleasedEventArgs>();

            IMouse mouse = browser.Mouse;
            mouse.Pressed.Handler = Gate<IMousePressedEventArgs>();
            mouse.Released.Handler = Gate<IMouseReleasedEventArgs>();
            mouse.Moved.Handler = Gate<IMouseMovedEventArgs>();
            mouse.WheelMoved.Handler = Gate<IMouseWheelMovedEventArgs>();
            mouse.Entered.Handler = Gate<IMouseEnteredEventArgs>();
            mouse.Exited.Handler = Gate<IMouseExitedEventArgs>();

            ITouch touch = browser.Touch;
            touch.Started.Handler = Gate<ITouchEventArgs>();
            touch.Moved.Handler = Gate<ITouchEventArgs>();
            touch.Ended.Handler = Gate<ITouchEventArgs>();
            touch.Canceled.Handler = Gate<ITouchEventArgs>();
        }

        /// <summary>
        ///     Releases the page to the human. The assistant should pause.
        /// </summary>
        public void TakeOver()
        {
            humanInControl = true;
            log.Record("input", "Human took over");
        }

        /// <summary>
        ///     Returns the page to the assistant. Human input is suppressed again.
        /// </summary>
        public void HandBack()
        {
            humanInControl = false;
            log.Record("input", "Handed back to the assistant");
        }

        /// <summary>
        ///     Marks the assistant's own simulated input, which passes through
        ///     the same handlers as human input.
        /// </summary>
        public AgentTurnScope BeginAgentTurn() => new(this);

        private Handler<T, InputEventResponse> Gate<T>()
        {
            return new Handler<T, InputEventResponse>(_ =>
                humanInControl || IsAgentTurn
                    ? InputEventResponse.Proceed
                    : InputEventResponse.Suppress);
        }

        // In hardware-accelerated mode, the engine reports simulated input
        // back to the handlers a moment after Raise() returns.
        private bool IsAgentTurn =>
            agentTurn || DateTime.UtcNow.Ticks < Interlocked.Read(ref graceEndsAt);

        public readonly struct AgentTurnScope : IDisposable
        {
            private readonly InputGate gate;

            internal AgentTurnScope(InputGate gate)
            {
                this.gate = gate;
                gate.agentTurn = true;
            }

            public void Dispose()
            {
                long graceEnd = DateTime.UtcNow.Add(AgentTurnGrace).Ticks;
                Interlocked.Exchange(ref gate.graceEndsAt, graceEnd);
                gate.agentTurn = false;
            }
        }
    }
}
