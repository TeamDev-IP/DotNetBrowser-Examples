Namespace Profiles.WinForms
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
			Me.profilesList = New System.Windows.Forms.ListBox()
			Me.tableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
			Me.label1 = New System.Windows.Forms.Label()
			Me.tableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
			Me.label2 = New System.Windows.Forms.Label()
			Me.label3 = New System.Windows.Forms.Label()
			Me.profileName = New System.Windows.Forms.TextBox()
			Me.createProfileButton = New System.Windows.Forms.Button()
			Me.tableLayoutPanel1.SuspendLayout()
			Me.tableLayoutPanel2.SuspendLayout()
			Me.tableLayoutPanel3.SuspendLayout()
			Me.SuspendLayout()
			' 
			' tableLayoutPanel1
			' 
			Me.tableLayoutPanel1.ColumnCount = 1
			Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel1.Controls.Add(Me.profilesList, 0, 1)
			Me.tableLayoutPanel1.Controls.Add(Me.tableLayoutPanel2, 0, 2)
			Me.tableLayoutPanel1.Controls.Add(Me.label1, 0, 0)
			Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
			Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
			Me.tableLayoutPanel1.RowCount = 3
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F))
			Me.tableLayoutPanel1.Size = New System.Drawing.Size(800, 450)
			Me.tableLayoutPanel1.TabIndex = 0
			' 
			' profilesList
			' 
			Me.profilesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
			Me.profilesList.Dock = System.Windows.Forms.DockStyle.Fill
			Me.profilesList.FormattingEnabled = True
			Me.profilesList.ItemHeight = 16
			Me.profilesList.Location = New System.Drawing.Point(3, 37)
			Me.profilesList.Name = "profilesList"
			Me.profilesList.Size = New System.Drawing.Size(794, 325)
			Me.profilesList.TabIndex = 0
			AddHandler profilesList.DoubleClick, AddressOf listBox1_DoubleClick_1
			AddHandler profilesList.MouseUp, AddressOf profilesList_MouseUp
			' 
			' tableLayoutPanel2
			' 
			Me.tableLayoutPanel2.ColumnCount = 1
			Me.tableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel2.Controls.Add(Me.tableLayoutPanel3, 0, 1)
			Me.tableLayoutPanel2.Controls.Add(Me.label2, 0, 0)
			Me.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
			Me.tableLayoutPanel2.Location = New System.Drawing.Point(3, 368)
			Me.tableLayoutPanel2.Name = "tableLayoutPanel2"
			Me.tableLayoutPanel2.RowCount = 2
			Me.tableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F))
			Me.tableLayoutPanel2.Size = New System.Drawing.Size(794, 79)
			Me.tableLayoutPanel2.TabIndex = 1
			' 
			' label1
			' 
			Me.label1.Anchor = System.Windows.Forms.AnchorStyles.Left
			Me.label1.AutoSize = True
			Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label1.ForeColor = System.Drawing.SystemColors.GrayText
			Me.label1.Location = New System.Drawing.Point(3, 8)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(594, 17)
			Me.label1.TabIndex = 2
			Me.label1.Text = "Double-click profile in the list to launch the new browser, right-click to try re" & "moving the profile."
			' 
			' tableLayoutPanel3
			' 
			Me.tableLayoutPanel3.ColumnCount = 3
			Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F))
			Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F))
			Me.tableLayoutPanel3.Controls.Add(Me.label3, 0, 0)
			Me.tableLayoutPanel3.Controls.Add(Me.profileName, 1, 0)
			Me.tableLayoutPanel3.Controls.Add(Me.createProfileButton, 2, 0)
			Me.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
			Me.tableLayoutPanel3.Location = New System.Drawing.Point(3, 29)
			Me.tableLayoutPanel3.Name = "tableLayoutPanel3"
			Me.tableLayoutPanel3.RowCount = 1
			Me.tableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F))
			Me.tableLayoutPanel3.Size = New System.Drawing.Size(788, 47)
			Me.tableLayoutPanel3.TabIndex = 0
			' 
			' label2
			' 
			Me.label2.Anchor = System.Windows.Forms.AnchorStyles.Left
			Me.label2.AutoSize = True
			Me.label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte(254)))
			Me.label2.Location = New System.Drawing.Point(3, 4)
			Me.label2.Name = "label2"
			Me.label2.Size = New System.Drawing.Size(140, 17)
			Me.label2.TabIndex = 1
			Me.label2.Text = "Create new profile"
			' 
			' label3
			' 
			Me.label3.Anchor = System.Windows.Forms.AnchorStyles.Left
			Me.label3.AutoSize = True
			Me.label3.Location = New System.Drawing.Point(3, 15)
			Me.label3.Name = "label3"
			Me.label3.Size = New System.Drawing.Size(91, 17)
			Me.label3.TabIndex = 0
			Me.label3.Text = "Profile name:"
			' 
			' profileName
			' 
			Me.profileName.Anchor = (CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.profileName.Location = New System.Drawing.Point(103, 12)
			Me.profileName.Name = "profileName"
			Me.profileName.Size = New System.Drawing.Size(597, 22)
			Me.profileName.TabIndex = 1
			Me.profileName.Text = "Test name"
			' 
			' createProfileButton
			' 
			Me.createProfileButton.Anchor = (CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
			Me.createProfileButton.Location = New System.Drawing.Point(706, 12)
			Me.createProfileButton.Name = "createProfileButton"
			Me.createProfileButton.Size = New System.Drawing.Size(79, 23)
			Me.createProfileButton.TabIndex = 2
			Me.createProfileButton.Text = "Create"
			Me.createProfileButton.UseVisualStyleBackColor = True
			AddHandler createProfileButton.Click, AddressOf createProfileButton_Click
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(800, 450)
			Me.Controls.Add(Me.tableLayoutPanel1)
			Me.Name = "Form1"
			Me.Text = "Profiles"
			AddHandler Me.FormClosed, AddressOf Form1_FormClosed
			Me.tableLayoutPanel1.ResumeLayout(False)
			Me.tableLayoutPanel1.PerformLayout()
			Me.tableLayoutPanel2.ResumeLayout(False)
			Me.tableLayoutPanel2.PerformLayout()
			Me.tableLayoutPanel3.ResumeLayout(False)
			Me.tableLayoutPanel3.PerformLayout()
			Me.ResumeLayout(False)


		End Sub

		#End Region

		Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
		Private profilesList As System.Windows.Forms.ListBox
		Private tableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
		Private tableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
		Private label3 As System.Windows.Forms.Label
		Private profileName As System.Windows.Forms.TextBox
		Private createProfileButton As System.Windows.Forms.Button
		Private label2 As System.Windows.Forms.Label
		Private label1 As System.Windows.Forms.Label
	End Class
End Namespace

