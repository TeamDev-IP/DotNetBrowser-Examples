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
Imports System.IO.Compression
Imports System.Linq
Imports System.Reflection

Namespace ChromiumBinariesResolver.Wpf
	Public Class BinariesResolver
		Inherits BinariesResolverBase

		Private Const UriTemplate As String = "https://storage.googleapis.com/cloud.teamdev.com/downloads/dotnetbrowser/{0}/dotnetbrowser-net45-{0}.zip"

		Public Sub New()
			MyBase.New(UriTemplate)
		End Sub


		Protected Overrides Function PrepareRequest(ByVal assemblyName As AssemblyName) As String
			'Use only the major and minor version components if the build component is 0.
			Dim fieldCount As Integer = If(assemblyName.Version.Build = 0, 2, 3)
			Return String.Format(RequestUri, assemblyName.Version.ToString(fieldCount))
		End Function

		Protected Overrides Function ProcessResponse(ByVal responseBody As Stream, ByVal assemblyName As AssemblyName) As System.Reflection.Assembly
			'The downloaded bytes represent a ZIP archive. Locate the DLL we need in this ar
			Dim archive As New ZipArchive(responseBody)
			Dim binariesDllEntry As ZipArchiveEntry = archive.Entries.FirstOrDefault(Function(entry) entry.FullName.EndsWith(".dll") AndAlso entry.FullName.Contains(assemblyName.Name))
			If binariesDllEntry Is Nothing Then
				Return Nothing
			End If

			'Unzip the found entry and load the DLL.
			OnStatusUpdated("Unzipping Chromium binaries")
			Dim unzippedEntryStream As Stream
			unzippedEntryStream = binariesDllEntry.Open()
			Using unzippedEntryStream
				Using memoryStream As New MemoryStream()
					unzippedEntryStream.CopyTo(memoryStream)
					OnStatusUpdated("Loading Chromium binaries assembly")
					Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.Load(memoryStream.ToArray())
					OnStatusUpdated("Chromium binaries assembly loaded.", True)
					Return assembly
				End Using
			End Using
		End Function

	End Class
End Namespace
