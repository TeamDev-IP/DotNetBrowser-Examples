Namespace DevTools.WinForms
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
			Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
			Me.browserView1 = New DotNetBrowser.WinForms.BrowserView()
			Me.browserView2 = New DotNetBrowser.WinForms.BrowserView()
			CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.splitContainer1.Panel1.SuspendLayout()
			Me.splitContainer1.Panel2.SuspendLayout()
			Me.splitContainer1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' splitContainer1
			' 
			Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.splitContainer1.Location = New System.Drawing.Point(0, 0)
			Me.splitContainer1.Name = "splitContainer1"
			Me.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
			' 
			' splitContainer1.Panel1
			' 
			Me.splitContainer1.Panel1.Controls.Add(Me.browserView1)
			' 
			' splitContainer1.Panel2
			' 
			Me.splitContainer1.Panel2.Controls.Add(Me.browserView2)
			Me.splitContainer1.Size = New System.Drawing.Size(800, 450)
			Me.splitContainer1.SplitterDistance = 266
			Me.splitContainer1.TabIndex = 0
			' 
			' browserView1
			' 
			Me.browserView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView1.Location = New System.Drawing.Point(0, 0)
			Me.browserView1.Name = "browserView1"
			Me.browserView1.Size = New System.Drawing.Size(800, 266)
			Me.browserView1.TabIndex = 0
			' 
			' browserView2
			' 
			Me.browserView2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView2.Location = New System.Drawing.Point(0, 0)
			Me.browserView2.Name = "browserView2"
			Me.browserView2.Size = New System.Drawing.Size(800, 180)
			Me.browserView2.TabIndex = 0
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(800, 450)
			Me.Controls.Add(Me.splitContainer1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.splitContainer1.Panel1.ResumeLayout(False)
			Me.splitContainer1.Panel2.ResumeLayout(False)
			CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.splitContainer1.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private splitContainer1 As System.Windows.Forms.SplitContainer
		Private browserView1 As DotNetBrowser.WinForms.BrowserView
		Private browserView2 As DotNetBrowser.WinForms.BrowserView
	End Class
End Namespace

