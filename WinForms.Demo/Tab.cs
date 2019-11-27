using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class Tab : IDisposable
    {
        public TabCaption Caption { get; set; }
        public TabContent Content { get; set; }


        public Tab(TabCaption caption, TabContent content)
        {
            this.Caption = caption;
            this.Content = content;
        }

        public void Dispose()
        {
            this.Caption.Dispose();
            this.Content.Dispose();
        }

    }
}
