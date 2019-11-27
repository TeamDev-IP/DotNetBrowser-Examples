using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DefaultMediaStreamDeviceSample
{
    /// <summary>
    /// The sample demonstrates how to get list of available audio and video capture devices,
    /// register own MediaStreamDeviceProvider to provide Chromium with default audio/video capture
    /// device to be used on a web page for working with webcam and accessing microphone.
    /// </summary>

    public class WindowMain : System.Windows.Window
    {
        private WPFBrowserView browserView;

        public WindowMain()
        {
            Browser browser = BrowserFactory.Create();
            browserView = new WPFBrowserView(browser);

            Content = browserView;

            Width = 1024;
            Height = 768;

            MediaStreamDeviceManager deviceManager = browser.MediaStreamDeviceManager;
            // Get list of all available audio capture devices (microphones).
            List<MediaStreamDevice> audioCaptureDevices =
                    deviceManager.GetMediaStreamDevices(MediaStreamType.AUDIO_CAPTURE);
            // Get list of all available video capture devices (webcams).
            List<MediaStreamDevice> videoCaptureDevices =
                    deviceManager.GetMediaStreamDevices(MediaStreamType.VIDEO_CAPTURE);

            // Register own provider to provide Chromium with default device.
            deviceManager.Provider = new MyMediaStreamDeviceProvider();

            this.Loaded += WindowMain_Loaded;

        }

        void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            browserView.Browser.LoadURL("https://alexandre.alapetite.fr/doc-alex/html5-webcam/index.en.html");
        }


        private class MyMediaStreamDeviceProvider : MediaStreamDeviceProvider
        {
            public void OnRequestDefaultDevice(MediaStreamDeviceRequest request)
            {
                // Set first available device as default.
                List<MediaStreamDevice> availableDevices = request.Devices;
                if (availableDevices.Count > 0)
                {
                    MediaStreamDevice defaultDevice = availableDevices[0];
                    request.SetDefaultMediaStreamDevice(defaultDevice);
                }
            }
        }

        [STAThread]
        public static void Main()
        {
            Application app = new Application();

            WindowMain wnd = new WindowMain();
            app.Run(wnd);

            var browser = wnd.browserView.Browser;
            wnd.browserView.Dispose();
            browser.Dispose();
        }
    }
}
