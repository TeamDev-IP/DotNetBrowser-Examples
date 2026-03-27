using Microsoft.Office.Core;
using System;

namespace ExcelComAddin.TaskPane
{
    /// <summary>
    /// Thin wrapper around Excel's <see cref="CustomTaskPane"/> that keeps
    /// <see cref="TaskPaneManager"/> free of raw COM API details. Exposes the
    /// <see cref="HostControl"/> and a <see cref="Visible"/> setter, and owns
    /// the <c>CustomTaskPane</c> lifetime.
    /// </summary>
    public class TaskPane : IDisposable
    {
        private readonly CustomTaskPane customTaskPane;
        private readonly BrowserHostControl _hostControl;
        private bool disposedValue;

        /// <summary>The browser host control embedded in this task pane.</summary>
        public BrowserHostControl HostControl => _hostControl;

        /// <summary>Gets or sets whether the task pane is visible in Excel.</summary>
        public bool Visible
        {
            get => customTaskPane?.Visible ?? false;
            set
            {
                if (customTaskPane != null)
                {
                    customTaskPane.Visible = value;
                }
            }
        }

        public TaskPane(BrowserHostControl browserHostControl)
        {
            _hostControl = browserHostControl;
        }
        internal TaskPane(CustomTaskPane customTaskPane)
            : this(customTaskPane.ContentControl as BrowserHostControl)
        {
            this.customTaskPane = customTaskPane;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    customTaskPane?.Delete();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
