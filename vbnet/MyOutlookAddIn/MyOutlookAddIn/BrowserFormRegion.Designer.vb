Namespace MyOutlookAddIn
	<System.ComponentModel.ToolboxItemAttribute(False)>
	Partial Friend Class BrowserFormRegion
		Inherits Microsoft.Office.Tools.Outlook.FormRegionBase

		Public Sub New(ByVal formRegion As Microsoft.Office.Interop.Outlook.FormRegion)
			MyBase.New(Globals.Factory, formRegion)
			Me.InitializeComponent()
		End Sub

		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.browserView1 = New DotNetBrowser.WinForms.BrowserView()
			Me.SuspendLayout()
			' 
			' browserView1
			' 
			Me.browserView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView1.Location = New System.Drawing.Point(0, 0)
			Me.browserView1.Name = "browserView1"
			Me.browserView1.Size = New System.Drawing.Size(567, 376)
			Me.browserView1.TabIndex = 0
			' 
			' BrowserFormRegion
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.browserView1)
			Me.Name = "BrowserFormRegion"
			Me.Size = New System.Drawing.Size(567, 376)
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.FormRegionShowing += new System.EventHandler(this.BrowserFormRegion_FormRegionShowing);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.FormRegionClosed += new System.EventHandler(this.BrowserFormRegion_FormRegionClosed);
			Me.ResumeLayout(False)

		End Sub

		#End Region

		#Region "Form Region Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Shared Sub InitializeManifest(ByVal manifest As Microsoft.Office.Tools.Outlook.FormRegionManifest, ByVal factory As Microsoft.Office.Tools.Outlook.Factory)
			manifest.FormRegionName = "BrowserFormRegion"
			manifest.ShowInspectorRead = False
			manifest.ShowReadingPane = False

		End Sub

		#End Region

		Private browserView1 As DotNetBrowser.WinForms.BrowserView

		Partial Public Class BrowserFormRegionFactory
			Implements Microsoft.Office.Tools.Outlook.IFormRegionFactory

			Public Event FormRegionInitializing As Microsoft.Office.Tools.Outlook.FormRegionInitializingEventHandler

			Private _Manifest As Microsoft.Office.Tools.Outlook.FormRegionManifest

			<System.Diagnostics.DebuggerNonUserCodeAttribute()>
			Public Sub New()
				Me._Manifest = Globals.Factory.CreateFormRegionManifest()
				BrowserFormRegion.InitializeManifest(Me._Manifest, Globals.Factory)
				AddHandler Me.FormRegionInitializing, AddressOf BrowserFormRegionFactory_FormRegionInitializing
			End Sub

			<System.Diagnostics.DebuggerNonUserCodeAttribute()>
			Public ReadOnly Property Manifest() As Microsoft.Office.Tools.Outlook.FormRegionManifest Implements Microsoft.Office.Tools.Outlook.IFormRegionFactory.Manifest
				Get
					Return Me._Manifest
				End Get
			End Property

			<System.Diagnostics.DebuggerNonUserCodeAttribute()>
			Private Function IFormRegionFactory_CreateFormRegion(ByVal formRegion As Microsoft.Office.Interop.Outlook.FormRegion) As Microsoft.Office.Tools.Outlook.IFormRegion Implements Microsoft.Office.Tools.Outlook.IFormRegionFactory.CreateFormRegion
				Dim form As New BrowserFormRegion(formRegion)
				form.Factory = Me
				Return form
			End Function

			<System.Diagnostics.DebuggerNonUserCodeAttribute()>
			Private Function IFormRegionFactory_GetFormRegionStorage(ByVal outlookItem As Object, ByVal formRegionMode As Microsoft.Office.Interop.Outlook.OlFormRegionMode, ByVal formRegionSize As Microsoft.Office.Interop.Outlook.OlFormRegionSize) As Byte() Implements Microsoft.Office.Tools.Outlook.IFormRegionFactory.GetFormRegionStorage
				Throw New System.NotSupportedException()
			End Function

			<System.Diagnostics.DebuggerNonUserCodeAttribute()>
			Private Function IFormRegionFactory_IsDisplayedForItem(ByVal outlookItem As Object, ByVal formRegionMode As Microsoft.Office.Interop.Outlook.OlFormRegionMode, ByVal formRegionSize As Microsoft.Office.Interop.Outlook.OlFormRegionSize) As Boolean Implements Microsoft.Office.Tools.Outlook.IFormRegionFactory.IsDisplayedForItem
				If Me.FormRegionInitializingEvent IsNot Nothing Then
					Dim cancelArgs As Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs = Globals.Factory.CreateFormRegionInitializingEventArgs(outlookItem, formRegionMode, formRegionSize, False)
					RaiseEvent FormRegionInitializing(Me, cancelArgs)
					Return Not cancelArgs.Cancel
				Else
					Return True
				End If
			End Function

			<System.Diagnostics.DebuggerNonUserCodeAttribute()>
			Private ReadOnly Property IFormRegionFactory_Kind() As Microsoft.Office.Tools.Outlook.FormRegionKindConstants Implements Microsoft.Office.Tools.Outlook.IFormRegionFactory.Kind
				Get
					Return Microsoft.Office.Tools.Outlook.FormRegionKindConstants.WindowsForms
				End Get
			End Property
		End Class
	End Class

	Partial Friend Class WindowFormRegionCollection
		Friend ReadOnly Property BrowserFormRegion() As BrowserFormRegion
			Get
				For Each item In Me
					If item.GetType() Is GetType(BrowserFormRegion) Then
						Return CType(item, BrowserFormRegion)
					End If
				Next item
				Return Nothing
			End Get
		End Property
	End Class
End Namespace
