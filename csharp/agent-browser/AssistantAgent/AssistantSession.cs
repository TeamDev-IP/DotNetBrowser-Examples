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
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Anthropic;
using System.Text.Json;
using Anthropic.Helpers.Beta;
using Anthropic.Models.Beta;
using Anthropic.Models.Beta.Messages;
using GuardedBrowser;

namespace AssistantAgent
{
    /// <summary>
    ///     Runs one task: the model calls tools in a loop until it reports.
    ///     The session pauses while the user holds the page and asks the
    ///     user before any form is submitted.
    /// </summary>
    public sealed class AssistantSession
    {
        private static readonly string SystemPrompt = LoadSystemPrompt();

        private readonly AnthropicClient client;
        private readonly BrowserActions actions;
        private readonly AuditLog log;
        private volatile TaskCompletionSource<bool> resumed = Completed();

        /// <summary>
        ///     Raised when the model wants to submit a form. Return true to allow it.
        /// </summary>
        public Func<SubmitRequest, Task<bool>>? ApprovalRequested { get; set; }

        /// <summary>
        ///     Raised when the model hands the page to the user. The session is
        ///     paused until <see cref="Resume" /> is called.
        /// </summary>
        public Action<string>? HandOverRequested { get; set; }

        public Action<string>? StatusChanged { get; set; }

        public AssistantSession(AnthropicClient client, BrowserActions actions, AuditLog log)
        {
            this.client = client;
            this.actions = actions;
            this.log = log;
        }

        public bool IsPaused => !resumed.Task.IsCompleted;

        public void Pause()
        {
            if (!IsPaused)
            {
                resumed = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
                Status("Paused: you have the page.");
            }
        }

        public void Resume()
        {
            resumed.TrySetResult(true);
        }

        public async Task<string> RunAsync(string task, CancellationToken cancellationToken)
        {
            log.Record("assistant", $"Task: {task}");
            Status("Thinking…");

            IReadOnlyList<IBetaRunnableTool> tools = BrowserTools.Create(actions, this);
            MessageCreateParams parameters = new()
            {
                Model = Anthropic.Models.Messages.Model.ClaudeOpus5_5,
                MaxTokens = 2048,
                System = SystemPrompt,
                // If a safety classifier declines a turn, the API retries it
                // on a fallback model instead of failing the run.
                Betas = [AnthropicBeta.ServerSideFallback2026_07_01],
                Fallbacks = new Default(),
                Messages = new List<BetaMessageParam>
                {
                    new() { Role = Role.User, Content = task }
                }
            };

            string report = "";
            long inputTokens = 0, outputTokens = 0;
            int turns = 0;
            BetaToolRunner runner =
                client.Beta.Messages.ToolRunner(parameters, tools, maxIterations: 40);
            await foreach (BetaMessage turn in runner.WithCancellation(cancellationToken))
            {
                turns++;
                long turnInput = turn.Usage.InputTokens
                                 + (turn.Usage.CacheCreationInputTokens ?? 0)
                                 + (turn.Usage.CacheReadInputTokens ?? 0);
                inputTokens += turnInput;
                outputTokens += turn.Usage.OutputTokens;
                log.Record("usage", $"Turn {turns}: {turnInput} input, "
                                    + $"{turn.Usage.OutputTokens} output tokens");

                foreach (BetaContentBlock block in turn.Content)
                {
                    if (block.TryPickText(out BetaTextBlock? text))
                    {
                        report = text.Text;
                        log.Record("assistant", text.Text);
                        Status(text.Text);
                    }
                    else if (block.TryPickToolUse(out BetaToolUseBlock? toolUse))
                    {
                        string input = JsonSerializer.Serialize(toolUse.Input);
                        log.Record("assistant", $"Tool call {toolUse.Name} {input}");
                        Status($"Calling {toolUse.Name}…");
                    }
                }
            }

            log.Record("usage", $"Run total: {turns} turns, {inputTokens} input, "
                                + $"{outputTokens} output tokens");
            Status("Done.");
            return report;
        }

        internal Task WaitWhilePaused(CancellationToken cancellationToken) =>
            resumed.Task.WaitAsync(cancellationToken);

        internal async Task<bool> RequestApproval(SubmitRequest request, CancellationToken cancellationToken)
        {
            await WaitWhilePaused(cancellationToken);
            log.Record("approval",
                       $"Requested for {request.Title}: {string.Join(", ", request.Fields)}");
            Status("Waiting for your approval…");
            bool approved = ApprovalRequested != null && await ApprovalRequested(request);
            log.Record("approval", approved ? "Approved by the user" : "Declined by the user");
            return approved;
        }

        internal async Task RequestHandOver(string reason, CancellationToken cancellationToken)
        {
            log.Record("assistant", $"Hand over: {reason}");
            Pause();
            HandOverRequested?.Invoke(reason);
            Status($"Waiting for you: {reason}");
            await WaitWhilePaused(cancellationToken);
        }

        private void Status(string status) => StatusChanged?.Invoke(status);

        private static TaskCompletionSource<bool> Completed()
        {
            TaskCompletionSource<bool> source = new();
            source.SetResult(true);
            return source;
        }

        private static string LoadSystemPrompt()
        {
            Assembly assembly = typeof(AssistantSession).Assembly;
            using Stream stream =
                assembly.GetManifestResourceStream("AssistantAgent.SystemPrompt.md")!;
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}
