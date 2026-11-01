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

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PriceMonitor
{
    /// <summary>
    ///     Keeps every change of every product in a JSON Lines file, one
    ///     snapshot per line, and remembers the latest snapshot of each
    ///     product, so a restart doesn't report the same change twice.
    /// </summary>
    public sealed class PriceHistory
    {
        private readonly string file;
        private readonly Dictionary<string, ProductSnapshot> latest =
            new Dictionary<string, ProductSnapshot>();

        /// <summary>
        ///     Reads the history file from the data folder, if there is one.
        /// </summary>
        public PriceHistory(MonitorOptions options, ILogger<PriceHistory> logger)
        {
            file = Path.Combine(options.DataDirectory, "history.jsonl");
            if (!File.Exists(file))
            {
                return;
            }
            foreach (string line in File.ReadLines(file))
            {
                // A crash during a write can leave a broken last line.
                ProductSnapshot snapshot = TryParse(line);
                if (snapshot?.Url == null)
                {
                    logger.LogWarning("Skipping a broken line in {File}.", file);
                    continue;
                }
                latest[snapshot.Url] = snapshot;
            }
        }

        /// <summary>
        ///     Returns the latest snapshot of the product, or null if the
        ///     product has never been read.
        /// </summary>
        public ProductSnapshot GetLatest(string url)
        {
            return latest.GetValueOrDefault(url);
        }

        /// <summary>
        ///     Appends the snapshot to the file and makes it the latest one.
        /// </summary>
        public void Add(ProductSnapshot snapshot)
        {
            // First in memory: if the file can't be written, the same change
            // isn't reported again on the next check.
            latest[snapshot.Url] = snapshot;
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.AppendAllLines(file, new[] { JsonSerializer.Serialize(snapshot) });
        }

        private static ProductSnapshot TryParse(string line)
        {
            try
            {
                return JsonSerializer.Deserialize<ProductSnapshot>(line);
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
