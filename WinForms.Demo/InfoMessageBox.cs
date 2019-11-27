using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class InfoMessageBox
    {
        public static void Show(Control control, string text, string caption)
        {
            Form resultBox = new Form();

            resultBox.ShowIcon = false;
            resultBox.Size = new Size(230, 140);
            resultBox.Text = caption;
            resultBox.MaximizeBox = false;
            resultBox.MinimizeBox = false;
            resultBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            resultBox.StartPosition = FormStartPosition.CenterParent;

            Label resultMessage = new Label();
            resultMessage.Location = new Point(0, 10);
            resultMessage.Size = new Size(resultBox.Width, 25);
            resultMessage.Font = new Font(FontFamily.GenericSansSerif, 8);
            resultMessage.TextAlign = ContentAlignment.MiddleCenter;
            resultMessage.Text = text;

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.Margin = new Padding(30, 0, 30, 0);
            buttonOk.Location = new Point(75, 60);
            buttonOk.Click += delegate
            {
                resultBox.Close();
            };

            resultBox.Controls.Add(resultMessage);
            resultBox.Controls.Add(buttonOk);

            resultBox.ShowDialog(control.Parent);
        }
    }
}
