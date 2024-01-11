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
Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine
Imports DotNetBrowser.WinForms

Public Class Form1
    Dim ReadOnly browser As IBrowser

    Public Sub New()
        InitializeComponent()

        Dim engine As IEngine = EngineFactory.Create()
        browser = engine.CreateBrowser()

        AddHandler Me.Closed, Sub(sender, args)
            engine.Dispose()
        End Sub

        Dim browserView As New BrowserView With {.Dock = DockStyle.Fill}
        browserView.InitializeFrom(browser)

        tableLayoutPanel1.Controls.Add(browserView, 0, 0)

        browser.Navigation.LoadUrl(Path.Combine(Directory.GetCurrentDirectory(), "form1.html"))
        AddHandler button1.Click, AddressOf button1_Click
    End Sub

    Private Sub button1_Click(sender As Object, e As EventArgs)
        browser.Focus()

        Dim document As IDocument = browser.MainFrame.Document

        'Get necessary input elements on the form.
        Dim firstName = DirectCast(document.GetElementById("firstname"), IInputElement)
        Dim lastName = DirectCast(document.GetElementById("lastname"), IInputElement)
        Dim submitForm1 = DirectCast(document.GetElementById("submitForm1"), IInputElement)

        'Fill the input elements.
        firstName.Value = "John"
        lastName.Value = "Doe"

        Dim waitEvent As New ManualResetEvent(False)

        'Subscribe to the navigation event which indicates that the new web page was loaded.
        AddHandler browser.Navigation.FrameDocumentLoadFinished, Sub(o, args)
            If args.Frame.IsMain Then
                waitEvent.Set()
            End If
        End Sub

        'Simulate click.
        submitForm1.Click()
        waitEvent.WaitOne()
        Thread.Sleep(500)

        document = browser.MainFrame.Document

        'Get the necessary elements of the new form. 
        Dim gender = DirectCast(document.GetElementById("gender"), ISelectElement)
        Dim checkbox = DirectCast(document.GetElementById("checkbox"), IInputElement)
        Dim submitForm2 = DirectCast(document.GetElementById("submitForm2"), IInputElement)

        'Choose the necessary 'option' element from the 'select' element.
        For Each item As IOptionElement In gender.Options
            If item.InnerText.StartsWith("Male") Then
                item.Selected = True
            End If
        Next

        'Put the flag on the 'checkbox' element.
        checkbox.Checked = True
        Thread.Sleep(500)

        'Simulate click.
        submitForm2.Click()
    End Sub
End Class
