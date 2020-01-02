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

Imports System
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Certificates
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers

Namespace CertificateError
	''' <summary>
	'''     Demonstrates how to handle SSL certificate errors.
	''' </summary>
	Friend Class Program
		#Region "Methods"

		Public Shared Sub Main()
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")
						engine.NetworkService.VerifyCertificateHandler = New Handler(Of CertificateVerifyHandlerParameters, CertificateVerifyResult)(AddressOf HandleCertError)
						browser.Navigation.LoadUrl("https://untrusted-root.badssl.com/").Wait()
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try
			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

		Private Shared Function HandleCertError(ByVal errorParams As CertificateVerifyHandlerParameters) As CertificateVerifyResult
			Dim certificate As Certificate = errorParams.Certificate

			For Each status As CertificateVerifyStatus In errorParams.VerifyStatuses
                Console.WriteLine("CertificateVerifyStatus = " & status.ToString())
            Next
            
			Console.WriteLine("SerialNumber = " & certificate.SerialNumber)
			Console.WriteLine("FingerPrint = " & certificate.Fingerprint)
			Console.WriteLine("CAFingerPrint = " & certificate.CaFingerPrint)

			Dim subject As String = certificate.Subject
			Console.WriteLine("Subject = " & subject)

			Dim issuer As String = certificate.Issuer
			Console.WriteLine("Issuer = " & issuer)

			Console.WriteLine("KeyUsages = " & String.Join(", ", certificate.KeyUsages))
			Console.WriteLine("ExtendedKeyUsages = " & String.Join(", ", certificate.ExtendedKeyUsages))

			Console.WriteLine("Expired = " & certificate.Expired)

			' Return Valid to ignore certificate error.
			Return CertificateVerifyResult.Valid
		End Function

		#End Region
	End Class
End Namespace
