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

Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

Public Class SeleniumInstance
    
    Private ApplicationFullPath as String
    Private RemoteDebuggingAddress as String

    Public Event Connected As Action

    Public Sub New(debuggingPort As Integer)
        ApplicationFullPath = Process.GetCurrentProcess().MainModule.FileName
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
        Dim options As ChromeOptions = new ChromeOptions
        With options
            .BinaryLocation = ApplicationFullPath
            .DebuggerAddress = RemoteDebuggingAddress
        End With

        Dim webDriver As IWebDriver = new ChromeDriver(options)
        With webDriver
            .Url = "https://www.teamdev.com/dotnetbrowser"
        End With

        RaiseEvent Connected

        Return webDriver
    End Function

    Private Async Function RunScenarioAsync(webDriver As IWebDriver) As Task
        Dim evaluateButton As IWebElement = webDriver.FindElement(By.XPath("//*[@id='header']/div[1]/div/ul/li[5]/a"))
        evaluateButton.Click()

        Dim nameTextbox As IWebElement = webDriver.FindElement(By.Name("name"))
        nameTextbox.SendKeys("John Doe")

        Dim emailTextbox As IWebElement = webDriver.FindElement(By.Name("email"))
        emailTextbox.SendKeys("sales@teamdev.com")
    End Function

End Class
