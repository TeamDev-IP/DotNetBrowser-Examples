#Region "Copyright"

' Copyright 2019, TeamDev. All rights reserved.
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
Imports System.Threading
Imports DotNetBrowser.Browser
Imports DotNetBrowser.Dom
Imports DotNetBrowser.Engine

Namespace DOMFormSample
	Friend Class Program
		#Region "Methods"

		Public Shared Sub Main()
			Try
				Using engine As IEngine = EngineFactory.Create((New EngineOptions.Builder()).Build())
					Console.WriteLine("Engine created")

					Using browser As IBrowser = engine.CreateBrowser()
						Console.WriteLine("Browser created")

						browser.MainFrame.LoadHtml("<html><body><form name=""myForm"">" & "First name: <input type=""text"" id=""firstName"" name=""firstName""/><br/>" & "Last name: <input type=""text"" id=""lastName"" name=""lastName""/><br/>" & "<input type='checkbox' id='agreement' name='agreement' value='agreed'>I agree<br>" & "<input type='button' id='saveButton' value=""Save"" onclick=""" & "if(document.getElementById('agreement').checked){" & "    console.log(document.getElementById('firstName').value +' '+" & "document.getElementById('lastName').value);}" & """/>" & "</form></body></html>").Wait()
						Dim document As IDocument = browser.MainFrame.Document
						Dim firstName As IInputElement = DirectCast(document.GetElementByName("firstName"), IInputElement)
						Dim lastName As IInputElement = DirectCast(document.GetElementByName("lastName"), IInputElement)
						Dim agreement As IInputElement = DirectCast(document.GetElementByName("agreement"), IInputElement)

						firstName.Value = "John"
						lastName.Value = "Doe"
						agreement.Checked = True

						AddHandler browser.ConsoleMessage, Sub(sender, args)
							Console.WriteLine("JS Console: < " & args.Message)
						End Sub
						document.GetElementById("saveButton").Click()
						Thread.Sleep(3000)
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