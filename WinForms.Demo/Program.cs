using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
{
    static class Program
    {
        private static readonly TraceSource Log = new TraceSource("DotNetBrowser.Demo.WinForms");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.TraceEvent(TraceEventType.Information, 1, "{0} : Started the application", DateTime.Now);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();     
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            var message1 = "Unhandled exception in current domain : " + ex.ToString();
            Console.Error.WriteLine(message1);
            Log.TraceEvent(TraceEventType.Error, 1, message1);
            var message2 = "Runtime terminating: " + e.IsTerminating;
            Console.Error.WriteLine(message2);
            Log.TraceEvent(TraceEventType.Error, 1, message2);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = (Exception)e.Exception;
            var message = "Unhandled exception in main thread : " + ex.ToString();
            Console.Error.WriteLine(message);
            Log.TraceEvent(TraceEventType.Error, 1, message);
        }
    }
}
