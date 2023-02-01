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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;

namespace GoogleStreetView.WinForms
{
    /// <summary>
    ///     This example demonstrates how to embed Google Street View into your WinForms application
    ///     and integrate it to change the POV and position from the .NET side.
    ///     To make this example work, please configure the valid Google API key in streetviewevents.htm(line 7)
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;
        private StreetViewPanorama panorama;

        public Form1()
        {
            engine = EngineFactory
               .Create(new EngineOptions.Builder
                           {
                               RenderingMode = RenderingMode.HardwareAccelerated
                           }
                          .Build());
            browser = engine.CreateBrowser();
            InitializeComponent();
            browserView1.InitializeFrom(browser);
            browser.InjectJsHandler = new Handler<InjectJsParameters>(OnInjectJs);
            browser.Navigation.LoadUrl(new Uri(Path.GetFullPath("streetviewevents.htm")).AbsoluteUri);
        }

        //JS-.NET callback
        public void OnPanoramaInitialized(IJsObject jsPanorama)
        {
            //Create a wrapper for StreetViewPanorama and subscribe to the events
            //to update the form properly.
            panorama = new StreetViewPanorama(jsPanorama);
            panorama.PanoChanged += OnPanoChanged;
            panorama.PovChanged += OnPovChanged;
            panorama.PositionChanged += OnPositionChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal latitude = latitudeValue.Value;
            decimal longitude = longitudeValue.Value;
            string povHeading = povHeadingValue.Text;
            string povPitch = povPitchValue.Text;

            Task.Run(() =>
            {
                if (panorama != null)
                {
                    try
                    {
                        panorama.Position = new LatLng(decimal.ToDouble(latitude),
                                                       decimal.ToDouble(longitude));

                        panorama.Pov = new Pov(povHeading, povPitch);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }
                }
            });
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private void OnInjectJs(InjectJsParameters parameters)
        {
            //Inject window.external into the HTML page
            IJsObject window = parameters.Frame.ExecuteJavaScript<IJsObject>("window").Result;
            window.Properties["external"] = this;
        }

        private void OnPanoChanged(object sender, EventArgs e)
        {
            string pano = panorama.Pano;
            BeginInvoke((Action) (() => { panoValue.Text = pano; }));
        }

        private void OnPositionChanged(object sender, EventArgs e)
        {
            LatLng position = panorama.Position;
            BeginInvoke((Action) (() =>
                                     {
                                         latitudeValue.Value = (decimal) position.Latitude;
                                         longitudeValue.Value = (decimal) position.Longitude;
                                     }));
        }

        private void OnPovChanged(object sender, EventArgs e)
        {
            Pov pov = panorama.Pov;
            BeginInvoke((Action) (() =>
                                     {
                                         povHeadingValue.Text = pov.Heading;
                                         povPitchValue.Text = pov.Pitch;
                                     }));
        }
    }
}
