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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Dom;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Input.Keyboard.Events;
using DotNetBrowser.Input.Mouse;
using DotNetBrowser.Input.Mouse.Events;
using DotNetBrowser.Navigation.Events;

namespace GuardedBrowser
{
    public sealed record FormFields(string Url, string Title, IReadOnlyList<KeyValuePair<string, string>> Fields);

    /// <summary>
    ///     The assistant's hands: every action goes through the policy, the
    ///     input gate, and DotNetBrowser's simulated input, so the page sees
    ///     the same events a person would produce.
    /// </summary>
    public sealed class BrowserActions
    {
        // One browser, one pair of hands: the model may call several tools at
        // once, but their keystrokes and clicks must not interleave.
        private readonly SemaphoreSlim oneAtATime = new(1, 1);

        private readonly IBrowser browser;
        private readonly BrowserPolicy policy;
        private readonly InputGate gate;
        private readonly SubmitGuard submitGuard;
        private readonly AuditLog log;
        private PageSnapshot snapshot = new();

        public BrowserActions(IBrowser browser, BrowserPolicy policy, InputGate gate,
                              SubmitGuard submitGuard, AuditLog log)
        {
            this.browser = browser;
            this.policy = policy;
            this.gate = gate;
            this.submitGuard = submitGuard;
            this.log = log;
        }

        public Task<string> Navigate(string url) => Exclusive(async () =>
        {
            log.Record("tool", $"navigate {url}");
            if (!policy.CanRead(url))
            {
                return $"Blocked by policy: {url} is not an allowed site.";
            }
            await browser.Navigation.LoadUrl(url);
            // A download or a blocked request is reported just after the load returns.
            await Task.Delay(300);
            return await Snapshot();
        });

        public Task<string> ReadPage() => Exclusive(Snapshot);

        public Task<string> Click(int reference) => Exclusive(async () =>
        {
            log.Record("tool", $"click [{reference}]");
            if (!TryGetElement(reference, out SnapshotElement? info, out IElement? element, out string error))
            {
                return error;
            }
            if (info.IsSubmit)
            {
                return $"[{reference}] submits a form. "
                       + "Use submit_form instead, so the user can approve it.";
            }
            await ClickAndWait(element);
            return await Snapshot();
        });

        public Task<string> Type(int reference, string text) => Exclusive(async () =>
        {
            log.Record("tool", $"type [{reference}] \"{text}\"");
            if (!TryGetElement(reference, out SnapshotElement? info, out IElement? element, out string error))
            {
                return error;
            }
            if (info.IsPassword)
            {
                return "Refused: the assistant never types into password fields. "
                       + "Ask the user to sign in.";
            }
            using (gate.BeginAgentTurn())
            {
                browser.Focus();
                element.Focus();
                await browser.MainFrame.ExecuteJavaScript(
                    $"document.querySelector('[data-agent-ref=\"{reference}\"]').select()");
                TypeText(browser.Keyboard, text);
            }
            return await Snapshot();
        });

        public Task<string> Scroll(string direction) => Exclusive(async () =>
        {
            log.Record("tool", $"scroll {direction}");
            int delta = direction == "up" ? -600 : 600;
            await browser.MainFrame.ExecuteJavaScript($"window.scrollBy(0, {delta})");
            return await Snapshot();
        });

        public byte[] Screenshot()
        {
            log.Record("tool", "screenshot");
            return PngEncoder.Encode(browser.TakeImage());
        }

        /// <summary>
        ///     Submits a form by clicking its submit element once the user
        ///     approves the form data. Other actions wait meanwhile, so the
        ///     form can't change between the approval and the click.
        /// </summary>
        public Task<string> Submit(int reference, Func<FormFields, Task<bool>> approve) => Exclusive(async () =>
        {
            log.Record("tool", $"submit_form [{reference}]");
            if (!TryGetElement(reference, out _, out IElement? element, out string error))
            {
                return error;
            }
            FormFields? form = await ReadForm(reference);
            if (form == null)
            {
                return $"[{reference}] does not belong to a form.";
            }
            if (!await approve(form))
            {
                return "The user declined the submission. "
                       + "Stop and report what you prepared.";
            }
            // A page script could still change the form; send only what the user saw.
            FormFields? current = await ReadForm(reference);
            if (current == null || !current.Fields.SequenceEqual(form.Fields))
            {
                return "The form changed after the user approved it, so it was not sent. "
                       + "Call read_page and submit it again.";
            }
            submitGuard.AllowNext = true;
            try
            {
                await ClickAndWait(element);
            }
            finally
            {
                submitGuard.AllowNext = false;
            }
            return await Snapshot();
        });

