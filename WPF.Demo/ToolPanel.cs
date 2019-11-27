using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DotNetBrowser;
using DotNetBrowser.Events;
using drawing = System.Drawing;
using System.Windows.Controls.Primitives;
using DotNetBrowser.WPF;

namespace Demo.WPF
{
    public class ToolPanel : StackPanel
    {
        private const String DEFAULT_URL = "about:blank";

        private ImageButton backwardButton;
        private ImageButton forwardButton;
        private ImageButton refreshButton;
        private ImageButton stopButton;
        private ImageButton gearButton;

        private TextBox addressBar;
        private BrowserView browserView;

        public ToolPanel(BrowserView browserView)
        {
            this.browserView = browserView;

            this.Loaded += delegate { SetAddressBarWidth(); };
            this.SizeChanged += delegate { SetAddressBarWidth(); }; 

            this.Children.Add(CreateActionsPane());
        }

        private void SetAddressBarWidth()
        {
            if (this.ActualWidth == 0)
                return;

            var addressBarWidth = this.ActualWidth - backwardButton.ActualWidth - forwardButton.ActualWidth - refreshButton.ActualWidth - stopButton.ActualWidth - gearButton.ActualWidth - 4;
            addressBarWidth = addressBarWidth > 0 ? addressBarWidth : 0;
            addressBar.Width = addressBarWidth;
        }

        private WrapPanel CreateActionsPane()
        {
            backwardButton = CreateBackwardButton(browserView);
            forwardButton = CreateForwardButton(browserView);
            refreshButton = CreateRefreshButton(browserView);
            stopButton = CreateStopButton(browserView);
            gearButton = CreateGearButton(browserView);
            addressBar = CreateAddressBar();

            backwardButton.ToolTip = Demo.WPF.Resources.BackwardButtonTooltip;
            forwardButton.ToolTip = Demo.WPF.Resources.ForwardButtonTooltip;
            refreshButton.ToolTip = Demo.WPF.Resources.RefreshButtonTooltip;
            stopButton.ToolTip = Demo.WPF.Resources.StopButtonTooltip;
            gearButton.ToolTip = Demo.WPF.Resources.GearButtonTooltip;

            WrapPanel actionsPanel = new WrapPanel();
            actionsPanel.Margin = new Thickness(0);
            actionsPanel.Height = 30;
            actionsPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            actionsPanel.Children.Add(backwardButton);
            actionsPanel.Children.Add(forwardButton);
            actionsPanel.Children.Add(refreshButton);
            actionsPanel.Children.Add(stopButton);
            actionsPanel.Children.Add(addressBar);
            actionsPanel.Children.Add(gearButton);
            return actionsPanel;
        }

        private TextBox CreateAddressBar()
        {
            TextBox result = new TextBox();
            result.Margin = new Thickness(2);

            result.Text = DEFAULT_URL;
            result.KeyUp += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key != Key.Enter) { return; }

                browserView.Browser.LoadURL(result.Text);
            };

            browserView.Browser.StartLoadingFrameEvent += delegate(object sender, StartLoadingArgs e)
            {
                if (e.IsMainFrame)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                         DispatcherPriority.Input,
                         (ThreadStart)delegate
                         {
                             refreshButton.IsEnabled = false;
                             stopButton.IsEnabled = true;
                         });
                }
            };

            browserView.Browser.ProvisionalLoadingFrameEvent += delegate(object sender, ProvisionalLoadingArgs e)
            {
                if (e.IsMainFrame)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                         DispatcherPriority.Input,
                         (ThreadStart)delegate
                         {
                             result.Text = e.Url;
                             result.CaretIndex = result.Text.Length;
                         });
                }
            };

            browserView.Browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    bool canGoForward = e.Browser.CanGoForward();
                    bool canGoBack = e.Browser.CanGoBack();
                    Application.Current.Dispatcher.BeginInvoke(
                         DispatcherPriority.Input,
                         (ThreadStart)delegate
                         {
                             refreshButton.IsEnabled = true;
                             stopButton.IsEnabled = false;

                             forwardButton.IsEnabled = canGoForward;
                             backwardButton.IsEnabled = canGoBack;
                         });
                }
            };

            browserView.Browser.FailLoadingFrameEvent += delegate (object sender, FailLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                         DispatcherPriority.Input,
                         (ThreadStart)delegate
                         {
                             refreshButton.IsEnabled = true;
                             stopButton.IsEnabled = false;
                         });
                }

            };

            return result;
        }

        private static ImageButton CreateBackwardButton(BrowserView browserView)
        {

            var button = CreateButton(Demo.WPF.Resources.Back, Demo.WPF.Resources.BackSelected);
            button.Click += delegate
            {
                browserView.Browser.GoBack();
            };
            return button;
        }

        private static ImageButton CreateForwardButton(BrowserView browserView)
        {
            var button = CreateButton(Demo.WPF.Resources.Forward, Demo.WPF.Resources.ForwardSelected);
            button.Click += delegate
            {
                browserView.Browser.GoForward();
            };

            return button;
        }

        private static ImageButton CreateRefreshButton(BrowserView browserView)
        {
            var button = CreateButton(Demo.WPF.Resources.Refresh, Demo.WPF.Resources.RefreshSelected);
            button.Click += delegate
            {
                browserView.Browser.Reload(true);
            };

            return button;
        }

        private static ImageButton CreateStopButton(BrowserView browserView)
        {
            var button = CreateButton(Demo.WPF.Resources.Stop, Demo.WPF.Resources.StopSelected);
            button.Click += delegate
            {
                browserView.Browser.Stop();
            };

            return button;
        }

        private bool IsFocusRequired()
        {
            String url = addressBar.Text;
            return String.IsNullOrEmpty(url) || url.Equals(DEFAULT_URL);
        }

        private static ImageButton CreateButton(drawing.Bitmap icon, drawing.Bitmap rolloverIcon)
        {
            ImageButton button = new ImageButton();
            button.Icon = icon;
            button.RolloverIcon = rolloverIcon;

            return button;
        }

        private static ImageButton CreateGearButton(BrowserView browserView)
        {
            var button = CreateButton(Demo.WPF.Resources.Gear, Demo.WPF.Resources.Gear);

            PreferenceMenu preferenceMenuHandler = new PreferenceMenu(browserView);
            preferenceMenuHandler.AddPreferenceMenu(button);
                     
            return button;
        }     
    }
}
