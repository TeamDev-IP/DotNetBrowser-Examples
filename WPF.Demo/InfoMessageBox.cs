using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Demo.WPF
{
    public class InfoMessageBox
    {
        public static void Show(FrameworkElement control, string text, string caption)
        {
            Grid grid = new Grid();
            Window resultBox = new Window();
            resultBox.Width = 235;
            resultBox.Height = 120;
            resultBox.Title = caption;
            resultBox.Content = grid;
            resultBox.ResizeMode = ResizeMode.NoResize;
            resultBox.WindowStyle = WindowStyle.SingleBorderWindow;
            resultBox.Owner = Window.GetWindow(control);
            resultBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            RowDefinition rowDefinition = new RowDefinition { Height = new GridLength(40) };
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(rowDefinition);

            Label resultMessage = new Label();
            resultMessage.Content = text;
            resultMessage.Width = resultBox.Width;
            resultMessage.Height = 40;
            resultMessage.HorizontalContentAlignment = HorizontalAlignment.Center;

            Button buttonOk = new Button();
            buttonOk.Content = "OK";
            buttonOk.Width = 60;
            buttonOk.Height = 20;
            buttonOk.HorizontalAlignment = HorizontalAlignment.Center;

            grid.Set(resultMessage, 0, 0);
            grid.Set(buttonOk, 0, 1);

            resultBox.Topmost = true;
            buttonOk.Click += delegate
            {
                resultBox.Close();
            };

            resultBox.ShowDialog();
        }
    }
}
