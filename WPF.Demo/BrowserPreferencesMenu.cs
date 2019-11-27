using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Demo.WPF
{
    class BrowserPreferencesMenu
    {
        private BrowserView browserView;
        private Browser browser;

        public BrowserPreferencesMenu(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;
        }

        public MenuItem PreferencesItem()
        {
            MenuItem preferences = BuildMenuItem.Build("Preferences", true, false, delegate { });

            MenuItem JSEn = new MenuItem();
            MenuItem ImEn = new MenuItem();
            MenuItem PlEn = new MenuItem();
            MenuItem JSAcsClboard = new MenuItem();
            MenuItem JSOpWin = new MenuItem();
            MenuItem WebAudEn = new MenuItem();
            MenuItem AppCachedEn = new MenuItem();
            MenuItem JSClWin = new MenuItem();
            MenuItem DispInsecContEn = new MenuItem();
            MenuItem RunInsecContEn = new MenuItem();
            MenuItem KeyEventEn = new MenuItem();
            MenuItem MouseEventEn = new MenuItem();
            MenuItem DatabaseEn = new MenuItem();
            MenuItem LoadImAuto = new MenuItem();
            MenuItem LocalStoreEn = new MenuItem();
            MenuItem UniTextcheckEn = new MenuItem();

            JSEn = BuildMenuItem.Build("JavaScript Enabled", true, browser.Preferences.JavaScriptEnabled, delegate
            {
                if (browser.Preferences.JavaScriptEnabled)
                {
                    browser.Preferences.JavaScriptEnabled = false;
                    JSEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.JavaScriptEnabled = true;
                    JSEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(JSEn);

            ImEn = BuildMenuItem.Build("Images Enabled", true, browser.Preferences.ImagesEnabled, delegate
            {
                if (browser.Preferences.ImagesEnabled)
                {
                    browser.Preferences.ImagesEnabled = false;
                    ImEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.ImagesEnabled = true;
                    ImEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(ImEn);

            PlEn = BuildMenuItem.Build("Plugins Enabled", true, browser.Preferences.PluginsEnabled, delegate
            {
                if (browser.Preferences.PluginsEnabled)
                {
                    browser.Preferences.PluginsEnabled = false;
                    PlEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.PluginsEnabled = true;
                    PlEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(PlEn);

            JSAcsClboard = BuildMenuItem.Build("JavaScript Can Access Clipboard", true, browser.Preferences.JavaScriptCanAccessClipboard, delegate
            {
                if (browser.Preferences.JavaScriptCanAccessClipboard)
                {
                    browser.Preferences.JavaScriptCanAccessClipboard = false;
                    JSAcsClboard.IsChecked = false;
                }
                else
                {
                    browser.Preferences.JavaScriptCanAccessClipboard = true;
                    JSAcsClboard.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(JSAcsClboard);

            JSOpWin = BuildMenuItem.Build("JavaScript Can Open Windows", true, browser.Preferences.JavaScriptCanOpenWindowsAutomatically, delegate
            {
                if (browser.Preferences.JavaScriptCanOpenWindowsAutomatically)
                {
                    browser.Preferences.JavaScriptCanOpenWindowsAutomatically = false;
                    JSOpWin.IsChecked = false;
                }
                else
                {
                    browser.Preferences.JavaScriptCanOpenWindowsAutomatically = true;
                    JSOpWin.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(JSOpWin);

            JSClWin = BuildMenuItem.Build("JavaScript Can Close Windows", true, browser.Preferences.AllowScriptsToCloseWindows, delegate
            {
                if (browser.Preferences.AllowScriptsToCloseWindows)
                {
                    browser.Preferences.AllowScriptsToCloseWindows = false;
                    JSClWin.IsChecked = false;
                }
                else
                {
                    browser.Preferences.AllowScriptsToCloseWindows = true;
                    JSClWin.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(JSClWin);

            WebAudEn = BuildMenuItem.Build("Web Audio Enabled", true, browser.Preferences.WebAudioEnabled, delegate
            {
                if (browser.Preferences.WebAudioEnabled)
                {
                    browser.Preferences.WebAudioEnabled = false;
                    WebAudEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.WebAudioEnabled = true;
                    WebAudEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(WebAudEn);

            AppCachedEn = BuildMenuItem.Build("Application Cache Enabled", true, browser.Preferences.ApplicationCacheEnabled, delegate
            {
                if (browser.Preferences.ApplicationCacheEnabled)
                {
                    browser.Preferences.ApplicationCacheEnabled = false;
                    AppCachedEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.ApplicationCacheEnabled = true;
                    AppCachedEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(AppCachedEn);

            DispInsecContEn = BuildMenuItem.Build("Allow Displaying Insecure Content", true, browser.Preferences.AllowDisplayingInsecureContent, delegate
            {
                if (browser.Preferences.AllowDisplayingInsecureContent)
                {
                    browser.Preferences.AllowDisplayingInsecureContent = false;
                    DispInsecContEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.AllowDisplayingInsecureContent = true;
                    DispInsecContEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(DispInsecContEn);

            RunInsecContEn = BuildMenuItem.Build("Allow Running Insecure Content", true, browser.Preferences.AllowRunningInsecureContent, delegate
            {
                if (browser.Preferences.AllowRunningInsecureContent)
                {
                    browser.Preferences.AllowRunningInsecureContent = false;
                    RunInsecContEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.AllowRunningInsecureContent = true;
                    RunInsecContEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(RunInsecContEn);

            KeyEventEn = BuildMenuItem.Build("Fire Keyboard Events Enabled", true, browser.Preferences.FireKeyboardEventsEnabled, delegate
            {
                if (browser.Preferences.FireKeyboardEventsEnabled)
                {
                    browser.Preferences.FireKeyboardEventsEnabled = false;
                    KeyEventEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.FireKeyboardEventsEnabled = true;
                    KeyEventEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(KeyEventEn);

            MouseEventEn = BuildMenuItem.Build("Fire Mouse Events Enabled", true, browser.Preferences.FireMouseEventsEnabled, delegate
            {
                if (browser.Preferences.FireMouseEventsEnabled)
                {
                    browser.Preferences.FireMouseEventsEnabled = false;
                    MouseEventEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.FireMouseEventsEnabled = true;
                    MouseEventEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(MouseEventEn);

            DatabaseEn = BuildMenuItem.Build("Databases Enabled", true, browser.Preferences.DatabasesEnabled, delegate
            {
                if (browser.Preferences.DatabasesEnabled)
                {
                    browser.Preferences.DatabasesEnabled = false;
                    DatabaseEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.DatabasesEnabled = true;
                    DatabaseEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(DatabaseEn);

            LoadImAuto = BuildMenuItem.Build("Loads Images Automatically", true, browser.Preferences.LoadsImagesAutomatically, delegate
            {
                if (browser.Preferences.LoadsImagesAutomatically)
                {
                    browser.Preferences.LoadsImagesAutomatically = false;
                    LoadImAuto.IsChecked = false;
                }
                else
                {
                    browser.Preferences.LoadsImagesAutomatically = true;
                    LoadImAuto.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(LoadImAuto);

            LocalStoreEn = BuildMenuItem.Build("Local Storage Enabled", true, browser.Preferences.LocalStorageEnabled, delegate
            {
                if (browser.Preferences.LocalStorageEnabled)
                {
                    browser.Preferences.LocalStorageEnabled = false;
                    LocalStoreEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.LocalStorageEnabled = true;
                    LocalStoreEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(LocalStoreEn);

            UniTextcheckEn = BuildMenuItem.Build("Unified Textchecker Enabled", true, browser.Preferences.UnifiedTextcheckerEnabled, delegate
            {
                if (browser.Preferences.UnifiedTextcheckerEnabled)
                {
                    browser.Preferences.UnifiedTextcheckerEnabled = false;
                    UniTextcheckEn.IsChecked = false;
                }
                else
                {
                    browser.Preferences.UnifiedTextcheckerEnabled = true;
                    UniTextcheckEn.IsChecked = true;
                }
                browser.Reload();
            });
            preferences.Items.Add(UniTextcheckEn);

            return preferences;
        }
    }
}
