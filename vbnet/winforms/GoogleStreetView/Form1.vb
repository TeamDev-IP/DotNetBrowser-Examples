#Region "Copyright"

' Copyright © 2024, TeamDev. All rights reserved.
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

Namespace GoogleStreetView.WinForms
	''' <summary>
	'''     This example demonstrates how to embed Google Street View into your WinForms application
	'''     and integrate it to change the POV and position from the .NET side.
	'''     To make this example work, please configure the valid Google API key in streetviewevents.htm(line 7)
	''' </summary>
	Partial Public Class Form1
		Inherits Form

		Private ReadOnly browser As IBrowser
		Private ReadOnly engine As IEngine
		Private panorama As StreetViewPanorama

		Public Sub New()
			engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build())
			browser = engine.CreateBrowser()
			InitializeComponent()
			browserView1.InitializeFrom(browser)
			browser.InjectJsHandler = New Handler(Of InjectJsParameters)(AddressOf OnInjectJs)
			browser.Navigation.LoadUrl((New Uri(Path.GetFullPath("streetviewevents.htm"))).AbsoluteUri)
		End Sub

		'JS-.NET callback
		Public Sub OnPanoramaInitialized(jsPanorama As IJsObject)
			'Create a wrapper for StreetViewPanorama and subscribe to the events
			'to update the form properly.
			panorama = New StreetViewPanorama(jsPanorama)
			AddHandler panorama.PanoChanged, AddressOf OnPanoChanged
			AddHandler panorama.PovChanged, AddressOf OnPovChanged
			AddHandler panorama.PositionChanged, AddressOf OnPositionChanged
		End Sub

		Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
			Dim latitude As Decimal = latitudeValue.Value
			Dim longitude As Decimal = longitudeValue.Value
			Dim povHeading As String = povHeadingValue.Text
			Dim povPitch As String = povPitchValue.Text

			Task.Run(Sub()
				If panorama IsNot Nothing Then
					Try
						panorama.Position = New LatLng(Decimal.ToDouble(latitude), Decimal.ToDouble(longitude))

						panorama.Pov = New Pov(povHeading, povPitch)
					Catch exception As Exception
						Debug.WriteLine(exception)
					End Try
				End If
			End Sub)
		End Sub

		Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
			browser?.Dispose()
			engine?.Dispose()
		End Sub

		Private Sub OnInjectJs(parameters As InjectJsParameters)
			'Inject window.external into the HTML page
			Dim window As IJsObject = parameters.Frame.ExecuteJavaScript(Of IJsObject)("window").Result
			window.Properties("external") = Me
		End Sub

		Private Sub OnPanoChanged(sender As Object, e As EventArgs)
			Dim pano As String = panorama.Pano
			BeginInvoke(CType(Sub()
				panoValue.Text = pano
			End Sub, Action))
		End Sub

		Private Sub OnPositionChanged(sender As Object, e As EventArgs)
			Dim position As LatLng = panorama.Position
			BeginInvoke(CType(Sub()
										 latitudeValue.Value = CDec(position.Latitude)
										 longitudeValue.Value = CDec(position.Longitude)
			End Sub, Action))
		End Sub

		Private Sub OnPovChanged(sender As Object, e As EventArgs)
			Dim pov As Pov = panorama.Pov
			BeginInvoke(CType(Sub()
										 povHeadingValue.Text = pov.Heading
										 povPitchValue.Text = pov.Pitch
			End Sub, Action))
		End Sub

	End Class
End Namespace
