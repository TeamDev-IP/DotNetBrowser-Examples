namespace DotNetBrowser.WinForms.Demo.Components
{
    partial class Tab
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.layoutPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(3, 3);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(64, 17);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "New Tab";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.titleLabel_MouseUp);
            // 
            // closeButton
            // 
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Image = global::DotNetBrowser.WinForms.Demo.Properties.Resources.icon_close;
            this.closeButton.Location = new System.Drawing.Point(172, 0);
            this.closeButton.Margin = new System.Windows.Forms.Padding(0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(26, 28);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = " ";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 2;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutPanel.Controls.Add(this.closeButton, 1, 0);
            this.layoutPanel.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 1;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Size = new System.Drawing.Size(198, 28);
            this.layoutPanel.TabIndex = 1;
            this.layoutPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel1_MouseUp);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.titleLabel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(166, 22);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // Tab
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.layoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(200, 30);
            this.MinimumSize = new System.Drawing.Size(40, 30);
            this.Name = "Tab";
            this.Size = new System.Drawing.Size(198, 28);
            this.layoutPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
