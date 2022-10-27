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
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Widgets.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Geometry;
using DotNetBrowser.Handlers;
using DotNetBrowser.Logging;
using DotNetBrowser.Ui;
using UnityEngine;
using Color = DotNetBrowser.Ui.Color;

namespace Assets.Scripts
{
    /// <summary>
    ///     Controls Browser life cycle. Creates engine and browser and destroys it at the end of the work.
    /// </summary>
    public class BrowserScript : MonoBehaviour
    {
        public string DefaultUrl = "www.google.com";
        public Vector2 Size = new Vector2(1024, 768);

        /// <summary>
        ///     Gets the latest bitmap data of browser web page.
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        ///     Gets an instance of IBrowser controlled by this script.
        /// </summary>
        public IBrowser Browser { get; private set; }

        /// <summary>
        ///     Gets an instance of <see cref="IEngine" /> controlled by this script.
        /// </summary>
        public IEngine Engine { get; private set; }

        /// <summary>
        ///     Navigates to specified url.
        /// </summary>
        /// <param name="url">Url to load.</param>
        public void Navigate(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                string path = Path.GetFullPath(url);
                if(File.Exists(path))
                    url = path;
                Browser.Navigation.LoadUrl(url);
            }
        }

        protected virtual void CreateBrowser()
        {
            SetupLogging();

            EngineOptions engineOptions = new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.OffScreen
            }.Build();
            Engine = EngineFactory.Create(engineOptions);

            Browser = Engine.CreateBrowser();
            Browser.Size = new Size((uint)Size.x, (uint)Size.y);
            Browser.Settings.TransparentBackgroundEnabled = true;
            Browser.Settings.DefaultBackgroundColor = new Color(0, 0, 0, 0);

            IOffScreenRenderProvider provider = (IOffScreenRenderProvider)Browser;
            if (provider.PaintHandler != null)
            {
                throw new InvalidOperationException("The paint handler already specified");
            }

            provider.PaintHandler = new Handler<PaintParameters>(p => Bitmap = p.View);
            provider.Show();
        }

        private void Awake()
        {
            CreateBrowser();
        }

        private void OnDestroy() => Engine?.Dispose();

        private void SetupLogging()
        {
            LoggerProvider.Instance.Level = SourceLevels.All;
            LoggerProvider.Instance.FileLoggingEnabled = true;
            string logFile = $"DotNetBrowser-{Guid.NewGuid()}.log";
            LoggerProvider.Instance.OutputFile = Path.GetFullPath(logFile);
        }

        private void Start() => Navigate(DefaultUrl);
    }
}