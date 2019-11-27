using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Demo.WPF
{
    public class TabCaption : Grid
    {
        public event EventHandler ClosedTab;

        private bool selected;
        private TabCaptionComponent component;

        public TabCaption()
        {
            this.Children.Add(CreateComponent());
        }

        private DockPanel CreateComponent()
        {
            component = new TabCaptionComponent();

            component.ClosedTab += delegate
            {
                if (ClosedTab != null)
                {
                    ClosedTab.Invoke(this, null);
                }

            };

            return component;
        }

        public void SetTitle(String title)
        {
            component.SetTitle(title);
        }

        public void SetLabelWidth(int width)
        {
            component.SetLabelWidth(width);
        }


        public bool IsSelected()
        {
            return selected;
        }

        public void SetSelected(bool selected)
        {
            bool oldValue = this.selected;
            this.selected = selected;
            component.SetSelected(selected);
        }

        private class TabCaptionComponent : DockPanel
        {
            public event EventHandler ClosedTab;
            private Label label;
            private Brush defaultBackground;
            private ImageButton closeButton;

            public TabCaptionComponent()
            {
                defaultBackground = this.Background;
                this.Children.Add(CreateLabel());
                this.Children.Add(CreateCloseButton());
            }

            private Label CreateLabel()
            {
                label = new Label();
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                label.Margin = new Thickness(2);
                label.MaxWidth = 100;
                label.MouseUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (e.MiddleButton == MouseButtonState.Pressed)
                    {
                        if (ClosedTab != null)
                        {
                            ClosedTab.Invoke(this, null);
                        }
                    }

                };
                return label;
            }

            private ImageButton CreateCloseButton()
            {
                closeButton = new ImageButton();
                closeButton.Icon = Demo.WPF.Resources.Close;
                closeButton.PressedIcon = Demo.WPF.Resources.ClosePressed;

                closeButton.ToolTip = Demo.WPF.Resources.CloseTabButtonTooltip;
                closeButton.Width = closeButton.Height = 25;
                closeButton.Focusable = false;
                closeButton.Click += delegate
                {
                    if (ClosedTab != null)
                    {
                        ClosedTab.Invoke(this, null);
                    }
                };
                return closeButton;
            }

            public void SetTitle(String title)
            {
                Application.Current.Dispatcher.BeginInvoke(
                     DispatcherPriority.Input,
                     (ThreadStart)delegate
                     {
                         label.Content = title;
                         label.ToolTip = title;
                     });
            }

            public void SetLabelWidth(int width)
            {
                Application.Current.Dispatcher.BeginInvoke(
                     DispatcherPriority.Input,
                     (ThreadStart)delegate
                     {
                         label.Width = width;
                     });
            }

            public void SetSelected(bool selected)
            {
              //  this.Background = selected ? defaultBackground : new SolidColorBrush(Color.FromRgb(150, 150, 150));
            }
        }
    }
}
