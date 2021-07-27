#region Copyright

// Copyright 2021, TeamDev. All rights reserved.
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
using System.Linq;
using System.Windows.Forms;
using DotNetBrowser.Engine;
using DotNetBrowser.Net.Proxy;
using DotNetBrowser.Profile;

namespace Profiles.WinForms
{
    /// <summary>
    ///     This example demonstrates how to work with different profiles
    ///     and its Browser instances.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly IEngine engine;

        #region Constructors

        public Form1()
        {
            engine = EngineFactory
               .Create(new EngineOptions.Builder
                           {
                               RenderingMode = RenderingMode.HardwareAccelerated,
                           }
                          .Build());

            InitializeComponent();

            profilesList.DataSource = engine.Profiles.ToArray();
            profilesList.DisplayMember = "Name";
        }

        #endregion

        #region Methods

        private void createProfileButton_Click(object sender, EventArgs e)
        {
            string profileNameText = profileName.Text;
            if (profileNameText != null)
            {
                //The incognito mode for the profile can be set by passing the second parameter to Profiles.Create().
                IProfile profile = engine.Profiles.Create(profileNameText);
                //Here is how to set a proxy per profile.
                profile.Proxy.Settings = new SystemProxySettings();

                profilesList.DataSource = engine.Profiles.ToArray();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            engine?.Dispose();
        }

        private void listBox1_DoubleClick_1(object sender, EventArgs e)
        {
            IProfile selectedItem = profilesList.SelectedItem as IProfile;
            if (selectedItem != null)
            {
                BrowserForm browserForm = new BrowserForm
                {
                    Browser = selectedItem.CreateBrowser()
                };
                browserForm.Show(this);
            }
        }

        private void profilesList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            int index = profilesList.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                IProfile selectedItem = profilesList.Items[index] as IProfile;
                if (selectedItem != null)
                {
                    engine.Profiles.Remove(selectedItem);
                    profilesList.DataSource = engine.Profiles.ToArray();
                }
            }
        }

        #endregion
    }
}