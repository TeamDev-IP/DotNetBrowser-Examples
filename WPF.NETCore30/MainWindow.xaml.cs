using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using DotNetBrowser.Browser;
using DotNetBrowser.Engine;

namespace WPF.NETCore30
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBrowser browser;
        private IEngine engine;

        public MainWindow()
        {
            Task.Run(() =>
            {
                engine = EngineFactory.Create(new EngineOptions.Builder { RenderingMode = RenderingMode.HardwareAccelerated }
                                                  .Build());
                browser = engine.CreateBrowser();
            })
            .ContinueWith(t =>
            {
                webView.InitializeFrom(browser);
                browser.Navigation.LoadUrl("https://www.teamdev.com/");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
