<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.BrowserView = New DotNetBrowser.WinForms.BrowserView()
        Me.TextBox = New System.Windows.Forms.TextBox()
        Me.FindButton = New System.Windows.Forms.Button()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.TableLayoutPanel.SuspendLayout
        Me.SuspendLayout
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.ColumnCount = 4
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel.Controls.Add(Me.BrowserView, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.TextBox, 0, 1)
        Me.TableLayoutPanel.Controls.Add(Me.FindButton, 1, 1)
        Me.TableLayoutPanel.Controls.Add(Me.ClearButton, 2, 1)
        Me.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 2
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.TableLayoutPanel.Size = New System.Drawing.Size(1017, 522)
        Me.TableLayoutPanel.TabIndex = 0
        '
        'BrowserView
        '
        Me.TableLayoutPanel.SetColumnSpan(Me.BrowserView, 4)
        Me.BrowserView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BrowserView.Location = New System.Drawing.Point(3, 3)
        Me.BrowserView.Name = "BrowserView"
        Me.BrowserView.Size = New System.Drawing.Size(1011, 476)
        Me.BrowserView.TabIndex = 0
        '
        'TextBox
        '
        Me.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox.Location = New System.Drawing.Point(3, 485)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.Size = New System.Drawing.Size(811, 22)
        Me.TextBox.TabIndex = 1
        '
        'FindButton
        '
        Me.FindButton.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FindButton.Location = New System.Drawing.Point(820, 485)
        Me.FindButton.Name = "FindButton"
        Me.FindButton.Size = New System.Drawing.Size(94, 34)
        Me.FindButton.TabIndex = 2
        Me.FindButton.Text = "Find"
        Me.FindButton.UseVisualStyleBackColor = true
        '
        'ClearButton
        '
        Me.ClearButton.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClearButton.Location = New System.Drawing.Point(920, 485)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(94, 34)
        Me.ClearButton.TabIndex = 3
        Me.ClearButton.Text = "Clear"
        Me.ClearButton.UseVisualStyleBackColor = true
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 16!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1017, 522)
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.TableLayoutPanel.ResumeLayout(false)
        Me.TableLayoutPanel.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents TableLayoutPanel As TableLayoutPanel
    Friend WithEvents BrowserView As DotNetBrowser.WinForms.BrowserView
    Friend WithEvents TextBox As System.Windows.Forms.TextBox
    Friend WithEvents FindButton As System.Windows.Forms.Button
    Friend WithEvents ClearButton As System.Windows.Forms.Button
End Class
