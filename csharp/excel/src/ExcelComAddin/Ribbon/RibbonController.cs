namespace ExcelComAddin.Ribbon
{
    /// <summary>
    /// Owns the ribbon XML and handles the ribbon callbacks declared in it.
    /// Exposes <see cref="PanelOpenRequested"/> so the rest of the add-in can react
    /// to the "Open Panel" button without depending on COM ribbon types.
    /// </summary>
    public class RibbonController
    {
        private const string RibbonXml =
            "<customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui'>" +
            "<ribbon>" +
            "<tabs>" +
            "<tab id='excelComAddinTab' label='Sales Lead Add-in' getVisible='GetTabVisible'>" +
            "<group id='excelComAddinGroup' label='Lead Tools'>" +
            "<button id='excelComAddinOpenPanelButton' size='large' label='Open Panel' imageMso='TableInsert' onAction='OnOpenPanel'/>" +
            "</group>" +
            "</tab>" +
            "</tabs>" +
            "</ribbon>" +
            "</customUI>";

        /// <summary>Whether the add-in ribbon tab is currently enabled.</summary>
        public bool IsEnabled { get; private set; } = true;

        /// <summary>Returns the ribbon XML. Called by Excel via <see cref="Connect.GetCustomUI"/>.</summary>
        public string GetCustomUi(string ribbonId)
        {
            return RibbonXml;
        }

        /// <summary>Enables or disables the ribbon tab. Called after Excel startup completes.</summary>
        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }

        /// <summary>Ribbon callback bound to <c>getVisible</c> on the add-in tab.</summary>
        public bool GetTabVisible(object control)
        {
            return IsEnabled;
        }

        /// <summary>Ribbon callback bound to <c>onAction</c> on the "Open Panel" button.</summary>
        public void OnOpenPanel(object control)
        {
            PanelOpenRequested?.Invoke();
        }

        /// <summary>Raised when the user clicks "Open Panel" in the ribbon.</summary>
        public event System.Action PanelOpenRequested;
    }
}