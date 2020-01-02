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
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry

Namespace Inspect
	''' <summary>
	'''     This sample demonstrates how to get DOM Node at a specific point on the web page.
	''' </summary>
	Friend Class Program
		#Region "Methods"

		Public Shared Sub Main()
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")
						browser.Size = New Size(700, 500)
						browser.Navigation.LoadUrl("http://www.teamdev.com").Wait()

						Dim pointInspection = browser.MainFrame.Inspect(New Point(50, 50))

						Console.WriteLine("Inspection result:")
						Console.WriteLine(vbTab & "AbsoluteImageUrl: {pointInspection.AbsoluteImageUrl}")
						Console.WriteLine(vbTab & "AbsoluteLinkUrl: {pointInspection.AbsoluteLinkUrl}")
						If pointInspection.LocalPoint IsNot Nothing Then
							Console.WriteLine(vbTab & "LocalPoint: ({pointInspection.LocalPoint.X},{pointInspection.LocalPoint.Y})")
						End If
						Console.WriteLine(vbTab & "Node: {pointInspection.Node?.NodeName}")
						Console.WriteLine(vbTab & "UrlNode: {pointInspection.UrlNode?.NodeName}")
					End Using
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try
			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

		#End Region
	End Class
End Namespace
