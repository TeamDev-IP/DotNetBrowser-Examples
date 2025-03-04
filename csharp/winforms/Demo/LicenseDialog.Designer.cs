using System.ComponentModel;

namespace DotNetBrowser.WinForms.Demo
{
    partial class LicenseDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.TextBox InputBox;
        
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
            this.ApplyButton = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.lbMessage = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ApplyButton
            // 
            this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyButton.AutoSize = true;
            this.ApplyButton.Location = new System.Drawing.Point(226, 99);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(0, 15, 15, 10);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(134, 23);
            this.ApplyButton.TabIndex = 0;
            this.ApplyButton.Text = "Apply and start the demo";
            this.ApplyButton.UseVisualStyleBackColor = true;
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(15, 64);
            this.InputBox.Margin = new System.Windows.Forms.Padding(15, 10, 15, 0);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(345, 20);
            this.InputBox.TabIndex = 1;
            // 
            // lbMessage
            // 
            this.lbMessage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbMessage.AutoSize = true;
            this.lbMessage.Location = new System.Drawing.Point(35, 15);
            this.lbMessage.Margin = new System.Windows.Forms.Padding(35, 15, 25, 0);
            this.lbMessage.MaximumSize = new System.Drawing.Size(310, 1000);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(291, 39);
            this.lbMessage.TabIndex = 2;
            this.lbMessage.Text = "Thank you for trying DotNetBrowser in Windows Forms.\r\n\r\nTo run the demo, get the " + "free trial license and insert it below:\r\n";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbMessage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ApplyButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.InputBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(375, 130);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // LicenseDialog
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(375, 130);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LicenseDialog";
            this.Text = "Configure DotNetBrowser license";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.Label lbMessage;

        #endregion
    }
}

