﻿#region Copyright

// Copyright 2024, TeamDev. All rights reserved.
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DotNetBrowser.Extensions;

namespace DotNetBrowser.WinForms.Demo.Components
{
    public partial class ExtensionsPanel : FlowLayoutPanel
    {
        #region Constructors

        public ExtensionsPanel()
        {
            InitializeComponent();
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        #endregion

        #region Methods

        public void UpdateActions(IEnumerable<IExtensionAction> actions)
        {
            Controls.Clear();
            if (actions == null)
            {
                return;
            }

            foreach (IExtensionAction action in actions)
            {
                Button item = CreateItem(action);
                Controls.Add(item);
            }
        }

        private Button CreateItem(IExtensionAction action)
        {
            Button actionButton = new Button
            {
                Width = 25,
                Height = 25,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            ToolTip toolTip = new ToolTip();

            action.Updated += (s, e) =>
            {
                BeginInvoke((Action)(() =>
                                        {
                                            actionButton.Image = action.Icon.ToBitmap();
                                            toolTip.SetToolTip(actionButton, action.Tooltip);
                                        }));
            };
            toolTip.SetToolTip(actionButton, action.Tooltip);
            actionButton.Image = action.Icon.ToBitmap();

            actionButton.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Remove extension");
            menuItem.Click += (sender, args) => action.Extension.Uninstall();
            actionButton.ContextMenuStrip.Items.Add(menuItem);

            actionButton.Click += (s, e) => { action.Click(); };

            return actionButton;
        }

        #endregion
    }
}