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

Namespace JavaScriptBridge.Promises
	Public Module JsObjectExtensions

		<System.Runtime.CompilerServices.Extension> _
		Public Function AsPromise(ByVal jsObject As IJsObject) As JsPromise
			Return JsPromise.AsPromise(jsObject)
		End Function

	End Module

	''' <summary>
	'''     An example of a wrapper for IJsObject that simplifies handling JavaScript promises.
	''' </summary>
	Public NotInheritable Class JsPromise
		Private ReadOnly jsObject As IJsObject

		Private Sub New(ByVal jsObject As IJsObject)
			Me.jsObject = jsObject
		End Sub

		''' <summary>
		'''     Creates a JavaScript promise representation of the IJsObject instance.
		''' </summary>
		''' <param name="jsObject">The JavaScript object.</param>
		''' <returns>The JsPromise representation of the object.</returns>
		Public Shared Function AsPromise(ByVal jsObject As IJsObject) As JsPromise
			Return If(Not IsPromise(jsObject), Nothing, New JsPromise(jsObject))
		End Function

		''' <summary>
		'''     Appends a rejection handler callback to the promise, and returns a new <see cref="JsPromise" />
		'''     resolving to the return value of the callback if it is called, or to its original fulfillment
		'''     value if the promise is instead fulfilled.
		''' </summary>
		''' <param name="onRejected">The rejection handler.</param>
		''' <returns>A new <see cref="JsPromise" /></returns>
		Public Function [Catch](ByVal onRejected As Func(Of Object, Object)) As JsPromise
			Dim newPromise As IJsObject = TryCast(jsObject.Invoke("catch", onRejected), IJsObject)
			Return New JsPromise(newPromise)
		End Function

		''' <summary>
		'''     Appends fulfillment and rejection handlers to the promise, and returns a <c>Task</c>
		'''     that becomes completed as soon as the promise is resolved.
		''' </summary>
		''' <returns>
		'''     a <c>Task</c> that becomes completed as soon as any of these handlers is called.
		''' </returns>
		Public Function ResolveAsync() As Task(Of Result)
			Dim promiseTcs As New TaskCompletionSource(Of Result)()
			[Then](Sub(obj)
				promiseTcs.TrySetResult(Fulfilled(obj))
			End Sub, Sub(obj)
				promiseTcs.TrySetResult(Rejected(obj))
			End Sub)

			promiseTcs.Task.ConfigureAwait(False)
			Return promiseTcs.Task
		End Function

		''' <summary>
		'''     Appends fulfillment and rejection handlers to the promise.
		''' </summary>
		''' <param name="onFulfilled"> The fulfillment handler.</param>
		''' <param name="onRejected">The rejection handler.</param>
		Public Sub [Then](ByVal onFulfilled As Action(Of Object), Optional ByVal onRejected As Action(Of Object) = Nothing)
			jsObject.Invoke("then", onFulfilled, onRejected)
		End Sub

		''' <summary>
		'''     Appends fulfillment and rejection handlers to the promise, and returns a new <see cref="JsPromise" />
		'''     resolving to the return value of the called handler, or to its original settled value
		'''     if the promise was not handled.
		''' </summary>
		''' <param name="onFulfilled"> The fulfillment handler.</param>
		''' <param name="onRejected">The rejection handler.</param>
		''' <returns>A new <see cref="JsPromise" /></returns>
		Public Function [Then](ByVal onFulfilled As Func(Of Object, Object), Optional ByVal onRejected As Func(Of Object, Object) = Nothing) As JsPromise
			Dim newPromise As IJsObject = TryCast(jsObject.Invoke("then", onFulfilled, onRejected), IJsObject)
			Return New JsPromise(newPromise)
		End Function

		Private Function Fulfilled(ByVal o As Object) As Result
			Return New Result(ResultState.Fulfilled, o)
		End Function

		Private Shared Function IsPromise(ByVal jsObject As IJsObject) As Boolean
			If jsObject Is Nothing OrElse jsObject.IsDisposed Then
				Return False
			End If

			Dim promisePrototype As IJsObject = jsObject.Frame.ExecuteJavaScript(Of IJsObject)("Promise.prototype").Result
			Return promisePrototype.Invoke(Of Boolean)("isPrototypeOf", jsObject)
		End Function

		Private Function Rejected(ByVal o As Object) As Result
			Return New Result(ResultState.Rejected, o)
		End Function

		''' <summary>
		'''     The Promise result state.
		''' </summary>
		Public Enum ResultState
			''' <summary>
			'''     The Promise was fulfilled.
			''' </summary>
			Fulfilled

			''' <summary>
			'''     The promise was rejected.
			''' </summary>
			Rejected
		End Enum

		Public Class Result

			''' <summary>
			'''     The object passed to the fulfillment or rejection handler.
			''' </summary>
			Public ReadOnly Property Data() As Object

			''' <summary>
			'''     The promise result state that indicates whether the promise was fulfilled or rejected.
			''' </summary>
			Public ReadOnly Property State() As ResultState

			Friend Sub New(ByVal promiseState As ResultState, ByVal promiseData As Object)
				Me.State = promiseState
				Me.Data = promiseData
			End Sub

		End Class
	End Class
End Namespace