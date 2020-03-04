Namespace JavaScriptBridge.WinForms
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
			Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
			Me.richTextBox1 = New System.Windows.Forms.RichTextBox()
			Me.button1 = New System.Windows.Forms.Button()
			Me.tableLayoutPanel1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' tableLayoutPanel1
			' 
			Me.tableLayoutPanel1.ColumnCount = 2
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F))
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F))
			Me.tableLayoutPanel1.Controls.Add(Me.richTextBox1, 0, 0)
			Me.tableLayoutPanel1.Controls.Add(Me.button1, 0, 1)
			Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
			Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
			Me.tableLayoutPanel1.RowCount = 2
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F))
			Me.tableLayoutPanel1.Size = New System.Drawing.Size(800, 450)
			Me.tableLayoutPanel1.TabIndex = 0
			' 
			' richTextBox1
			' 
			Me.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.richTextBox1.Location = New System.Drawing.Point(3, 3)
			Me.richTextBox1.Name = "richTextBox1"
			Me.richTextBox1.Size = New System.Drawing.Size(394, 404)
			Me.richTextBox1.TabIndex = 0
			Me.richTextBox1.Text = "Sample text in Form"
			' 
			' button1
			' 
			Me.button1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.button1.Location = New System.Drawing.Point(3, 413)
			Me.button1.Name = "button1"
			Me.button1.Size = New System.Drawing.Size(394, 34)
			Me.button1.TabIndex = 1
			Me.button1.Text = "Update BrowserView >"
			Me.button1.UseVisualStyleBackColor = True
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(800, 450)
			Me.Controls.Add(Me.tableLayoutPanel1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.tableLayoutPanel1.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
		Private richTextBox1 As System.Windows.Forms.RichTextBox
		Private WithEvents button1 As System.Windows.Forms.Button
	End Class
End Namespace

