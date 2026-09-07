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
using System.Globalization;
using DotNetBrowser.Frames;
using DotNetBrowser.Js;

namespace GoogleMaps.WinForms
{
    /// <summary>
    ///     The wrapper class for the <c>google.maps.Map</c> object displayed on the page.
    /// </summary>
    internal class GoogleMap
    {
        /// <summary>
        ///     The zoom levels supported by Google Maps.
        /// </summary>
        private const int MinZoomLevel = 0;
        private const int MaxZoomLevel = 21;

        private readonly IJsObject map;

        /// <summary>
        ///     Gets or sets the zoom level of the map. The value being set is
        ///     clamped to the range supported by Google Maps.
        /// </summary>
        // #docfragment "GoogleMaps.Zoom"
        public int Zoom
        {
            get { return (int) map.Invoke<double>("getZoom"); }

            set
            {
                int zoom = Math.Min(MaxZoomLevel, Math.Max(MinZoomLevel, value));
                map.Invoke("setZoom", zoom);
            }
        }
        // #enddocfragment "GoogleMaps.Zoom"

        private IFrame Frame => map.Frame;

        public GoogleMap(IJsObject jsMap)
        {
            map = jsMap ?? throw new ArgumentNullException(nameof(jsMap));
        }

        /// <summary>
        ///     Puts a marker at the given position.
        /// </summary>
        public void AddMarker(double latitude, double longitude)
        {
            // #docfragment "GoogleMaps.AddMarker"
            // Create the marker in the page context and attach it to the map.
            IJsObject marker = Frame
                              .ExecuteJavaScript<IJsObject>(
                                   "new google.maps.marker.AdvancedMarkerElement({map: map})")
                              .Result;

            // AdvancedMarkerElement exposes the position as a property rather
            // than through a setter method.
            marker.Properties["position"] = ToLatLngLiteral(latitude, longitude);
            // #enddocfragment "GoogleMaps.AddMarker"
        }

        /// <summary>
        ///     Centers the map on the given position.
        /// </summary>
        public void SetCenter(double latitude, double longitude)
        {
            map.Invoke("setCenter", ToLatLngLiteral(latitude, longitude));
        }

        /// <summary>
        ///     Creates a JavaScript <c>LatLngLiteral</c> object for the given coordinates.
        /// </summary>
        private object ToLatLngLiteral(double latitude, double longitude)
        {
            // The invariant culture is used on purpose: JSON requires a dot as
            // the decimal separator regardless of the current locale.
            string json = string.Format(CultureInfo.InvariantCulture,
                                        "{{ \"lat\": {0}, \"lng\": {1} }}",
                                        latitude,
                                        longitude);
            return Frame.ParseJsonString(json);
        }
    }
}
