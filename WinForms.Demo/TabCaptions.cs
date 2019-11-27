using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    public class TabCaptions : FlowLayoutPanel
    {
        private TabCaption selectedTab;

        private FlowLayoutPanel tabsPane;
        private FlowLayoutPanel buttonsPane;

        public TabCaptions()
        {
            CreateUI();
        }

        private void CreateUI()
        {
            this.BackColor = Color.FromArgb(64,64,64);
            this.Padding = this.Margin = new Padding(0);
            this.Controls.Add(CreateItemsPane());
            this.Controls.Add(CreateButtonsPane());
        }

        private Control CreateItemsPane()
        {
            tabsPane = new FlowLayoutPanel();
            tabsPane.Dock = DockStyle.Top;
            tabsPane.AutoSize = true;
            tabsPane.Padding = tabsPane.Margin = new Padding(0);
            return tabsPane;
        }

        private Control CreateButtonsPane()
        {
            buttonsPane = new FlowLayoutPanel();
            buttonsPane.AutoSize = true;
            buttonsPane.Padding = buttonsPane.Margin = new Padding(0);
            return buttonsPane;
        }

        public void AddTab(TabCaption item)
        {
            tabsPane.Controls.Add(item);
        }

        public void RemoveTab(TabCaption item)
        {
            tabsPane.Controls.Remove(item);
        }

        public void AddTabButton(ImageButton button)
        {
            button.Padding = button.Margin = new Padding(0);
            buttonsPane.Controls.Add(button);
        }

        public TabCaption GetSelectedTab()
        {
            return selectedTab;
        }

        public void SetSelectedTab(TabCaption selectedTab)
        {
            this.selectedTab = selectedTab;
            this.selectedTab.SetSelected(true);
        }
    }
}
