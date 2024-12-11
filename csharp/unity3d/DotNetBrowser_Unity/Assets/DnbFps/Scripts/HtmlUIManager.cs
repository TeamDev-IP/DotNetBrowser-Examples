using Assets.DnbFps.Scripts;
using DotNetBrowser.Dom;
using DotNetBrowser.Input.Mouse;
using System.Collections.Generic;
using System.Linq;
using Unity.FPS.Game;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class HtmlUIManager : MonoBehaviour
{
    public GameObject MenuRawImage;
    public GameObject ChatRawImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
            || Input.GetButtonDown(GameConstants.k_ButtonNameCancel) && MenuRawImage.activeInHierarchy)
        {
            var htmlChat = ChatRawImage.GetComponent<RawImageViewScript>();
            if (!MenuRawImage.activeInHierarchy)
            {
                htmlChat.Unfocus();
                MenuRawImage.SetActive(true);
            }
            else
            {
                MenuRawImage.SetActive(false);
                htmlChat.Focus();
            }
        }

        UpdateChat();
    }

    private void UpdateChat()
    {
        var htmlChat = ChatRawImage.GetComponent<RawImageViewScript>();
        if (Input.GetKeyDown(KeyCode.Return) && htmlChat.IsFocused)
        {
            htmlChat.Unfocus();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (htmlChat.IsFocused)
            {
                htmlChat.Unfocus();
            }
            else
            {
                htmlChat.Focus();
                ClickTextArea();
            }
        }
    }

    private void ClickTextArea()
    {
        var htmlChat = ChatRawImage.GetComponent<RawImageViewScript>();
        DotNetBrowser.Frames.IFrame frame = htmlChat.Browser.MainFrame;
        IDocument document = frame.Document;
        IElement documentElement = document.DocumentElement;
        IEnumerable<IElement> elements = documentElement?.GetElementsByTagName("textarea");
        htmlChat.Browser.Mouse.SimulateClick(DotNetBrowser.Input.Mouse.Events.MouseButton.Left, elements.First());
    }
}
