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

Imports System.Runtime.InteropServices
Imports DotNetBrowser.Engine

Namespace ComWrapper.WinForms.Impl
	<Guid("DAE7A4AC-2D70-4830-81D8-3D7E5D8A7981")>
	<ClassInterface(ClassInterfaceType.None)>
	<ProgId("DotNetBrowser.ComWrapper.Engine")>
	Public Class EngineWrapper
		Implements IComEngine

		Private engine As IEngine
		Private initialized As Boolean

		Public Function CreateBrowser() As IComBrowser Implements IComEngine.CreateBrowser
			Return New BrowserImpl(engine.CreateBrowser())
		End Function

		Public Sub Dispose() Implements IComEngine.Dispose
			If initialized Then
				engine?.Dispose()
				engine = Nothing
				initialized = False
			End If
		End Sub

		Public Sub Initialize() Implements IComEngine.Initialize
			If Not initialized Then
				engine = EngineFactory.Create(New EngineOptions.Builder With {.RenderingMode = RenderingMode.OffScreen}.Build())
				initialized = True
			End If
		End Sub

	End Class
End Namespace
