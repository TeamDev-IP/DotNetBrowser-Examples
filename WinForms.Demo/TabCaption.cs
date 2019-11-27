using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Demo.Properties;

namespace WinForms.Demo
{
    public class TabCaption : FlowLayoutPanel
    {
        public event EventHandler ClosedTab;
        public event EventHandler SelectedTab;

        private bool selected;
        private TabCaptionComponent component;

        public TabCaption()
        {
            this.Controls.Add(CreateComponent());
            this.Dock = DockStyle.Top;
            this.AutoSize = true;
            this.Padding = new Padding(0);
            this.Margin = new Padding(0, 0, 1, 0);
        }

        private FlowLayoutPanel CreateComponent()
        {
            component = new TabCaptionComponent();

            component.ClosedTab += delegate
            {
                if (ClosedTab != null)
                {
                    ClosedTab.Invoke(this, null);
                }

            };

            component.SelectedTab += delegate
            {
                SetSelected(true);
                if (SelectedTab != null)
                {
                    SelectedTab.Invoke(this, null);
                }
            };

            return component;
        }

        public void SetTitle(String title)
        {
            WinFormsUIContext.Instance.Send(new SendOrPostCallback(
                delegate(object state)
                {
                    component.SetTitle(title);
                }), title);
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

        private class TabCaptionComponent : FlowLayoutPanel
        {
            public event EventHandler ClosedTab;
            public event EventHandler SelectedTab;
            private TabLabel label;
            private ImageButton closeButton;

            private Color defaultBackground;

            public TabCaptionComponent()
            {
                defaultBackground = this.BackColor;
                this.AutoSize = true;
                this.Padding = this.Margin = new Padding(0);
                this.Controls.Add(CreateLabel());
                this.Controls.Add(CreateCloseButton());
            }

            private Label CreateLabel()
            {
                label = new TabLabel();
                label.Text = String.Empty;
                label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                label.Margin = new System.Windows.Forms.Padding(2);
                label.MaximumSize = new System.Drawing.Size(125, 26);
                label.MouseUp += delegate(object sender, MouseEventArgs e)
                {
                    if (e.Button == MouseButtons.Middle)
                    {
                        if (ClosedTab != null)
                        {
                            ClosedTab.Invoke(this, null);
                        }
                    }
                    if(e.Button == MouseButtons.Left)
                    {
                        if(SelectedTab != null)
                        {
                            SelectedTab.Invoke(this, null);
                        }
                    }

                };
                return label;
            }

            private ImageButton CreateCloseButton()
            {
                closeButton = new ImageButton();
                closeButton.Icon = Resources.Close;
                closeButton.PressedIcon = Resources.ClosePressed;
                closeButton.Dock = DockStyle.Right;
                closeButton.Margin = new System.Windows.Forms.Padding(3);

                closeButton.Text = String.Empty;
                closeButton.ToolTip = WinForms.Demo.Properties.Resources.CloseTabButtonTooltip;
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
                label.Text = title;
            }

            public void SetLabelWidth(int width)
            {
                if(label.MaximumSize.Width < width)
                {
                    label.Width = label.MaximumSize.Width;
                    return;
                }
                label.Width = width;
            }

            public void SetSelected(bool selected)
            {
                  this.BackColor = selected ? defaultBackground : Color.FromArgb(150, 150, 150);
            }
        }
    }
}
