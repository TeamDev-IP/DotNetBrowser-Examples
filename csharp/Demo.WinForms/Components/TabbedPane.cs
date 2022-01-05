#region Copyright

// Copyright 2022, TeamDev. All rights reserved.
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetBrowser.Engine;

namespace DotNetBrowser.WinForms.Demo.Components
{
    public partial class TabbedPane : UserControl
    {
        private readonly List<Tab> tabs = new List<Tab>();
        private Tab selectedTab;
        

        public RenderingMode RenderingMode { get; set; }
        internal IEngine Engine { get; set; }

        internal Tab SelectedTab
        {
            get => selectedTab;
            set
            {
                if (selectedTab != value)
                {
                    DeselectTab(selectedTab);
                    SelectTab(value);
                }

                selectedTab = value;
            }
        }

        public TabbedPane()
        {
            InitializeComponent();
            AddTab(new Tab());
        }

        internal void AddTab(Tab tab)
        {
            tab.Selected += OnTabSelected;
            tab.Closed += OnTabClosed;

            captions.Controls.Add(tab);
            tabs.Add(tab);
            UpdateWidthForHeaders();
            SelectedTab = tab;
        }

        internal void RemoveTab(Tab tab)
        {
            tab.Selected -= OnTabSelected;
            tab.Closed -= OnTabClosed;

            captions.Controls.Remove(tab);
            tabs.Remove(tab);

            SelectedTab = tabs.LastOrDefault();

            UpdateWidthForHeaders();
        }

        private void addTabButton_Click(object sender, EventArgs e)
        {
            Tab tab = new Tab();
            tab.Contents.renderingMode.Text = RenderingMode.ToString();
            AddTab(tab);
            Task.Run(() => Engine?.CreateBrowser())
                .ContinueWith(t => { tab.Contents.Browser = t.Result; },
                              TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void captions_Resize(object sender, EventArgs e)
        {
            UpdateWidthForHeaders();
        }

        private void DeselectTab(Tab tab)
        {
            if (tab != null)
            {
                tab.IsSelected = false;
            }
        }

        private void OnTabClosed(object sender, EventArgs e)
        {
            Tab tab = sender as Tab;
            RemoveTab(tab);
            tab?.Contents.CloseTab();

            if (tabs.Count == 0)
            {
                Form window = FindForm();
                if (window != null)
                {
                    window.Hide();
                    window.Close();
                    window.Dispose();
                }
            }
        }

        private void OnTabSelected(object sender, EventArgs e)
        {
            SelectedTab = (Tab) sender;
        }

        private void SelectTab(Tab tab)
        {
            if (tab != null)
            {
                tab.IsSelected = true;
                contentContainer.Controls.Clear();
                contentContainer.Controls.Add(tab.Contents);
                //tab.BrowserTab.RestoreFocusedState();
            }
        }

        private void UpdateWidthForHeaders()
        {
            int count = tabs.Count;
            if (count == 0)
            {
                return;
            }

            int paddings = count * 25 + 37;
            int width = (Width - paddings) / count;

            width = width > 0 ? width : 0;

            foreach (Tab tab in tabs)
            {
                tab?.SetLabelWidth(width);
            }
        }
    }
}
