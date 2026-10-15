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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Anthropic.Helpers.Beta;
using Anthropic.Models.Beta.Messages;
using GuardedBrowser;

namespace AssistantAgent
{
    /// <summary>
    ///     The tools the model is offered. Each one maps to a
    ///     <see cref="BrowserActions" /> call, and the two that need a person,
    ///     submit_form and hand_over, go through the session first.
    /// </summary>
    public static class BrowserTools
    {
        public static IReadOnlyList<IBetaRunnableTool> Create(BrowserActions actions,
                                                              AssistantSession session)
        {
            return new IBetaRunnableTool[]
            {
                Tool(session, "read_page",
                     "Describes the current page: its title, URL, numbered "
                     + "interactive elements, and text.",
                     new { },
                     (_, _) => actions.ReadPage()),

                Tool(session, "navigate",
                     "Opens a URL. Only allowlisted sites open.",
                     new { url = Str("The absolute URL to open.") },
                     (input, _) => actions.Navigate(input.Get("url")), "url"),

                Tool(session, "click",
                     "Clicks an element by its number from read_page.",
                     new { @ref = Int("The element number.") },
                     (input, _) => actions.Click(input.GetInt("ref")), "ref"),

                Tool(session, "type",
                     "Types text into a text field by its number, replacing its "
                     + "content. Never used for passwords.",
                     new { @ref = Int("The element number."), text = Str("The text.") },
                     (input, _) => actions.Type(input.GetInt("ref"), input.Get("text")),
                     "ref", "text"),

                Tool(session, "scroll",
                     "Scrolls the page up or down by one screen.",
                     new
                     {
                         direction = new { type = "string", @enum = new[] { "up", "down" } }
                     },
                     (input, _) => actions.Scroll(input.Get("direction")), "direction"),

                Tool(session, "submit_form",
                     "Submits the form that a submit element belongs to. The user "
                     + "sees the form data and approves or declines.",
                     new { @ref = Int("The number of the form's submit button.") },
                     (input, ct) => actions.Submit(
                         input.GetInt("ref"),
                         form => session.RequestApproval(form, ct)),
                     "ref"),

                Tool(session, "hand_over",
                     "Gives the page to the user, for example to sign in. "
                     + "Returns when the user hands it back.",
                     new { reason = Str("What the user should do, in one sentence.") },
                     async (input, ct) =>
                     {
                         await session.RequestHandOver(input.Get("reason"), ct);
                         return "The user handed the page back. "
                                + "Call read_page to see the current page.";
                     }, "reason"),

                new BetaRunnableTool
                {
                    Name = "take_screenshot",
                    Definition = Definition("take_screenshot",
                                            "Takes a screenshot of the current page. Use it "
                                            + "only when the text description is not enough.",
                                            new { }),
                    Run = async (_, ct) =>
                    {
                        await session.WaitWhilePaused(ct);
                        byte[] png = actions.Screenshot();
                        return new List<Block>
                        {
                            new BetaImageBlockParam
                            {
                                Source = new BetaBase64ImageSource
                                {
                                    Data = Convert.ToBase64String(png),
                                    MediaType = MediaType.ImagePng
                                }
                            }
                        };
                    }
                }
            };
        }

        private static BetaRunnableTool Tool(
            AssistantSession session, string name, string description, object properties,
            Func<IReadOnlyDictionary<string, JsonElement>, CancellationToken,
                Task<string>> run,
            params string[] required)
        {
            return new BetaRunnableTool
            {
                Name = name,
                Definition = Definition(name, description, properties, required),
                Run = async (toolUse, ct) =>
                {
                    // A paused session acts again only when the user hands the page back.
                    await session.WaitWhilePaused(ct);
                    return await run(toolUse.Input, ct);
                }
            };
        }

        private static BetaTool Definition(string name, string description, object properties,
                                           params string[] required)
        {
            Dictionary<string, JsonElement> schema = new Dictionary<string, JsonElement>();
            JsonElement serialized = JsonSerializer.SerializeToElement(properties);
            foreach (JsonProperty property in serialized.EnumerateObject())
            {
                schema[property.Name] = property.Value.Clone();
            }
            return new BetaTool
            {
                Name = name,
                Description = description,
                InputSchema = new InputSchema { Properties = schema, Required = required }
            };
        }

        private static object Str(string description)
        {
            return new { type = "string", description };
        }

        private static object Int(string description)
        {
            return new { type = "integer", description };
        }

        private static string Get(this IReadOnlyDictionary<string, JsonElement> input,
                                  string name)
        {
            return input.TryGetValue(name, out JsonElement value)
                ? value.GetString() ?? ""
                : "";
        }

        private static int GetInt(this IReadOnlyDictionary<string, JsonElement> input,
                                  string name)
        {
            return input.TryGetValue(name, out JsonElement value) ? value.GetInt32() : 0;
        }
    }
}
