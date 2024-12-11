using Assets.Scripts;
using UnityEngine;

namespace Assets.DnbFps.Scripts {
    public class ChatBrowserScript : BrowserScript
    {
        protected override void Start()
        {
#if UNITY_EDITOR
            DefaultUrl = @"Assets/DnbFps/Html/Chat/Chat.html";
#else
            DefaultUrl = $"{Application.dataPath}/../DnbFps/Html/Chat/Chat.html";
#endif
            base.Start();
        }
    }
}