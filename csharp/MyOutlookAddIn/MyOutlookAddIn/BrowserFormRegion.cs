#region Copyright

// Copyright 2020, TeamDev. All rights reserved.
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
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Microsoft.Office.Tools.Outlook;
using Office = Microsoft.Office.Core;

namespace MyOutlookAddIn
{
    /// <summary>
    ///     The sample demonstrates how to embedded WinForms BrowserView instance
    ///     into Outlook as an AddIn.
    /// </summary>
    internal partial class BrowserFormRegion
    {
        private IBrowser browser;

        private IEngine engine;

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void BrowserFormRegion_FormRegionShowing(object sender, EventArgs e)
        {
            Task.Run(() =>
                {
                    engine = EngineFactory.Create(new EngineOptions.Builder
                    {
                        RenderingMode = RenderingMode.HardwareAccelerated
                    }.Build());
                    browser = engine.CreateBrowser();
                })
                .ContinueWith(t =>
                {
                    browserView1.InitializeFrom(browser);
                    browser?.Navigation.LoadUrl("https://www.teamdev.com/dotnetbrowser");
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        // Occurs when the form region is closed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void BrowserFormRegion_FormRegionClosed(object sender, EventArgs e)
        {
            engine?.Dispose();
        }

        #region Form Region Factory 

        [FormRegionMessageClass(FormRegionMessageClassAttribute.Note)]
        [FormRegionName("MyOutlookAddIn.BrowserFormRegion")]
        public partial class BrowserFormRegionFactory
        {
            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void BrowserFormRegionFactory_FormRegionInitializing(object sender,
                FormRegionInitializingEventArgs e)
            {
                e.Cancel = false;
            }
        }

        #endregion
    }
}