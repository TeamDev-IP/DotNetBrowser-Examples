#region Copyright

// Copyright 2022, TeamDev. All rights reserved.
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

using System.Drawing;
using System.IO;
using System.Reflection;

namespace DotNetBrowser.WinForms.Demo.Resources
{
    internal static class ResourceLocator
    {
        internal static Bitmap CloseButtonBitmap
            => (Bitmap) Image.FromStream(GetStream("Resources/Images/icon-close.png"));

        private static Assembly ResourceAssembly
            => Assembly.GetExecutingAssembly();

        private static string GetResourceFullPath(string localPath)
            => $"{ResourceAssembly.GetName().Name}.{ReplacePathSeparators(localPath)}";

        private static Stream GetStream(string localPath)
            => ResourceAssembly.GetManifestResourceStream(GetResourceFullPath(localPath));

        private static string ReplacePathSeparators(string localPath)
            => localPath.Replace("/", ".").Replace("\\", ".");
    }
}
