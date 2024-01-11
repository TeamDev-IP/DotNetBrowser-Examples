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

Imports System.IO
Imports DotNetBrowser.Cookies

''' <summary>
'''     The example demonstrates how to share cookies between
'''     two Engine instances.
''' </summary>
Public Class Form1
    Private browserForm1 As BrowserForm
    Private browserForm2 As BrowserForm

    Private ReadOnly dataPath1 As String = Path.GetFullPath("data1")
    Private ReadOnly dataPath2 As String = Path.GetFullPath("data2")
    Private cookies As IEnumerable(Of Cookie)

    Public Sub New()
        InitializeComponent()
        AddHandler button1.Click, AddressOf button1_Click
        AddHandler button2.Click, AddressOf button2_Click
        AddHandler button3.Click, AddressOf button3_Click
    End Sub

    Private Sub button1_Click(sender As Object, e As EventArgs)
        LaunchBrowser(browserForm1, dataPath1)
    End Sub

    Private Sub LaunchBrowser(ByRef browserForm As BrowserForm, dataPath As String,
                              Optional ByVal cookies As IEnumerable(Of Cookie) = Nothing)
        If browserForm Is Nothing OrElse browserForm.IsDisposed Then
            browserForm = New BrowserForm(dataPath, cookies)
            browserForm.Text += $": {Path.GetFileNameWithoutExtension(dataPath)}"
            browserForm.Show(Me)
        End If
    End Sub

    Private Sub button3_Click(sender As Object, e As EventArgs)
        LaunchBrowser(browserForm2, dataPath2, cookies)
    End Sub

    Private Sub button2_Click(sender As Object, e As EventArgs)
        If browserForm1 IsNot Nothing AndAlso Not browserForm1.IsDisposed Then
            cookies = browserForm1.GetAllCookies()
            Debug.WriteLine("Cookies copied")
        End If
    End Sub
End Class
