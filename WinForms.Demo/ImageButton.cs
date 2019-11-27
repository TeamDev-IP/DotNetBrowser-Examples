using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class ImageButton : Label
    {
        private ToolTip toolTip;
        private string toolTipText;
        private bool toolTipShown = false;
        private Form currentForm;

        public new Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;
                this.Size = new Size(this.Image.Size.Width + 8, this.Image.Size.Height + 8);
            }
        }

        public Bitmap Icon
        {
            set
            {
                Image sourceIcon = value;
                this.Image = sourceIcon;
                this.MouseLeave += delegate
                {
                    this.Image = sourceIcon;
                };
            }
        }

        public Bitmap RolloverIcon
        {
            set
            {
                Image sourceRolloverIcon = value;
                this.MouseEnter += delegate
                {
                    this.Image = sourceRolloverIcon;
                };
            }
        }

        public Bitmap PressedIcon
        {
            set
            {
                Image notPressedIcon = this.Image;
                Image sourcePressedIcon = value;
                this.MouseDown += delegate
                {
                    this.Image = sourcePressedIcon;
                };
                this.MouseUp += delegate
                {
                    this.Image = notPressedIcon;
                };
            }
        }

        public string ToolTip
        {
            get
            {
                return toolTipText;
            }
            set
            {
                toolTipText = value;
                toolTip.SetToolTip(this, toolTipText);
            }
        }

        public ImageButton()
            : base()
        {
            this.Margin = this.Padding = new System.Windows.Forms.Padding(0);
            toolTip = new ToolTip
            {
                AutoPopDelay = 15000,
                ShowAlways = true,
                InitialDelay = 200,
                ReshowDelay = 200,
                UseAnimation = true
            };

            this.EnabledChanged += delegate
            {
                currentForm = this.FindForm();
                if (!this.Enabled)
                {
                    AddMouseMoveEvent(currentForm);
                    if (this.Parent != null)
                    {
                        this.Parent.MouseLeave += OnMouseLeave;
                    }
                }
                else
                {
                    RemoveMouseMoveEvent(currentForm);
                    if (this.Parent != null)
                    {
                        this.Parent.MouseLeave -= OnMouseLeave;
                    }
                    HideToolTip();
                }

            };
        }

        protected override void Dispose(bool disposing)
        {
            if (currentForm != null)
            {
                RemoveMouseMoveEvent(currentForm);
            }
            if (this.Parent != null)
            {
                this.Parent.MouseLeave -= OnMouseLeave;
            }
            if (toolTip != null)
            {
                toolTip.Dispose();
            }
            base.Dispose(disposing);
        }

        private void RemoveMouseMoveEvent(Control control)
        {
            if (control != null)
            {
                control.MouseMove -= Control_MouseMove;

                foreach (Control childControl in control.Controls)
                {
                    RemoveMouseMoveEvent(childControl);
                }
            }
        }

        private void AddMouseMoveEvent(Control control)
        {
            if (control != null)
            {
                control.MouseMove += Control_MouseMove;

                foreach (Control childControl in control.Controls)
                {
                    AddMouseMoveEvent(childControl);
                }
            }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            HideToolTip();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var parent = sender as Control;
            if (parent == null)
            {
                return;
            }
            var ctrl = parent.GetChildAtPoint(e.Location);
            if (ctrl != null && ctrl.Equals(this))
            {
                if (!this.Enabled && !toolTipShown)
                {
                    toolTip.Show(toolTipText, currentForm
                                            , Control.MousePosition.X - currentForm.Left
                                            , Control.MousePosition.Y - currentForm.Top + (Cursor.Size.Height / 2));
                    toolTipShown = true;
                }
            }
            else if (toolTipShown)
            {
                HideToolTip();
            }
        }

        private void HideToolTip()
        {
            if (currentForm != null)
            {
                toolTip.Hide(currentForm);
            }
            toolTipShown = false;
        }
    }
}
