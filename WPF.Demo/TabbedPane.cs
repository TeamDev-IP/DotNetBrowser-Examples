using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Demo.WPF
{
    public class TabbedPane : TabControl
    {
        private ObservableCollection<Tab> tabs;
        private Tab newTabButtonItem;

        public TabbedPane()
        {
            this.tabs = new ObservableCollection<Tab>();
            this.ItemsSource = tabs;

            this.SelectionChanged += delegate(object sender, SelectionChangedEventArgs e)
            {
                Application.Current.Dispatcher.BeginInvoke(
                     DispatcherPriority.Input,
                     (ThreadStart)delegate
                     {
                         foreach (Tab tab in GetTabs())
                         {
                             if (tab.Header != null && tab.Header is TabCaption)
                             {
                                 ((TabCaption)tab.Header).SetSelected(tab.IsSelected);
                             }
                             else if (tab.IsSelected && tab.Header != null && tab.Header is ImageButton)
                             {
                                 ((ImageButton)tab.Header).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                             }
                         }
                     });
            };

            this.SizeChanged += delegate
            {
                UpdateWidthForHeaders();
            };
        }

        public void DisposeAllTabs()
        {
            foreach (Tab tab in GetTabs())
            {
                DisposeTab(tab);
            }
        }

        private void DisposeTab(Tab tab)
        {
            if (tab.IsSelected)
            {
                Tab firstTab = GetFirstTab();
                firstTab.IsSelected = true;

            }
            RemoveTab(tab);
            if (tab.Content != null && tab.Content is TabContent)
            {
                ((TabContent)tab.Content).Dispose();
            }

            if (!HasTabs())
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    Window window = Window.GetWindow(this);

                    if (window != null)
                    {
                        window.Close();
                    }
                }));
            }
        }

        public void AddNewTabButton(ImageButton newTab)
        {
            newTabButtonItem = new Tab(null, null);
            newTabButtonItem.Margin = new Thickness(0);
            newTabButtonItem.Header = newTab;
            newTabButtonItem.Focusable = false;
            newTabButtonItem.ToolTip = Demo.WPF.Resources.NewTabButtonTooltip;
            tabs.Add(newTabButtonItem);
        }


        public void AddTab(Tab tab)
        {
            ((TabCaption)tab.Header).ClosedTab += delegate
            {
                DisposeTab(tab);
            };

            ((TabCaption)tab.Header).MouseDown += delegate(object sender, MouseButtonEventArgs e)
            {
                if (e.ChangedButton == MouseButton.Middle)
                {
                    DisposeTab(tab);
                }
            };

            if (tabs.Count > 0)
            {
                tabs.Insert(tabs.Count - 1, tab);
                tab.IsSelected = true;
                UpdateWidthForHeaders();
            }
            else
            {
                tabs.Add(tab);
            }
        }

        public void RemoveTab(Tab tab)
        {
            tabs.Remove(tab);
            UpdateWidthForHeaders();
        }

        public void RemoveSelectedTab()
        {
            Tab tab = tabs.FirstOrDefault(item => item.IsSelected);

            if (tab != null)
            {
                DisposeTab(tab);
            }

        }
        private bool HasTabs()
        {
            return tabs.Count(tab => tab.Header != null && tab.Header is TabCaption) > 0;
        }

        private Tab GetFirstTab()
        {
            return tabs[0];
        }

        private List<Tab> GetTabs()
        {
            return new List<Tab>(tabs);
        }

        private void UpdateWidthForHeaders()
        {
            Application.Current.Dispatcher.BeginInvoke(
             DispatcherPriority.Input,
             (ThreadStart)delegate
             {
                 var count = tabs.Count(tab => tab.Header != null && tab.Header is TabCaption);
                 var paddings = count * 42 + 37;
                 int width = ((int)this.ActualWidth - paddings) / count;

                 width = width > 0 ? width : 0;

                 foreach (Tab tab in GetTabs())
                 {
                     if (tab.Header != null && tab.Header is TabCaption)
                     {
                         ((TabCaption)tab.Header).SetLabelWidth(width);
                     }
                 }
             });
        }
    }
}
