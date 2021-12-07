#Region "Copyright"

' Copyright 2021, TeamDev. All rights reserved.
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
Imports DotNetBrowser.Js

''' <summary>
'''     The wrapper class for a street-view panorama object, which displays
'''     the panorama for a given LatLng or panorama ID.
''' </summary>
Friend Class StreetViewPanorama
    Private ReadOnly panorama As IJsObject
    Private positionValue As LatLng
    Private povValue As Pov

    ''' <summary>
    '''     Gets the current panorama ID for the Street View panorama.
    ''' </summary>
    Private panoValue As String
    Public Property Pano() As String
        Get
            Return panoValue
        End Get
        Private Set(ByVal value As String)
            panoValue = value
        End Set
    End Property

    ''' <summary>
    '''     Gets or sets the Position of the current panorama.
    ''' </summary>
    Public Property Position() As LatLng
        Get
            Return positionValue
        End Get
        Set
            positionValue = value
            If positionValue IsNot Nothing Then
                Dim latlng As IJsObject = panorama.Frame.ExecuteJavaScript(Of IJsObject )($"new google.maps.LatLng({positionValue.Latitude},{positionValue.Longitude})").Result
                panorama.Invoke("setPosition", latlng)
            End If
        End Set
    End Property

    ''' <summary>
    '''     Gets or sets the current point of view for the Street View panorama.
    ''' </summary>
    Public Property Pov() As Pov
        Get
            Return povValue
        End Get
        Set
            povValue = value
            If povValue IsNot Nothing Then
                Dim jsPov As Object = panorama.Frame.ParseJsonString($"{{ ""heading"": {povValue.Heading}, ""pitch"": {povValue.Pitch} }}")
                panorama.Invoke("setPov", jsPov)
            End If
        End Set
    End Property

    Public Event PanoChanged As EventHandler
    Public Event PovChanged As EventHandler
    Public Event PositionChanged As EventHandler

    Public Sub New(jsPanorama As IJsObject)
        panorama = jsPanorama
        AddListener("pano_changed", AddressOf OnPanoramaChanged)
        AddListener("pov_changed", AddressOf OnPovChanged)
        AddListener("position_changed", AddressOf OnPositionChanged)
    End Sub

    Private Sub AddListener(eventName As String, handler As Action)
        panorama.Invoke("addListener", eventName, handler)
    End Sub

    Private Sub OnPanoramaChanged()
        Pano = panorama.Invoke(Of String)("getPano")
        PanoChangedEvent?.Invoke(Me, EventArgs.Empty)
    End Sub

    Private Sub OnPositionChanged()
        Dim jsPosition = panorama.Invoke(Of IJsObject)("getPosition")
        Dim latitude = jsPosition.Invoke(Of Double)("lat")
        Dim longitude = jsPosition.Invoke(Of Double)("lng")
        positionValue = New LatLng(latitude, longitude)
        PositionChangedEvent?.Invoke(Me, EventArgs.Empty)
    End Sub

    Private Sub OnPovChanged()
        Dim jsPov = panorama.Invoke(Of IJsObject)("getPov")
        povValue = New Pov(jsPov.Properties("heading"), jsPov.Properties("pitch"))
        PovChangedEvent?.Invoke(Me, EventArgs.Empty)
    End Sub

End Class