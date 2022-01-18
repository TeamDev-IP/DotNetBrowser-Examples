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
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace ChromiumBinariesResolver.Wpf
{
    /// <summary>
    ///     This example demonstrates how to deploy Chromium binaries
    ///     over network.
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly string chromiumDirectory;
        private IBrowser browser;
        private IEngine engine;
        private string initializationStatus = "Initializing";


        public BinariesResolver BinariesResolver { get; }

        public string InitializationStatus
        {
            get { return initializationStatus; }
            private set
            {
                initializationStatus = value;
                OnPropertyChanged();
            }
        }

        public bool IsInitializationInProgress { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            chromiumDirectory = Path.GetFullPath("chromium");

            //Delete the Chromium directory it it exists - this will force downloading the binaries over network.
            if (Directory.Exists(chromiumDirectory))
            {
                Directory.Delete(chromiumDirectory, true);
            }

            Directory.CreateDirectory(chromiumDirectory);

            //Create and initialize the BinariesResolver
            BinariesResolver = new BinariesResolver();
            //Subscribe to the StatusUpdated event to update the UI accordingly.
            BinariesResolver.StatusUpdated += (sender, e) => InitializationStatus = e.Message;

            DataContext = this;

            Task.Run(() =>
                 {
                     IsInitializationInProgress = true;
                     EngineOptions engineOptions = new EngineOptions.Builder
                         {
                             RenderingMode = RenderingMode.HardwareAccelerated,
                             ChromiumDirectory = chromiumDirectory
                         }
                        .Build();
                     InitializationStatus = "Creating DotNetBrowser engine";
                     engine = EngineFactory.Create(engineOptions);
                     InitializationStatus = "DotNetBrowser engine created";
                     browser = engine.CreateBrowser();
                 })
                .ContinueWith(t =>
                 {
                     BrowserView.InitializeFrom(browser);
                     IsInitializationInProgress = false;
                     browser.Navigation.LoadUrl("https://www.teamdev.com/");
                 }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            engine?.Dispose();
        }
    }
}
