'====================================================================================================
'The Free Edition of Instant VB limits conversion output to 100 lines per file.

'To purchase the Premium Edition, visit our website:
'https://www.tangiblesoftwaresolutions.com/order/order-instant-vb.html
'====================================================================================================

Imports System.Windows.Forms

Namespace GoogleStreetView.WinForms
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
			Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
			Me.longitudeValue = New System.Windows.Forms.NumericUpDown()
			Me.label5 = New System.Windows.Forms.Label()
			Me.latitudeValue = New System.Windows.Forms.NumericUpDown()
			Me.povHeadingValue = New System.Windows.Forms.NumericUpDown()
			Me.povPitchValue = New System.Windows.Forms.NumericUpDown()
			Me.panoValue = New System.Windows.Forms.Label()
			Me.label4 = New System.Windows.Forms.Label()
			Me.label3 = New System.Windows.Forms.Label()
			Me.label2 = New System.Windows.Forms.Label()
			Me.label1 = New System.Windows.Forms.Label()
			Me.button1 = New System.Windows.Forms.Button()
			CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.splitContainer1.Panel1.SuspendLayout()
			Me.splitContainer1.Panel2.SuspendLayout()
			Me.splitContainer1.SuspendLayout()
			Me.tableLayoutPanel1.SuspendLayout()
			CType(Me.longitudeValue, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.latitudeValue, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.povHeadingValue, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.povPitchValue, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' splitContainer1
			' 
			Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.splitContainer1.Location = New System.Drawing.Point(0, 0)
			Me.splitContainer1.Margin = New System.Windows.Forms.Padding(0)
			Me.splitContainer1.Name = "splitContainer1"
			' 
			' splitContainer1.Panel1
			' 
			Me.splitContainer1.Panel1.Controls.Add(Me.browserView1)
			' 
			' splitContainer1.Panel2
			' 
			Me.splitContainer1.Panel2.Controls.Add(Me.tableLayoutPanel1)
			Me.splitContainer1.Size = New System.Drawing.Size(1006, 721)
			Me.splitContainer1.SplitterDistance = 679
			Me.splitContainer1.TabIndex = 0
			' 
			' browserView1
			' 
			Me.browserView1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView1.Location = New System.Drawing.Point(0, 0)
			Me.browserView1.Name = "browserView1"
			Me.browserView1.Size = New System.Drawing.Size(679, 721)
			Me.browserView1.TabIndex = 0
			' 
			' tableLayoutPanel1
			' 
			Me.tableLayoutPanel1.ColumnCount = 5
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F))
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.Controls.Add(Me.longitudeValue, 3, 9)
			Me.tableLayoutPanel1.Controls.Add(Me.label5, 1, 9)
			Me.tableLayoutPanel1.Controls.Add(Me.latitudeValue, 3, 7)
			Me.tableLayoutPanel1.Controls.Add(Me.povHeadingValue, 3, 3)
			Me.tableLayoutPanel1.Controls.Add(Me.povPitchValue, 3, 5)
			Me.tableLayoutPanel1.Controls.Add(Me.panoValue, 3, 1)
			Me.tableLayoutPanel1.Controls.Add(Me.label4, 1, 7)
			Me.tableLayoutPanel1.Controls.Add(Me.label3, 1, 1)
			Me.tableLayoutPanel1.Controls.Add(Me.label2, 1, 5)
			Me.tableLayoutPanel1.Controls.Add(Me.label1, 1, 3)
			Me.tableLayoutPanel1.Controls.Add(Me.button1, 1, 11)
			Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
			Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
			Me.tableLayoutPanel1.RowCount = 15
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F))
			Me.tableLayoutPanel1.Size = New System.Drawing.Size(323, 721)
			Me.tableLayoutPanel1.TabIndex = 0
			' 
			' longitudeValue
			' 
			Me.longitudeValue.DecimalPlaces = 12
			Me.longitudeValue.Dock = System.Windows.Forms.DockStyle.Fill
			Me.longitudeValue.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.longitudeValue.Increment = New Decimal(New Integer() { 1, 0, 0, 393216})
			Me.longitudeValue.Location = New System.Drawing.Point(170, 190)
			Me.longitudeValue.Margin = New System.Windows.Forms.Padding(0)
			Me.longitudeValue.Maximum = New Decimal(New Integer() { 180, 0, 0, 0})
			Me.longitudeValue.Minimum = New Decimal(New Integer() { 180, 0, 0, -2147483648})
			Me.longitudeValue.Name = "longitudeValue"
			Me.longitudeValue.Size = New System.Drawing.Size(143, 34)
			Me.longitudeValue.TabIndex = 10
			' 
			' label5
			' 
			Me.label5.AutoSize = True
			Me.label5.Dock = System.Windows.Forms.DockStyle.Fill
			Me.label5.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label5.Location = New System.Drawing.Point(10, 190)
			Me.label5.Margin = New System.Windows.Forms.Padding(0)
			Me.label5.Name = "label5"
			Me.label5.Size = New System.Drawing.Size(150, 35)
			Me.label5.TabIndex = 8
			Me.label5.Text = "Longitude"
			Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' latitudeValue
			' 
			Me.latitudeValue.DecimalPlaces = 12
			Me.latitudeValue.Dock = System.Windows.Forms.DockStyle.Fill
			Me.latitudeValue.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.latitudeValue.Increment = New Decimal(New Integer() { 1, 0, 0, 393216})
			Me.latitudeValue.Location = New System.Drawing.Point(170, 145)
			Me.latitudeValue.Margin = New System.Windows.Forms.Padding(0)
			Me.latitudeValue.Maximum = New Decimal(New Integer() { 180, 0, 0, 0})
			Me.latitudeValue.Minimum = New Decimal(New Integer() { 180, 0, 0, -2147483648})
			Me.latitudeValue.Name = "latitudeValue"
			Me.latitudeValue.Size = New System.Drawing.Size(143, 34)
			Me.latitudeValue.TabIndex = 7
			' 
			' povHeadingValue
			' 
			Me.povHeadingValue.DecimalPlaces = 10
			Me.povHeadingValue.Dock = System.Windows.Forms.DockStyle.Fill
			Me.povHeadingValue.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.povHeadingValue.Increment = New Decimal(New Integer() { 5, 0, 0, 65536})
			Me.povHeadingValue.Location = New System.Drawing.Point(170, 55)
			Me.povHeadingValue.Margin = New System.Windows.Forms.Padding(0)
			Me.povHeadingValue.Maximum = New Decimal(New Integer() { 360, 0, 0, 0})
			Me.povHeadingValue.Name = "povHeadingValue"
			Me.povHeadingValue.Size = New System.Drawing.Size(143, 34)
			Me.povHeadingValue.TabIndex = 6
			' 
			' povPitchValue
			' 
			Me.povPitchValue.DecimalPlaces = 10
			Me.povPitchValue.Dock = System.Windows.Forms.DockStyle.Fill
			Me.povPitchValue.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.povPitchValue.Increment = New Decimal(New Integer() { 5, 0, 0, 65536})
			Me.povPitchValue.Location = New System.Drawing.Point(170, 100)
			Me.povPitchValue.Margin = New System.Windows.Forms.Padding(0)
			Me.povPitchValue.Maximum = New Decimal(New Integer() { 90, 0, 0, 0})
			Me.povPitchValue.Minimum = New Decimal(New Integer() { 90, 0, 0, -2147483648})
			Me.povPitchValue.Name = "povPitchValue"
			Me.povPitchValue.Size = New System.Drawing.Size(143, 34)
			Me.povPitchValue.TabIndex = 5
			' 
			' panoValue
			' 
			Me.panoValue.AutoSize = True
			Me.panoValue.Dock = System.Windows.Forms.DockStyle.Fill
			Me.panoValue.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.panoValue.Location = New System.Drawing.Point(170, 10)
			Me.panoValue.Margin = New System.Windows.Forms.Padding(0)
			Me.panoValue.Name = "panoValue"
			Me.panoValue.Size = New System.Drawing.Size(143, 35)
			Me.panoValue.TabIndex = 4
			Me.panoValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
					   ' 
			' label4
			' 
			Me.label4.AutoSize = True
			Me.label4.Dock = System.Windows.Forms.DockStyle.Fill
			Me.label4.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label4.Location = New System.Drawing.Point(10, 145)
			Me.label4.Margin = New System.Windows.Forms.Padding(0)
			Me.label4.Name = "label4"
			Me.label4.Size = New System.Drawing.Size(150, 35)
			Me.label4.TabIndex = 3
			Me.label4.Text = "Latitude"
			Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' label3
			' 
			Me.label3.AutoSize = True
			Me.label3.Dock = System.Windows.Forms.DockStyle.Fill
			Me.label3.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label3.Location = New System.Drawing.Point(10, 10)
			Me.label3.Margin = New System.Windows.Forms.Padding(0)
			Me.label3.Name = "label3"
			Me.label3.Size = New System.Drawing.Size(150, 35)
			Me.label3.TabIndex = 2
			Me.label3.Text = "Pano ID"
			Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' label2
			' 
			Me.label2.AutoSize = True
			Me.label2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.label2.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label2.Location = New System.Drawing.Point(10, 100)
			Me.label2.Margin = New System.Windows.Forms.Padding(0)
			Me.label2.Name = "label2"
			Me.label2.Size = New System.Drawing.Size(150, 35)
			Me.label2.TabIndex = 1
			Me.label2.Text = "POV Pitch"
			Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.label1.Font = New System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label1.Location = New System.Drawing.Point(10, 55)
			Me.label1.Margin = New System.Windows.Forms.Padding(0)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(150, 35)
			Me.label1.TabIndex = 0
			Me.label1.Text = "POV Heading"
			Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' button1
			' 
			Me.button1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.button1.Location = New System.Drawing.Point(13, 238)
			Me.button1.Name = "button1"
			Me.button1.Size = New System.Drawing.Size(144, 29)
			Me.button1.TabIndex = 9
			Me.button1.Text = "Apply Changes"
			Me.button1.UseVisualStyleBackColor = True
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1006, 721)
			Me.Controls.Add(Me.splitContainer1)
			Me.Name = "Form1"
			Me.Text = "Form1"
			Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
			AddHandler Me.FormClosed, AddressOf Form1_FormClosed
			Me.splitContainer1.Panel1.ResumeLayout(False)
			Me.splitContainer1.Panel2.ResumeLayout(False)
			DirectCast(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.splitContainer1.ResumeLayout(False)
			Me.tableLayoutPanel1.ResumeLayout(False)
			Me.tableLayoutPanel1.PerformLayout()
			DirectCast(Me.longitudeValue, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.latitudeValue, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.povHeadingValue, System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast(Me.povPitchValue, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

        Private splitContainer1 As System.Windows.Forms.SplitContainer
        Private browserView1 As DotNetBrowser.WinForms.BrowserView
        Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Private panoValue As System.Windows.Forms.Label
        Private label4 As System.Windows.Forms.Label
        Private label3 As System.Windows.Forms.Label
        Private label2 As System.Windows.Forms.Label
        Private label1 As System.Windows.Forms.Label
        Private povHeadingValue As System.Windows.Forms.NumericUpDown
        Private povPitchValue As System.Windows.Forms.NumericUpDown
        Private latitudeValue As System.Windows.Forms.NumericUpDown
        Private longitudeValue As NumericUpDown
        Private label5 As Label
        Private WithEvents button1 As Button
	End Class
End Namespace
