﻿#Region "Copyright"

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

Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Js

Namespace JavaScriptBridge.Arrays
	''' <summary>
	'''     This example demonstrates how to implement an IJsObject wrapper and
	'''     simplify work with the common JavaScript arrays.
	''' </summary>
	Friend Class Program
		Private Const JsArray As String = "['Cabbage', 'Turnip', 'Radish', 'Carrot']"

		#Region "Methods"

		Public Shared Sub Main(ByVal args() As String)
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")
						Dim arrayObject As IJsObject = browser.MainFrame.ExecuteJavaScript(Of IJsObject)(JsArray).Result

						Dim array As JsArray = arrayObject.AsArray()
						If array IsNot Nothing Then
							Console.Out.WriteLine("Item count: " & array.Count)
							For Each item As Object In array
								Console.Out.WriteLine("Item: " & item.ToString())
							Next item
						End If
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