using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Handlers;
using System.Windows.Threading;
using System.Windows;
using System;
using DotNetBrowser.Wpf;
using DotNetBrowser.Browser;

namespace Extensions.Wpf
{
    internal class PopupHandler : IHandler<OpenExtensionActionPopupParameters, OpenExtensionActionPopupResponse>
    {
        private readonly FrameworkElement parent;

        private Dispatcher Dispatcher => parent?.Dispatcher
                                        ?? Application.Current.Dispatcher;

        public PopupHandler(FrameworkElement parent)
        {
            this.parent = parent;
        }

        public OpenExtensionActionPopupResponse Handle(OpenExtensionActionPopupParameters parameters)
        {
            // This handler is invoked before an extension shows its pop-up.
            // It's the same the pop-up that you see in the browser when
            // clicking on the extension icon in the browser
            //
            // This pop-up is, in fact, a separate browser. You can find it
            // in the parameters as `parameters.PopupBrowser`. This is a more
            // or less normal browser: you can use its DOM and automate your
            // actions.
            //
            // This browser has limitations and we will describe them when
            // the feature is ready.
            Dispatcher.BeginInvoke(new Action(() =>
            {
                BrowserView browserView = new BrowserView();
                browserView.InitializeFrom(parameters.PopupBrowser);
                Window window = new Window { Owner = Window.GetWindow(parent) };

                // These are arbitrary numbers. In the final API version,
                // DotNetBrowser will create and size the new window automatically.
                window.Width = 254;
                window.Height = 480;

                window.Content = browserView;
                window.Show();
            }));
            return OpenExtensionActionPopupResponse.Open();
        }
    }
}
