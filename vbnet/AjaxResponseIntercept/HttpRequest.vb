#Region "Copyright"

' Copyright © 2022, TeamDev. All rights reserved.
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

Imports System.Text
Imports DotNetBrowser.Net

Friend NotInheritable Class HttpRequest
    Private _completed As Boolean
    Private ReadOnly _responseData As New List(Of Byte)()

    ''' <summary>
    ''' Aggregated response data.
    ''' </summary>
    Public ReadOnly Property ResponseData() As IReadOnlyList(Of Byte)
        Get
            Return _responseData
        End Get
    End Property

    ''' <summary>
    '''     Indicates whether the request is already completed.
    ''' </summary>
    Public ReadOnly Property IsCompleted() As Boolean
        Get
            Return _completed
        End Get
    End Property

    ''' <summary>
    '''     The HTTP method used to perform this request.
    ''' </summary>
    Public ReadOnly Property Method() As String

    ''' <summary>
    '''     The MIME type from the request headers.
    ''' </summary>
    Public Property MimeType() As MimeType

    ''' <summary>
    '''     The string representation of the response received.
    '''     Is incomplete until the request itself is completed.
    ''' </summary>
    Public ReadOnly Property Response() As String
        Get
            Return Encoding.UTF8.GetString(_responseData.ToArray())
        End Get
    End Property

    ''' <summary>
    '''     The request URL.
    ''' </summary>
    Public ReadOnly Property Url() As String

    Public Sub New(ByVal requestUrl As String, ByVal requestMethod As String)
        Url = requestUrl
        Method = requestMethod
    End Sub

    ''' <summary>
    '''     Mark the request as completed.
    ''' </summary>
    Public Sub Complete()
        _completed = True
    End Sub

    ''' <summary>
    '''     Append received response bytes.
    ''' </summary>
    ''' <param name="data"></param>
    Public Sub AppendResponseBytes(ByVal data() As Byte)
        If data IsNot Nothing Then
            _responseData.AddRange(data)
        End If
    End Sub

End Class
