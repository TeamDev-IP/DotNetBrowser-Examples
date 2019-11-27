using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Demo.WPF
{
    class JSConsole
    {
        private BrowserView browserView;
        private  Grid gridWithConsole;
        private  TextBox consoleIn;
        private  TextBox consoleOut;
        private  Label title;
        private  Image closeButton;
        private  BitmapImage closeImage;
        private  BitmapImage closePressedImage;
        private  RowDefinition rowDefinitionSplit;
        private  RowDefinition rowDefinitionLabel;
        private  RowDefinition rowDefinitionConsoleOut;
        private  RowDefinition rowDefinitionConsoleIn;

        public bool consoleStatus = true;

        public JSConsole(BrowserView browserView)
        {
            this.browserView = browserView;
        }

        public bool runJSConsole(bool JSConsoleEnable)
        {
            DependencyObject tmp = ((FrameworkElement)browserView).Parent;
            DockPanel dockPanel = ((DockPanel)tmp);
            if (JSConsoleEnable)
            {
                gridWithConsole = new Grid();

                consoleIn = new TextBox();
                consoleOut = new TextBox();
                title = new Label();
                closeButton = new Image();
                closeImage = new BitmapImage(new Uri(@"Resources/close.png", UriKind.RelativeOrAbsolute));
                closePressedImage = new BitmapImage(new Uri(@"Resources/close-pressed.png", UriKind.RelativeOrAbsolute));

                rowDefinitionSplit = new RowDefinition();
                rowDefinitionLabel = new RowDefinition { Height = new GridLength(25) };
                rowDefinitionConsoleOut = new RowDefinition { Height = new GridLength(150) };
                rowDefinitionConsoleIn = new RowDefinition { Height = new GridLength(25) };

                gridWithConsole.RowDefinitions.Add(rowDefinitionSplit);
                gridWithConsole.RowDefinitions.Add(rowDefinitionLabel);
                gridWithConsole.RowDefinitions.Add(rowDefinitionConsoleOut);
                gridWithConsole.RowDefinitions.Add(rowDefinitionConsoleIn);

                gridWithConsole.Set(title, 0, 1);
                gridWithConsole.Set(closeButton, 0, 1);
                gridWithConsole.Set(consoleOut, 0, 2);
                gridWithConsole.Set(consoleIn, 0, 3);

                closeButton.Height = 14;
                closeButton.Width = 14;
                closeButton.Source = closeImage;
                closeButton.HorizontalAlignment = HorizontalAlignment.Right;
                closeButton.Margin = new Thickness(0, 0, 3, 0);

                title.Content = "JavaScript Console";
                title.HorizontalAlignment = HorizontalAlignment.Left;

                consoleOut.TextWrapping = TextWrapping.Wrap;
                consoleOut.IsReadOnly = true;
                consoleOut.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                closeButton.MouseDown += (s, e) =>
                {
                    if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                        closeButton.Source = closePressedImage;
                };

                closeButton.MouseUp += (s, e) =>
                {
                    if (e.LeftButton == System.Windows.Input.MouseButtonState.Released)
                    {
                        consoleStatus = true;

                        gridWithConsole.Children.Remove(title);
                        gridWithConsole.Children.Remove(closeButton);
                        gridWithConsole.Children.Remove(consoleOut);
                        gridWithConsole.Children.Remove(consoleIn);

                        gridWithConsole.RowDefinitions.Remove(rowDefinitionSplit);
                        gridWithConsole.RowDefinitions.Remove(rowDefinitionLabel);
                        gridWithConsole.RowDefinitions.Remove(rowDefinitionConsoleOut);
                        gridWithConsole.RowDefinitions.Remove(rowDefinitionConsoleIn);

                        dockPanel.Children.Remove(gridWithConsole);
                    }
                };

                consoleOut.TextChanged += delegate
                {
                    consoleOut.ScrollToEnd();
                };
                consoleIn.KeyDown += (s, e) =>
                {
                    if (e.Key == System.Windows.Input.Key.Enter && consoleIn.Text != String.Empty)
                    {
                        ExecuteJS(consoleIn.Text);
                    }
                };
                consoleIn.VerticalContentAlignment = VerticalAlignment.Center;

                consoleStatus = false;

                gridWithConsole.Width = Window.GetWindow((FrameworkElement)browserView).Width - 22;

                Window.GetWindow((FrameworkElement)browserView).SizeChanged += delegate
                {
                    gridWithConsole.Width = Window.GetWindow((FrameworkElement)browserView).Width - 22;
                };

                gridWithConsole.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                DockPanel.SetDock(gridWithConsole, Dock.Bottom);
                dockPanel.Children.Insert(((DockPanel)tmp).Children.Count - 1, gridWithConsole);
            }
            else if (!JSConsoleEnable)
            {
                gridWithConsole.Children.Remove(title);
                gridWithConsole.Children.Remove(closeButton);
                gridWithConsole.Children.Remove(consoleOut);
                gridWithConsole.Children.Remove(consoleIn);

                gridWithConsole.RowDefinitions.Remove(rowDefinitionSplit);
                gridWithConsole.RowDefinitions.Remove(rowDefinitionLabel);
                gridWithConsole.RowDefinitions.Remove(rowDefinitionConsoleOut);
                gridWithConsole.RowDefinitions.Remove(rowDefinitionConsoleIn);
                consoleStatus = true;
                dockPanel.Children.Remove(gridWithConsole);
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
