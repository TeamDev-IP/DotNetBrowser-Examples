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
			Me.browserView = New DotNetBrowser.WinForms.BrowserView()
			Me.mapControls = New System.Windows.Forms.FlowLayoutPanel()
			Me.ZoomInBtn = New System.Windows.Forms.Button()
			Me.ZoomOutBtn = New System.Windows.Forms.Button()
			Me.latitudeLabel = New System.Windows.Forms.Label()
			Me.latitudeValue = New System.Windows.Forms.NumericUpDown()
			Me.longitudeLabel = New System.Windows.Forms.Label()
			Me.longitudeValue = New System.Windows.Forms.NumericUpDown()
			Me.AddMarkerBtn = New System.Windows.Forms.Button()
			Me.MyLocationBtn = New System.Windows.Forms.Button()
			Me.mapControls.SuspendLayout()
			CType(Me.latitudeValue, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.longitudeValue, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			' browserView
			'
			Me.browserView.Dock = System.Windows.Forms.DockStyle.Fill
			Me.browserView.Location = New System.Drawing.Point(0, 46)
			Me.browserView.Name = "browserView"
			Me.browserView.Size = New System.Drawing.Size(1067, 508)
			Me.browserView.TabIndex = 1
			'
			' mapControls
			'
			Me.mapControls.Controls.Add(Me.ZoomInBtn)
			Me.mapControls.Controls.Add(Me.ZoomOutBtn)
			Me.mapControls.Controls.Add(Me.latitudeLabel)
			Me.mapControls.Controls.Add(Me.latitudeValue)
			Me.mapControls.Controls.Add(Me.longitudeLabel)
			Me.mapControls.Controls.Add(Me.longitudeValue)
			Me.mapControls.Controls.Add(Me.AddMarkerBtn)
			Me.mapControls.Controls.Add(Me.MyLocationBtn)
			Me.mapControls.Dock = System.Windows.Forms.DockStyle.Top
			' The controls stay disabled until the map reports that it is ready.
			Me.mapControls.Enabled = False
			Me.mapControls.Location = New System.Drawing.Point(0, 0)
			Me.mapControls.Name = "mapControls"
			Me.mapControls.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.mapControls.Size = New System.Drawing.Size(1067, 46)
			Me.mapControls.TabIndex = 0
			Me.mapControls.WrapContents = False
			'
			' ZoomInBtn
			'
			Me.ZoomInBtn.Location = New System.Drawing.Point(8, 8)
			Me.ZoomInBtn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.ZoomInBtn.Name = "ZoomInBtn"
			Me.ZoomInBtn.Size = New System.Drawing.Size(100, 28)
			Me.ZoomInBtn.TabIndex = 0
			Me.ZoomInBtn.Text = "Zoom +"
			Me.ZoomInBtn.UseVisualStyleBackColor = True
			'
			' ZoomOutBtn
			'
			Me.ZoomOutBtn.Location = New System.Drawing.Point(116, 8)
			Me.ZoomOutBtn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.ZoomOutBtn.Name = "ZoomOutBtn"
			Me.ZoomOutBtn.Size = New System.Drawing.Size(100, 28)
			Me.ZoomOutBtn.TabIndex = 1
			Me.ZoomOutBtn.Text = "Zoom -"
			Me.ZoomOutBtn.UseVisualStyleBackColor = True
			'
			' latitudeLabel
			'
			Me.latitudeLabel.AutoSize = True
			Me.latitudeLabel.Location = New System.Drawing.Point(240, 14)
			Me.latitudeLabel.Margin = New System.Windows.Forms.Padding(20, 10, 4, 4)
			Me.latitudeLabel.Name = "latitudeLabel"
			Me.latitudeLabel.Size = New System.Drawing.Size(62, 17)
			Me.latitudeLabel.TabIndex = 2
			Me.latitudeLabel.Text = "Latitude:"
			'
			' latitudeValue
			'
			Me.latitudeValue.DecimalPlaces = 6
			Me.latitudeValue.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
			Me.latitudeValue.Location = New System.Drawing.Point(310, 8)
			Me.latitudeValue.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.latitudeValue.Maximum = New Decimal(New Integer() {90, 0, 0, 0})
			Me.latitudeValue.Minimum = New Decimal(New Integer() {90, 0, 0, -2147483648})
			Me.latitudeValue.Name = "latitudeValue"
			Me.latitudeValue.Size = New System.Drawing.Size(120, 22)
			Me.latitudeValue.TabIndex = 3
			Me.latitudeValue.Value = New Decimal(New Integer() {48209331, 0, 0, 393216})
			'
			' longitudeLabel
			'
			Me.longitudeLabel.AutoSize = True
			Me.longitudeLabel.Location = New System.Drawing.Point(454, 14)
			Me.longitudeLabel.Margin = New System.Windows.Forms.Padding(20, 10, 4, 4)
			Me.longitudeLabel.Name = "longitudeLabel"
			Me.longitudeLabel.Size = New System.Drawing.Size(72, 17)
			Me.longitudeLabel.TabIndex = 4
			Me.longitudeLabel.Text = "Longitude:"
			'
			' longitudeValue
			'
			Me.longitudeValue.DecimalPlaces = 6
			Me.longitudeValue.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
			Me.longitudeValue.Location = New System.Drawing.Point(534, 8)
			Me.longitudeValue.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.longitudeValue.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
			Me.longitudeValue.Minimum = New Decimal(New Integer() {180, 0, 0, -2147483648})
			Me.longitudeValue.Name = "longitudeValue"
			Me.longitudeValue.Size = New System.Drawing.Size(120, 22)
			Me.longitudeValue.TabIndex = 5
			Me.longitudeValue.Value = New Decimal(New Integer() {16381302, 0, 0, 393216})
			'
			' AddMarkerBtn
			'
			Me.AddMarkerBtn.Location = New System.Drawing.Point(678, 8)
			Me.AddMarkerBtn.Margin = New System.Windows.Forms.Padding(20, 4, 4, 4)
			Me.AddMarkerBtn.Name = "AddMarkerBtn"
			Me.AddMarkerBtn.Size = New System.Drawing.Size(120, 28)
			Me.AddMarkerBtn.TabIndex = 6
			Me.AddMarkerBtn.Text = "Add marker"
			Me.AddMarkerBtn.UseVisualStyleBackColor = True
			'
			' MyLocationBtn
			'
			Me.MyLocationBtn.Location = New System.Drawing.Point(806, 8)
			Me.MyLocationBtn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.MyLocationBtn.Name = "MyLocationBtn"
			Me.MyLocationBtn.Size = New System.Drawing.Size(120, 28)
			Me.MyLocationBtn.TabIndex = 7
			Me.MyLocationBtn.Text = "My location"
			Me.MyLocationBtn.UseVisualStyleBackColor = True
			'
			' MainForm
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(8F, 16F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1067, 554)
			' The browser view is added first on purpose: WinForms lays docked
			' controls out from the last control in the collection to the first,
			' so the one that fills the remaining space must come first.
			Me.Controls.Add(Me.browserView)
			Me.Controls.Add(Me.mapControls)
			Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
			Me.Name = "MainForm"
			Me.Text = "Google Maps"
			Me.mapControls.ResumeLayout(False)
			Me.mapControls.PerformLayout()
			CType(Me.latitudeValue, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.longitudeValue, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private WithEvents browserView As DotNetBrowser.WinForms.BrowserView
		Private WithEvents mapControls As System.Windows.Forms.FlowLayoutPanel
		Private WithEvents ZoomInBtn As System.Windows.Forms.Button
		Private WithEvents ZoomOutBtn As System.Windows.Forms.Button
		Private WithEvents latitudeLabel As System.Windows.Forms.Label
		Private WithEvents latitudeValue As System.Windows.Forms.NumericUpDown
		Private WithEvents longitudeLabel As System.Windows.Forms.Label
		Private WithEvents longitudeValue As System.Windows.Forms.NumericUpDown
		Private WithEvents AddMarkerBtn As System.Windows.Forms.Button
		Private WithEvents MyLocationBtn As System.Windows.Forms.Button
	End Class
End Namespace
