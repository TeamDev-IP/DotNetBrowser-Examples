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

Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.Win32

Namespace ComWrapper.WinForms
	Friend Module ControlRegistration
		''' <summary>
		'''     When the container resizes the space allocated to displaying one of the object's presentations,
		'''     the object wants to recompose the presentation. This means that on resize, the object wants
		'''     to do more than scale its picture.
		''' </summary>
		Private Const OLEMISC_RECOMPOSEONRESIZE As Integer = 1

		''' <summary>
		'''     This object cannot be the link source that when bound to activates (runs) the object.
		'''     If the object is selected and copied to the clipboard, the object's container can offer a link
		'''     in a clipboard data transfer that, when bound, must connect to the outside of the object.
		'''     The user would see the object selected in its container, not open for editing.
		'''     Rather than doing this, the container can simply refuse to offer a link source
		'''     when transferring objects with this bit set.
		''' </summary>
		Private Const OLEMISC_CANTLINKINSIDE As Integer = 16

		''' <summary>
		'''     This object is capable of activating in-place, without requiring installation of menus
		'''     and toolbars to run. Several such objects can be active concurrently. Some containers,
		'''     such as forms, may choose to activate such objects automatically.
		''' </summary>
		Private Const OLEMISC_INSIDEOUT As Integer = 128

		''' <summary>
		'''     This bit is set only when OLEMISC_INSIDEOUT is set, and indicates that this object
		'''     prefers to be activated whenever it is visible. Some containers may always ignore this hint.
		''' </summary>
		Private Const OLEMISC_ACTIVATEWHENVISIBLE As Integer = 256

		''' <summary>
		'''     This value is used with controls. It indicates that the control wants to use IOleObject::SetClientSite
		'''     as its initialization function, even before a call such as IPersistStreamInit::InitNew or
		'''     IPersistStorage::InitNew. This allows the control to access a container's ambient properties
		'''     before loading information from persistent storage. Note that the current implementations of OleCreate,
		'''     OleCreateFromData, OleCreateFromFile, OleLoad, and the default handler do not understand this value.
		'''     Control containers that wish to honor this value must currently implement their own versions of
		'''     these functions in order to establish the correct initialization sequence for the control.
		''' </summary>
		Private Const OLEMISC_SETCLIENTSITEFIRST As Integer = 131072

		''' <summary>
		'''     Current process bitness.
		''' </summary>
		Private ReadOnly Bitness As String = If(Environment.Is64BitProcess, "64-bit", "32-bit")

		Public Sub RegisterControl(ByVal t As Type, Optional ByVal iconResourceIndex As Integer = 101)
			Try
				If t Is Nothing Then
					Throw New ArgumentNullException(NameOf(t))
				End If

				If Not GetType(Control).IsAssignableFrom(t) Then
					Throw New ArgumentException("Type argument must be a Windows Forms control.")
				End If


				Dim key As String = $"CLSID\{t.GUID:B}"

				Using registryKey As RegistryKey = Registry.ClassesRoot.OpenSubKey(key, True)
					' InprocServer32

					Using inprocServerKey As RegistryKey = registryKey?.OpenSubKey("InprocServer32", True)
						'Override the default value - to make sure that the applications will be able to locate the DLL
						inprocServerKey?.SetValue(Nothing, $"{Environment.SystemDirectory}\mscoree.dll")
						' Create the CodeBase entry - needed if not string named and GACced.
						inprocServerKey?.SetValue("CodeBase", t.Assembly.CodeBase)
					End Using

					' Control
					' Create the 'Control' key - this allows our control to show up in 
					' the ActiveX control container 
					Dim controlKey As RegistryKey = registryKey?.CreateSubKey("Control")
					controlKey?.Close()

					' MiscStatus
					Using miscKey As RegistryKey = registryKey?.CreateSubKey("MiscStatus")
						Const miscStatusValue As Integer = OLEMISC_RECOMPOSEONRESIZE + OLEMISC_CANTLINKINSIDE + OLEMISC_INSIDEOUT + OLEMISC_ACTIVATEWHENVISIBLE + OLEMISC_SETCLIENTSITEFIRST

						miscKey?.SetValue("", miscStatusValue.ToString(), RegistryValueKind.String)
					End Using

					' ToolBoxBitmap32
					Using bitmapKey As RegistryKey = registryKey?.CreateSubKey("ToolboxBitmap32")
						bitmapKey?.SetValue("", $"{t.Assembly.Location}, {iconResourceIndex}", RegistryValueKind.String)
					End Using

					' TypeLib
					Using typeLibKey As RegistryKey = registryKey?.CreateSubKey("TypeLib")
						Dim libId As Guid = Marshal.GetTypeLibGuidForAssembly(t.Assembly)
						typeLibKey?.SetValue("", libId.ToString("B"), RegistryValueKind.String)
					End Using

					' Version
					Using versionKey As RegistryKey = registryKey?.CreateSubKey("Version")
						Dim major As Integer = Nothing, minor As Integer = Nothing
						Marshal.GetTypeLibVersionForAssembly(t.Assembly, major, minor)
						versionKey?.SetValue("", $"{major}.{minor}")
					End Using
				End Using

				EventLogWrapper.Log($"Control registered for {Bitness} applications: {t.FullName}, {key}", EventLogEntryType.Information, 200)
			Catch ex As Exception
				EventLogWrapper.Log($"Control was not registered: {t.FullName}" & vbLf & "{ex}", EventLogEntryType.Error, 500)
			End Try
		End Sub

		Public Sub UnregisterControl(ByVal t As Type)
			Try
				If t Is Nothing Then
					Throw New ArgumentNullException(NameOf(t))
				End If

				If Not GetType(Control).IsAssignableFrom(t) Then
					Throw New ArgumentException("Type argument must be a Windows Forms control.")
				End If

				' CLSID
				Dim key As String = $"CLSID\{t.GUID:B}"
				Registry.ClassesRoot.DeleteSubKeyTree(key)
				EventLogWrapper.Log($"Control unregistered for {Bitness} applications: {t.FullName}, {key}", EventLogEntryType.Information, 200)
			Catch ex As Exception
				EventLogWrapper.Log($"Control was not unregistered: {t.FullName}" & vbLf & "{ex}", EventLogEntryType.Error, 500)
			End Try
		End Sub

	End Module
End Namespace