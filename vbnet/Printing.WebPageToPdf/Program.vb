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
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Browser.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Print
Imports DotNetBrowser.Print.Handlers
Imports DotNetBrowser.Print.Settings

Namespace Printing.WebPageToPdf
	''' <summary>
	'''     This example demonstrates how to load a web page and print it to PDF.
	''' </summary>
	Friend Class Program
		Public Shared Sub Main()
			Dim url As String = "https://dotnetbrowser-support.teamdev.com/docs/guides/gs/printing.html"
			Dim pdfFilePath As String = Path.GetFullPath("result.pdf")

			Dim viewWidth As UInteger = 1024
			Dim viewHeight As UInteger = 768
			Dim browserSize As New Size(viewWidth, viewHeight)
			Try
				Using engine As IEngine = EngineFactory.Create(New EngineOptions.Builder With {
					.RenderingMode = RenderingMode.OffScreen,
					.GpuDisabled = True
				}.Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")
						' 1. Resize browser to the required dimension.
						browser.Size = browserSize

						' 2. Load the required web page and wait until it is loaded completely.
						Console.WriteLine("Loading " & url)
						browser.Navigation.LoadUrl(url).Wait()


						' 3. Configure print handlers.
						browser.RequestPrintHandler = New Handler(Of RequestPrintParameters, RequestPrintResponse)(Function(p)
								Return RequestPrintResponse.Print()
						End Function)

						Dim printCompletedTcs As New TaskCompletionSource(Of String)()
						browser.PrintHtmlContentHandler = New Handler(Of PrintHtmlContentParameters, PrintHtmlContentResponse)(Function(p)
								Try
									' Get the print job for the built-in PDF printer.
									Dim pdfPrinter As PdfPrinter(Of DotNetBrowser.Print.PdfPrinter.IHtmlSettings) = p.Printers.Pdf
									Dim printJob As IPrintJob(Of DotNetBrowser.Print.PdfPrinter.IHtmlSettings) = pdfPrinter.PrintJob

									' Apply the necessary print settings
									printJob.Settings.Apply(Sub(s)
										s.PaperSize = pdfPrinter.Capabilities.PaperSizes.FirstOrDefault(Function(size) size.Name.Contains("A4"))
										s.PrintingHeaderFooterEnabled = True
										' Specify the path to save the result.
										s.PdfFilePath = pdfFilePath
									End Sub)

									Dim browserUrl As String = p.Browser.Url
									AddHandler printJob.PrintCompleted, Sub(sender, args)
										printCompletedTcs.TrySetResult(browserUrl)
									End Sub

									' Tell Chromium to use the built-in PDF printer for printing.
									Return PrintHtmlContentResponse.Print(pdfPrinter)
								Catch e As Exception
									printCompletedTcs.TrySetException(e)
									Throw
								End Try
						End Function)

						' 4. Initiate printing and wait until it is completed.
						Console.WriteLine("URL loaded. Initiate printing")
						browser.MainFrame.Print()
						Dim printedUrl As String = printCompletedTcs.Task.Result
						Console.WriteLine("Printing completed for the URL: " & printedUrl)
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try

			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

	End Class
End Namespace