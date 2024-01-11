using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.FPS.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.DnbFps.Scripts
{
    internal class MenuViewScript : RawImageViewScript
    {
        [Tooltip("InGameMenu game object should be assigned here.")]
        public GameObject MenuManagerGameObject;

        private InGameMenuManager inGameMenuManager;

        private MenuBrowserScript MenuBrowserScript => (MenuBrowserScript)browserScript;

        protected override void Start()
        {
            inGameMenuManager = MenuManagerGameObject.GetComponent<InGameMenuManager>();
            base.Start();
            MenuBrowserScript.ContinueEvent += MenuBrowserScript_ContinueEvent;
            MenuBrowserScript.NewGameEvent += MenuBrowserScript_NewGameEvent;
            MenuBrowserScript.QuitEvent += MenuBrowserScript_QuitEvent;
        }

        protected override void Update()
        {
            base.Update();
        }

        private void MenuBrowserScript_ContinueEvent(object sender, EventArgs e)
        {
            Dispatch((HideMenu, sender, e));
        }

        private void MenuBrowserScript_NewGameEvent(object sender, EventArgs e)
        {
            Dispatch((NewGame, sender, e));
        }

        private void MenuBrowserScript_QuitEvent(object sender, EventArgs e)
        {
            Dispatch((Quit, sender, e));
        }

        private void NewGame(object sender, EventArgs e)
        {
            HideMenu(sender, e);
            Unity.FPS.Game.EventManager.Broadcast(Unity.FPS.Game.Events.PlayerDeathEvent);
        }

        private void Quit(object sender, EventArgs e)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void HideMenu(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
            inGameMenuManager.SetPauseMenuActivation(false);
        }
    }
}
