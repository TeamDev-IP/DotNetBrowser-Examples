using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Demo.WPF
{
    class BuildMenuItem
    {        
        public static MenuItem Build(string item, bool isEnabled, bool isChecked, RoutedEventHandler clickHandler)
        {
            MenuItem result = new MenuItem();
            result.Header = item;
            result.IsEnabled = isEnabled;
            result.IsChecked = isChecked;
            result.Click += clickHandler;
            return result;
        }
    }
}
