using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WPF;

namespace Demo.WPF
{
    public class TabContent : Grid
    {
        public delegate void PropertyChangeHandler(string propertyName,
                                          object oldValue, object newValue);
        public event PropertyChangeHandler PropertyChangeEvent;

        private Browser browser;
        private BrowserView browserView;
        
        private ToolPanel toolBar;
        private DockPanel container;
        private DockPanel browserContainer;

        public TabContent(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;

            this.browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    if (PropertyChangeEvent != null)
                    {
                        PropertyChangeEvent.Invoke("PageTitleChanged", null, browser.Title);
                    }
                }
            };

            browserContainer = CreateBrowserContainer();
            toolBar = CreateToolBar(browserView);

            container = new DockPanel();
            container.Children.Add(browserContainer);
            container.Margin = new Thickness(0, 30, 0, 0);
            this.Children.Add(toolBar);
            this.Children.Add(container);
        }

        private ToolPanel CreateToolBar(BrowserView browserView)
        {
            ToolPanel toolBar = new ToolPanel(browserView);
            return toolBar;
        }

        private void HideConsole()
        {
            ShowComponent(browserContainer);
        }

        private void ShowComponent(UIElement component)
        {
            container.Children.Clear();
            container.Children.Add(component);
        }

        private DockPanel CreateBrowserContainer()
        {
            DockPanel container = new DockPanel();
            container.Children.Add((UIElement)this.browserView.GetComponent());
            return container;
        }

        public void Dispose()
        {
            this.browserView.Dispose();
            this.browser.Dispose();
        }
    }
}
