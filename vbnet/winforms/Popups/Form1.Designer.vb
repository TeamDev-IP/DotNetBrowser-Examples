Namespace Popups.WinForms
	Partial Public Class Form1
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
			Me.browserView = New DotNetBrowser.WinForms.BrowserView()
			Me.SuspendLayout()
			' 
			' browserView
			' 
			Me.browserView.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView.Location = New System.Drawing.Point(0, 0)
			Me.browserView.Name = "browserView"
			Me.browserView.Size = New System.Drawing.Size(800, 450)
			Me.browserView.TabIndex = 0
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(800, 450)
			Me.Controls.Add(Me.browserView)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private browserView As DotNetBrowser.WinForms.BrowserView
	End Class
End Namespace

