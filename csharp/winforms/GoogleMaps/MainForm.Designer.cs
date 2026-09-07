namespace GoogleMaps.WinForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.browserView = new DotNetBrowser.WinForms.BrowserView();
            this.mapControls = new System.Windows.Forms.FlowLayoutPanel();
            this.ZoomInBtn = new System.Windows.Forms.Button();
            this.ZoomOutBtn = new System.Windows.Forms.Button();
            this.latitudeLabel = new System.Windows.Forms.Label();
            this.latitudeValue = new System.Windows.Forms.NumericUpDown();
            this.longitudeLabel = new System.Windows.Forms.Label();
            this.longitudeValue = new System.Windows.Forms.NumericUpDown();
            this.AddMarkerBtn = new System.Windows.Forms.Button();
            this.MyLocationBtn = new System.Windows.Forms.Button();
            this.mapControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.latitudeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.longitudeValue)).BeginInit();
            this.SuspendLayout();
            //
            // browserView
            //
            this.browserView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserView.Location = new System.Drawing.Point(0, 46);
            this.browserView.Name = "browserView";
            this.browserView.Size = new System.Drawing.Size(1067, 508);
            this.browserView.TabIndex = 1;
            //
            // mapControls
            //
            this.mapControls.Controls.Add(this.ZoomInBtn);
            this.mapControls.Controls.Add(this.ZoomOutBtn);
            this.mapControls.Controls.Add(this.latitudeLabel);
            this.mapControls.Controls.Add(this.latitudeValue);
            this.mapControls.Controls.Add(this.longitudeLabel);
            this.mapControls.Controls.Add(this.longitudeValue);
            this.mapControls.Controls.Add(this.AddMarkerBtn);
            this.mapControls.Controls.Add(this.MyLocationBtn);
            this.mapControls.Dock = System.Windows.Forms.DockStyle.Top;
            // The controls stay disabled until the map reports that it is ready.
            this.mapControls.Enabled = false;
            this.mapControls.Location = new System.Drawing.Point(0, 0);
            this.mapControls.Name = "mapControls";
            this.mapControls.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.mapControls.Size = new System.Drawing.Size(1067, 46);
            this.mapControls.TabIndex = 0;
            this.mapControls.WrapContents = false;
            //
            // ZoomInBtn
            //
            this.ZoomInBtn.Location = new System.Drawing.Point(8, 8);
            this.ZoomInBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ZoomInBtn.Name = "ZoomInBtn";
            this.ZoomInBtn.Size = new System.Drawing.Size(100, 28);
            this.ZoomInBtn.TabIndex = 0;
            this.ZoomInBtn.Text = "Zoom +";
            this.ZoomInBtn.UseVisualStyleBackColor = true;
            this.ZoomInBtn.Click += new System.EventHandler(this.ZoomInBtn_Click);
            //
            // ZoomOutBtn
            //
            this.ZoomOutBtn.Location = new System.Drawing.Point(116, 8);
            this.ZoomOutBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ZoomOutBtn.Name = "ZoomOutBtn";
            this.ZoomOutBtn.Size = new System.Drawing.Size(100, 28);
            this.ZoomOutBtn.TabIndex = 1;
            this.ZoomOutBtn.Text = "Zoom -";
            this.ZoomOutBtn.UseVisualStyleBackColor = true;
            this.ZoomOutBtn.Click += new System.EventHandler(this.ZoomOutBtn_Click);
            //
            // latitudeLabel
            //
            this.latitudeLabel.AutoSize = true;
            this.latitudeLabel.Location = new System.Drawing.Point(240, 14);
            this.latitudeLabel.Margin = new System.Windows.Forms.Padding(20, 10, 4, 4);
            this.latitudeLabel.Name = "latitudeLabel";
            this.latitudeLabel.Size = new System.Drawing.Size(62, 17);
            this.latitudeLabel.TabIndex = 2;
            this.latitudeLabel.Text = "Latitude:";
            //
            // latitudeValue
            //
            this.latitudeValue.DecimalPlaces = 6;
            this.latitudeValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.latitudeValue.Location = new System.Drawing.Point(310, 8);
            this.latitudeValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.latitudeValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.latitudeValue.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.latitudeValue.Name = "latitudeValue";
            this.latitudeValue.Size = new System.Drawing.Size(120, 22);
            this.latitudeValue.TabIndex = 3;
            this.latitudeValue.Value = new decimal(new int[] {
            48209331,
            0,
            0,
            393216});
            //
            // longitudeLabel
            //
            this.longitudeLabel.AutoSize = true;
            this.longitudeLabel.Location = new System.Drawing.Point(454, 14);
            this.longitudeLabel.Margin = new System.Windows.Forms.Padding(20, 10, 4, 4);
            this.longitudeLabel.Name = "longitudeLabel";
            this.longitudeLabel.Size = new System.Drawing.Size(72, 17);
            this.longitudeLabel.TabIndex = 4;
            this.longitudeLabel.Text = "Longitude:";
            //
            // longitudeValue
            //
            this.longitudeValue.DecimalPlaces = 6;
            this.longitudeValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.longitudeValue.Location = new System.Drawing.Point(534, 8);
            this.longitudeValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.longitudeValue.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.longitudeValue.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.longitudeValue.Name = "longitudeValue";
            this.longitudeValue.Size = new System.Drawing.Size(120, 22);
            this.longitudeValue.TabIndex = 5;
            this.longitudeValue.Value = new decimal(new int[] {
            16381302,
            0,
            0,
            393216});
            //
            // AddMarkerBtn
            //
            this.AddMarkerBtn.Location = new System.Drawing.Point(678, 8);
            this.AddMarkerBtn.Margin = new System.Windows.Forms.Padding(20, 4, 4, 4);
            this.AddMarkerBtn.Name = "AddMarkerBtn";
            this.AddMarkerBtn.Size = new System.Drawing.Size(120, 28);
            this.AddMarkerBtn.TabIndex = 6;
            this.AddMarkerBtn.Text = "Add marker";
            this.AddMarkerBtn.UseVisualStyleBackColor = true;
            this.AddMarkerBtn.Click += new System.EventHandler(this.AddMarkerBtn_Click);
            //
            // MyLocationBtn
            //
            this.MyLocationBtn.Location = new System.Drawing.Point(806, 8);
            this.MyLocationBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MyLocationBtn.Name = "MyLocationBtn";
            this.MyLocationBtn.Size = new System.Drawing.Size(120, 28);
            this.MyLocationBtn.TabIndex = 7;
            this.MyLocationBtn.Text = "My location";
            this.MyLocationBtn.UseVisualStyleBackColor = true;
            this.MyLocationBtn.Click += new System.EventHandler(this.MyLocationBtn_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            // The browser view is added first on purpose: WinForms lays docked
            // controls out from the last control in the collection to the first,
            // so the one that fills the remaining space must come first.
            this.Controls.Add(this.browserView);
            this.Controls.Add(this.mapControls);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "Google Maps";
            this.mapControls.ResumeLayout(false);
            this.mapControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.latitudeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.longitudeValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetBrowser.WinForms.BrowserView browserView;
        private System.Windows.Forms.FlowLayoutPanel mapControls;
        private System.Windows.Forms.Button ZoomInBtn;
        private System.Windows.Forms.Button ZoomOutBtn;
        private System.Windows.Forms.Label latitudeLabel;
        private System.Windows.Forms.NumericUpDown latitudeValue;
        private System.Windows.Forms.Label longitudeLabel;
        private System.Windows.Forms.NumericUpDown longitudeValue;
        private System.Windows.Forms.Button AddMarkerBtn;
        private System.Windows.Forms.Button MyLocationBtn;
    }
}
