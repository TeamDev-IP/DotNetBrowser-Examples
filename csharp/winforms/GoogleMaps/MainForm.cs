#region Copyright

// Copyright © 2023, TeamDev. All rights reserved.
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
using System.IO;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.WinForms;

namespace GoogleMaps.WinForms
{
    /// <summary>
    ///     This example demonstrates how to use Google Maps with DotNetBrowser.
    ///     To make this sample work, please configure the valid Google API key in map.html(line 11)
    /// </summary>
    public partial class MainForm : Form
    {
        private const int MinZoomLevel = 0;
        private const int MaxZoomLevel = 21;

        private int currentZoomLevel = 4; //The default value for Google Maps zoom

        private IBrowser Browser { get; }
        private BrowserView BrowserView { get; }

        private int CurrentZoomLevel
        {
            get { return currentZoomLevel; }

            set
            {
                if (value != currentZoomLevel && value > MinZoomLevel && value < MaxZoomLevel)
                {
                    if (!Browser.IsDisposed)
                    {
                        currentZoomLevel = value;
                        Browser.MainFrame.ExecuteJavaScript($"map.setZoom({currentZoomLevel})");
                    }
                }
            }
        }

        private IEngine Engine { get; }

        private string PathToMapFile => Path.GetFullPath("map.html");

        public MainForm()
        {
            InitializeComponent();

            Engine = EngineFactory.Create();
            Browser = Engine.CreateBrowser();
            BrowserView = new BrowserView {Dock = DockStyle.Fill};

            BrowserView.InitializeFrom(Browser);
            Controls.Add(BrowserView);

            Browser.Navigation.LoadUrl(PathToMapFile);

            Closed += MainForm_Closed;
        }

        private void MainForm_Closed(object sender, EventArgs e)
        {
            Browser.Dispose();
            Engine.Dispose();
        }

        private void ZoomInBtn_Click(object sender, EventArgs e)
        {
            CurrentZoomLevel++;
        }

        private void ZoomOutBtn_Click(object sender, EventArgs e)
        {
            CurrentZoomLevel--;
        }
    }
}
