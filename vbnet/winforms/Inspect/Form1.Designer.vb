Namespace Inspect.WinForms
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
			Me.browserView1 = New DotNetBrowser.WinForms.BrowserView()
			Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
			Me.statusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
			Me.statusStrip1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' browserView1
			' 
			Me.browserView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView1.Location = New System.Drawing.Point(0, 0)
			Me.browserView1.Name = "browserView1"
			Me.browserView1.Size = New System.Drawing.Size(800, 450)
			Me.browserView1.TabIndex = 0
			' 
			' statusStrip1
			' 
			Me.statusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
			Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.statusLabel1})
			Me.statusStrip1.Location = New System.Drawing.Point(0, 428)
			Me.statusStrip1.Name = "statusStrip1"
			Me.statusStrip1.Size = New System.Drawing.Size(800, 22)
			Me.statusStrip1.TabIndex = 1
			Me.statusStrip1.Text = "statusStrip1"
			' 
			' statusLabel1
			' 
			Me.statusLabel1.Name = "statusLabel1"
			Me.statusLabel1.Size = New System.Drawing.Size(0, 18)
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(800, 450)
			Me.Controls.Add(Me.statusStrip1)
			Me.Controls.Add(Me.browserView1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.statusStrip1.ResumeLayout(False)
			Me.statusStrip1.PerformLayout()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub

		#End Region

		Private browserView1 As DotNetBrowser.WinForms.BrowserView
		Private statusStrip1 As System.Windows.Forms.StatusStrip
		Private statusLabel1 As System.Windows.Forms.ToolStripStatusLabel
	End Class
End Namespace

