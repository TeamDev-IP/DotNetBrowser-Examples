#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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
using System.Diagnostics;
using System.Text;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Input.DragAndDrop.Events;
using DotNetBrowser.Input.DragAndDrop.Handlers;
using DotNetBrowser.Net;

namespace DragAndDrop.Wpf
{
    /// <summary>
    ///     This example demonstrates how to intercept drag and drop events
    ///     and extract data from them.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBrowser browser;
        private readonly IEngine engine;

        #region Constructors

        public MainWindow()
        {
            engine = EngineFactory
               .Create(new EngineOptions.Builder
                           {
                               RenderingMode = RenderingMode.HardwareAccelerated,
                               ChromiumSwitches = {"--enable-com-in-drag-drop"},
                               SandboxDisabled = true
                           }
                          .Build());

            browser = engine.CreateBrowser();

            browser.DragAndDrop.EnterDragHandler = new Handler<EnterDragParameters>(OnDragEnter);
            browser.DragAndDrop.DropHandler = new Handler<DropParameters>(OnDrop);

            InitializeComponent();
            browserView.InitializeFrom(browser);
            browser.Navigation.LoadUrl("teamdev.com");
        }

        #endregion

        #region Methods

        private void ExtractData(string name, IDataObject dataObject)
        {
            if (dataObject == null)
            {
                Debug.WriteLine("IDataObject is null");
                return;
            }

            StringBuilder sb = new StringBuilder("=====================================================");
            sb.AppendLine("\nEvent name:" + name);
            sb.AppendLine("\nIDataObject:" + dataObject);
            sb.AppendLine("=====================================================");
            foreach (string format in dataObject.GetFormats())
            {
                sb.AppendLine("Format:" + format);
                try
                {
                    object data = dataObject.GetData(format);
                    sb.AppendLine("Type:" + (data == null ? "[null]" : data.GetType().ToString()));

                    sb.AppendLine("Data:" + data);
                    IEnumerable<string> strings = data as IEnumerable<string>;
                    if (strings != null)
                    {
                        foreach (string s in strings)
                        {
                            sb.AppendLine("\tValue: " + s);
                        }
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine("Exception thrown: " + ex.Message);
                }

                sb.AppendLine("=====================================================");
            }

            string message = sb.ToString();
            Dispatcher.BeginInvoke((Action) (() => { Output.Text = message; }));

            Debug.WriteLine(message);
        }

        private void LogData(IDropData dropData, string evtName)
        {
            Debug.WriteLine($"{evtName}:DropData is null? {dropData == null}");
            if (dropData != null)
            {
                foreach (IFileValue file in dropData.Files)
                {
                    Debug.WriteLine($"{evtName}:File = {file?.FileName}");
                }
            }
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            browser?.Dispose();
            engine?.Dispose();
        }

        private void OnDragEnter(EnterDragParameters arg)
        {
            LogData(arg.Event.DropData, nameof(OnDragEnter));
            Debug.WriteLine("Data is null? " + (arg.Event.DataObject == null));
            System.Runtime.InteropServices.ComTypes.IDataObject dataObject = arg.Event.DataObject;
            if (dataObject != null)
            {
                ExtractData(nameof(OnDragEnter), new DataObject(dataObject));
            }
        }

        private void OnDrop(DropParameters arg)
        {
            LogData(arg.Event.DropData, nameof(OnDrop));
            Debug.WriteLine("Data is null? " + (arg.Event.DataObject == null));
            System.Runtime.InteropServices.ComTypes.IDataObject dataObject = arg.Event.DataObject;
            if (dataObject != null)
            {
                ExtractData(nameof(OnDrop), new DataObject(dataObject));
            }
        }

        #endregion
    }
}