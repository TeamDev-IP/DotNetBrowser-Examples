using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Demo.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly TraceSource Log = new TraceSource("DotNetBrowser.Demo.WPF");
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += 
                new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);
            Log.TraceEvent(TraceEventType.Information, 1, "{0} : Started the application", DateTime.Now);
        }

        private void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            Exception ex = (Exception)e.Exception;

            var message = "Unhandled exception in main thread : " + ex;
            Console.Error.WriteLine(message);
            Log.TraceEvent(TraceEventType.Error, 1, message);
        }
    }
}
