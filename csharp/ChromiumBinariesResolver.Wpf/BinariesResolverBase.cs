#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ChromiumBinariesResolver.Wpf
{
    /// <summary>
    ///     The abstract class for implementing the binaries resolver.
    /// </summary>
    public abstract class BinariesResolverBase
    {
        private readonly HttpClient client;

        protected string RequestUri { get; }

        /// <summary>
        ///     Occurs when the status update messages are sent by the implementation.
        ///     These messages can be used to update the application UI and indicate the progress.
        /// </summary>
        public event EventHandler<BinariesResolverStatusEventArgs> StatusUpdated;

        protected BinariesResolverBase(string requestUri, AppDomain domain = null)
        {
            if (domain == null)
            {
                domain = AppDomain.CurrentDomain;
            }

            RequestUri = requestUri;
            client = new HttpClient();
            domain.AssemblyResolve += Resolve;
        }

        /// <summary>
        ///     The AssemblyResolve event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Assembly Resolve(object sender, ResolveEventArgs args)
            => args.Name.StartsWith("DotNetBrowser.Chromium") ? Resolve(args.Name).Result : null;

        protected void OnStatusUpdated(string message, bool completed = false)
        {
            StatusUpdated?.Invoke(this, new BinariesResolverStatusEventArgs(message, completed));
        }

        protected abstract string PrepareRequest(AssemblyName assemblyName);
        protected abstract Assembly ProcessResponse(Stream responseBody, AssemblyName assemblyName);

        private async Task<Assembly> Resolve(string binariesAssemblyName)
        {
            //Note: assemblies are usually resolved in the background thread of the UI application.
            try
            {
                //Construct a request using the fully-qualified assembly name.
                AssemblyName assemblyName = new AssemblyName(binariesAssemblyName);
                string request = PrepareRequest(assemblyName);

                //Perform the request and download the response.
                OnStatusUpdated("Downloading Chromium binaries...");
                Debug.WriteLine($"Downloading {request}");
                HttpResponseMessage response = await client.GetAsync(request);

                response.EnsureSuccessStatusCode();
                OnStatusUpdated("Chromium binaries package downloaded");
                Stream responseBody = await response.Content.ReadAsStreamAsync();

                //Process the response bytes and load the assembly.
                return ProcessResponse(responseBody, assemblyName);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception caught: {e} ");
            }

            return null;
        }
    }
}
