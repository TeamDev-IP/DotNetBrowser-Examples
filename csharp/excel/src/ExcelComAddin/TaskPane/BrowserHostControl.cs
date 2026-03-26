using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Js;
using DotNetBrowser.WinForms;
using DotNetBrowser.Handlers;

namespace ExcelComAddin.TaskPane
{
    /// <summary>
    /// COM-visible WinForms <see cref="UserControl"/> that hosts the DotNetBrowser view inside
    /// an Excel task pane. Excel instantiates it by ProgId when the task pane is created.
    /// After initialization, it injects itself into the browser's JavaScript context as
    /// <c>window.excelBridge</c>, exposing <see cref="readFromExcel"/> and
    /// <see cref="writeToExcel"/> to the web page.
    /// </summary>
    [ComVisible(true)]
    [Guid("B2A7A2C2-9C2B-4B2E-8A1B-1A2B3C4D5E6F")]
    [ProgId("ExcelComAddin.BrowserHostControl")]
    public class BrowserHostControl : UserControl
    {
        private BrowserView _dotNetBrowserView;
        private Control _browserView;
        private IBrowser _browser;
        private Func<string> _read;
        private Action<string> _write;
        private bool _disposed;

        public BrowserHostControl()
        {
            Dock = DockStyle.Fill;
        }

        /// <summary><c>true</c> once <see cref="InitializeBrowser"/> has been called.</summary>
        public bool IsInitialized => _browser != null;

        /// <summary>JavaScript-callable method: reads a value from Excel and returns it to the page.</summary>
        public string readFromExcel() => _read();

        /// <summary>JavaScript-callable method: writes <paramref name="value"/> to Excel from the page.</summary>
        public void writeToExcel(string value) => _write(value);

        /// <summary>
        /// Supplies the delegates that back <see cref="readFromExcel"/> and <see cref="writeToExcel"/>.
        /// Must be called before <see cref="InitializeBrowser"/>.
        /// </summary>
        public void SetJavaScriptCallbacks(Func<string> readFromExcel, Action<string> writeToExcel)
        {
            _read = readFromExcel;
            _write = writeToExcel;
        }

        /// <summary>
        /// Creates the DotNetBrowser <see cref="IBrowser"/>, mounts the browser view inside this
        /// control, and navigates to <paramref name="initialUrl"/>. Also registers the JS injection
        /// handler that exposes this control as <c>window.excelBridge</c>.
        /// </summary>
        public void InitializeBrowser(object engine, Func<Control> browserViewFactory, string initialUrl)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BrowserHostControl));

            if (!(engine is IEngine browserEngine))
                throw new ArgumentException("Expected DotNetBrowser IEngine instance.", nameof(engine));

            _browser?.Dispose();
            _browser = browserEngine.CreateBrowser();

            _browser.InjectJsHandler = new Handler<InjectJsParameters>(args =>
            {
                IJsObject window = args.Frame.ExecuteJavaScript<IJsObject>("window").Result;
                window.Properties["excelBridge"] = this;
            });

            var providedView = browserViewFactory?.Invoke();
            var browserView = providedView as BrowserView ?? new BrowserView();
            browserView.InitializeFrom(_browser);
            _dotNetBrowserView = browserView;

            browserView.Dock = DockStyle.Fill;
            Controls.Add(browserView);
            _browserView = browserView;

            if (!string.IsNullOrWhiteSpace(initialUrl))
                _browser.Navigation.LoadUrl(initialUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) { base.Dispose(disposing); return; }

            if (disposing)
            {
                if (_browserView != null)
                {
                    Controls.Remove(_browserView);
                    (_browserView as IDisposable)?.Dispose();
                    _browserView = null;
                }

                _dotNetBrowserView = null;
                _browser?.Dispose();
                _browser = null;
                _read = null;
                _write = null;
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
