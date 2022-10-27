using Assets.Scripts;
using DotNetBrowser.Dom;
using DotNetBrowser.Dom.Events;
using DotNetBrowser.Js;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DnbFps.Scripts
{
    internal class MenuBrowserScript : BrowserScript
    {
        public event EventHandler ContinueEvent;
        public event EventHandler NewGameEvent;
        public event EventHandler QuitEvent;

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
                btnContinue.Events[EventType.Click] += (s, e) => {
                    ContinueEvent?.Invoke(s, e);
                };
            }

            IElement btnNewGame = document.GetElementById("btnNewGame");
            if (btnNewGame != null)
            {
                btnNewGame.Events[EventType.Click] += (s, e) => {
                    NewGameEvent?.Invoke(s, e);
                };
            }

            IElement btnQuit = document.GetElementById("btnQuit");
            if (btnQuit != null)
            {
                btnQuit.Events[EventType.Click] += (s, e) => {
                    QuitEvent?.Invoke(s, e);
                };
            }
        }
    }
}
