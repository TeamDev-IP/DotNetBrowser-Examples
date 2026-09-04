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

Imports System.IO
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

Public Class SeleniumInstance
    
    Private RemoteDebuggingAddress as String

    ''' <summary>
    '''     The page shipped alongside the application, so that the scenario
    '''     does not depend on an external web site.
    ''' </summary>
    Private Shared ReadOnly Property StartPage As String
        Get
            Return New Uri(
                Path.Combine(Directory.GetCurrentDirectory(), "home.html")
                ).AbsoluteUri
        End Get
    End Property

    Public Event Connected As Action

    Public Sub New(debuggingPort As Integer)
        RemoteDebuggingAddress = $"localhost:{debuggingPort}"
    End Sub

    Public Async Function ConnectAndRun() As Task
        Await Task.Run(Function() ConnectAndRunAsync())
    End Function

    Private Async Function ConnectAndRunAsync() As Task
        Dim webDriver As IWebDriver = Await ConnectAsync()
        'Time for displaying the loaded page
        Await Task.Delay(3000)
        Await RunScenarioAsync(webDriver)
        webDriver.Quit()
    End Function

    Private Async Function ConnectAsync() As Task(Of IWebDriver)
        ' #docfragment "Selenium.Connect"
        Dim options As ChromeOptions = new ChromeOptions
        With options
            .DebuggerAddress = RemoteDebuggingAddress
        End With

        Dim webDriver As IWebDriver = new ChromeDriver(options)
        With webDriver
            .Url = StartPage
        End With

        ' Give FindElement time to wait for the page to load.
        webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10)
        ' #enddocfragment "Selenium.Connect"

        RaiseEvent Connected

        Return webDriver
    End Function

    Private Async Function RunScenarioAsync(webDriver As IWebDriver) As Task
        Dim evaluateLink As IWebElement = webDriver.FindElement(By.Id("evaluate"))
        evaluateLink.Click()

        Dim nameTextbox As IWebElement = webDriver.FindElement(By.Id("name"))
        nameTextbox.SendKeys("John Doe")

        Dim emailTextbox As IWebElement = webDriver.FindElement(By.Id("email"))
        emailTextbox.SendKeys("sales@teamdev.com")
    End Function

End Class
