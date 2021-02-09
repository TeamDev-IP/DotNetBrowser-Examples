namespace DotNetBrowser.WinForms.Demo.Components
{
    partial class TabContents
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
            this.components = new System.ComponentModel.Container();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.renderingMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.browserView = new DotNetBrowser.WinForms.BrowserView();
            this.controlsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BackButton = new System.Windows.Forms.Button();
            this.menuButton = new System.Windows.Forms.Button();
            this.AddressBar = new System.Windows.Forms.TextBox();
            this.ForwardButton = new System.Windows.Forms.Button();
            this.jsConsoleLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.jsConsoleOutput = new System.Windows.Forms.RichTextBox();
            this.jsConsoleInput = new System.Windows.Forms.TextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.javaScriptConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideScrollbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.popupWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectOptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.javaScriptDialogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pDFViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hTML5VideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cssCursorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.jsConsoleLayoutPanel.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 1;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.statusStrip, 0, 3);
            this.layoutPanel.Controls.Add(this.browserView, 0, 1);
            this.layoutPanel.Controls.Add(this.controlsPanel, 0, 0);
            this.layoutPanel.Controls.Add(this.jsConsoleLayoutPanel, 0, 2);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 4;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.layoutPanel.Size = new System.Drawing.Size(800, 600);
            this.layoutPanel.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status,
            this.renderingMode});
            this.statusStrip.Location = new System.Drawing.Point(0, 575);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip.Size = new System.Drawing.Size(800, 25);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // Status
            // 
            this.Status.AutoSize = false;
            this.Status.Name = "Status";
            this.Status.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Status.Size = new System.Drawing.Size(585, 20);
            this.Status.Spring = true;
            this.Status.Text = " ";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // renderingMode
            // 
            this.renderingMode.AutoSize = false;
            this.renderingMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.renderingMode.Name = "renderingMode";
            this.renderingMode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.renderingMode.Size = new System.Drawing.Size(200, 20);
            this.renderingMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // browserView
            // 
            this.browserView.AutoSize = true;
            this.browserView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserView.Location = new System.Drawing.Point(3, 53);
            this.browserView.Name = "browserView";
            this.browserView.Size = new System.Drawing.Size(794, 419);
            this.browserView.TabIndex = 2;
            // 
            // controlsPanel
            // 
            this.controlsPanel.AutoSize = true;
            this.controlsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controlsPanel.ColumnCount = 4;
            this.controlsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.controlsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.controlsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controlsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.controlsPanel.Controls.Add(this.BackButton, 0, 0);
            this.controlsPanel.Controls.Add(this.menuButton, 3, 0);
            this.controlsPanel.Controls.Add(this.AddressBar, 2, 0);
            this.controlsPanel.Controls.Add(this.ForwardButton, 1, 0);
            this.controlsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlsPanel.Location = new System.Drawing.Point(3, 3);
            this.controlsPanel.MinimumSize = new System.Drawing.Size(800, 45);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.RowCount = 1;
            this.controlsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.controlsPanel.Size = new System.Drawing.Size(800, 45);
            this.controlsPanel.TabIndex = 3;
            // 
            // BackButton
            // 
            this.BackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackButton.FlatAppearance.BorderSize = 0;
            this.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackButton.Location = new System.Drawing.Point(3, 3);
            this.BackButton.MinimumSize = new System.Drawing.Size(38, 38);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(39, 41);
            this.BackButton.TabIndex = 0;
            this.BackButton.Text = "<";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // menuButton
            // 
            this.menuButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.menuButton.FlatAppearance.BorderSize = 0;
            this.menuButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuButton.Location = new System.Drawing.Point(759, 4);
            this.menuButton.MaximumSize = new System.Drawing.Size(38, 38);
            this.menuButton.MinimumSize = new System.Drawing.Size(38, 38);
            this.menuButton.Name = "menuButton";
            this.menuButton.Size = new System.Drawing.Size(38, 38);
            this.menuButton.TabIndex = 2;
            this.menuButton.Text = "=";
            this.menuButton.UseVisualStyleBackColor = true;
            this.menuButton.Click += new System.EventHandler(this.menuButton_Click);
            // 
            // AddressBar
            // 
            this.AddressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddressBar.Location = new System.Drawing.Point(93, 10);
            this.AddressBar.MinimumSize = new System.Drawing.Size(600, 4);
            this.AddressBar.Name = "AddressBar";
            this.AddressBar.Size = new System.Drawing.Size(659, 26);
            this.AddressBar.TabIndex = 1;
            this.AddressBar.Text = "http://teamdev.com/dotnetbrowser";
            this.AddressBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddressBar_KeyDown);
            // 
            // ForwardButton
            // 
            this.ForwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForwardButton.FlatAppearance.BorderSize = 0;
            this.ForwardButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ForwardButton.Location = new System.Drawing.Point(48, 3);
            this.ForwardButton.MinimumSize = new System.Drawing.Size(38, 38);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(39, 41);
            this.ForwardButton.TabIndex = 3;
            this.ForwardButton.Text = ">";
            this.ForwardButton.UseVisualStyleBackColor = true;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // jsConsoleLayoutPanel
            // 
            this.jsConsoleLayoutPanel.ColumnCount = 1;
            this.jsConsoleLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.jsConsoleLayoutPanel.Controls.Add(this.jsConsoleOutput, 0, 0);
            this.jsConsoleLayoutPanel.Controls.Add(this.jsConsoleInput, 0, 1);
            this.jsConsoleLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jsConsoleLayoutPanel.Location = new System.Drawing.Point(0, 475);
            this.jsConsoleLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.jsConsoleLayoutPanel.Name = "jsConsoleLayoutPanel";
            this.jsConsoleLayoutPanel.RowCount = 2;
            this.jsConsoleLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.jsConsoleLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.jsConsoleLayoutPanel.Size = new System.Drawing.Size(800, 100);
            this.jsConsoleLayoutPanel.TabIndex = 4;
            this.jsConsoleLayoutPanel.Visible = false;
            // 
            // jsConsoleOutput
            // 
            this.jsConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jsConsoleOutput.Location = new System.Drawing.Point(2, 0);
            this.jsConsoleOutput.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jsConsoleOutput.MinimumSize = new System.Drawing.Size(4, 50);
            this.jsConsoleOutput.Name = "jsConsoleOutput";
            this.jsConsoleOutput.ReadOnly = true;
            this.jsConsoleOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.jsConsoleOutput.Size = new System.Drawing.Size(796, 74);
            this.jsConsoleOutput.TabIndex = 0;
            this.jsConsoleOutput.Text = "";
            // 
            // jsConsoleInput
            // 
            this.jsConsoleInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jsConsoleInput.Location = new System.Drawing.Point(2, 76);
            this.jsConsoleInput.Margin = new System.Windows.Forms.Padding(2);
            this.jsConsoleInput.Name = "jsConsoleInput";
            this.jsConsoleInput.Size = new System.Drawing.Size(796, 22);
            this.jsConsoleInput.TabIndex = 1;
            this.jsConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.jsConsoleInput_KeyDown);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.javaScriptConsoleToolStripMenuItem,
            this.hideScrollbarsToolStripMenuItem,
            this.toolStripSeparator1,
            this.popupWindowsToolStripMenuItem,
            this.selectOptionToolStripMenuItem,
            this.uploadFileToolStripMenuItem,
            this.downloadFileToolStripMenuItem,
            this.javaScriptDialogsToolStripMenuItem,
            this.pDFViewerToolStripMenuItem,
            this.googleMapsToolStripMenuItem,
            this.hTML5VideoToolStripMenuItem,
            this.cssCursorsToolStripMenuItem,
            this.toolStripSeparator2,
            this.printToolStripMenuItem,
            this.takeScreenshotToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(202, 328);
            // 
            // javaScriptConsoleToolStripMenuItem
            // 
            this.javaScriptConsoleToolStripMenuItem.CheckOnClick = true;
            this.javaScriptConsoleToolStripMenuItem.Name = "javaScriptConsoleToolStripMenuItem";
            this.javaScriptConsoleToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.javaScriptConsoleToolStripMenuItem.Text = "JavaScript Console";
            this.javaScriptConsoleToolStripMenuItem.CheckedChanged += new System.EventHandler(this.javaScriptConsoleToolStripMenuItem_CheckedChanged);
            // 
            // hideScrollbarsToolStripMenuItem
            // 
            this.hideScrollbarsToolStripMenuItem.CheckOnClick = true;
            this.hideScrollbarsToolStripMenuItem.Name = "hideScrollbarsToolStripMenuItem";
            this.hideScrollbarsToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.hideScrollbarsToolStripMenuItem.Text = "Hide Scrollbars";
            this.hideScrollbarsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.hideScrollbarsToolStripMenuItem_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // popupWindowsToolStripMenuItem
            // 
            this.popupWindowsToolStripMenuItem.Name = "popupWindowsToolStripMenuItem";
            this.popupWindowsToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.popupWindowsToolStripMenuItem.Text = "Popup Windows";
            this.popupWindowsToolStripMenuItem.Click += new System.EventHandler(this.popupWindowsToolStripMenuItem_Click);
            // 
            // selectOptionToolStripMenuItem
            // 
            this.selectOptionToolStripMenuItem.Name = "selectOptionToolStripMenuItem";
            this.selectOptionToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.selectOptionToolStripMenuItem.Text = "Select && Option";
            this.selectOptionToolStripMenuItem.Click += new System.EventHandler(this.selectOptionToolStripMenuItem_Click);
            // 
            // uploadFileToolStripMenuItem
            // 
            this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.uploadFileToolStripMenuItem.Text = "Upload File";
            this.uploadFileToolStripMenuItem.Click += new System.EventHandler(this.uploadFileToolStripMenuItem_Click);
            // 
            // downloadFileToolStripMenuItem
            // 
            this.downloadFileToolStripMenuItem.Name = "downloadFileToolStripMenuItem";
            this.downloadFileToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.downloadFileToolStripMenuItem.Text = "Download File";
            this.downloadFileToolStripMenuItem.Click += new System.EventHandler(this.downloadFileToolStripMenuItem_Click);
            // 
            // javaScriptDialogsToolStripMenuItem
            // 
            this.javaScriptDialogsToolStripMenuItem.Name = "javaScriptDialogsToolStripMenuItem";
            this.javaScriptDialogsToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.javaScriptDialogsToolStripMenuItem.Text = "JavaScript Dialogs";
            this.javaScriptDialogsToolStripMenuItem.Click += new System.EventHandler(this.javaScriptDialogsToolStripMenuItem_Click);
            // 
            // pDFViewerToolStripMenuItem
            // 
            this.pDFViewerToolStripMenuItem.Name = "pDFViewerToolStripMenuItem";
            this.pDFViewerToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.pDFViewerToolStripMenuItem.Text = "PDF Viewer";
            this.pDFViewerToolStripMenuItem.Click += new System.EventHandler(this.pDFViewerToolStripMenuItem_Click);
            // 
            // googleMapsToolStripMenuItem
            // 
            this.googleMapsToolStripMenuItem.Name = "googleMapsToolStripMenuItem";
            this.googleMapsToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.googleMapsToolStripMenuItem.Text = "Google Maps";
            this.googleMapsToolStripMenuItem.Click += new System.EventHandler(this.googleMapsToolStripMenuItem_Click);
            // 
            // hTML5VideoToolStripMenuItem
            // 
            this.hTML5VideoToolStripMenuItem.Name = "hTML5VideoToolStripMenuItem";
            this.hTML5VideoToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.hTML5VideoToolStripMenuItem.Text = "HTML5 Video";
            this.hTML5VideoToolStripMenuItem.Click += new System.EventHandler(this.hTML5VideoToolStripMenuItem_Click);
            // 
            // cssCursorsToolStripMenuItem
            // 
            this.cssCursorsToolStripMenuItem.Name = "cssCursorsToolStripMenuItem";
            this.cssCursorsToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.cssCursorsToolStripMenuItem.Text = "CSS Cursors";
            this.cssCursorsToolStripMenuItem.Click += new System.EventHandler(this.cssCursorsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // takeScreenshotToolStripMenuItem
            // 
            this.takeScreenshotToolStripMenuItem.Name = "takeScreenshotToolStripMenuItem";
            this.takeScreenshotToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.takeScreenshotToolStripMenuItem.Text = "Take Screenshot";
            this.takeScreenshotToolStripMenuItem.Click += new System.EventHandler(this.takeScreenshotToolStripMenuItem_Click);
            // 
            // TabContents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutPanel);
            this.Name = "TabContents";
            this.Size = new System.Drawing.Size(800, 600);
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.jsConsoleLayoutPanel.ResumeLayout(false);
            this.jsConsoleLayoutPanel.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private BrowserView browserView;
        private System.Windows.Forms.TableLayoutPanel controlsPanel;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button menuButton;
        private System.Windows.Forms.Button ForwardButton;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        public System.Windows.Forms.TextBox AddressBar;
        public System.Windows.Forms.ToolStripStatusLabel renderingMode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem javaScriptConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideScrollbarsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem popupWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectOptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem javaScriptDialogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pDFViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hTML5VideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeScreenshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cssCursorsToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel jsConsoleLayoutPanel;
        private System.Windows.Forms.RichTextBox jsConsoleOutput;
        private System.Windows.Forms.TextBox jsConsoleInput;
    }
}
