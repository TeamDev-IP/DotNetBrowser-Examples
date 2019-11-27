using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Demo.WPF
{
    class ProxyConfigForm
    {
        private BrowserView browserView;
        private Browser browser;

        public ProxyConfigForm(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;
        }


        public void Show()
        {
            Grid grid = new Grid();
            Window proxyConfigForm = new Window();
            proxyConfigForm.Width = 500;
            proxyConfigForm.Height = 300;
            proxyConfigForm.Title = "Proxy Settings";
            proxyConfigForm.Content = grid;
            proxyConfigForm.ResizeMode = ResizeMode.NoResize;
            proxyConfigForm.WindowStyle = WindowStyle.SingleBorderWindow;
            proxyConfigForm.Owner = Window.GetWindow((FrameworkElement)browserView);
            proxyConfigForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            proxyConfigForm.Topmost = true;

            //Grid layout
            ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(185) };
            RowDefinition rowComboBox = new RowDefinition { Height = new GridLength(60) };
            RowDefinition rowForSettings = new RowDefinition { Height = new GridLength(80) };
            RowDefinition rowForButtons = new RowDefinition { Height = new GridLength(80) };
            grid.ColumnDefinitions.Add(columnDefinition);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(rowComboBox);
            grid.RowDefinitions.Add(rowForSettings);
            grid.RowDefinitions.Add(rowForButtons);

            //Choose proxy config
            TextBlock nameProxyList = new TextBlock();
            nameProxyList.Width = 150;
            nameProxyList.Height = 50;
            nameProxyList.Margin = new Thickness(5, 10, 5, 0);
            nameProxyList.Inlines.Add(new Bold(new Run("Choose proxy type:")));
            ComboBox proxyList = new ComboBox();
            proxyList.Margin = new Thickness(5);
            proxyList.HorizontalAlignment = HorizontalAlignment.Left;
            proxyList.Width = 100;
            proxyList.Height = 25;
            proxyList.Items.Add("AutoDetect");
            proxyList.Items.Add("Direct");
            proxyList.Items.Add("PAC File");
            proxyList.Items.Add("Custom");
            proxyList.SelectedIndex = 0;

            //Set proxy auto-config
            TextBlock namePacFileUrl = new TextBlock();
            namePacFileUrl.Width = 150;
            namePacFileUrl.Height = 50;
            namePacFileUrl.Margin = new Thickness(5, 10, 5, 0);
            namePacFileUrl.TextWrapping = TextWrapping.Wrap;
            namePacFileUrl.Text = "Set URL address of the proxy auto-config (PAC) file:";
            namePacFileUrl.VerticalAlignment = VerticalAlignment.Bottom;
            TextBox pacFileUrl = new TextBox();
            pacFileUrl.Width = 250;
            pacFileUrl.Height = 20;
            pacFileUrl.Margin = new Thickness(5);
            pacFileUrl.HorizontalAlignment = HorizontalAlignment.Left;
            pacFileUrl.VerticalAlignment = VerticalAlignment.Center;

            //Set proxy rules
            TextBlock nameProxyRules = new TextBlock();
            nameProxyRules.Width = 150;
            nameProxyRules.Height = 50;
            nameProxyRules.Margin = new Thickness(5, 10, 5, 0);
            nameProxyRules.Text = "\nSet proxy rules:";
            nameProxyRules.VerticalAlignment = VerticalAlignment.Bottom;
            TextBox proxyRules = new TextBox();
            proxyRules.Width = 250;
            proxyRules.Height = 20;
            proxyRules.Margin = new Thickness(5);
            proxyRules.ToolTip = "Ex.: 'http=foopy:80;ftp=foopy:20'";
            proxyRules.HorizontalAlignment = HorizontalAlignment.Left;
            proxyRules.VerticalAlignment = VerticalAlignment.Center;

            //Set bypass rules
            TextBlock nameExceptions = new TextBlock();
            nameExceptions.Width = 150;
            nameExceptions.Height = 50;
            nameExceptions.TextWrapping = TextWrapping.Wrap;
            nameExceptions.Margin = new Thickness(5, 5, 5, 10);
            nameExceptions.Text = "Set URLs that should bypass the proxy settings:";
            nameExceptions.VerticalAlignment = VerticalAlignment.Bottom;
            TextBox exceptions = new TextBox();
            exceptions.Width = 250;
            exceptions.Height = 20;
            exceptions.Margin = new Thickness(5);
            exceptions.ToolTip = "Match local addresses.";
            exceptions.HorizontalAlignment = HorizontalAlignment.Left;
            exceptions.VerticalAlignment = VerticalAlignment.Center;

            //Buttons
            Button buttonOk = new Button();
            buttonOk.Content = "OK";
            buttonOk.Width = 60;
            buttonOk.Height = 20;
            buttonOk.Margin = new Thickness(100, 0, 0, 0);
            buttonOk.HorizontalAlignment = HorizontalAlignment.Center;
            Button buttonCancel = new Button();
            buttonCancel.Content = "Cancel";
            buttonCancel.Width = 60;
            buttonCancel.Height = 20;
            buttonCancel.Margin = new Thickness(20, 0, 0, 0);
            buttonCancel.HorizontalAlignment = HorizontalAlignment.Center;

            namePacFileUrl.Visibility = Visibility.Collapsed;
            pacFileUrl.Visibility = Visibility.Collapsed;
            nameProxyRules.Visibility = Visibility.Collapsed;
            proxyRules.Visibility = Visibility.Collapsed;
            nameExceptions.Visibility = Visibility.Collapsed;
            exceptions.Visibility = Visibility.Collapsed;

            //Added controls on the grid
            grid.Set(nameProxyList, 0, 0);
            grid.Set(proxyList, 1, 0);
            grid.Set(namePacFileUrl, 0, 1);
            grid.Set(pacFileUrl, 1, 1);
            grid.Set(nameProxyRules, 0, 1);
            grid.Set(proxyRules, 1, 1);
            grid.Set(nameExceptions, 0, 2);
            grid.Set(exceptions, 1, 2);
            grid.Set(buttonOk, 0, 3);
            grid.Set(buttonCancel, 1, 3);

            proxyList.SelectionChanged += delegate
            {
                if (proxyList.SelectedIndex == 0)
                {
                    namePacFileUrl.Visibility = Visibility.Collapsed;
                    pacFileUrl.Visibility = Visibility.Collapsed;
                    nameProxyRules.Visibility = Visibility.Collapsed;
                    proxyRules.Visibility = Visibility.Collapsed;
                    nameExceptions.Visibility = Visibility.Collapsed;
                    exceptions.Visibility = Visibility.Collapsed;
                }
                else if (proxyList.SelectedIndex == 1)
                {
                    namePacFileUrl.Visibility = Visibility.Collapsed;
                    pacFileUrl.Visibility = Visibility.Collapsed;
                    nameProxyRules.Visibility = Visibility.Collapsed;
                    proxyRules.Visibility = Visibility.Collapsed;
                    nameExceptions.Visibility = Visibility.Collapsed;
                    exceptions.Visibility = Visibility.Collapsed;
                }
                else if (proxyList.SelectedIndex == 2)
                {
                    namePacFileUrl.Visibility = Visibility.Visible;
                    pacFileUrl.Visibility = Visibility.Visible;
                    nameProxyRules.Visibility = Visibility.Collapsed;
                    proxyRules.Visibility = Visibility.Collapsed;
                    nameExceptions.Visibility = Visibility.Collapsed;
                    exceptions.Visibility = Visibility.Collapsed;
                }
                else if (proxyList.SelectedIndex == 3)
                {
                    namePacFileUrl.Visibility = Visibility.Collapsed;
                    pacFileUrl.Visibility = Visibility.Collapsed;
                    nameProxyRules.Visibility = Visibility.Visible;
                    proxyRules.Visibility = Visibility.Visible;
                    nameExceptions.Visibility = Visibility.Visible;
                    exceptions.Visibility = Visibility.Visible;
                }
            };

            buttonOk.Click += delegate
            {
                if (proxyList.SelectedIndex == 0)
                {
                    browser.Context.ProxyConfig = new AutoDetectProxyConfig();
                }
                else if (proxyList.SelectedIndex == 1)
                {
                    browser.Context.ProxyConfig = new DirectProxyConfig();
                }
                else if (proxyList.SelectedIndex == 2)
                {
                    browser.Context.ProxyConfig = new URLProxyConfig(pacFileUrl.Text);
                }
                else if (proxyList.SelectedIndex == 3)
                {
                    browser.Context.ProxyConfig = new CustomProxyConfig(proxyRules.Text, exceptions.Text);
                }

                InfoMessageBox.Show((FrameworkElement)browserView, "Proxy Settings successfully applied", "Warning");
                proxyConfigForm.Close();
            };

            buttonCancel.Click += delegate
            {
                proxyConfigForm.Close();
            };

            proxyConfigForm.ShowDialog();
        }
    }
}
