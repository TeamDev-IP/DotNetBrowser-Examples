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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using DotNetBrowser.Browser;
using DotNetBrowser.Navigation.Events;

namespace Mvvm.Wpf.ViewModels
{
    public class MyBrowserViewModel : INotifyPropertyChanged
    {
        public IBrowser Browser { get; }

        public string Url
        {
            get { return Browser.Url; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Browser.Navigation.LoadUrl(value).Wait();
                }

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MyBrowserViewModel(IBrowser browser)
        {
            Browser = browser;
            Browser.Navigation.FrameLoadFinished += NavigationOnFrameLoadFinished;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NavigationOnFrameLoadFinished(object sender, FrameLoadFinishedEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                //Navigation is finished, notify that the URL is possibly updated.
                OnPropertyChanged(nameof(Url));
            }
        }
    }
}
