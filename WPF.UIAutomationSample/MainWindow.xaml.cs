using System;
using System.Diagnostics;
using System.Windows;
using DotNetBrowser;
using System.Windows.Automation;
using Condition = System.Windows.Automation.Condition;

namespace WPF.UIAutomationSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            BrowserPreferences.SetChromiumSwitches("--force-renderer-accessibility");
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextOutput.Clear();
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

        }

        private static AutomationElement GetChromiumElement(Process process)
        {
            AutomationElement element = null;
            if (process != null && process.MainWindowHandle != IntPtr.Zero)
            {

                AutomationElement rootElement =
                    AutomationElement.FromHandle(process.MainWindowHandle);
                if (rootElement == null)
                    return null;
                System.Windows.Automation.Condition conditions =
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
                (Action)(
                    () => TextOutput.AppendText(text + Environment.NewLine))
            );
        }
    }

}