        /// <summary>
        ///     Reads the fields of the form a submit element belongs to, so the
        ///     user can see what would be sent.
        /// </summary>
        private async Task<FormFields?> ReadForm(int reference)
        {
            string json = await browser.MainFrame.ExecuteJavaScript<string>($@"
                (() => {{
                    const el = document.querySelector('[data-agent-ref=""{reference}""]');
                    const form = el && el.form;
                    if (!form) return '';
                    const fields = [];
                    for (const [name, value] of new FormData(form)) {{
                        const input = form.elements[name];
                        const label = input && input.labels && input.labels.length ? input.labels[0].innerText.trim() : name;
                        fields.push([label, input && input.type === 'password' ? '••••' : String(value)]);
                    }}
                    return JSON.stringify({{ title: document.title, fields }});
                }})()");
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            List<KeyValuePair<string, string>> fields = new();
            foreach (var pair in doc.RootElement.GetProperty("fields").EnumerateArray())
            {
                fields.Add(new KeyValuePair<string, string>(pair[0].GetString() ?? "", pair[1].GetString() ?? ""));
            }
            return new FormFields(browser.Url, doc.RootElement.GetProperty("title").GetString() ?? "", fields);
        }

        private async Task<string> Snapshot()
        {
            snapshot = await PageSnapshot.Capture(browser.MainFrame);
            return snapshot.ToString();
        }

        private async Task<string> Exclusive(Func<Task<string>> action)
        {
            await oneAtATime.WaitAsync();
            // Tell the model what the policy stopped during this action, so it
            // does not have to guess from an unchanged page.
            List<string> blocked = new();
            void OnEntry(AuditEntry entry)
            {
                if (entry.Category == "blocked")
                {
                    lock (blocked) blocked.Add(entry.Message);
                }
            }
            log.EntryAdded += OnEntry;
            try
            {
                string result = await action();
                lock (blocked)
                {
                    return blocked.Count == 0
                        ? result
                        : $"Blocked by policy during this action: {string.Join("; ", blocked)}\n\n{result}";
                }
            }
            finally
            {
                log.EntryAdded -= OnEntry;
                oneAtATime.Release();
            }
        }

        private bool TryGetElement(int reference, out SnapshotElement info, out IElement element, out string error)
        {
            info = snapshot.Find(reference)!;
            element = null!;
            error = "";
            if (info == null)
            {
                error = $"No element [{reference}] in the current page. Call read_page for a fresh list.";
                return false;
            }
            if (!policy.CanAct(browser.Url))
            {
                error = $"Blocked by policy: {browser.Url} is read-only for the assistant.";
                return false;
            }
            element = browser.MainFrame.Document.GetElementByCssSelector($"[data-agent-ref=\"{reference}\"]");
            if (element == null)
            {
                error = $"Element [{reference}] is gone. Call read_page for a fresh list.";
                return false;
            }
            return true;
        }

        private async Task ClickAndWait(IElement element)
        {
            TaskCompletionSource<bool> loaded = new();
            void OnFinished(object? s, FrameLoadFinishedEventArgs e)
            {
                if (e.Frame.IsMain) loaded.TrySetResult(true);
            }
            browser.Navigation.FrameLoadFinished += OnFinished;
            try
            {
                using (gate.BeginAgentTurn())
                {
                    browser.Mouse.SimulateClick(MouseButton.Left, element);
                }
                // A click may or may not navigate; wait briefly for a load either way.
                await Task.WhenAny(loaded.Task, Task.Delay(TimeSpan.FromMilliseconds(800)));
                if (loaded.Task.IsCompleted)
                {
                    await Task.Delay(200);
                }
            }
            finally
            {
                browser.Navigation.FrameLoadFinished -= OnFinished;
            }
        }

        private static void TypeText(IKeyboard keyboard, string text)
        {
            foreach (char c in text)
            {
                bool simpleKey = char.IsAsciiLetterOrDigit(c) || c == ' ';
                KeyCode key = simpleKey ? (KeyCode) char.ToUpperInvariant(c) : KeyCode.Unknown;
                string keyChar = c.ToString();
                KeyModifiers modifiers = new() { ShiftDown = char.IsUpper(c) };

                keyboard.KeyPressed.Raise(new KeyPressedEventArgs
                {
                    VirtualKey = key, KeyChar = keyChar, Modifiers = modifiers
                });
                keyboard.KeyTyped.Raise(new KeyTypedEventArgs
                {
                    VirtualKey = key, KeyChar = keyChar, Modifiers = modifiers
                });
                keyboard.KeyReleased.Raise(new KeyReleasedEventArgs
                {
                    VirtualKey = key, Modifiers = modifiers
                });
            }
        }
    }
}
