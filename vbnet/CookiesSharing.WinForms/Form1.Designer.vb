Partial Class Form1
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
        Me.button3 = New System.Windows.Forms.Button()
        Me.button1 = New System.Windows.Forms.Button()
        Me.button2 = New System.Windows.Forms.Button()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.tableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'button3
        '
        Me.button3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.button3.Location = New System.Drawing.Point(567, 71)
        Me.button3.Margin = New System.Windows.Forms.Padding(7)
        Me.button3.Name = "button3"
        Me.button3.Size = New System.Drawing.Size(148, 35)
        Me.button3.TabIndex = 2
        Me.button3.Text = "Launch Browser 2"
        Me.button3.UseVisualStyleBackColor = True
        '
        'button1
        '
        Me.button1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.button1.Location = New System.Drawing.Point(54, 71)
        Me.button1.Margin = New System.Windows.Forms.Padding(7)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(148, 35)
        Me.button1.TabIndex = 0
        Me.button1.Text = "Launch Browser 1"
        Me.button1.UseVisualStyleBackColor = True
        '
        'button2
        '
        Me.button2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.button2.Location = New System.Drawing.Point(263, 71)
        Me.button2.Margin = New System.Windows.Forms.Padding(7)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(242, 35)
        Me.button2.TabIndex = 1
        Me.button2.Text = "Copy cookies from Browser 1"
        Me.button2.UseVisualStyleBackColor = True
        '
        'label3
        '
        Me.label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(616, 16)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(49, 17)
        Me.label3.TabIndex = 5
        Me.label3.Text = "Step 3"
        '
        'label2
        '
        Me.label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(359, 16)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(49, 17)
        Me.label2.TabIndex = 4
        Me.label2.Text = "Step 2"
        '
        'label1
        '
        Me.label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(103, 16)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(49, 17)
        Me.label1.TabIndex = 3
        Me.label1.Text = "Step 1"
        '
        'tableLayoutPanel1
        '
        Me.tableLayoutPanel1.ColumnCount = 3
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F))
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F))
        Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F))
        Me.tableLayoutPanel1.Controls.Add(Me.button2, 1, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.button3, 2, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.label3, 2, 0)
        Me.tableLayoutPanel1.Controls.Add(Me.label2, 1, 0)
        Me.tableLayoutPanel1.Controls.Add(Me.button1, 0, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.label1, 0, 0)
        Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
        Me.tableLayoutPanel1.RowCount = 2
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.75969F))
        Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.24031F))
        Me.tableLayoutPanel1.Size = New System.Drawing.Size(770, 129)
        Me.tableLayoutPanel1.TabIndex = 6
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0F, 16.0F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(770, 129)
        Me.Controls.Add(Me.tableLayoutPanel1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.tableLayoutPanel1.ResumeLayout(False)
        Me.tableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

        components = New System.ComponentModel.Container()
    End Sub

    Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Private WithEvents button1 As System.Windows.Forms.Button
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents button3 As System.Windows.Forms.Button
    Private label1 As System.Windows.Forms.Label
    Private label2 As System.Windows.Forms.Label
    Private label3 As System.Windows.Forms.Label

End Class
