using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Print;
using DotNetBrowser.Print.Handlers;

/// <summary>
/// This application demonstrates how to generate a PDF document
/// using DotNetBrowser.
/// </summary>
class Program
{
    private static async Task Main()
    {
        var engineOptions = new EngineOptions.Builder
        {
            RenderingMode = RenderingMode.OffScreen,
            LicenseKey = "your license key"
        }.Build();

        using var engine = EngineFactory.Create(engineOptions);
        using var browser = engine.CreateBrowser();

        await browser.Navigation.LoadUrl(Path.GetFullPath("template.html"));
        FillInData(browser);

        var whenPrintCompleted = ConfigurePrinting(browser);
        browser.MainFrame.Print();
        var resultPath = await whenPrintCompleted.Task;
    }


    private static void FillInData(IBrowser browser)
    {
        var accountNumber = "123-4567";
        var name = "Dr. Emmett Brown";
        var address = "1640 Riverside Drive";
        var reportingPeriod = "Oct 25 — November 25, 1985";

        browser.MainFrame.ExecuteJavaScript(
            $"setBillInfo('{accountNumber}', '{name}', '{address}', '{reportingPeriod}')"
        );

        var dayCost = 500;
        var nightCost = 312;
        var dayUsage = 1.212;
        var nightUsage = 88;

        browser.MainFrame.ExecuteJavaScript(
            $"addCharge('Day Tariff', {dayUsage}, {dayCost});" +
            $"addCharge('Night Tariff', {nightUsage}, {nightCost});"
        );
    }

    private static TaskCompletionSource<string> ConfigurePrinting(IBrowser browser)
    {
        // Tell the browser to print automatically instead of showing the print preview.
        browser.RequestPrintHandler = new Handler<RequestPrintParameters, RequestPrintResponse>(
           p => RequestPrintResponse.Print()
        );

        TaskCompletionSource<string> whenCompleted = new();
        // When the browser prints an HTML page.
        browser.PrintHtmlContentHandler = new Handler<PrintHtmlContentParameters, PrintHtmlContentResponse>(
            parameters =>
            {
                // Use the PDF printer.
                var printer = parameters.Printers.Pdf;
                var job = printer.PrintJob;

                // Generate a random name for PDF file.
                var guid = Guid.NewGuid();
                var path = Path.GetFullPath($"{guid}.pdf");
                job.Settings.PdfFilePath = path;

                // Remove white areas on the sides.
                job.Settings.PageMargins = PageMargins.None;
                // Remove default browser headers and footers.
                job.Settings.PrintingHeaderFooterEnabled = false;
                job.PrintCompleted += (_, _) => whenCompleted.SetResult(path);

                // Proceed with printing.
                return PrintHtmlContentResponse.Print(printer);
            });
        return whenCompleted;
    }
}
