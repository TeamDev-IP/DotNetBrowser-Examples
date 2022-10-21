Namespace GoogleMaps.WinForms
	Partial Public Class MainForm
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
			Me.ZoomInBtn = New System.Windows.Forms.Button()
			Me.ZoomOutBtn = New System.Windows.Forms.Button()
			Me.SuspendLayout()
			' 
			' ZoomInBtn
			' 
			Me.ZoomInBtn.Location = New System.Drawing.Point(927, 15)
			Me.ZoomInBtn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.ZoomInBtn.Name = "ZoomInBtn"
			Me.ZoomInBtn.Size = New System.Drawing.Size(100, 28)
			Me.ZoomInBtn.TabIndex = 0
			Me.ZoomInBtn.Text = "Zoom +"
			Me.ZoomInBtn.UseVisualStyleBackColor = True
			' 
			' ZoomOutBtn
			' 
			Me.ZoomOutBtn.Location = New System.Drawing.Point(927, 50)
			Me.ZoomOutBtn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.ZoomOutBtn.Name = "ZoomOutBtn"
			Me.ZoomOutBtn.Size = New System.Drawing.Size(100, 28)
			Me.ZoomOutBtn.TabIndex = 1
			Me.ZoomOutBtn.Text = "Zoom -"
			Me.ZoomOutBtn.UseVisualStyleBackColor = True
			' 
			' MainForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1067, 554)
			Me.Controls.Add(Me.ZoomOutBtn)
			Me.Controls.Add(Me.ZoomInBtn)
			Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.Name = "MainForm"
			Me.Text = "Google Maps "
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private WithEvents ZoomInBtn As System.Windows.Forms.Button
		Private WithEvents ZoomOutBtn As System.Windows.Forms.Button
	End Class
End Namespace

