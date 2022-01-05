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

using System.Runtime.InteropServices;
using DotNetBrowser.Engine;

namespace ComWrapper.WinForms.Impl
{
    [Guid("DAE7A4AC-2D70-4830-81D8-3D7E5D8A7981")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("DotNetBrowser.ComWrapper.Engine")]
    public class EngineWrapper : IComEngine
    {
        private IEngine engine;
        private bool initialized;

        public IComBrowser CreateBrowser() => new BrowserImpl(engine.CreateBrowser());

        public void Dispose()
        {
            if (initialized)
            {
                engine?.Dispose();
                engine = null;
                initialized = false;
            }
        }

        public void Initialize()
        {
            if (!initialized)
            {
                engine = EngineFactory.Create(new EngineOptions.Builder
                {
                    RenderingMode = RenderingMode.OffScreen
                }.Build());
                initialized = true;
            }
        }
    }
}
