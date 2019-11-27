using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class TabbedPane : TableLayoutPanel
    {
        private List<Tab> tabs { get; set; }
        private TabCaptions captions;
        private Panel contentContainer;

        public TabbedPane()
        {
            this.captions = new TabCaptions();
            this.captions.Dock = DockStyle.Top;

            this.tabs = new List<Tab>();
            this.contentContainer = new Panel();
            this.contentContainer.Dock = DockStyle.Fill;

            this.ColumnCount = 1;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Location = new System.Drawing.Point(0, 0);
            this.RowCount = 2;
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));


            this.Controls.Add(captions, 0, 0);
            this.Controls.Add(contentContainer, 0, 1);

            captions.Resize += delegate
            {
                UpdateWidthForHeaders();
            };
        }

        public void DisposeAllTabs()
        {
            foreach (Tab tab in tabs)
            {
                tab.Dispose();
            }
            tabs.Clear();
        }

        private void DisposeTab(Tab tab)
        {
            var isSelected = tab.Caption.IsSelected();

            if (isSelected)
            {
                tab.Caption.SetSelected(false);
            }
            RemoveTab(tab);
            tab.Dispose();

            if (HasTabs() && isSelected)
            {
                Tab firstTab = GetFirstTab();
                SelectTab(firstTab);
            }
            else if (!HasTabs())
            {
                Form window = this.FindForm();
                if (window != null)
                {
                    window.Hide();
                    window.Close();
                    window.Dispose();
                }
            }
        }

        private Tab FindTab(TabCaption item)
        {
            foreach (Tab tab in tabs)
            {
                if (tab.Caption.Equals(item))
                {
                    return tab;
                }
            }
            return null;
        }

        public void AddTab(Tab tab)
        {
            TabCaption caption = tab.Caption;
            caption.ClosedTab += TabCaption_ClosedTab;
            caption.SelectedTab += TabCaption_SelectedTab;

            TabContent content = tab.Content;

            captions.AddTab(caption);
            tabs.Add(tab);
            UpdateWidthForHeaders();
        }

        private bool HasTabs()
        {
            return tabs.Count > 0;
        }

        private Tab GetFirstTab()
        {
            return tabs.FirstOrDefault();
        }

        public void RemoveTab(Tab tab)
        {
            TabCaption tabCaption = tab.Caption;
            captions.RemoveTab(tabCaption);
            tabs.Remove(tab);
            UpdateWidthForHeaders();
        }

        public void RemoveSelectedTab()
        {
            TabCaption selectedTab = captions.GetSelectedTab();
            Tab tab = FindTab(selectedTab);

            if (tab != null)
            {
                DisposeTab(tab);
            }

        }

        public void AddTabButton(ImageButton button)
        {
            captions.AddTabButton(button);
        }

        public void SelectTab(Tab tab)
        {
            TabCaption tabCaption = tab.Caption;
            TabCaption selectedTab = captions.GetSelectedTab();
            if (selectedTab != null && !selectedTab.Equals(tabCaption))
            {
                selectedTab.SetSelected(false);
            }
            captions.SetSelectedTab(tabCaption);
            if (!tabCaption.IsSelected())
            {
                TabContent content = tab.Content;
                contentContainer.Controls.Remove(content);
            }
            else
            {
                TabContent content = tab.Content;
                contentContainer.Controls.Clear();
                contentContainer.Controls.Add(content);
                content.Focus();
            }
        }

        private void TabCaption_ClosedTab(object sender, EventArgs e)
        {
            TabCaption caption = (TabCaption)sender;
            Tab tab = FindTab(caption);
            DisposeTab(tab);
        }

        private void TabCaption_SelectedTab(object sender, EventArgs e)
        {
            TabCaption caption = (TabCaption)sender;
            Tab tab = FindTab(caption);
            if (caption.IsSelected())
            {
                SelectTab(tab);
            }
        }

        private Tab FindTab(TabContent content)
        {
            foreach (Tab tab in tabs)
            {
                if (tab.Content.Equals(content))
                {
                    return tab;
                }
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

        private void UpdateWidthForHeaders()
        {
            var count = tabs.Count;
            if (count == 0)
                return;
            var paddings = count * 25 + 37;
            int width = ((int)this.Width - paddings) / count;

            width = width > 0 ? width : 0;

            foreach (Tab tab in tabs)
            {
                if (tab.Caption != null)
                {
                    tab.Caption.SetLabelWidth(width);
                }
            }
        }
    }
}
