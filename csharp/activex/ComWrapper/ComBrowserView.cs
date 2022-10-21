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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ComWrapper.WinForms.Impl;
using DotNetBrowser.Browser;
using DotNetBrowser.Logging;

namespace ComWrapper.WinForms
{
    [Guid("5D300D12-E721-4711-8262-215E75894FE5")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("DotNetBrowser.ComWrapper.ComBrowserView")]
    public partial class ComBrowserView : UserControl, IComBrowserView
    {
        private readonly EngineWrapper _engineWrapper;

        public IComBrowser Browser { get; }

        public IComEngine Engine => _engineWrapper;

        public ComBrowserView()
        {
            try
            {
                ConfigureLogging();
                InitializeComponent();
                _engineWrapper = new EngineWrapper();
                _engineWrapper.Initialize();
                Browser = _engineWrapper.CreateBrowser();
                InitializeFrom(Browser);
                Browser.LoadUrl("teamdev.com/dotnetbrowser");
                EventLogWrapper.Log("ComBrowserView initialized", EventLogEntryType.Information, 201);
            }
            catch (Exception e)
            {
                EventLogWrapper.Log(e.ToString(), EventLogEntryType.Error, 500);
                throw;
            }
        }

        void IComBrowserView.Dispose()
        {
            _engineWrapper.Dispose();
            EventLogWrapper.Log("ComBrowserView disposed", EventLogEntryType.Information, 201);
        }

        public void InitializeFrom(IComBrowser browser)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => { browserView1.InitializeFrom((browser as BrowserImpl)?.Browser); }));
            }
            else
            {
                browserView1.InitializeFrom((browser as BrowserImpl)?.Browser);
            }
        }

        private static void ConfigureLogging()
        {
            LoggerProvider.Instance.Level = SourceLevels.Information;
            LoggerProvider.Instance.FileLoggingEnabled = true;
            string outputFile = Path.GetFullPath($"dotnetbrowser-comwrapper-{Guid.NewGuid()}.log");
            LoggerProvider.Instance.OutputFile = outputFile;
            EventLogWrapper.Log($"DotNetBrowser logs can be found at {outputFile}", EventLogEntryType.Information, 202);
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction]
        private static void Register(Type t)
        {
            ControlRegistration.RegisterControl(t);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction]
        private static void Unregister(Type t)
        {
            ControlRegistration.UnregisterControl(t);
        }
    }
}
