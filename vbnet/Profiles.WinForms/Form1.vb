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

Imports DotNetBrowser.Engine
Imports DotNetBrowser.Net.Proxy
Imports DotNetBrowser.Profile

Namespace Profiles.WinForms
    ''' <summary>
    '''     This example demonstrates how to work with different profiles
    '''     and its Browser instances.
    ''' </summary>
    Partial Public Class Form1
        Inherits Form

        Private ReadOnly engine As IEngine

        Public Sub New()
            engine =
                EngineFactory.Create(
                    New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())

            InitializeComponent()

            profilesList.DataSource = engine.Profiles.ToArray()
            profilesList.DisplayMember = "Name"
        End Sub

        Private Sub createProfileButton_Click(sender As Object, e As EventArgs)
            Dim profileNameText As String = profileName.Text
            If profileNameText IsNot Nothing Then
                'The incognito mode for the profile can be set by passing the second parameter to Profiles.Create().
                Dim profile As IProfile = engine.Profiles.Create(profileNameText)
                'Here is how to set a proxy per profile.
                profile.Proxy.Settings = New SystemProxySettings()

                profilesList.DataSource = engine.Profiles.ToArray()
            End If
        End Sub

        Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
            engine?.Dispose()
        End Sub

        Private Sub listBox1_DoubleClick_1(sender As Object, e As EventArgs)
            Dim selectedItem = TryCast(profilesList.SelectedItem, IProfile)
            If selectedItem IsNot Nothing Then
                Dim browserForm = New BrowserForm With {.Browser = selectedItem.CreateBrowser()}
                browserForm.Show(Me)
            End If
        End Sub

        Private Sub profilesList_MouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then
                Return
            End If

            Dim index As Integer = profilesList.IndexFromPoint(e.Location)
            If index <> ListBox.NoMatches Then
                Dim selectedItem = TryCast(profilesList.Items(index), IProfile)
                If selectedItem IsNot Nothing Then
                    engine.Profiles.Remove(selectedItem)
                    profilesList.DataSource = engine.Profiles.ToArray()
                End If
            End If
        End Sub

    End Class
End Namespace
