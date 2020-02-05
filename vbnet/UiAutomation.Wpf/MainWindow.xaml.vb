#Region "Copyright"

' Copyright © 2020, TeamDev. All rights reserved.
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

Imports System.Threading.Tasks
Imports System.Windows.Automation
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine

Partial Public Class MainWindow
    Inherits Window

    Private browser As IBrowser
    Private engine As IEngine

#Region "Constructors"

    Public Sub New()
        Try
            Task.Run(Sub()
                Dim builder = New EngineOptions.Builder With {
                        .RenderingMode = RenderingMode.HardwareAccelerated
                        }
                builder.ChromiumSwitches.Add("--force-renderer-accessibility")
                engine = EngineFactory.Create(builder.Build())
                browser = engine.CreateBrowser()
            End Sub).ContinueWith(Sub(t)
                BrowserView.InitializeFrom(browser)
                browser.Navigation.LoadUrl("https://teamdev.com/dotnetbrowser")
            End Sub, TaskScheduler.FromCurrentSynchronizationContext())

            InitializeComponent()
        Catch exception As Exception
            Debug.WriteLine(exception)
        End Try
    End Sub

#End Region

#Region "Methods"

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        TextOutput.Clear()
        Task.Run(Sub()
            Dim currentProcess As Process = Process.GetCurrentProcess()
            Dim chromiumElement As AutomationElement = GetChromiumElement(currentProcess)
            If chromiumElement IsNot Nothing Then
                Log("-- Element Properties --")
                Dim properties() As AutomationProperty = chromiumElement.GetSupportedProperties()
                For Each prop As AutomationProperty In properties
                    Log("ProgrammaticName: " & prop.ProgrammaticName)
                    Log(vbTab & "Property Name: " & Automation.PropertyName(prop))
                    Dim currentPropertyValue = chromiumElement.GetCurrentPropertyValue(prop)
                    Log(vbTab & "Property Value: " & Convert.ToString(currentPropertyValue))
                Next prop
                Log("-- Element Patterns --")
                Dim patterns() As AutomationPattern = chromiumElement.GetSupportedPatterns()
                For Each pattern As AutomationPattern In patterns
                    Log("ProgrammaticName: " & pattern.ProgrammaticName)
                    Log(vbTab & "Pattern Name: " & Automation.PatternName(pattern))
                    Dim currentPattern = chromiumElement.GetCurrentPattern(pattern)
                    Log(vbTab & "Pattern Value: " & currentPattern.ToString())
                    If TypeOf currentPattern Is ValuePattern Then
                        Dim valuePattern = TryCast(currentPattern, ValuePattern)
                        Dim value As String = valuePattern.Current.Value
                        Log(vbTab & "ValuePattern Value: " & value)
                    End If
                Next pattern

                Dim children = chromiumElement.FindAll(TreeScope.Descendants, Condition.TrueCondition)
                Log("-- Element Children --")
                Log("Children count: " & children.Count)
                Log("-- End --")
            Else
                Log("-- Chromium automation element not found --")
            End If
        End Sub)
    End Sub

    Private Shared Function GetChromiumElement(process As Process) As AutomationElement
        Dim element As AutomationElement = Nothing
        If process IsNot Nothing AndAlso process.MainWindowHandle <> IntPtr.Zero Then
            Dim rootElement As AutomationElement = AutomationElement.FromHandle(process.MainWindowHandle)
            If rootElement Is Nothing Then
                Return Nothing
            End If
            Dim conditions As Condition = New AndCondition(
                New PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_RenderWidgetHostHWND"),
                New PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document))
            element = rootElement.FindFirst(TreeScope.Descendants, conditions)
        End If
        Return element
    End Function

    Private Sub Log(text As String)
        Dispatcher.BeginInvoke(CType(Sub() TextOutput.AppendText(text & Environment.NewLine), Action))
    End Sub

    Private Sub Window_Closed(sender As Object, e As EventArgs)
        engine?.Dispose()
    End Sub

#End Region
End Class