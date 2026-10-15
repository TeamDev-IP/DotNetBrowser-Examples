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
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DotNetBrowser.Frames;

namespace GuardedBrowser
{
    /// <summary>
    ///     An interactive element of the page, numbered for the model.
    /// </summary>
    public sealed class SnapshotElement
    {
        [JsonPropertyName("ref")] public int Ref { get; set; }
        [JsonPropertyName("role")] public string Role { get; set; } = "";
        [JsonPropertyName("text")] public string Text { get; set; } = "";
        [JsonPropertyName("value")] public string? Value { get; set; }
        [JsonPropertyName("href")] public string? Href { get; set; }
        [JsonPropertyName("isPassword")] public bool IsPassword { get; set; }
        [JsonPropertyName("isSubmit")] public bool IsSubmit { get; set; }
        [JsonPropertyName("disabled")] public bool Disabled { get; set; }
    }

    /// <summary>
    ///     What the assistant sees: the page title, URL, numbered interactive
    ///     elements, and the visible text.
    /// </summary>
    public sealed class PageSnapshot
    {
        private static readonly string Script = LoadScript();

        [JsonPropertyName("title")] public string Title { get; set; } = "";
        [JsonPropertyName("url")] public string Url { get; set; } = "";
        [JsonPropertyName("elements")]
        public List<SnapshotElement> Elements { get; set; } = new List<SnapshotElement>();

        [JsonPropertyName("text")] public string Text { get; set; } = "";

        public static async Task<PageSnapshot> Capture(IFrame frame)
        {
            string json = await frame.ExecuteJavaScript<string>(Script);
            return JsonSerializer.Deserialize<PageSnapshot>(json) ?? new PageSnapshot();
        }

        public SnapshotElement? Find(int reference)
        {
            return Elements.Find(e => e.Ref == reference);
        }

        /// <summary>
        ///     The compact text form sent to the model.
        /// </summary>
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine($"Page: {Title} ({Url})");
            foreach (SnapshotElement e in Elements)
            {
                text.Append($"[{e.Ref}] {e.Role} \"{e.Text}\"");
                if (!string.IsNullOrEmpty(e.Value))
                {
                    text.Append($" value=\"{e.Value}\"");
                }
                if (e.Href != null)
                {
                    text.Append($" -> {e.Href}");
                }
                if (e.IsPassword)
                {
                    text.Append(" (password)");
                }
                if (e.IsSubmit)
                {
                    text.Append(" (submits a form)");
                }
                if (e.Disabled)
                {
                    text.Append(" (disabled)");
                }
                text.AppendLine();
            }
            text.AppendLine("Text:");
            text.AppendLine(Text);
            return text.ToString();
        }

        private static string LoadScript()
        {
            Assembly assembly = typeof(PageSnapshot).Assembly;
            using Stream stream =
                assembly.GetManifestResourceStream("GuardedBrowser.snapshot.js")
                ?? throw new InvalidOperationException("The snapshot script is missing.");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
