using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MyOutlookAddIn
{
    partial class BrowserFormRegion
    {
        #region Form Region Factory 

        [Microsoft.Office.Tools.Outlook.FormRegionMessageClass(Microsoft.Office.Tools.Outlook.FormRegionMessageClassAttribute.Note)]
        [Microsoft.Office.Tools.Outlook.FormRegionName("MyOutlookAddIn.BrowserFormRegion")]
        public partial class BrowserFormRegionFactory
        {
            // Occurs before the form region is initialized.
            // To prevent the form region from appearing, set e.Cancel to true.
            // Use e.OutlookItem to get a reference to the current Outlook item.
            private void BrowserFormRegionFactory_FormRegionInitializing(object sender, Microsoft.Office.Tools.Outlook.FormRegionInitializingEventArgs e)
            {
                e.Cancel = false;
            }
        }

        #endregion

        private IEngine engine;
        private IBrowser browser;

        // Occurs before the form region is displayed.
        // Use this.OutlookItem to get a reference to the current Outlook item.
        // Use this.OutlookFormRegion to get a reference to the form region.
        private void BrowserFormRegion_FormRegionShowing(object sender, System.EventArgs e)
        {
            Task.Run(() =>
            {
                engine = EngineFactory.Create(new EngineOptions.Builder()
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
        private void BrowserFormRegion_FormRegionClosed(object sender, System.EventArgs e)
        {
            engine?.Dispose();
        }
    }
}
