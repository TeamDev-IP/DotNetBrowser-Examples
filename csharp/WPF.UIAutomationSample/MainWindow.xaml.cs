#region Copyright

// Copyright 2019, TeamDev. All rights reserved.
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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Condition = System.Windows.Automation.Condition;

namespace WPF.UIAutomationSample
{
    public partial class MainWindow : Window
    {
        private IBrowser browser;
        private IEngine engine;

        #region Constructors

        public MainWindow()
        {
            try
            {
                Task.Run(() =>
                    {
                        engine = EngineFactory.Create(new EngineOptions.Builder
                        {
                            RenderingMode = RenderingMode.HardwareAccelerated,
                            ChromiumSwitches = {"--force-renderer-accessibility"}
                        }.Build());
                        browser = engine.CreateBrowser();
                    })
                    .ContinueWith(t =>
                    {
                        BrowserView.InitializeFrom(browser);
                        browser.Navigation.LoadUrl("https://teamdev.com/dotnetbrowser");
                    }, TaskScheduler.FromCurrentSynchronizationContext());

                InitializeComponent();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        #endregion

        #region Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextOutput.Clear();
            Task.Run(() =>
            {
                Process currentProcess = Process.GetCurrentProcess();
                AutomationElement chromiumElement =
                    GetChromiumElement(currentProcess);
                if (chromiumElement != null)
                {
                    Log("-- Element Properties --");
                    AutomationProperty[] properties =
                        chromiumElement.GetSupportedProperties();
                    foreach (AutomationProperty prop in properties)
                    {
                        Log("ProgrammaticName: " + prop.ProgrammaticName);
                        Log("\tProperty Name: " + Automation.PropertyName(prop));
                        var currentPropertyValue =
                            chromiumElement.GetCurrentPropertyValue(prop);
                        Log("\tProperty Value: " + currentPropertyValue);
                    }

                    Log("-- Element Patterns --");
                    AutomationPattern[] patterns =
                        chromiumElement.GetSupportedPatterns();
                    foreach (AutomationPattern pattern in patterns)
                    {
                        Log("ProgrammaticName: " + pattern.ProgrammaticName);
                        Log("\tPattern Name: " + Automation.PatternName(pattern));
                        var currentPattern = chromiumElement.GetCurrentPattern(pattern);
                        Log("\tPattern Value: " + currentPattern);
                        if (currentPattern is ValuePattern)
                        {
                            ValuePattern valuePattern = currentPattern as ValuePattern;
                            string value = valuePattern.Current.Value;
                            Log("\tValuePattern Value: " + value);
                        }
                    }

                    var children = chromiumElement.FindAll(
                                                           TreeScope.Descendants,
                                                           Condition.TrueCondition);
                    Log("-- Element Children --");
                    Log("Children count: " + children.Count);
                    Log("-- End --");
                }
                else
                {
                    Log("-- Chromium automation element not found --");
                }
            });
        }

        private static AutomationElement GetChromiumElement(Process process)
        {
            AutomationElement element = null;
            if (process != null && process.MainWindowHandle != IntPtr.Zero)
            {
                AutomationElement rootElement =
                    AutomationElement.FromHandle(process.MainWindowHandle);
                if (rootElement == null)
                {
                    return null;
                }
                Condition conditions =
                    new AndCondition(
                                     new PropertyCondition(
                                                           AutomationElement.ClassNameProperty,
                                                           "Chrome_RenderWidgetHostHWND"),
                                     new PropertyCondition(
                                                           AutomationElement.ControlTypeProperty,
                                                           ControlType.Document)
                                    );
                element = rootElement.FindFirst(
                                                TreeScope.Descendants,
                                                conditions);
            }
            return element;
        }

        private void Log(string text)
        {
            Dispatcher.BeginInvoke(
                                   (Action) (
                                                () => TextOutput.AppendText(text + Environment.NewLine))
                                  );
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            engine?.Dispose();
        }

        #endregion
    }
}