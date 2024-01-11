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
using System.Windows;
using DotNetBrowser.Engine;
using Mvvm.Wpf.ViewModels;

namespace Mvvm.Wpf
{
    /// <summary>
    ///     This example demonstrates the possible approach to use DotNetBrowser
    ///     with WPF data binding.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEngine engine;

        public MyBrowserViewModel MyBrowser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            EngineOptions engineOptions = new EngineOptions.Builder
                {
                    RenderingMode = RenderingMode.HardwareAccelerated
                }
               .Build();
            engine = EngineFactory.Create(engineOptions);

            MyBrowser = new MyBrowserViewModel(engine.CreateBrowser())
            {
                Url = "www.teamdev.com/dotnetbrowser"
            };
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            engine?.Dispose();
        }
    }
}
