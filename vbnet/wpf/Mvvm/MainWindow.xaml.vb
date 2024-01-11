#Region "Copyright"

' Copyright © 2024, TeamDev. All rights reserved.
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

Imports DotNetBrowser.Engine
Imports Mvvm.Wpf.ViewModels

Namespace Mvvm.Wpf
	''' <summary>
	'''     This example demonstrates the possible approach to use DotNetBrowser
	'''     with WPF data binding.
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

		Private ReadOnly engine As IEngine

		Public Property MyBrowser() As MyBrowserViewModel

		Public Sub New()
			InitializeComponent()
			DataContext = Me

			Dim engineOptions As EngineOptions = New DotNetBrowser.Engine.EngineOptions.Builder With {.RenderingMode = RenderingMode.HardwareAccelerated} .Build()
			engine = EngineFactory.Create(engineOptions)

			MyBrowser = New MyBrowserViewModel(engine.CreateBrowser()) With {.Url = "www.teamdev.com/dotnetbrowser"}
		End Sub

		Private Sub MainWindow_OnClosed(ByVal sender As Object, ByVal e As EventArgs)
			engine?.Dispose()
		End Sub

	End Class
End Namespace
