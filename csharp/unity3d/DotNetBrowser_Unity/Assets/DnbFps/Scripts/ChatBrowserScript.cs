using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DnbFps.Scripts {
    public class ChatBrowserScript : BrowserScript
    {
        protected override void Start()
        {
#if UNITY_EDITOR
            DefaultUrl = @"Assets/DnbFps/Html/Chat/Chat.html";
#else
            DefaultUrl = System.IO.Path.Combine(System.Environment.CurrentDirectory, @"DnbFps/Html/Chat/Chat.html");
#endif
            base.Start();
        }

    }
}