#region Copyright

// Copyright 2019, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotNetBrowser.WinForms.Demo.Components
{
    public partial class Tab : UserControl
    {
        private readonly Color defaultBackground;
        private bool selected;

        #region Properties

        public TabContents Contents { get; }

        public bool IsSelected
        {
            get => selected;
            set
            {
                selected = value;

                BackColor = !selected ? Color.FromArgb(150, 150, 150) : defaultBackground;
            }
        }

        #endregion

        #region Events

        public event EventHandler Selected;
        public event EventHandler Closed;

        #endregion

        #region Constructors

        public Tab()
        {
            InitializeComponent();
            defaultBackground = BackColor;

            Contents = new TabContents {Dock = DockStyle.Fill};
            Contents.TitleChanged += BrowserTab_TitleChanged;
        }

        #endregion

        #region Methods

        public void SetLabelWidth(int width)
        {
            if (MaximumSize.Width < width)
            {
                width = MaximumSize.Width;
            }
            Width = width;
        }

        private void BrowserTab_TitleChanged(object sender, string e)
        {
            titleLabel.Text = e;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Selected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void titleLabel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
            if (e.Button == MouseButtons.Left)
            {
                Selected?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}