using System;
using System.Runtime.InteropServices;
using ExcelComAddin.Browser;
using ExcelComAddin.Interop;
using ExcelComAddin.Ribbon;
using ExcelComAddin.TaskPane;
using Extensibility;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace ExcelComAddin.AddIn
{
    /// <summary>
    /// COM entry point for the Excel add-in. Excel loads this class at startup and drives
    /// it through <see cref="IDTExtensibility2"/> lifecycle callbacks. It also supplies the
    /// ribbon XML (<see cref="IRibbonExtensibility"/>) and receives the task-pane factory
    /// (<see cref="ICustomTaskPaneConsumer"/>).
    /// </summary>
    [ComVisible(true)]
    [Guid(ComConstants.RootClassGuid)]
    [ProgId(ComConstants.ProgId)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class Connect : IDTExtensibility2, IRibbonExtensibility, ICustomTaskPaneConsumer
    {
        private readonly RibbonController _ribbonController;
        private readonly TaskPaneManager _taskPaneManager;
        private Application _excelApplication;
        private ICTPFactory _ctpFactory;

        /// <summary>Called by <c>regasm.exe</c> to write the Excel add-in registry keys.</summary>
        [ComRegisterFunction]
        public static void Register(Type t) => ComRegistration.RegisterExcelAddInKeys();

        /// <summary>Called by <c>regasm.exe /u</c> to remove the Excel add-in registry keys.</summary>
        [ComUnregisterFunction]
        public static void Unregister(Type t) => ComRegistration.UnregisterExcelAddInKeys();

        public Connect()
        {
            _ribbonController = new RibbonController();
            _taskPaneManager = new TaskPaneManager(
                () => _excelApplication,
                CreateTaskPane,
                new EngineManager());
            _ribbonController.PanelOpenRequested += () => _taskPaneManager.Show();
        }

        /// <summary>
        /// Called by Excel when the add-in connects. Captures the <see cref="Application"/> object
        /// and enables the ribbon after a late startup.
        /// </summary>
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInstance, ref Array custom)
        {
            _excelApplication = application as Application;
            TryExposeAutomationObject(addInInstance);
            if (connectMode == ext_ConnectMode.ext_cm_AfterStartup)
                _ribbonController.SetEnabled(true);
        }

        /// <summary>Called by Excel when the add-in disconnects. Disposes the task pane and browser.</summary>
        public void OnDisconnection(ext_DisconnectMode removeMode, ref Array custom)
        {
            _taskPaneManager.Dispose();
            _excelApplication = null;
        }

        /// <inheritdoc/>
        public void OnAddInsUpdate(ref Array custom) { }

        /// <summary>Called by Excel after all add-ins have loaded. Enables the ribbon tab.</summary>
        public void OnStartupComplete(ref Array custom)
        {
            _ribbonController.SetEnabled(true);
        }

        /// <summary>Called by Excel just before shutdown. Disposes the task pane and browser.</summary>
        public void OnBeginShutdown(ref Array custom)
        {
            _taskPaneManager.Dispose();
        }

        /// <summary>Returns the ribbon XML that adds the "Sales Lead Add-in" tab to Excel.</summary>
        public string GetCustomUI(string ribbonId) => _ribbonController.GetCustomUi(ribbonId);

        /// <summary>Ribbon callback: returns whether the add-in tab should be visible.</summary>
        public bool GetTabVisible(object control) => _ribbonController.GetTabVisible(control);

        /// <summary>Ribbon callback: invoked when the user clicks "Open Panel".</summary>
        public void OnOpenPanel(object control) => _ribbonController.OnOpenPanel(control);

        /// <summary>
        /// Receives the <see cref="ICTPFactory"/> from Excel. The factory is stored and used
        /// later to create the task pane on demand.
        /// </summary>
        public void CTPFactoryAvailable(ICTPFactory CTPFactoryInst)
        {
            _ctpFactory = CTPFactoryInst;
        }

        private TaskPane.TaskPane CreateTaskPane()
        {
            if (_ctpFactory == null)
                throw new InvalidOperationException("CTP Factory is not available.");
            var ctp = _ctpFactory.CreateCTP("ExcelComAddin.BrowserHostControl", "Demo Panel", Type.Missing);
            return new TaskPane.TaskPane(ctp);
        }

        private static void TryExposeAutomationObject(object addInInstance)
        {
            try { ((dynamic)addInInstance).Object = addInInstance; } catch { }
        }
    }
}
