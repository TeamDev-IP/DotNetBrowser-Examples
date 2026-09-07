#Region "Copyright"

' Copyright © 2026, TeamDev. All rights reserved.
' 
' Redistribution and use in source and/or binary forms, with or without
' modification, must retain the above copyright notice and the following
' disclaimer.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
' "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
' LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
' A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
' OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
' DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
' (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
' OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#End Region
Imports System
Imports System.Globalization
Imports DotNetBrowser.Frames
Imports DotNetBrowser.Js

Namespace GoogleMaps.WinForms
	''' <summary>
	'''     The wrapper class for the <c>google.maps.Map</c> object displayed on the page.
	''' </summary>
	Friend Class GoogleMap

		''' <summary>
		'''     The zoom levels supported by Google Maps.
		''' </summary>
		Private Const MinZoomLevel As Integer = 0
		Private Const MaxZoomLevel As Integer = 21

		Private ReadOnly map As IJsObject

		''' <summary>
		'''     Gets or sets the zoom level of the map. The value being set is
		'''     clamped to the range supported by Google Maps.
		''' </summary>
		' #docfragment "GoogleMaps.Zoom"
		Public Property Zoom() As Integer
			Get
				Return CInt(map.Invoke(Of Double)("getZoom"))
			End Get

			Set
				Dim zoomLevel As Integer = Math.Min(MaxZoomLevel, Math.Max(MinZoomLevel, Value))
				map.Invoke("setZoom", zoomLevel)
			End Set
		End Property
		' #enddocfragment "GoogleMaps.Zoom"

		Private ReadOnly Property Frame() As IFrame
			Get
				Return map.Frame
			End Get
		End Property

		Public Sub New(jsMap As IJsObject)
			If jsMap Is Nothing Then
				Throw New ArgumentNullException(NameOf(jsMap))
			End If
			map = jsMap
		End Sub

		''' <summary>
		'''     Puts a marker at the given position.
		''' </summary>
		Public Sub AddMarker(latitude As Double, longitude As Double)
			' #docfragment "GoogleMaps.AddMarker"
			' Create the marker in the page context and attach it to the map.
			Dim marker As IJsObject =
				Frame.ExecuteJavaScript(Of IJsObject)(
					"new google.maps.marker.AdvancedMarkerElement({map: map})").Result

			' AdvancedMarkerElement exposes the position as a property rather
			' than through a setter method.
			marker.Properties("position") = ToLatLngLiteral(latitude, longitude)
			' #enddocfragment "GoogleMaps.AddMarker"
		End Sub

		''' <summary>
		'''     Centers the map on the given position.
		''' </summary>
		Public Sub SetCenter(latitude As Double, longitude As Double)
			map.Invoke("setCenter", ToLatLngLiteral(latitude, longitude))
		End Sub

		''' <summary>
		'''     Creates a JavaScript <c>LatLngLiteral</c> object for the given coordinates.
		''' </summary>
		Private Function ToLatLngLiteral(latitude As Double, longitude As Double) As Object
			' The invariant culture is used on purpose: JSON requires a dot as
			' the decimal separator regardless of the current locale.
			Dim json As String = String.Format(CultureInfo.InvariantCulture,
											   "{{ ""lat"": {0}, ""lng"": {1} }}",
											   latitude,
											   longitude)
			Return Frame.ParseJsonString(json)
		End Function
	End Class
End Namespace
