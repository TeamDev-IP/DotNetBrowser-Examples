namespace WinForms.ApplicationCacheSample
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
            this.browserView = new DotNetBrowser.WinForms.WinFormsBrowserView();
            this.OriginURLs = new System.Windows.Forms.Button();
            this.getByOriginURL = new System.Windows.Forms.Button();
            this.removalForManifestURL = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // browserView
            // 
            this.browserView.AcceptLanguage = null;
            this.browserView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browserView.AudioMuted = null;
            this.browserView.BrowserType = DotNetBrowser.BrowserType.HEAVYWEIGHT;
            this.browserView.Location = new System.Drawing.Point(0, 31);
            this.browserView.Name = "browserView";
            this.browserView.Preferences = null;
            this.browserView.Size = new System.Drawing.Size(883, 531);
            this.browserView.TabIndex = 0;
            this.browserView.URL = null;
            this.browserView.ZoomLevel = null;
            // 
            // OriginURLs
            // 
            this.OriginURLs.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OriginURLs.Location = new System.Drawing.Point(22, 3);
            this.OriginURLs.Name = "OriginURLs";
            this.OriginURLs.Size = new System.Drawing.Size(250, 23);
            this.OriginURLs.TabIndex = 0;
            this.OriginURLs.Text = "All sites with application cache";
            this.OriginURLs.UseVisualStyleBackColor = true;
            this.OriginURLs.Click += new System.EventHandler(this.OriginURLs_Click);
            // 
            // getByOriginURL
            // 
            this.getByOriginURL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.getByOriginURL.Location = new System.Drawing.Point(317, 3);
            this.getByOriginURL.Name = "getByOriginURL";
            this.getByOriginURL.Size = new System.Drawing.Size(250, 23);
            this.getByOriginURL.TabIndex = 1;
            this.getByOriginURL.Text = "All application cache manifests";
            this.getByOriginURL.UseVisualStyleBackColor = true;
            this.getByOriginURL.Click += new System.EventHandler(this.getByOriginURL_Click);
            // 
            // removalForManifestURL
            // 
            this.removalForManifestURL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.removalForManifestURL.Location = new System.Drawing.Point(612, 3);
            this.removalForManifestURL.Name = "removalForManifestURL";
            this.removalForManifestURL.Size = new System.Drawing.Size(250, 23);
            this.removalForManifestURL.TabIndex = 2;
            this.removalForManifestURL.Text = "Remove \'http://www.w3schools.com\' manifest";
            this.removalForManifestURL.UseVisualStyleBackColor = true;
            this.removalForManifestURL.Click += new System.EventHandler(this.removalForManifestURL_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 293F));
            this.tableLayoutPanel1.Controls.Add(this.getByOriginURL, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.removalForManifestURL, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.OriginURLs, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 32);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.browserView);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetBrowser.WinForms.WinFormsBrowserView browserView;
        private System.Windows.Forms.Button OriginURLs;
        private System.Windows.Forms.Button getByOriginURL;
        private System.Windows.Forms.Button removalForManifestURL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

