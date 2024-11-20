using Assets.Scripts;
using DotNetBrowser.Dom;
using System;
using DnbEventType = DotNetBrowser.Dom.Events.EventType;

namespace Assets.DnbFps.Scripts
{
    internal class MenuBrowserScript : BrowserScript
    {
        public event EventHandler ContinueEvent;
        public event EventHandler NewGameEvent;
        public event EventHandler QuitEvent;

        protected override void Start()
        {
#if UNITY_EDITOR
            DefaultUrl = @"Assets/DnbFps/Html/Menu/MenuPage.html";
#else
            DefaultUrl = System.IO.Path.Combine(Environment.CurrentDirectory,
                                                @"DnbFps/Html/Menu/MenuPage.html");
#endif
            base.Start();
        }

        protected override void CreateBrowser()
        {
            base.CreateBrowser();

            Browser.Navigation.LoadFinished += Navigation_LoadFinished;
        }

        private void Navigation_LoadFinished(object sender, DotNetBrowser.Navigation.Events.LoadFinishedEventArgs e)
        {
            IDocument document = Browser.MainFrame.Document;
            IElement btnContinue = document.GetElementById("btnContinue");
            if (btnContinue != null)
            {
                btnContinue.Events[DnbEventType.Click] += (s, e) => {
                    ContinueEvent?.Invoke(s, e);
                };
            }

            IElement btnNewGame = document.GetElementById("btnNewGame");
            if (btnNewGame != null)
            {
                btnNewGame.Events[DnbEventType.Click] += (s, e) => {
                    NewGameEvent?.Invoke(s, e);
                };
            }

            IElement btnQuit = document.GetElementById("btnQuit");
            if (btnQuit != null)
            {
                btnQuit.Events[DnbEventType.Click] += (s, e) => {
                    QuitEvent?.Invoke(s, e);
                };
            }
        }
    }
}
