using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinForms.Demo
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
            MenuItem preferences = new MenuItem("Preferences");

            MenuItem JSEn = new MenuItem("JavaScript Enabled");
            MenuItem ImEn = new MenuItem("Images Enabled");
            MenuItem PlEn = new MenuItem("Plugins Enabled");
            MenuItem JSAcsClboard = new MenuItem("JavaScript Can Access Clipboard");
            MenuItem JSOpWin = new MenuItem("JavaScript Can Open Windows");
            MenuItem WebAudEn = new MenuItem("Web Audio Enabled");
            MenuItem AppCachedEn = new MenuItem("Application Cache Enabled");            
            MenuItem JSClWin = new MenuItem("JavaScript Can Close Windows");
            MenuItem DispInsecContEn = new MenuItem("Allow Displaying Insecure Content");
            MenuItem RunInsecContEn = new MenuItem("Allow Running Insecure Content");
            MenuItem KeyEventEn = new MenuItem("Fire Keyboard Events Enabled");
            MenuItem MouseEventEn = new MenuItem("Fire Mouse Events Enabled");
            MenuItem DatabaseEn = new MenuItem("Databases Enabled");
            MenuItem LoadImAuto = new MenuItem("Loads Images Automatically");
            MenuItem LocalStoreEn = new MenuItem("Local Storage Enabled");
            MenuItem UniTextcheckEn = new MenuItem("Unified Textchecker Enabled");

            JSEn.Checked = browser.Preferences.JavaScriptEnabled;
            ImEn.Checked = browser.Preferences.ImagesEnabled;
            PlEn.Checked = browser.Preferences.PluginsEnabled;
            JSAcsClboard.Checked = browser.Preferences.JavaScriptCanAccessClipboard;
            JSOpWin.Checked = browser.Preferences.JavaScriptCanOpenWindowsAutomatically;
            WebAudEn.Checked = browser.Preferences.WebAudioEnabled;
            AppCachedEn.Checked = browser.Preferences.ApplicationCacheEnabled;
            JSClWin.Checked = browser.Preferences.AllowScriptsToCloseWindows;
            DispInsecContEn.Checked = browser.Preferences.AllowDisplayingInsecureContent;
            RunInsecContEn.Checked = browser.Preferences.AllowRunningInsecureContent;
            KeyEventEn.Checked = browser.Preferences.FireKeyboardEventsEnabled;
            MouseEventEn.Checked = browser.Preferences.FireMouseEventsEnabled;
            DatabaseEn.Checked = browser.Preferences.DatabasesEnabled;
            LoadImAuto.Checked = browser.Preferences.LoadsImagesAutomatically;
            LocalStoreEn.Checked = browser.Preferences.LocalStorageEnabled;
            UniTextcheckEn.Checked = browser.Preferences.UnifiedTextcheckerEnabled;

            JSEn.Click += delegate
            {
                if (browser.Preferences.JavaScriptEnabled)
                {
                    browser.Preferences.JavaScriptEnabled = false;
                    JSEn.Checked = false;                    
                }
                else
                {
                    browser.Preferences.JavaScriptEnabled = true;
                    JSEn.Checked = true;
                }
                browser.Reload();
            };

            ImEn.Click += delegate
            {
                if (browser.Preferences.ImagesEnabled)
                {
                    browser.Preferences.ImagesEnabled = false;
                    ImEn.Checked = false;
                }
                else
                {
                    browser.Preferences.ImagesEnabled = true;
                    ImEn.Checked = true;
                }
                browser.Reload();
            };

            PlEn.Click += delegate
            {
                if (browser.Preferences.PluginsEnabled)
                {
                    browser.Preferences.PluginsEnabled = false;
                    PlEn.Checked = false;
                }
                else
                {
                    browser.Preferences.PluginsEnabled = true;
                    PlEn.Checked = true;
                }
                browser.Reload();
            };

            JSAcsClboard.Click += delegate
            {
                if (browser.Preferences.JavaScriptCanAccessClipboard)
                {
                    browser.Preferences.JavaScriptCanAccessClipboard = false;
                    JSAcsClboard.Checked = false;
                }
                else
                {
                    browser.Preferences.JavaScriptCanAccessClipboard = true;
                    JSAcsClboard.Checked = true;
                }
                browser.Reload();
            };

            JSOpWin.Click += delegate
            {
                if (browser.Preferences.JavaScriptCanOpenWindowsAutomatically)
                {
                    browser.Preferences.JavaScriptCanOpenWindowsAutomatically = false;
                    JSOpWin.Checked = false;
                }
                else
                {
                    browser.Preferences.JavaScriptCanOpenWindowsAutomatically = true;
                    JSOpWin.Checked = true;
                }
                browser.Reload();
            };

            WebAudEn.Click += delegate
            {
                if (browser.Preferences.WebAudioEnabled)
                {
                    browser.Preferences.WebAudioEnabled = false;
                    WebAudEn.Checked = false;
                }
                else
                {                    
                    browser.Preferences.WebAudioEnabled = true;
                    WebAudEn.Checked = true;
                }
                browser.Reload();
            };

            AppCachedEn.Click += delegate
            {
                if (browser.Preferences.ApplicationCacheEnabled)
                {
                    browser.Preferences.ApplicationCacheEnabled = false;
                    AppCachedEn.Checked = false;
                }
                else
                {
                    browser.Preferences.ApplicationCacheEnabled = true;
                    AppCachedEn.Checked = true;
                }
                browser.Reload();
            };

            JSClWin.Click += delegate
            {
                if (browser.Preferences.AllowScriptsToCloseWindows)
                {
                    browser.Preferences.AllowScriptsToCloseWindows = false;
                    JSClWin.Checked = false;
                }
                else
                {
                    browser.Preferences.AllowScriptsToCloseWindows = true;
                    JSClWin.Checked = true;
                }
                browser.Reload();
            };

            DispInsecContEn.Click += delegate
            {
                if (browser.Preferences.AllowDisplayingInsecureContent)
                {
                    browser.Preferences.AllowDisplayingInsecureContent = false;
                    DispInsecContEn.Checked = false;
                }
                else
                {
                    browser.Preferences.AllowDisplayingInsecureContent = true;
                    DispInsecContEn.Checked = true;
                }
                browser.Reload();
            };

            RunInsecContEn.Click += delegate
            {
                if (browser.Preferences.AllowRunningInsecureContent)
                {
                    browser.Preferences.AllowRunningInsecureContent = false;
                    RunInsecContEn.Checked = false;
                }
                else
                {
                    browser.Preferences.AllowRunningInsecureContent = true;
                    RunInsecContEn.Checked = true;
                }
                browser.Reload();
            };

            KeyEventEn.Click += delegate
            {
                if (browser.Preferences.FireKeyboardEventsEnabled)
                {
                    browser.Preferences.FireKeyboardEventsEnabled = false;
                    KeyEventEn.Checked = false;
                }
                else
                {
                    browser.Preferences.FireKeyboardEventsEnabled = true;
                    KeyEventEn.Checked = true;
                }
                browser.Reload();
            };

            MouseEventEn.Click += delegate
            {
                if (browser.Preferences.FireMouseEventsEnabled)
                {
                    browser.Preferences.FireMouseEventsEnabled = false;
                    MouseEventEn.Checked = false;
                }
                else
                {
                    browser.Preferences.FireMouseEventsEnabled = true;
                    MouseEventEn.Checked = true;
                }
                browser.Reload();
            };

            DatabaseEn.Click += delegate
            {
                if (browser.Preferences.DatabasesEnabled)
                {
                    browser.Preferences.DatabasesEnabled = false;
                    DatabaseEn.Checked = false;
                }
                else
                {
                    browser.Preferences.DatabasesEnabled = true;
                    DatabaseEn.Checked = true;
                }
                browser.Reload();
            };

            LoadImAuto.Click += delegate
            {
                if (browser.Preferences.LoadsImagesAutomatically)
                {
                    browser.Preferences.LoadsImagesAutomatically = false;
                    LoadImAuto.Checked = false;
                }
                else
                {
                    browser.Preferences.LoadsImagesAutomatically = true;
                    LoadImAuto.Checked = true;
                }
                browser.Reload();
            };

            LocalStoreEn.Click += delegate
            {
                if (browser.Preferences.LocalStorageEnabled)
                {
                    browser.Preferences.LocalStorageEnabled = false;
                    LocalStoreEn.Checked = false;
                }
                else
                {
                    browser.Preferences.LocalStorageEnabled = true;
                    LocalStoreEn.Checked = true;
                }
                browser.Reload();
            };

            UniTextcheckEn.Click += delegate
            {
                if (browser.Preferences.UnifiedTextcheckerEnabled)
                {
                    browser.Preferences.UnifiedTextcheckerEnabled = false;
                    UniTextcheckEn.Checked = false;
                }
                else
                {
                    browser.Preferences.UnifiedTextcheckerEnabled = true;
                    UniTextcheckEn.Checked = true;
                }
                browser.Reload();
            };

            preferences.MenuItems.Add(JSEn);
            preferences.MenuItems.Add(ImEn);
            preferences.MenuItems.Add(PlEn);
            preferences.MenuItems.Add(JSAcsClboard);
            preferences.MenuItems.Add(JSOpWin);
            preferences.MenuItems.Add(JSClWin);
            preferences.MenuItems.Add(WebAudEn);
            preferences.MenuItems.Add(AppCachedEn);
            preferences.MenuItems.Add(DispInsecContEn);
            preferences.MenuItems.Add(RunInsecContEn);
            preferences.MenuItems.Add(KeyEventEn);
            preferences.MenuItems.Add(MouseEventEn);
            preferences.MenuItems.Add(DatabaseEn);
            preferences.MenuItems.Add(LoadImAuto);
            preferences.MenuItems.Add(LocalStoreEn);
            preferences.MenuItems.Add(UniTextcheckEn);

            return preferences;
        }        
    }
}
