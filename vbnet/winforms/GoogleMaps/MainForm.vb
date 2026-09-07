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
Imports System.Diagnostics
Imports System.IO
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Js
Imports DotNetBrowser.Permissions
Imports DotNetBrowser.Permissions.Handlers

Namespace GoogleMaps.WinForms
	''' <summary>
	'''     This example demonstrates how to display Google Maps in a WinForms
	'''     application and drive the Maps JavaScript API from .NET: change the
	'''     zoom level, put markers on the map, and center the map on the
	'''     current location.
	''' </summary>
	''' <remarks>
	'''     To make this example work, configure a valid Google API key in
	'''     map.html. Because the page is loaded from the local file system, the
	'''     key must not be restricted by HTTP referrer.
	'''     The "My location" button additionally requires the Google Maps
	'''     Geolocation API to be enabled for the Chromium engine. See
	'''     https://teamdev.com/dotnetbrowser/docs/guides/gs/engine/#google-apis
	''' </remarks>
	Partial Public Class MainForm
		Inherits Form

		Private ReadOnly browser As IBrowser
		Private ReadOnly engine As IEngine

		''' <summary>
		'''     The wrapper for the map displayed on the page. Stays <c>Nothing</c>
		'''     until map.html reports that the Maps JavaScript API has loaded.
		''' </summary>
		Private map As GoogleMap

		Private Shared ReadOnly Property PathToMapFile() As String
			Get
				Return (New Uri(Path.GetFullPath("map.html"))).AbsoluteUri
			End Get
		End Property

		Public Sub New()
			InitializeComponent()

			engine = EngineFactory.Create()

			' #docfragment "GoogleMaps.Geolocation"
			' navigator.geolocation asks for a permission, which is denied
			' unless a permission handler grants it.
			engine.Profiles.Default.Permissions.RequestPermissionHandler =
				New Handler(Of RequestPermissionParameters, RequestPermissionResponse)(
					Function(p)
						If p.Type = PermissionType.Geolocation Then
							Return RequestPermissionResponse.Grant()
						End If
						Return RequestPermissionResponse.Deny()
					End Function)
			' #enddocfragment "GoogleMaps.Geolocation"

			browser = engine.CreateBrowser()

			' #docfragment "GoogleMaps.InjectExternal"
			' Inject this form into the page as window.external, so that
			' map.html can call back into .NET.
			browser.InjectJsHandler = New Handler(Of InjectJsParameters)(AddressOf OnInjectJs)
			' #enddocfragment "GoogleMaps.InjectExternal"

			browserView.InitializeFrom(browser)
			browser.Navigation.LoadUrl(PathToMapFile)

			AddHandler Me.FormClosed, AddressOf MainForm_FormClosed
		End Sub

		''' <summary>
		'''     Called from map.html when the current position has been determined.
		''' </summary>
		Public Sub OnLocationDetected(latitude As Double, longitude As Double)
			BeginInvoke(New Action(Sub()
									   latitudeValue.Value = CDec(latitude)
									   longitudeValue.Value = CDec(longitude)
								   End Sub))
		End Sub

		''' <summary>
		'''     Called from map.html when the current position cannot be determined.
		''' </summary>
		Public Sub OnLocationFailed(message As String)
			' Chromium reports an empty message when it cannot determine the
			' position because the Google API keys are not configured.
			Dim details As String = If(String.IsNullOrWhiteSpace(message),
									   "The current position could not be determined. Make sure the " &
									   "Google Maps Geolocation API is enabled and the Google API " &
									   "keys are configured through EngineOptions.",
									   message)

			BeginInvoke(New Action(Sub()
									   MessageBox.Show(Me,
													   details,
													   "Geolocation is unavailable",
													   MessageBoxButtons.OK,
													   MessageBoxIcon.Warning)
								   End Sub))
		End Sub

		' #docfragment "GoogleMaps.MapInitialized"
		''' <summary>
		'''     Called from map.html once the Maps JavaScript API has loaded and
		'''     the map has been created.
		''' </summary>
		Public Sub OnMapInitialized(jsMap As IJsObject)
			map = New GoogleMap(jsMap)
			BeginInvoke(New Action(Sub() mapControls.Enabled = True))
		End Sub
		' #enddocfragment "GoogleMaps.MapInitialized"

		Private Sub AddMarkerBtn_Click(sender As Object, e As EventArgs) Handles AddMarkerBtn.Click
			Dim latitude As Double = Decimal.ToDouble(latitudeValue.Value)
			Dim longitude As Double = Decimal.ToDouble(longitudeValue.Value)
			InvokeOnMap(Sub(m)
							m.SetCenter(latitude, longitude)
							m.AddMarker(latitude, longitude)
						End Sub)
		End Sub

		''' <summary>
		'''     Runs the given action on the map from a background thread.
		''' </summary>
		''' <remarks>
		'''     The JavaScript calls the action makes block the calling thread
		'''     until the browser returns the result, so they must not be made
		'''     on the UI thread.
		''' </remarks>
		Private Sub InvokeOnMap(action As Action(Of GoogleMap))
			Dim currentMap As GoogleMap = map
			If currentMap Is Nothing Then
				Return
			End If

			Task.Run(Sub()
						 Try
							 action(currentMap)
						 Catch exception As Exception
							 Debug.WriteLine(exception)
						 End Try
					 End Sub)
		End Sub

		Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs)
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Sub MyLocationBtn_Click(sender As Object, e As EventArgs) Handles MyLocationBtn.Click
			browser.MainFrame?.ExecuteJavaScript("showMyLocation()")
		End Sub

		Private Sub OnInjectJs(parameters As InjectJsParameters)
			' Inject window.external into the HTML page.
			Dim window As IJsObject = parameters.Frame.ExecuteJavaScript(Of IJsObject)("window").Result
			window.Properties("external") = Me
		End Sub

		Private Sub ZoomInBtn_Click(sender As Object, e As EventArgs) Handles ZoomInBtn.Click
			InvokeOnMap(Sub(m) m.Zoom += 1)
		End Sub

		Private Sub ZoomOutBtn_Click(sender As Object, e As EventArgs) Handles ZoomOutBtn.Click
			InvokeOnMap(Sub(m) m.Zoom -= 1)
		End Sub
	End Class
End Namespace
