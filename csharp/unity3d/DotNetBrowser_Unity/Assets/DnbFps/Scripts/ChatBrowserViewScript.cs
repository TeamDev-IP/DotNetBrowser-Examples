using Assets.DnbFps.Scripts;
using Unity.FPS.Gameplay;
using UnityEngine;

public class ChatBrowserViewScript : RawImageViewScript
{
    public GameObject PlayerGameObject;
    private PlayerCharacterController controller;

    protected override void Start()
    {
        base.Start();
        controller = PlayerGameObject.GetComponent<PlayerCharacterController>();
    }

    public override void Focus()
    {
        base.Focus();
        controller.enabled = false;
    }

    public override void Unfocus()
    {
        base.Unfocus();
        controller.enabled = true;
    }
}
