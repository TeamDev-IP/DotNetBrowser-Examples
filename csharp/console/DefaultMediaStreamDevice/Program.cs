#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Media;
using DotNetBrowser.Media.Handlers;

namespace DefaultMediaStreamDevice
{
    /// <summary>
    ///     The sample demonstrates how to get list of available audio and video capture devices,
    ///     register own SelectMediaDeviceHandler to provide Chromium with default audio/video capture
    ///     device to be used on a web page for working with webcam and accessing microphone.
    /// </summary>
    public class WindowMain : Window
    {
        public static void Main()
        {
            using (IEngine engine = EngineFactory.Create())
            {
                using (IBrowser browser = engine.CreateBrowser())
                {
                    IMediaDevices mediaDevices = engine.MediaDevices;

                    Console.WriteLine("\nAvailable audio capture devices:");
                    PrintDevices(mediaDevices.AudioCaptureDevices);

                    Console.WriteLine("\nAvailable video capture devices:");
                    PrintDevices(mediaDevices.VideoCaptureDevices);

                    mediaDevices.SelectMediaDeviceHandler =
                        new Handler<SelectMediaDeviceParameters,
                            SelectMediaDeviceResponse>(SelectDevice);

                    browser.Navigation
                           .LoadUrl("https://alexandre.alapetite.fr/doc-alex/html5-webcam/index.en.html")
                           .Wait();
                }
            }

            Console.WriteLine("Press any key to terminate...");
            Console.ReadKey();
        }

        private static void PrintDevices(IEnumerable<MediaDevice> devices)
        {
            foreach (MediaDevice device in devices)
            {
                Console.WriteLine($"- {device.Name}");
            }
        }

        private static SelectMediaDeviceResponse SelectDevice(SelectMediaDeviceParameters arg)
        {
            Console.WriteLine($"\nRequested device type: {arg.Type}");
            // Set first available device as default.
            IEnumerable<MediaDevice> availableDevices = arg.Devices;
            MediaDevice defaultDevice = availableDevices.FirstOrDefault();

            if (defaultDevice != null)
            {
                Console.WriteLine($"Default device is set to {defaultDevice.Name}");
            }

            return SelectMediaDeviceResponse.Select(defaultDevice);
        }
    }
}
