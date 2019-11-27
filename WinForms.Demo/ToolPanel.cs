using DotNetBrowser;
using DotNetBrowser.Events;
using DotNetBrowser.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Demo.Properties;

namespace WinForms.Demo
{
    public class ToolPanel : Panel
    {
        private const String DEFAULT_URL = "about:blank";

        private ImageButton backwardButton;
        private ImageButton forwardButton;
        private ImageButton refreshButton;
        private ImageButton stopButton;
        private ImageButton gearButton;
        private BrowserView browserView;
        private TextBox addressBar;


        public ToolPanel(BrowserView browserView)
        {
            this.browserView = browserView;
            this.Dock = DockStyle.Top;
            this.AutoSize = true;
            this.SizeChanged += delegate { SetAddressBarWidth(); };
            this.Controls.Add(CreateActionsPane());
        }

        private void SetAddressBarWidth()
        {
            if (this.Width == 0)
                return;

            var addressBarWidth = this.Width - backwardButton.Width - forwardButton.Width - refreshButton.Width - stopButton.Width - gearButton.Width - 4;
            addressBarWidth = addressBarWidth > 0 ? addressBarWidth : 0;
            addressBar.Width = addressBarWidth;
        }

        private Panel CreateActionsPane()
        {
            backwardButton = CreateBackwardButton(browserView);
            forwardButton = CreateForwardButton(browserView);
            refreshButton = CreateRefreshButton(browserView);
            stopButton = CreateStopButton(browserView);
            gearButton = CreateGearButton(browserView);

            backwardButton.ToolTip = Resources.BackwardButtonTooltip;
            forwardButton.ToolTip = Resources.ForwardButtonTooltip;
            refreshButton.ToolTip = Resources.RefreshButtonTooltip;
            stopButton.ToolTip = Resources.StopButtonTooltip;
            gearButton.ToolTip = Resources.GearButtonTooltip;

            addressBar = CreateAddressBar();

            FlowLayoutPanel actionsPanel = new FlowLayoutPanel();
            actionsPanel.Margin = new Padding(0);
            actionsPanel.Dock = DockStyle.Top;
            actionsPanel.AutoSize = true;

            actionsPanel.Controls.Add(backwardButton);
            actionsPanel.Controls.Add(forwardButton);
            actionsPanel.Controls.Add(refreshButton);
            actionsPanel.Controls.Add(stopButton);
            actionsPanel.Controls.Add(addressBar);
            actionsPanel.Controls.Add(gearButton);

            return actionsPanel;
        }


        private TextBox CreateAddressBar()
        {
            TextBox result = new TextBox();
            result.Margin = new Padding(2);
            result.Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10f);

            result.Text = DEFAULT_URL;
            result.KeyUp += delegate(object sender, KeyEventArgs e)
            {
                if (e.KeyData != Keys.Enter) { return; }

                browserView.Browser.LoadURL(result.Text);
            };

            browserView.Browser.StartLoadingFrameEvent += delegate(object sender, StartLoadingArgs e)
            {
                if (e.IsMainFrame)
                {
                    WinFormsUIContext.Instance.Send(new SendOrPostCallback(
                        delegate(object state)
                        {
                            refreshButton.Enabled = false;
                             stopButton.Enabled = true;
                        }), null);
                }
            };

            browserView.Browser.ProvisionalLoadingFrameEvent += delegate(object sender, ProvisionalLoadingArgs e)
            {
                if (e.IsMainFrame)
                {
                    WinFormsUIContext.Instance.Post(new SendOrPostCallback(
                        delegate(object state)
                        {
                            result.Text = e.Url;
                            result.SelectionStart = result.Text.Length;
                        }), e);
                }
            };

            browserView.Browser.FinishLoadingFrameEvent += delegate(object sender, FinishLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    bool canGoForward = e.Browser.CanGoForward();
                    bool canGoBack = e.Browser.CanGoBack();
                    WinFormsUIContext.Instance.Send(new SendOrPostCallback(
                        delegate(object state)
                        {
                            refreshButton.Enabled = true;
                             stopButton.Enabled = false;

                             forwardButton.Enabled = canGoForward;
                             backwardButton.Enabled = canGoBack;
                        }), null);
                }
            };

            browserView.Browser.FailLoadingFrameEvent += delegate(object sender, FailLoadingEventArgs e)
            {
                if (e.IsMainFrame)
                {
                    WinFormsUIContext.Instance.Send(new SendOrPostCallback(
                        delegate(object state)
                        {
                            refreshButton.Enabled = true;
                             stopButton.Enabled = false;
                        }), null);
                }
            };

            return result;
        }

        private static ImageButton CreateBackwardButton(BrowserView browserView)
        {

            var button = CreateButton(Resources.Back, Resources.BackSelected);
            button.Click += delegate
            {
                browserView.Browser.GoBack();
            };
            ((Control)button).TabStop = true;
            return button;
        }

        private static ImageButton CreateForwardButton(BrowserView browserView)
        {
            var button = CreateButton(Resources.Forward, Resources.ForwardSelected);
            button.Click += delegate
            {
                browserView.Browser.GoForward();
            };

            ((Control)button).TabStop = true;
            return button;
        }

        private static ImageButton CreateRefreshButton(BrowserView browserView)
        {
            var button = CreateButton(Resources.Refresh, Resources.RefreshSelected);
            button.Click += delegate
            {
                browserView.Browser.Reload(true);
            };

            ((Control)button).TabStop = true;
            return button;
        }

        private static ImageButton CreateStopButton(BrowserView browserView)
        {
            var button = CreateButton(Resources.Stop, Resources.StopSelected);
            button.Click += delegate
            {
                browserView.Browser.Stop();
            };

            ((Control)button).TabStop = true;
            return button;
        }

        private bool IsFocusRequired()
        {
            String url = addressBar.Text;
            return String.IsNullOrEmpty(url) || url.Equals(DEFAULT_URL);
        }

        private static ImageButton CreateButton(Bitmap icon, Bitmap rolloverIcon)
        {
            ImageButton button = new ImageButton();
            button.Icon = icon;
            button.RolloverIcon = rolloverIcon;

            return button;
        }

        private static ImageButton CreateGearButton(BrowserView browserView)
        {
            var button = CreateButton(Resources.Gear, Resources.Gear);
            PreferenceMenu preferenceMenuHandler = new PreferenceMenu(browserView);
            preferenceMenuHandler.AddPreferenceMenu(button);            
            ((Control)button).TabStop = true;

            return button;
        }
    }
}