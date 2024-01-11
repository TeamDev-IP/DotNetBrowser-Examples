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

Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Engine
Imports DotNetBrowser.Geometry

Namespace SeparateEngines.AppDomains
	''' <summary>
	'''     This example demonstrates how to create and use separate IEngine instances
	'''     in different application domains.
	''' </summary>
	Friend Class Program

		Public Shared Sub Main(ByVal args() As String)
			' Create two separate application domains.
			Dim domain1 As AppDomain = AppDomain.CreateDomain("Domain1")
			Dim domain2 As AppDomain = AppDomain.CreateDomain("Domain2")

			' Create an instance of EngineWrapper in the first AppDomain.
			' A proxy to the object is returned.
			Console.WriteLine("Create wrapper1")
			Dim wrapper1 As EngineWrapper = DirectCast(domain1.CreateInstanceAndUnwrap(GetType(EngineWrapper).Assembly.FullName, GetType(EngineWrapper).FullName), EngineWrapper)

			' Create an instance of EngineWrapper in the second AppDomain.
			' A proxy to the object is returned.
			Console.WriteLine("Create wrapper2")
			Dim wrapper2 As EngineWrapper = DirectCast(domain2.CreateInstanceAndUnwrap(GetType(EngineWrapper).Assembly.FullName, GetType(EngineWrapper).FullName), EngineWrapper)

			'Execute an action in the first application domain.
			Dim title1 As String = wrapper1.LoadAndGetTitle("teamdev.com")
			Console.WriteLine("Title 1: {0}", title1)

			'Dispose the wrapper and unload the first application domain.
			Console.WriteLine("Dispose wrapper1")
			wrapper1.Dispose()
			AppDomain.Unload(domain1)

			'After unloading the first domain, the engine in the second domain is alive, and we can execute actions.
			Dim title2 As String = wrapper2.LoadAndGetTitle("teamdev.com/dotnetbrowser")
			Console.WriteLine("Title 2: {0}", title2)

			'Dispose the wrapper and unload the second application domain.
			Console.WriteLine("Dispose wrapper2")
			wrapper2.Dispose()
			AppDomain.Unload(domain2)

			Console.WriteLine("Press any key to terminate...")
			Console.ReadKey()
		End Sub

	End Class

	''' <summary>
	'''     This class serves as a wrapper for DotNetBrowser objects and the logic related to these objects,
	'''     since they cannot be marshaled directly.
	''' </summary>
	Friend Class EngineWrapper
		Inherits MarshalByRefObject
		Implements IDisposable

		Private ReadOnly Property Engine() As IEngine

		Public Sub New()
			'Perform complex engine initialization here if necessary.
			Engine = EngineFactory.Create()
		End Sub

		Public Sub Dispose() Implements IDisposable.Dispose
			Engine?.Dispose()
		End Sub

		''' <summary>
		'''     This method demonstrates how to implement a particular scenario that
		'''     should be performed in a separate application domain.
		'''     <para>
		'''         For instance, this method returns a web page title after loading that web page completely
		'''         in a separate browser instance.
		'''     </para>
		''' </summary>
		''' <param name="url">The URL to load.</param>
		''' <returns>The title of the loaded web page.</returns>
		Public Function LoadAndGetTitle(ByVal url As String) As String
			Dim result As String = Nothing
			Console.WriteLine("Loading URL '{0}' in '{1}'.", url, Thread.GetDomain().FriendlyName)
			Try
				Using browser As IBrowser = Engine.CreateBrowser()
					browser.Size = New Size(700, 500)
					browser.Navigation.LoadUrl(url).Wait()
					Console.WriteLine("URL loaded")
					result = browser.Title
				End Using
			Catch e As Exception
				Console.WriteLine(e)
			End Try

			Return result
		End Function

	End Class
End Namespace
