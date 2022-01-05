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

Imports System
Imports System.IO
Imports System.Threading.Tasks
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Downloads.Handlers
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers

Namespace DownloadPdf
	''' <summary>
	'''     This example demonstrates how to download the PDF from the given URL.
	''' </summary>
	Friend Class Program

		Public Shared Sub Main()
			Dim url As String = "https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf"

			Try
				Using engine As IEngine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.OffScreen}.Build())
					Console.WriteLine("Engine created")

					'1. Disable PDF viewer to trigger PDF file download on loading the URL.
					engine.Profiles.Default.Plugins.Settings.PdfViewerEnabled = False
					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")

						' 2. Configure the download handler.
						Dim downloadFinishedTcs As New TaskCompletionSource(Of String)()
						browser.StartDownloadHandler = New Handler(Of StartDownloadParameters, StartDownloadResponse)(Function(p)
							Try
								Console.WriteLine("Starting download for: " & p.Download.Info.Url)
								Dim suggestedFileName As String = p.Download.Info.SuggestedFileName
								Dim targetPath As String = Path.GetFullPath(suggestedFileName)
								AddHandler p.Download.Finished, Sub(sender, args)
									downloadFinishedTcs.TrySetResult(targetPath)
								End Sub
								Return StartDownloadResponse.DownloadTo(targetPath)
							Catch e As Exception
								downloadFinishedTcs.TrySetException(e)
								Throw
							End Try
						End Function)

						' 2. Load the required web page and wait until it is loaded completely.
						Console.WriteLine("Loading " & url)
						browser.Navigation.LoadUrl(url).Wait()
						Console.WriteLine("URL loaded.")

						' 3. Wait until the download is finished.
						Dim downloadedUrl As String = downloadFinishedTcs.Task.Result
						Console.WriteLine("Download completed: " & downloadedUrl)
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
