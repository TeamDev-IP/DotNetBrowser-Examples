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

Imports DotNetBrowser.Js

Namespace JavaScriptBridge.Arrays
	Public Module JsObjectExtensions
		#Region "Methods"

		<System.Runtime.CompilerServices.Extension> _
		Public Function AsArray(ByVal jsObject As IJsObject) As JsArray
			Return JsArray.AsArray(jsObject)
		End Function

		#End Region
	End Module

	Public Class JsArray
		Implements IReadOnlyList(Of Object)

		Private ReadOnly jsObject As IJsObject

		#Region "Properties"

		Public ReadOnly Property Count() As Integer Implements IReadOnlyCollection(Of Object).Count
			Get
				Return Convert.ToInt32(DirectCast(jsObject.Properties("length"), Double))
			End Get
		End Property

		Default Public ReadOnly Property Item(ByVal index As Integer) As Object Implements IReadOnlyList(Of Object).Item
			Get
				Return jsObject.Properties(CUInt(index))
			End Get
		End Property

		#End Region

		#Region "Constructors"

		Private Sub New(ByVal jsObject As IJsObject)
			Me.jsObject = jsObject
		End Sub

		#End Region

		#Region "Methods"

		Public Function GetEnumerator() As IEnumerator(Of Object) Implements IEnumerable(Of Object).GetEnumerator
			Return New JsArrayEnumerator(Me)
		End Function

		Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
			Return GetEnumerator()
		End Function

		Public Shared Function AsArray(ByVal jsObject As IJsObject) As JsArray
			Return If(Not IsArray(jsObject), Nothing, New JsArray(jsObject))
		End Function

		Private Shared Function IsArray(ByVal jsObject As IJsObject) As Boolean
			If jsObject Is Nothing OrElse jsObject.IsDisposed Then
				Return False
			End If

			Dim isArrayFunction As IJsFunction = jsObject.Frame.ExecuteJavaScript(Of IJsFunction)("Array.isArray").Result
			Return isArrayFunction.Invoke(Of Boolean)(CType(Nothing, IJsObject), jsObject)
		End Function

		#End Region

		Private Class JsArrayEnumerator
			Implements IEnumerator(Of Object)

			Private ReadOnly array As JsArray
			Private ReadOnly count As Integer
			Private index As Integer

			#Region "Constructors"

			Public Sub New(ByVal array As JsArray)
				Me.array = array
				count = array.Count
				index = -1
			End Sub

			#End Region

			Public Sub Dispose() Implements System.IDisposable.Dispose

			End Sub

			Public Function MoveNext() As Boolean Implements IEnumerator(Of Object).MoveNext
				If index >= count - 1 Then
					Return False
				End If

				index += 1
				Return True

			End Function

			Public Sub Reset() Implements IEnumerator(Of Object).Reset
				index = -1
			End Sub

			Public ReadOnly Property Current() As Object Implements IEnumerator(Of Object).Current
				Get
					Return array(index)
				End Get
			End Property

			Private ReadOnly Property IEnumerator_Current() As Object Implements IEnumerator.Current
				Get
					Return Current
				End Get
			End Property
		End Class
	End Class
End Namespace