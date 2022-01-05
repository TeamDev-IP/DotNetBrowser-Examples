#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Events;
using DotNetBrowser.Navigation.Events;
using DotNetBrowser.Wpf;
using Microsoft.Win32;

namespace Demo.Wpf
{
    public partial class BrowserTab
    {
        private IBrowser browser;

        public IBrowser Browser
        {
            get => browser;
            set
            {
                browser = value;
                if (browser != null)
                {
                    browserView.InitializeFrom(browser);
                    browser.TitleChanged += Browser_TitleChanged;
                    browser.StatusChanged += Browser_StatusChanged;
                    browser.Navigation.FrameLoadFinished += Navigation_FrameLoadFinished;
                    browser.ShowContextMenuHandler = browserView.ShowContextMenuHandler;
                    LoadUrl(AddressBar.Text);
                }
            }
        }

        public event EventHandler Closed;

        public BrowserTab()
        {
            InitializeComponent();
        }

        public void CloseTab(bool raiseClosedEvent)
        {
            Browser?.Dispose();
            if (raiseClosedEvent)
            {
                Closed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Browser_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) (() => { Status.Text = e.Text; }));
        }

        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) (() => { Title.Text = e.Title; }));
        }

        private void HideJsConsole(object sender, RoutedEventArgs e)
        {
            JsConsole.Visibility = Visibility.Collapsed;
        }

        private void HideScrollbars(object sender, RoutedEventArgs e)
        {
            if (Browser != null)
            {
                Browser.Settings.ScrollbarsHidden = true;
            }
        }

        private void LoadCssCursorsUrl(object sender, RoutedEventArgs e)
        {
            LoadUrl("https://developer.mozilla.org/en-US/docs/Web/CSS/cursor");
        }
        
        private void LoadDownloadFileUrl(object sender, RoutedEventArgs e)
        {
            LoadUrl("https://storage.googleapis.com/cloud.teamdev.com/downloads/dotnetbrowser/2.7/dotnetbrowser-net45-2.7.zip");
        }

        private void LoadGoogleMaps(object sender, RoutedEventArgs e)
        {
            LoadUrl("http://maps.google.com");
        }

        private void LoadHtml5Video(object sender, RoutedEventArgs e)
        {
            LoadUrl("http://www.w3.org/2010/05/video/mediaevents.html");
        }

        private void LoadJsDialogsUrl(object sender, RoutedEventArgs e)
        {
            LoadUrl("http://www.javascripter.net/faq/alert.htm");
        }

        private void LoadPdf(object sender, RoutedEventArgs e)
        {
            LoadUrl("http://www.orimi.com/pdf-test.pdf");
        }

        private void LoadPopupUrl(object sender, RoutedEventArgs e)
        {
            LoadUrl("http://www.popuptest.com/");
        }

        private void LoadSelectOptionUrl(object sender, RoutedEventArgs e)
        {
            LoadUrl("https://www.w3schools.com/tags/tryit.asp?filename=tryhtml_option");
        }

        private void LoadUploadFileUrl(object sender, RoutedEventArgs e)
        {
            LoadUrl("https://www.w3schools.com/howto/tryit.asp?filename=tryhow_html_file_upload_button");
        }

        private void LoadUrl(string url)
        {
            Browser?.Navigation.LoadUrl(url)
                   .ContinueWith(t => { UpdateControlsStates(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Navigation_FrameLoadFinished(object sender, FrameLoadFinishedEventArgs e)
        {
            if (e.Frame?.IsMain == true)
            {
                Dispatcher?.BeginInvoke((Action) UpdateControlsStates);
            }
        }

        private void OnAddressBarKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoadUrl(AddressBar.Text);
            }
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            Browser?.Navigation.GoBack()
                   .ContinueWith(t => { UpdateControlsStates(); },
                                 TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            CloseTab(true);
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            Browser?.Navigation.GoForward()
                   .ContinueWith(t => { UpdateControlsStates(); },
                                 TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                CloseTab(true);
            }
        }

        private void OnJsConsoleInputKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                JsConsoleOutput.Text += ">> " + JsConsoleInput.Text + Environment.NewLine;
                Browser?.MainFrame?.ExecuteJavaScript(JsConsoleInput.Text)
                       .ContinueWith(t =>
                       {
                           JsConsoleOutput.Text += "<< " + t.Result + Environment.NewLine;
                           JsConsoleInput.Clear();
                       }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void OnMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            if (FindResource("BrowserMenu") is System.Windows.Controls.ContextMenu cm)
            {
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
        }

        private void Print(object sender, RoutedEventArgs e)
        {
            Browser?.MainFrame?.Print();
        }

        private void ShowJsConsole(object sender, RoutedEventArgs e)
        {
            JsConsole.Visibility = Visibility.Visible;
            JsConsoleInput.Focus();
        }

        private void ShowScrollbars(object sender, RoutedEventArgs e)
        {
            if (Browser != null)
            {
                Browser.Settings.ScrollbarsHidden = false;
            }
        }

        private void TakeScreenshot(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog {Filter = "PNG image (*.png)|*.png"};
            if (dialog.ShowDialog(Window.GetWindow(this)) == true)
            {
                Bitmap bmp = Browser.TakeImage().ToBitmap();
                bmp.Save(dialog.FileName, ImageFormat.Png);
            }
        }

        private void UpdateControlsStates()
        {
            if (!Browser.IsDisposed)
            {
                AddressBar.Text = Browser.Url;
                Title.Text = Browser.Title;
                BackButton.IsEnabled = Browser.Navigation.CanGoBack();
                ForwardButton.IsEnabled = Browser.Navigation.CanGoForward();
            }
        }
    }
}
