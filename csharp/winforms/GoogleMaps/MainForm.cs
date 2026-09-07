#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;
using DotNetBrowser.Permissions;
using DotNetBrowser.Permissions.Handlers;

namespace GoogleMaps.WinForms
{
    /// <summary>
    ///     This example demonstrates how to display Google Maps in a WinForms
    ///     application and drive the Maps JavaScript API from .NET: change the
    ///     zoom level, put markers on the map, and center the map on the
    ///     current location.
    /// </summary>
    /// <remarks>
    ///     To make this example work, configure a valid Google API key in
    ///     map.html. Because the page is loaded from the local file system, the
    ///     key must not be restricted by HTTP referrer.
    ///     The "My location" button additionally requires the Google Maps
    ///     Geolocation API to be enabled for the Chromium engine. See
    ///     https://teamdev.com/dotnetbrowser/docs/guides/gs/engine/#google-apis
    /// </remarks>
    public partial class MainForm : Form
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;

        /// <summary>
        ///     The wrapper for the map displayed on the page. Stays <c>null</c>
        ///     until map.html reports that the Maps JavaScript API has loaded.
        /// </summary>
        private GoogleMap map;

        private static string PathToMapFile => new Uri(Path.GetFullPath("map.html")).AbsoluteUri;

        public MainForm()
        {
            InitializeComponent();

            engine = EngineFactory.Create();

            // #docfragment "GoogleMaps.Geolocation"
            // navigator.geolocation asks for a permission, which is denied
            // unless a permission handler grants it.
            engine.Profiles.Default.Permissions.RequestPermissionHandler =
                new Handler<RequestPermissionParameters, RequestPermissionResponse>(p =>
                    p.Type == PermissionType.Geolocation
                        ? RequestPermissionResponse.Grant()
                        : RequestPermissionResponse.Deny());
            // #enddocfragment "GoogleMaps.Geolocation"

            browser = engine.CreateBrowser();

            // #docfragment "GoogleMaps.InjectExternal"
            // Inject this form into the page as window.external, so that
            // map.html can call back into .NET.
            browser.InjectJsHandler = new Handler<InjectJsParameters>(OnInjectJs);
            // #enddocfragment "GoogleMaps.InjectExternal"

            browserView.InitializeFrom(browser);
            browser.Navigation.LoadUrl(PathToMapFile);

            FormClosed += MainForm_FormClosed;
        }

        /// <summary>
        ///     Called from map.html when the current position has been determined.
        /// </summary>
        public void OnLocationDetected(double latitude, double longitude)
        {
            BeginInvoke((Action) (() =>
                                     {
                                         latitudeValue.Value = (decimal) latitude;
                                         longitudeValue.Value = (decimal) longitude;
                                     }));
        }

        /// <summary>
        ///     Called from map.html when the current position cannot be determined.
        /// </summary>
        public void OnLocationFailed(string message)
        {
            // Chromium reports an empty message when it cannot determine the
            // position because the Google API keys are not configured.
            string details = string.IsNullOrWhiteSpace(message)
                ? "The current position could not be determined. Make sure the "
                  + "Google Maps Geolocation API is enabled and the Google API "
                  + "keys are configured through EngineOptions."
                : message;

            BeginInvoke((Action) (() => MessageBox.Show(this,
                                                        details,
                                                        "Geolocation is unavailable",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning)));
        }

        // #docfragment "GoogleMaps.MapInitialized"
        /// <summary>
        ///     Called from map.html once the Maps JavaScript API has loaded and
        ///     the map has been created.
        /// </summary>
        public void OnMapInitialized(IJsObject jsMap)
        {
            map = new GoogleMap(jsMap);
            BeginInvoke((Action) (() => mapControls.Enabled = true));
        }
        // #enddocfragment "GoogleMaps.MapInitialized"

        private void AddMarkerBtn_Click(object sender, EventArgs e)
        {
            double latitude = decimal.ToDouble(latitudeValue.Value);
            double longitude = decimal.ToDouble(longitudeValue.Value);
            InvokeOnMap(m =>
                           {
                               m.SetCenter(latitude, longitude);
                               m.AddMarker(latitude, longitude);
                           });
        }

        /// <summary>
        ///     Runs the given action on the map from a background thread.
        /// </summary>
        /// <remarks>
        ///     The JavaScript calls the action makes block the calling thread
        ///     until the browser returns the result, so they must not be made
        ///     on the UI thread.
        /// </remarks>
        private void InvokeOnMap(Action<GoogleMap> action)
        {
            GoogleMap currentMap = map;
            if (currentMap == null)
            {
                return;
            }

            Task.Run(() =>
                        {
                            try
                            {
                                action(currentMap);
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine(exception);
                            }
                        });
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private void MyLocationBtn_Click(object sender, EventArgs e)
        {
            browser.MainFrame?.ExecuteJavaScript("showMyLocation()");
        }

        private void OnInjectJs(InjectJsParameters parameters)
        {
            // Inject window.external into the HTML page.
            IJsObject window = parameters.Frame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["external"] = this;
        }

        private void ZoomInBtn_Click(object sender, EventArgs e)
        {
            InvokeOnMap(m => m.Zoom++);
        }

        private void ZoomOutBtn_Click(object sender, EventArgs e)
        {
            InvokeOnMap(m => m.Zoom--);
        }
    }
}
