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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;


internal class DependencyResolver
{
    private const string UriTemplate =
        "https://storage.googleapis.com/cloud.teamdev.com/downloads/dotnetbrowser/{0}/dotnetbrowser-netstandard20-{0}.zip";

    private readonly HttpClient client;

    private readonly IReadOnlyList<string> requiredAssemblies = new List<string>()
        {
            "DotNetBrowser.dll",
            "DotNetBrowser.Logging.dll",
            "DotNetBrowser.Core.dll",
            "DotNetBrowser.Chromium.Linux-x64.dll",
            "DotNetBrowser.Chromium.macOS-x64.dll",
            "DotNetBrowser.Chromium.Win-x86.dll",
            "DotNetBrowser.Chromium.Win-x64.dll",
            "protobuf-net.dll",
        };

    public DependencyResolver()
    {
        this.client = new HttpClient();
    }

    public void Restore(string version, string targetLocation)
    {
        IEnumerable<string> missingAssemblies = requiredAssemblies
            .Where(a => !File.Exists(Path.Combine(targetLocation, a))).ToList();

        if (!missingAssemblies.Any())
        {
            Log("All DotNetBrowser assemblies are found.");
            return;
        }
        else
        {
            Log("The following assemblies are missing: " + string.Join(", ", missingAssemblies));
        }

        try
        {
            Uri request = PrepareRequest(version);
            EditorUtility.DisplayProgressBar("Restoring DotNetBrowser assemblies", "Downloading DotNetBrowser archive...", 0);
            //Perform the request and download the response.
            Log($"Downloading DotNetBrowser archive...");
            Stream responseBody = client.GetStreamAsync(request).Result;

            EditorUtility.DisplayProgressBar("Restoring DotNetBrowser assemblies", "Unzipping DotNetBrowser archive...", 0);
            
            //Process the response bytes
            ProcessResponse(responseBody, targetLocation);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Exception caught: {e} ");
        }
        finally{
            EditorUtility.ClearProgressBar();
        }
    }

    private void Log(string message)
    {
        UnityEngine.Debug.Log(message);
    }

    private void ProcessResponse(Stream responseBody, string targetLocation)
    {
        //The downloaded bytes represent a ZIP archive. Locate the DLLs we need
        ZipArchive archive = new ZipArchive(responseBody);
        var dlls = archive.Entries
            .Where(entry => requiredAssemblies.Contains(entry.Name)).ToList();

        float progressStep = 1.0f / dlls.Count;
        float currentProgress = 0;
        foreach (ZipArchiveEntry entry in dlls)
        {
            //Unzip the found entry and load the DLL.
            Log($"Unzipping {entry.Name}");
            EditorUtility.DisplayProgressBar("Restoring DotNetBrowser assemblies", $"Unzipping {entry.Name}", currentProgress);
            using (Stream unzippedEntryStream = entry.Open())
            {
                var destPath = Path.Combine(targetLocation, entry.Name);
                using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
                {
                    unzippedEntryStream.CopyTo(fileStream);
                }
            }
            currentProgress += progressStep;
        }
    }

    private static Uri PrepareRequest(string version)
    {
        string url = string.Format(UriTemplate, version);
        return new Uri(url);
    }
}
