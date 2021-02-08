using System.Windows.Forms;

namespace GoogleStreetView.WinForms
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.browserView1 = new DotNetBrowser.WinForms.BrowserView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.longitudeValue = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.latitudeValue = new System.Windows.Forms.NumericUpDown();
            this.povHeadingValue = new System.Windows.Forms.NumericUpDown();
            this.povPitchValue = new System.Windows.Forms.NumericUpDown();
            this.panoValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.longitudeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.latitudeValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.povHeadingValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.povPitchValue)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.browserView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1006, 721);
            this.splitContainer1.SplitterDistance = 679;
            this.splitContainer1.TabIndex = 0;
            // 
            // browserView1
            // 
            this.browserView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserView1.Location = new System.Drawing.Point(0, 0);
            this.browserView1.Name = "browserView1";
            this.browserView1.Size = new System.Drawing.Size(679, 721);
            this.browserView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.longitudeValue, 3, 9);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.latitudeValue, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.povHeadingValue, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.povPitchValue, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.panoValue, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 15;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(323, 721);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // longitudeValue
            // 
            this.longitudeValue.DecimalPlaces = 12;
            this.longitudeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.longitudeValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.longitudeValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.longitudeValue.Location = new System.Drawing.Point(170, 190);
            this.longitudeValue.Margin = new System.Windows.Forms.Padding(0);
            this.longitudeValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.longitudeValue.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.longitudeValue.Name = "longitudeValue";
            this.longitudeValue.Size = new System.Drawing.Size(143, 34);
            this.longitudeValue.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label5.Location = new System.Drawing.Point(10, 190);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 35);
            this.label5.TabIndex = 8;
            this.label5.Text = "Longitude";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // latitudeValue
            // 
            this.latitudeValue.DecimalPlaces = 12;
            this.latitudeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.latitudeValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.latitudeValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            393216});
            this.latitudeValue.Location = new System.Drawing.Point(170, 145);
            this.latitudeValue.Margin = new System.Windows.Forms.Padding(0);
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
            this.latitudeValue.Size = new System.Drawing.Size(143, 34);
            this.latitudeValue.TabIndex = 7;
            // 
            // povHeadingValue
            // 
            this.povHeadingValue.DecimalPlaces = 10;
            this.povHeadingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.povHeadingValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.povHeadingValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.povHeadingValue.Location = new System.Drawing.Point(170, 55);
            this.povHeadingValue.Margin = new System.Windows.Forms.Padding(0);
            this.povHeadingValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.povHeadingValue.Name = "povHeadingValue";
            this.povHeadingValue.Size = new System.Drawing.Size(143, 34);
            this.povHeadingValue.TabIndex = 6;
            // 
            // povPitchValue
            // 
            this.povPitchValue.DecimalPlaces = 10;
            this.povPitchValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.povPitchValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.povPitchValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.povPitchValue.Location = new System.Drawing.Point(170, 100);
            this.povPitchValue.Margin = new System.Windows.Forms.Padding(0);
            this.povPitchValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.povPitchValue.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.povPitchValue.Name = "povPitchValue";
            this.povPitchValue.Size = new System.Drawing.Size(143, 34);
            this.povPitchValue.TabIndex = 5;
            // 
            // panoValue
            // 
            this.panoValue.AutoSize = true;
            this.panoValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panoValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.panoValue.Location = new System.Drawing.Point(170, 10);
            this.panoValue.Margin = new System.Windows.Forms.Padding(0);
            this.panoValue.Name = "panoValue";
            this.panoValue.Size = new System.Drawing.Size(143, 35);
            this.panoValue.TabIndex = 4;
            this.panoValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label4.Location = new System.Drawing.Point(10, 145);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 35);
            this.label4.TabIndex = 3;
            this.label4.Text = "Latitude";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 35);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pano ID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label2.Location = new System.Drawing.Point(10, 100);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "POV Pitch";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label1.Location = new System.Drawing.Point(10, 55);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "POV Heading";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(13, 238);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 29);
            this.button1.TabIndex = 9;
            this.button1.Text = "Apply Changes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.longitudeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.latitudeValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.povHeadingValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.povPitchValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DotNetBrowser.WinForms.BrowserView browserView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label panoValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown povHeadingValue;
        private System.Windows.Forms.NumericUpDown povPitchValue;
        private System.Windows.Forms.NumericUpDown latitudeValue;
        private NumericUpDown longitudeValue;
        private Label label5;
        private Button button1;
    }
}

