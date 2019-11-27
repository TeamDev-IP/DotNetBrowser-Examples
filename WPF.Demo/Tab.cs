using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Demo.WPF
{
    public class Tab : TabItem
    {
        public Tab(TabCaption caption, TabContent content)
        {
            this.Header = caption;
            this.Content = content;

            if (content == null || !(content is TabContent))
                return;

            content.PropertyChangeEvent += delegate(string propertyName,
                                          object oldValue, object newValue)
            {
                if (propertyName == "PageTitleChanged")
                {
                    Application.Current.Dispatcher.BeginInvoke(
                         DispatcherPriority.Input,
                         (ThreadStart)delegate
                         {
                             caption.SetTitle(newValue.ToString());
                         });
                }
            };
        }
    }
}
