using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.TransparentPageSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WebBrowser1.Browser.LoadHTML(
                                 "<html>\n"
                 +"     <body>"
                 +"         <div style='background: yellow; opacity: 0.7;'>\n"
                 +"             This text is in the yellow half-transparent div."
                 +"        </div>\n"
                 +"         <div style='background: red;'>\n"
                 +"             This text is in the red opaque div and should appear as is."
                 +"        </div>\n"
                 +"         <div>\n"
                 +"             This text is in the non-styled div and should appear as a text on the completely transparent background."
                 +"        </div>\n"
                 +"    </body>\n"
                 +" </html>");

        }
    }
}
