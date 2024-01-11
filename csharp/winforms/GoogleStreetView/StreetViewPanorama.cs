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
using DotNetBrowser.Js;

namespace GoogleStreetView.WinForms
{
    /// <summary>
    ///     The wrapper class for a street-view panorama object, which displays
    ///     the panorama for a given LatLng or panorama ID.
    /// </summary>
    internal class StreetViewPanorama
    {
        private readonly IJsObject panorama;
        private LatLng position;
        private Pov pov;


        /// <summary>
        ///     Gets the current panorama ID for the Street View panorama.
        /// </summary>
        public string Pano { get; private set; }

        /// <summary>
        ///     Gets or sets the Position of the current panorama.
        /// </summary>
        public LatLng Position
        {
            get { return position; }
            set
            {
                position = value;
                if (position != null)
                {
                    IJsObject latlng = panorama.Frame
                                               .ExecuteJavaScript<IJsObject
                                                >($"new google.maps.LatLng({position.Latitude},{position.Longitude})")
                                               .Result;
                    panorama.Invoke("setPosition", latlng);
                }
            }
        }

        /// <summary>
        ///     Gets or sets the current point of view for the Street View panorama.
        /// </summary>
        public Pov Pov
        {
            get { return pov; }
            set
            {
                pov = value;
                if (pov != null)
                {
                    object jsPov =
                        panorama.Frame.ParseJsonString($"{{ \"heading\": {pov.Heading}, \"pitch\": {pov.Pitch} }}");
                    panorama.Invoke("setPov", jsPov);
                }
            }
        }


        public event EventHandler PanoChanged;
        public event EventHandler PovChanged;
        public event EventHandler PositionChanged;


        public StreetViewPanorama(IJsObject jsPanorama)
        {
            panorama = jsPanorama;
            AddListener("pano_changed", OnPanoramaChanged);
            AddListener("pov_changed", OnPovChanged);
            AddListener("position_changed", OnPositionChanged);
        }


        private void AddListener(string eventName, Action handler)
        {
            panorama.Invoke("addListener", eventName, handler);
        }

        private void OnPanoramaChanged()
        {
            Pano = panorama.Invoke<string>("getPano");
            PanoChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnPositionChanged()
        {
            IJsObject jsPosition = panorama.Invoke<IJsObject>("getPosition");
            double latitude = jsPosition.Invoke<double>("lat");
            double longitude = jsPosition.Invoke<double>("lng");
            position = new LatLng(latitude, longitude);
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnPovChanged()
        {
            IJsObject jsPov = panorama.Invoke<IJsObject>("getPov");
            pov = new Pov(jsPov.Properties["heading"], jsPov.Properties["pitch"]);
            PovChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
