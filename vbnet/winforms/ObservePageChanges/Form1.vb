#Region "Copyright"

' Copyright © 2023, TeamDev. All rights reserved.
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
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Js
Imports DotNetBrowser.WinForms

Namespace ObservePageChanges.WinForms
    ''' <summary>
    '''     This example demonstrates how to observe web page content changes on
    '''     .NET side using MutationObserver and JS-.NET bridge.
    ''' </summary>
    Partial Public Class Form1
        Inherits Form

        Private ReadOnly browser As IBrowser
        Private ReadOnly browserView As BrowserView
        Private ReadOnly engine As IEngine

        Public Sub New()
            InitializeComponent()
            browserView = New BrowserView With {.Dock = DockStyle.Fill}
            engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated}.Build())
            browser = engine.CreateBrowser()
            browserView.InitializeFrom(browser)

            Controls.Add(browserView)
            Task.Run(Sub()
                         browser.Navigation.LoadUrl(Path.GetFullPath("page.html")).Wait()

                         ' After the page is loaded successfully, we can configure the observer.
                         ConfigureObserver()
                     End Sub)
        End Sub

        Public Sub CharacterDataChanged(ByVal innerText As String)
            Console.WriteLine(innerText)
        End Sub

        Private Sub ConfigureObserver()
            ' Inject the listener .NET object into Javascript
            Dim window As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)("window").Result
            window.Properties("MutationObserverListener") = Me

            ' The script for configuring MutationObserver to observe the changes of
            ' the element with id 'countdown'.
            Dim observerScript As String = "var spanElement = document.getElementById('countdown');
            var observer = new MutationObserver(
            function(mutations){
            window.MutationObserverListener.CharacterDataChanged(spanElement.innerHTML);
            });
            var config = { childList: true };
            observer.observe(spanElement, config);"

            ' Execute the script that configures the observer.
            ' After the observer is configured, the .NET side starts receiving element changes.
            browser.MainFrame.ExecuteJavaScript(observerScript)
        End Sub

        Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            browser?.Dispose()
            engine?.Dispose()
        End Sub

    End Class
End Namespace
