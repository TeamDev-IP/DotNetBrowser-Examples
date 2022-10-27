using Assets.DnbFps.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

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
                htmlChat.PauseUpdating = true;
                MenuRawImage.SetActive(true);
            }
            else
            {
                MenuRawImage.SetActive(false);
                htmlChat.PauseUpdating = false;
            }
        }
    }
}
