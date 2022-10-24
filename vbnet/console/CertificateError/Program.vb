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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Handlers
Imports DotNetBrowser.Net.Certificates
Imports DotNetBrowser.Net.Handlers

''' <summary>
'''     The sample demonstrates how to handle SSL certificate errors.
''' </summary>
Friend Class Program
    Public Shared Sub Main()
        Using engine As IEngine = EngineFactory.Create()
            Using browser As IBrowser = engine.CreateBrowser()

                engine.Profiles.Default.Network.VerifyCertificateHandler =
                    New Handler(Of VerifyCertificateParameters,
                                   VerifyCertificateResponse)(AddressOf HandleCertError)
                browser.Navigation.LoadUrl("https://untrusted-root.badssl.com/").Wait()

            End Using
        End Using

        Console.WriteLine("Press any key to terminate...")
        Console.ReadKey()
    End Sub

    Private Shared Function HandleCertError(errorParams As VerifyCertificateParameters) As VerifyCertificateResponse
        Dim certificate As Certificate = errorParams.Certificate

        For Each certificateVerificationError As CertificateVerificationError In errorParams.VerifyErrors
            Console.WriteLine("CertificateVerifyStatus = " & certificateVerificationError.VerifyStatus.ToString())
            Console.WriteLine("Short description = " & certificateVerificationError.ShortDescription)
            Console.WriteLine("Detailed description = " & certificateVerificationError.DetailedDescription)
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
        Return VerifyCertificateResponse.Valid()
    End Function
End Class
