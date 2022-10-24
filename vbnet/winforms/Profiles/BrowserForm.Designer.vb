Namespace Profiles.WinForms
	Partial Public Class BrowserForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.AddressBar = New System.Windows.Forms.TextBox()
			Me.SuspendLayout()
			' 
			' AddressBar
			' 
			Me.AddressBar.Dock = System.Windows.Forms.DockStyle.Top
			Me.AddressBar.Location = New System.Drawing.Point(0, 0)
			Me.AddressBar.Name = "AddressBar"
			Me.AddressBar.Size = New System.Drawing.Size(800, 22)
			Me.AddressBar.TabIndex = 0
			Me.AddressBar.Text = "https://google.com"
			AddHandler Me.AddressBar.KeyDown, AddressOf AddressBar_KeyDown
			' 
			' BrowserForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(800, 450)
			Me.Controls.Add(Me.AddressBar)
			Me.Name = "BrowserForm"
			Me.Text = "BrowserForm"
			AddHandler Me.FormClosed, AddressOf BrowserForm_FormClosed
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private WithEvents AddressBar As System.Windows.Forms.TextBox
	End Class
End Namespace