#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
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

using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    ///     Allows to use plane mesh as a browser.
    /// </summary>
    public class PlaneScript : BrowserViewScript
    {
        /// <summary>
        ///     Input field that is used as a address line.
        /// </summary>
        public InputField Address;

        private Button back;

        /// <summary>
        ///     Button object is used for backward navigation.
        /// </summary>
        public GameObject BackGameObject;

        private Button forward;

        /// <summary>
        ///     Button object is used for forward navigation.
        /// </summary>
        public GameObject ForwardGameObject;

        /// <summary>
        /// Button object is used to switch to the next example.
        /// </summary>
        public GameObject ExampleCombo;

        private TMPro.TMP_Dropdown exampleCombo;

        /// <summary>
        /// Button object is used for refresh page.
        /// </summary>
        public GameObject RefreshGameObject;

        private Button refresh;

        public GameObject CubeBrowser;
        public GameObject SphereBrowser;

        protected override void Start()
        {
            base.Start();

            Address.onEndEdit.AddListener(browserScript.Navigate);

            if (ExampleCombo != null)
            {
                exampleCombo = ExampleCombo.GetComponent<TMPro.TMP_Dropdown>();
                exampleCombo.onValueChanged.AddListener(Call);
            }
            back = BackGameObject.GetComponent<Button>();
            forward = ForwardGameObject.GetComponent<Button>();
            if (RefreshGameObject != null)
            {
                refresh = RefreshGameObject.GetComponent<Button>();
                refresh.onClick.AddListener(() => Browser.Navigation.Reload());
            }

            back.onClick.AddListener(() => Browser.Navigation.GoBack());
            forward.onClick.AddListener(() => Browser.Navigation.GoForward());
        }

        private void Call(int index)
        {
            if (index == 0)
            {
                Browser.Navigation.LoadUrl("www.youtube.com");
                Address.gameObject.SetActive(true);
                forward.gameObject.SetActive(true);
                back.gameObject.SetActive(true);
                refresh.gameObject.SetActive(true);
                CubeBrowser.SetActive(true);
                SphereBrowser.SetActive(true);
                return;
            }
            Browser.Navigation.LoadUrl(Path.GetFullPath(Const.Pages[index]));
            CubeBrowser.SetActive(false);
            SphereBrowser.SetActive(false);
            Address.gameObject.SetActive(false);
            forward.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
            refresh.gameObject.SetActive(false);
        }
    }
}
