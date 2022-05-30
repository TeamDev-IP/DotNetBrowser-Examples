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
Imports System.Collections.Generic
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Js
Imports DotNetBrowser.Js.Collections

Namespace JavaScriptBridge.Arrays
	''' <summary>
	'''     This example demonstrates how to use IJsArray to
	'''     simplify work with the common JavaScript arrays.
	''' </summary>
	Friend Class Program
		Private Const JsArray As String = "['Cabbage', 'Turnip', 'Radish', 'Carrot']"

		Public Shared Sub Main(ByVal args() As String)
			Using engine As IEngine = EngineFactory.Create()
				Using browser As IBrowser = engine.CreateBrowser()
					Dim arrayObject As IJsArray = browser.MainFrame.ExecuteJavaScript(Of IJsArray)(JsArray).Result
					Console.Out.WriteLine($"Item count: {arrayObject.Count}")
					Dim list As IReadOnlyList(Of String) = arrayObject.ToReadOnlyList(Of String)()
					For Each item As String In list
						Console.Out.WriteLine($"Item: {item}")
					Next item
				End Using
			End Using

			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub
	End Class
End Namespace
