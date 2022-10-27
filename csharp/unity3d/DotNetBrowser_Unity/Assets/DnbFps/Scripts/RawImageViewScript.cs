using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
