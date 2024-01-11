namespace DotNetBrowser.WinForms.Demo.Components
{
    partial class TabbedPane
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
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.contentContainer = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.addTabButton = new System.Windows.Forms.Button();
            this.captions = new System.Windows.Forms.FlowLayoutPanel();
            this.layoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 1;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.contentContainer, 0, 1);
            this.layoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 2;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Size = new System.Drawing.Size(779, 503);
            this.layoutPanel.TabIndex = 0;
            // 
            // contentContainer
            // 
            this.contentContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentContainer.Location = new System.Drawing.Point(0, 30);
            this.contentContainer.Margin = new System.Windows.Forms.Padding(0);
            this.contentContainer.Name = "contentContainer";
            this.contentContainer.Size = new System.Drawing.Size(779, 473);
            this.contentContainer.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Controls.Add(this.addTabButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.captions, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(779, 30);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // addTabButton
            // 
            this.addTabButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addTabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTabButton.Location = new System.Drawing.Point(754, 0);
            this.addTabButton.Margin = new System.Windows.Forms.Padding(0);
            this.addTabButton.Name = "addTabButton";
            this.addTabButton.Size = new System.Drawing.Size(25, 30);
            this.addTabButton.TabIndex = 0;
            this.addTabButton.Text = "+";
            this.addTabButton.UseVisualStyleBackColor = true;
            this.addTabButton.Click += new System.EventHandler(this.addTabButton_Click);
            // 
            // captions
            // 
            this.captions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captions.Location = new System.Drawing.Point(0, 0);
            this.captions.Margin = new System.Windows.Forms.Padding(0);
            this.captions.Name = "captions";
            this.captions.Size = new System.Drawing.Size(754, 30);
            this.captions.TabIndex = 1;
            this.captions.Resize += new System.EventHandler(this.captions_Resize);
            // 
            // TabbedPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutPanel);
            this.Name = "TabbedPane";
            this.Size = new System.Drawing.Size(779, 503);
            this.layoutPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Panel contentContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button addTabButton;
        private System.Windows.Forms.FlowLayoutPanel captions;
    }
}
