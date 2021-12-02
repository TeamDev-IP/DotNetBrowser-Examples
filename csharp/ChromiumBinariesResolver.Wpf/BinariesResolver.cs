#region Copyright

// Copyright © 2021, TeamDev. All rights reserved.
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

using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace ChromiumBinariesResolver.Wpf
{
    public class BinariesResolver : BinariesResolverBase
    {
        private const string UriTemplate =
            "https://storage.googleapis.com/cloud.teamdev.com/downloads/dotnetbrowser/{0}/dotnetbrowser-net45-{0}.zip";


        public BinariesResolver() : base(UriTemplate)
        {
        }

        protected override string PrepareRequest(AssemblyName assemblyName)
        {
            //Use only the major and minor version components if the build component is 0.
            int fieldCount = assemblyName.Version.Build == 0 ? 2 : 3;
            return string.Format(RequestUri, assemblyName.Version.ToString(fieldCount));
        }

        protected override Assembly ProcessResponse(Stream responseBody, AssemblyName assemblyName)
        {
            //The downloaded bytes represent a ZIP archive. Locate the DLL we need in this ar
            ZipArchive archive = new ZipArchive(responseBody);
            ZipArchiveEntry binariesDllEntry = archive.Entries
                                                      .FirstOrDefault(entry => entry.FullName.EndsWith(".dll")
                                                                               && entry.FullName.Contains(assemblyName
                                                                                                             .Name));
            if (binariesDllEntry == null)
            {
                return null;
            }

            //Unzip the found entry and load the DLL.
            OnStatusUpdated("Unzipping Chromium binaries");
            Stream unzippedEntryStream;
            using (unzippedEntryStream = binariesDllEntry.Open())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    unzippedEntryStream.CopyTo(memoryStream);
                    OnStatusUpdated("Loading Chromium binaries assembly");
                    Assembly assembly = Assembly.Load(memoryStream.ToArray());
                    OnStatusUpdated("Chromium binaries assembly loaded.", true);
                    return assembly;
                }
            }
        }
    }
}