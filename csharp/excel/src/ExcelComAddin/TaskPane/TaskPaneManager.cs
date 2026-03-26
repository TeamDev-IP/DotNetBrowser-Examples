using System;
using ExcelComAddin.Browser;
using Microsoft.Office.Interop.Excel;

namespace ExcelComAddin.TaskPane
{
    /// <summary>
    /// Coordinates the task pane lifecycle: creates the pane on first show, wires the
    /// JavaScript bridge callbacks for reading and writing cell A1, and starts/stops the
    /// DotNetBrowser engine alongside the pane.
    /// </summary>
    public sealed class TaskPaneManager : IDisposable
    {
        private readonly Func<Application> _getApplication;
        private readonly Func<TaskPane> _paneFactory;
        private readonly EngineManager _engineManager;
        private TaskPane _pane;

        public TaskPaneManager(Func<Application> getApplication, Func<TaskPane> paneFactory, EngineManager engineManager)
        {
            _getApplication = getApplication;
            _paneFactory = paneFactory;
            _engineManager = engineManager;
        }

        /// <summary>
        /// Shows the task pane, creating it and starting the browser engine on the first call.
        /// Subsequent calls simply make the existing pane visible.
        /// </summary>
        public void Show()
        {
            if (_pane == null)
            {
                _pane = _paneFactory();
                _pane.HostControl.SetJavaScriptCallbacks(ReadCellA1, WriteCellA1);
                _pane.HostControl.InitializeBrowser(_engineManager.Start(), null, "app://excelcomaddin/index.html");
            }
            _pane.Visible = true;
        }

        /// <summary>Hides and destroys the task pane and shuts down the browser engine.</summary>
        public void Dispose()
        {
            _pane?.Dispose();
            _pane = null;
            _engineManager.Stop();
        }

        private string ReadCellA1()
        {
            try
            {
                var sheet = _getApplication()?.ActiveSheet as Worksheet;
                var cell = sheet?.Cells[1, 1] as Range;
                return cell?.Value?.ToString() ?? "(empty)";
            }
            catch { return "(error reading cell)"; }
        }

        private void WriteCellA1(string value)
        {
            try
            {
                var sheet = _getApplication()?.ActiveSheet as Worksheet;
                if (sheet != null)
                    sheet.Cells[1, 1] = value ?? string.Empty;
            }
            catch { }
        }
    }
}
