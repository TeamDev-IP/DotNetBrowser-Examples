Partial Class BrowserForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
		Private Sub InitializeComponent()
        Me.browserView1 = New DotNetBrowser.WinForms.BrowserView()
        Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.textBox1 = New System.Windows.Forms.TextBox()
        Me.tableLayoutPanel1.SuspendLayout
        Me.SuspendLayout
        '
        'browserView1
        '
        Me.browserView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.browserView1.Location = New System.Drawing.Point(3, 43)
        Me.browserView1.Name = "browserView1"
        Me.browserView1.Size = New System.Drawing.Size(1165, 596)
        Me.browserView1.TabIndex = 0
        '
        'tableLayoutPanel1
        '
        Me.tableLayoutPanel1.ColumnCount = 1
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.tableLayoutPanel1.Controls.Add(Me.browserView1, 0, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.textBox1, 0, 0)
        Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
        Me.tableLayoutPanel1.RowCount = 2
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tableLayoutPanel1.Size = New System.Drawing.Size(1171, 642)
        Me.tableLayoutPanel1.TabIndex = 0
        '
        'textBox1
        '
        Me.textBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.textBox1.Location = New System.Drawing.Point(3, 3)
        Me.textBox1.Name = "textBox1"
        Me.textBox1.Size = New System.Drawing.Size(1165, 22)
        Me.textBox1.TabIndex = 1
        '
        'BrowserForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 16!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1171, 642)
        Me.Controls.Add(Me.tableLayoutPanel1)
        Me.Name = "BrowserForm"
        Me.Text = "BrowserForm"
        Me.tableLayoutPanel1.ResumeLayout(false)
        Me.tableLayoutPanel1.PerformLayout
        Me.ResumeLayout(false)

End Sub

		Private browserView1 As DotNetBrowser.WinForms.BrowserView
		Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
		Private WithEvents textBox1 As System.Windows.Forms.TextBox

End Class
