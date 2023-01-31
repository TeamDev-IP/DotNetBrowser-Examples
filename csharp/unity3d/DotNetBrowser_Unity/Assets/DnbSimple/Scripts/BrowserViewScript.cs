#region Copyright

// Copyright © 2023, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DotNetBrowser.Browser;
using DotNetBrowser.Geometry;
using DotNetBrowser.Ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    ///     Updates texture of geometry primitive. Does forwarding of input to DotNetBrowser and makes focus control.
    /// </summary>
    public class BrowserViewScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerClickHandler
    {
        private Bitmap bitmap;

        /// <summary>
        ///     Browser scene object that contains <see cref="browserScript" /> instance.
        /// </summary>
        public GameObject BrowserGameObject;

        protected BrowserScript browserScript;
        private bool isFocused;
        private bool isMouseOver;
        private KeyboardHelper keyboardHelper;
        private RectTransform rectTransform;
        private MouseHelper mouseHelper;
        private Texture2D texture;
        private ConcurrentQueue<(EventHandler, object, EventArgs)> actions = new ConcurrentQueue<(EventHandler, object, EventArgs)>();

        /// <summary>
        ///     Gets an <see cref="IBrowser" /> instance which this view shows.
        /// </summary>
        public IBrowser Browser => browserScript.Browser;

        public bool PauseUpdating { get; set; }

        protected void Dispatch((EventHandler, object, EventArgs) eventHamdler) => actions.Enqueue(eventHamdler);

        protected virtual void Start()
        {
            browserScript = BrowserGameObject.GetComponent<BrowserScript>();
            mouseHelper = new MouseHelper(Browser.Mouse) {ViewSize = Browser.Size};
            keyboardHelper = new KeyboardHelper(Browser.Keyboard);
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Update()
        {
            if (PauseUpdating)
                return;

            while(actions.TryDequeue(out (EventHandler, object, EventArgs) action))
            {
                action.Item1.Invoke(action.Item2, action.Item3);
            }

            if (isMouseOver)
            {
                if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                {
                    mouseHelper?.MouseWheel();
                }
            }
            else
            {
                if (isFocused && Input.GetMouseButtonDown(0))
                {
                    Browser.Unfocus();
                    isFocused = false;
                }
            }

            UpdateTexture();
        }

        protected virtual void SetTexture(Texture texture)
            => gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture;


        private void OnGUI()
        {
            if (isFocused)
            {
                keyboardHelper?.HandleKeyboardEvents();
            }
        }

        private void OnMouseDown()
        {
            mouseHelper?.MouseDown();
            Browser.Focus();
            isFocused = true;
        }

        private void OnMouseDrag() => mouseHelper?.MouseDrag();
        private void OnMouseEnter() => isMouseOver = true;
        private void OnMouseExit() => isMouseOver = false;
        private void OnMouseOver() => mouseHelper?.MouseMoved();
        private void OnMouseUp() => mouseHelper?.MouseUp();

        private void UpdateTexture()
        {
            if (browserScript?.Bitmap == bitmap)
            {
                return;
            }

            bitmap = browserScript.Bitmap;
            int newWidth = (int) bitmap.Size.Width;
            int newHeight = (int) bitmap.Size.Height;
            if (texture == null || texture.width != newWidth || texture.height != newHeight)
            {
                texture = new Texture2D(newWidth, newHeight, TextureFormat.BGRA32, true);
                SetTexture(texture);
                mouseHelper.ViewSize = bitmap.Size;
            }

            texture.SetPixelData((byte[])bitmap.Pixels, 0);
            texture.Apply(true);
        }

        public void OnPointerEnter(PointerEventData eventData) => isMouseOver = true;
        public void OnPointerExit(PointerEventData eventData) => isMouseOver = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            SetPoint(eventData);
            mouseHelper?.MouseDown();
            Browser.Focus();
            isFocused = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetPoint(eventData);
            mouseHelper?.MouseUp();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            SetPoint(eventData);
            mouseHelper?.MouseMoved();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        private void SetPoint(PointerEventData data)
        {
            if(bitmap == null)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, null, out Vector2 localClick);
            localClick.x = (rectTransform.rect.xMin * -1) - (localClick.x * -1);
            localClick.y = (rectTransform.rect.yMin * -1) - (localClick.y * -1);

            Vector2 viewportClick = new Vector2(localClick.x / rectTransform.rect.size.x, localClick.y / rectTransform.rect.size.y);

            int x = (int)(viewportClick.x * bitmap.Size.Width);
            int y = (int)((1.0f - viewportClick.y) * bitmap.Size.Height);
            mouseHelper.Point = new Point(x, y);
        }
    }
}
