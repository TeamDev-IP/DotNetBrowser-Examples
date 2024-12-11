using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DnbFps.Scripts
{
    public class RawImageViewScript : BrowserViewScript
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void SetTexture(Texture texture)
        {
            GetComponent<RawImage>().texture = texture;
        }
    }
}
