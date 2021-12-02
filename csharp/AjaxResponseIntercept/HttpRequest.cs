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

using System.Collections.Generic;
using System.Text;
using DotNetBrowser.Net;

namespace AjaxResponseIntercept
{
    internal sealed class HttpRequest
    {
        private readonly List<byte> responseData = new List<byte>();

        /// <summary>
        ///     Indicates whether the request is already completed.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        ///     The HTTP method used to perform this request.
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     The MIME type from the request headers.
        /// </summary>
        public MimeType MimeType { get; set; }

        /// <summary>
        ///     The string representation of the response received.
        ///     Is incomplete until the request itself is completed.
        /// </summary>
        public string Response => Encoding.UTF8.GetString(responseData.ToArray());


        /// <summary>
        ///     Aggregated response data.
        /// </summary>
        public IReadOnlyList<byte> ResponseData => responseData;

        /// <summary>
        ///     The request URL.
        /// </summary>
        public string Url { get; }


        public HttpRequest(string requestUrl, string requestMethod)
        {
            Url = requestUrl;
            Method = requestMethod;
        }

        /// <summary>
        ///     Append received response bytes.
        /// </summary>
        /// <param name="data"></param>
        public void AppendResponseBytes(byte[] data)
        {
            responseData.AddRange(data);
        }

        /// <summary>
        ///     Mark the request as completed.
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
        }
    }
}