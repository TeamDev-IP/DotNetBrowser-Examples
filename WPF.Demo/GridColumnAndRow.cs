using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Demo.WPF
{
    static class GridColumnAndRow
    {
        public static void Set(this Grid grid, UIElement element, int Column, int Row)
        {
            grid.Children.Add(element);
            Grid.SetColumn(element, Column);
            Grid.SetRow(element, Row);
        }
    }
}
