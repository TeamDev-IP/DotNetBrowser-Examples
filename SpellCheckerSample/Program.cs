using DotNetBrowser;
using DotNetBrowser.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SpellCheckerSample
{
    class Program
    {
        public class WindowMain : System.Windows.Window
        {
            private WPFBrowserView browserView;
            private Button frLanguageButton;
            private Button enLanguageButton;
            private Grid layout;


            public WindowMain()
            {
                layout = new Grid();
                ColumnDefinition gridCol1 = new ColumnDefinition();
                layout.ColumnDefinitions.Add(gridCol1);
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                RowDefinition gridRow2 = new RowDefinition();
                gridRow2.Height = new GridLength(45);
                RowDefinition gridRow3 = new RowDefinition();

                layout.RowDefinitions.Add(gridRow1);
                layout.RowDefinitions.Add(gridRow2);
                layout.RowDefinitions.Add(gridRow3);


                Content = layout;

                enLanguageButton = new Button();
                enLanguageButton.Content = "English";
                enLanguageButton.Height = 23;
                enLanguageButton.Click += enLanguageButton_Click;
                Grid.SetRow(enLanguageButton, 0);
                Grid.SetColumn(enLanguageButton, 0);

                frLanguageButton = new Button();
                frLanguageButton.Content = "French";
                frLanguageButton.Height = 23;
                frLanguageButton.Click += frLanguageButton_Click;
                Grid.SetRow(frLanguageButton, 1);
                Grid.SetColumn(frLanguageButton, 0);


                Browser browser = BrowserFactory.Create();
                browserView = new WPFBrowserView(browser);
                browser.ContextMenuHandler = new MyContextMenuHandler((FrameworkElement)browserView, browser);

                // Enable SpellChecker service.
                browser.Context.SpellCheckerService.Enabled = true;
                // Configure SpellChecker's language.
                browser.Context.SpellCheckerService.Language = "en-US";

                Grid.SetRow(browserView, 2);
                Grid.SetColumn(browserView, 0);

                layout.Children.Add(enLanguageButton);
                layout.Children.Add(frLanguageButton);
                layout.Children.Add(browserView);

                Width = 1024;
                Height = 768;
                this.Loaded += WindowMain_Loaded;
            }

            private void frLanguageButton_Click(object sender, RoutedEventArgs e)
            {
                browserView.Browser.Context.SpellCheckerService.Language = "fr-FR";
            }

            private void enLanguageButton_Click(object sender, RoutedEventArgs e)
            {
                browserView.Browser.Context.SpellCheckerService.Language = "en-US";
            }

            void WindowMain_Loaded(object sender, RoutedEventArgs e)
            {
                browserView.Browser.LoadHTML("<html><body><textarea rows='20' cols='30'>" +
                "Smple text with mitake. \r\n \r\n Exmple de texte avec ereur.</textarea></body></html>");
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

            private class MyContextMenuHandler : ContextMenuHandler
            {

                private FrameworkElement component;
                private Browser browser;

                public MyContextMenuHandler(FrameworkElement parentComponent, Browser browser)
                {
                    this.component = parentComponent;
                    this.browser = browser;
                }

                public void ShowContextMenu(ContextMenuParams parameters)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (parameters.DictionarySuggestions.Count > 0)
                        {
                            this.component.ContextMenu = CreatePopupMenu(parameters);
                            this.component.ContextMenu.IsOpen = true;
                        }
                        else if (this.component.ContextMenu != null)
                        {
                            this.component.ContextMenu.Items.Clear();
                            this.component.ContextMenu.Width = 0;
                            this.component.ContextMenu.Height = 0;
                        }
                    }));
                }

                private static System.Windows.Controls.ContextMenu CreatePopupMenu(ContextMenuParams parameters)
                {
                    System.Windows.Controls.ContextMenu result = new System.Windows.Controls.ContextMenu();

                    // Add suggestions menu items.
                    List<String> suggestions = parameters.DictionarySuggestions;
                    foreach (String suggestion in suggestions)
                    {
                        result.Items.Add(CreateMenuItem(suggestion, true, delegate
                        {
                            parameters.Browser.ReplaceMisspelledWord(suggestion);
                        }));
                    }
                    if (suggestions.Count > 0)
                    {
                        // Add the "Add to Dictionary" menu item.
                        result.Items.Add(new Separator());

                        result.Items.Add(CreateMenuItem("Add to Dictionary", true, delegate
                        {
                            String misspelledWord = parameters.MisspelledWord;
                            parameters.Browser.AddWordToSpellCheckerDictionary(misspelledWord);
                        }));
                    }
                    return result;
                }

                private static MenuItem CreateMenuItem(string item, bool isEnabled, RoutedEventHandler clickHandler)
                {
                    MenuItem result = new MenuItem();
                    result.Header = item;
                    result.IsEnabled = isEnabled;
                    result.Click += clickHandler;
                    return result;
                }

            }
        }
    }
}
