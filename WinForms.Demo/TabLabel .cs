using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class TabLabel : Label
    {
        public override string Text
        {
            get
            {
                string text = base.Text;
                int lenght = base.Text.Length;
                while (TextRenderer.MeasureText(text, this.Font).Width > this.Width && lenght > 0)
                {
                    text = text.Substring(0, lenght - 1) + "...";
                    lenght = lenght - 1;
                }
                if (lenght == 0)
                {
                    text = String.Empty;
                }
                return text;
            }
            set
            {
                base.Text = value;
            }
        }

        public TabLabel() : base()
        {
            this.AutoSize = false;
        }
     }
}
