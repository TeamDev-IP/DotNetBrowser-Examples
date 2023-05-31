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
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Print
Imports DotNetBrowser.Print.Handlers

''' <summary>
'''     This example demonstrates how to load a web page and print it to PDF.
''' </summary>
Friend Class Program
    ' #docfragment "PrintToPdf"
    Public Shared Sub Main()
        Dim engineOptions As New EngineOptions.Builder With {
                .RenderingMode = RenderingMode.OffScreen,
                .LicenseKey = "your license key"
                }

        Using engine = EngineFactory.Create(engineOptions.Build())
            Using browser = engine.CreateBrowser()

                browser.Navigation.LoadUrl(Path.GetFullPath("template.html")).Wait()
                ' #enddocfragment "PrintToPdf"
                FillInData(browser)
                ' #docfragment "PrintToPdf"

                Dim whenPrintCompleted = ConfigurePrinting(browser)
                browser.MainFrame.Print()
                Dim resultPath = whenPrintCompleted.Task.Result
                Console.WriteLine($"PDF is generated: {resultPath}")
                Console.WriteLine("Press any key to terminate...")
                Console.ReadKey()
            End Using
        End Using
    End Sub

    Private Shared Function ConfigurePrinting(browser As IBrowser) _
        As TaskCompletionSource(Of String)
        ' Tell the browser to print automatically instead of showing the print preview.
        browser.RequestPrintHandler =
            New Handler(Of RequestPrintParameters, RequestPrintResponse)(
                Function(p) RequestPrintResponse.Print())

        Dim whenCompleted As New TaskCompletionSource(Of String)()
        ' Configure how the browser prints an HTML page.
        browser.PrintHtmlContentHandler =
            New Handler(Of PrintHtmlContentParameters, PrintHtmlContentResponse)(
                Function(parameters)
                    ' Use the PDF printer.
                    Dim printer = parameters.Printers.Pdf
                    Dim job = printer.PrintJob

                    ' Generate a random name for PDF file.
                    Dim guid As Guid = Guid.NewGuid()
                    Dim path As String = IO.Path.GetFullPath($"{guid}.pdf")
                    job.Settings.PdfFilePath = path

                    ' Remove white areas on the sides.
                    job.Settings.PageMargins = PageMargins.None
                    ' Remove default browser headers and footers.
                    job.Settings.PrintingHeaderFooterEnabled = False
                    AddHandler job.PrintCompleted, Sub(o, e) whenCompleted.SetResult(path)

                    ' Proceed with printing using the PDF printer.
                    Return PrintHtmlContentResponse.Print(printer)
                End Function)
        Return whenCompleted
    End Function
    ' #enddocfragment "PrintToPdf"

    Private Shared Sub FillInData(browser As IBrowser)
        Dim accountNumber = "123-4567"
        Dim name = "Dr. Emmett Brown"
        Dim address = "1640 Riverside Drive"
        Dim reportingPeriod = "Oct 25 — November 25, 1985"

        ' This JavaScript function is embedded into the template HTML page.
        ' Since this is a regular web page, you can use any JavaScript library,
        ' WebGL, SVG, and other technologies available in Chromium.
        browser.MainFrame.ExecuteJavaScript(
            $"setBillInfo('{accountNumber}', '{name}', '{address}', '{reportingPeriod}')")

        ' Arbitrary numbers.
        Dim dayCost = 500
        Dim nightCost = 312
        Dim dayUsage = 1.212
        Dim nightUsage = 88

        browser.MainFrame.ExecuteJavaScript(
            $"addCharge('Day Tariff', {dayUsage}, {dayCost});" &
            $"addCharge('Night Tariff', {nightUsage}, {nightCost});")
    End Sub
End Class
