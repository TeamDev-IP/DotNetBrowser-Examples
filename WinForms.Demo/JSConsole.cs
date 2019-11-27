using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Demo.Properties;

namespace WinForms.Demo
{
    class  JSConsole
    {
        private BrowserView browserView;
        private Panel panel;
        private TextBox consoleIn;
        private TextBox consoleOut;

        public bool consoleStatus = true;

        public JSConsole(BrowserView browserView)
        {
            this.browserView = browserView;
        }

        public bool runJSConsole(bool JSConsoleEnable)
        {
            if (JSConsoleEnable)
            {                
                panel = new Panel();
                consoleIn = new TextBox();
                consoleOut = new TextBox();
                Label title = new Label();
                Label closeButton = new Label();
                Image close = Resources.Close;
                Image closePressed = Resources.ClosePressed;
                string consoleResult = String.Empty;

                panel.BorderStyle = BorderStyle.Fixed3D;
                panel.Controls.Add(title);
                panel.Controls.Add(closeButton);
                panel.Controls.Add(consoleOut);
                panel.Controls.Add(consoleIn);
                panel.Dock = DockStyle.Bottom;
                panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                panel.Size = new Size(200, 200);
                panel.Resize += delegate
                {
                    consoleOut.Height = panel.Height - title.Height - consoleIn.Height - 5;
                };

                consoleOut.Multiline = true;
                consoleOut.BorderStyle = BorderStyle.None;
                consoleOut.Location = new Point(0, title.Height);
                consoleOut.ReadOnly = true;
                consoleOut.SelectionStart = 0;
                consoleOut.Width = consoleIn.Width;
                consoleOut.ScrollBars = ScrollBars.Vertical;
                consoleOut.Height = panel.Height - title.Height - consoleIn.Height - 5;
                consoleOut.BackColor = Color.White;
                consoleOut.TextChanged += delegate
                {
                    consoleOut.SelectionStart = consoleOut.TextLength;
                    consoleOut.ScrollToCaret();
                };

                title.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                title.TextAlign = ContentAlignment.MiddleLeft;
                title.Text = "JavaScript Console";

                closeButton.Image = close;
                closeButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                closeButton.Size = close.Size;
                closeButton.ImageAlign = ContentAlignment.MiddleCenter;
                closeButton.Location = new Point(panel.Size.Width - closeButton.Size.Width - 5, 3);
                closeButton.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                        closeButton.Image = closePressed;
                };
                closeButton.MouseUp += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {                        
                        consoleStatus = true;
                        ((Control)browserView).Parent.Controls.Remove(panel);                        
                    }
                };

                consoleIn.Dock = DockStyle.Bottom;
                consoleIn.BorderStyle = BorderStyle.Fixed3D;
                consoleIn.Resize += delegate
                {
                    consoleOut.Width = consoleIn.Width;
                };
                consoleIn.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter && consoleIn.Text != String.Empty)
                    {
                        ExecuteJS(consoleIn.Text); 
                    }
                };
                ((Control)browserView).Parent.Controls.Add(panel);
                consoleStatus = false;
            }

            else if (!JSConsoleEnable)
            {
                ((Control)browserView).Parent.Controls.Remove(panel);
                consoleStatus = true;
            }
            return consoleStatus;
        }

        private void ExecuteJS(string jsCode)
        {
            string tmp = String.Empty;
            var t = new Task(() =>
            {
                tmp = browserView.Browser.ExecuteJavaScriptAndReturnValue(jsCode).ToString();
            });

            t.ContinueWith((s) =>
            {
                
                    consoleOut.Text += ">> " + consoleIn.Text + Environment.NewLine;
                    consoleOut.Text += tmp + Environment.NewLine;
                    consoleIn.Clear();

            }, TaskScheduler.FromCurrentSynchronizationContext());
            
            t.Start();
        }
    }
}
