#Region "Copyright"

' Copyright 2019, TeamDev. All rights reserved.
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
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Media

Namespace DefaultMediaStreamDeviceSample
	''' <summary>
	'''     The sample demonstrates how to get list of available audio and video capture devices,
	'''     register own SelectMediaDeviceHandler to provide Chromium with default audio/video capture
	'''     device to be used on a web page for working with webcam and accessing microphone.
	''' </summary>
	Public Class WindowMain
		Inherits Window

		#Region "Methods"

		Public Shared Sub Main()
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")

						Dim mediaDevices As IMediaDevices = engine.MediaDevices
						Console.WriteLine(vbLf & "Available audio capture devices:")
						PrintDevices(mediaDevices.AudioCaptureDevices)
						Console.WriteLine(vbLf & "Available video capture devices:")
						PrintDevices(mediaDevices.VideoCaptureDevices)

						mediaDevices.SelectMediaDeviceHandler = New Handler(Of SelectMediaDeviceParams, MediaDevice)(AddressOf SelectDevice)

						browser.Navigation.LoadUrl("https://alexandre.alapetite.fr/doc-alex/html5-webcam/index.en.html").Wait()
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try
			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

		Private Shared Sub PrintDevices(ByVal devices As IEnumerable(Of MediaDevice))
			For Each device As MediaDevice In devices
				Console.WriteLine($"- {device.Name}")
			Next device
		End Sub

		Private Shared Function SelectDevice(ByVal arg As SelectMediaDeviceParams) As MediaDevice
			Console.WriteLine(vbLf & "Requested device type: "& arg.Type.ToString())
			' Set first available device as default.
			Dim availableDevices As IEnumerable(Of MediaDevice) = arg.Devices
			Dim defaultDevice As MediaDevice = availableDevices.FirstOrDefault()
			If defaultDevice IsNot Nothing Then
				Console.WriteLine($"Default device is set to {defaultDevice.Name}")
			End If
			Return defaultDevice
		End Function

		#End Region
	End Class
End Namespace