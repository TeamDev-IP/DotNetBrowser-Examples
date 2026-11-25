#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DotNetBrowser.Capture;
using DotNetBrowser.Capture.Handlers;
using DotNetBrowser.Wpf;

namespace ScreenSharing
{
    /// <summary>
    ///     A custom source picker: lists the screens, application windows, and
    ///     browsers that the page can share, with their thumbnails.
    /// </summary>
    public partial class SourcePicker : Window
    {
        public SourcePicker(StartSessionParameters parameters)
        {
            InitializeComponent();

            // The page decides whether it wants audio; the app can only say no.
            audioBox.IsEnabled = parameters.AudioMode == AudioMode.Capture;

            List<SourceItem> items = new List<SourceItem>();
            foreach (Source source in parameters.Sources.Screens)
            {
                items.Add(new SourceItem(source));
            }
            foreach (Source source in parameters.Sources.ApplicationWindows)
            {
                items.Add(new SourceItem(source));
            }
            foreach (Source source in parameters.Sources.Browsers)
            {
                items.Add(new SourceItem(source));
            }
            sourceList.ItemsSource = items;
        }

        /// <summary>
        ///     The answer for the page. Closing the picker cancels the request.
        /// </summary>
        public StartSessionResponse Response { get; private set; } =
            StartSessionResponse.Cancel();

        private void SourceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            shareButton.IsEnabled = sourceList.SelectedItem != null;
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            if (sourceList.SelectedItem is not SourceItem item)
            {
                return;
            }
            AudioMode audio = audioBox.IsChecked == true
                                  ? AudioMode.Capture
                                  : AudioMode.Ignore;
            NotificationVisibility notification = notificationBox.IsChecked == true
                                                      ? NotificationVisibility.Show
                                                      : NotificationVisibility.Hide;
            Response = StartSessionResponse.SelectSource(item.Source, audio, notification);
            DialogResult = true;
        }

        /// <summary>
        ///     A row in the list: the source, its name, and its thumbnail.
        /// </summary>
        public sealed class SourceItem
        {
            public SourceItem(Source source)
            {
                Source = source;
                Name = source.Type == SourceType.Browser
                           ? $"Browser: {source.Name}"
                           : source.Name;
                if (source.Thumbnail?.Size != null && source.Thumbnail.Size.Width > 0)
                {
                    Thumbnail = source.Thumbnail.ToBitmapSource();
                }
            }

            public string Name { get; }

            public Source Source { get; }

            public ImageSource Thumbnail { get; }
        }
    }
}
