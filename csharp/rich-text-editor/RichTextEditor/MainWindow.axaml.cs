#region Copyright

// Copyright © 2026, TeamDev. All rights reserved.
//
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Engine;
using DotNetBrowser.Handlers;
using DotNetBrowser.Js;
using DotNetBrowser.Navigation;
using DotNetBrowser.Navigation.Handlers;
using DotNetBrowser.Print;
using DotNetBrowser.Print.Handlers;
using DotNetBrowser.Print.Settings;
using DotNetBrowser.SpellCheck;

namespace RichTextEditor
{
    /// <summary>
    ///     A window with a native toolbar and a Quill editor hosted in DotNetBrowser.
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Welcome =
            "<h1>Release notes</h1>"
            + "<p>This version adds <strong>dark mode</strong> and fixes the "
            + "<em>\"Save as\"</em> dialog on C:\\Users paths.</p>"
            + "<p>Right-click a word with a red underline, like this "
            + "mispelled one, to see the suggestions.</p>";

        private static readonly FilePickerFileType HtmlFiles =
            new FilePickerFileType("HTML") { Patterns = new[] { "*.html", "*.htm" } };

        private static readonly FilePickerFileType PdfFiles =
            new FilePickerFileType("PDF") { Patterns = new[] { "*.pdf" } };

        // Chromium keeps the downloaded dictionaries and the words added to
        // the custom dictionary here, so they survive a restart.
        private static readonly string UserData = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
                                      Environment.SpecialFolderOption.Create),
            "RichTextEditor");

        private readonly IBrowser browser;
        private readonly IEngine engine;
        private readonly EditorHost host = new EditorHost();
        private IJsObject? editor;
        private string? pdfPath;

        public MainWindow()
        {
            CopyDictionaries();
            engine = EngineFactory.Create(new EngineOptions.Builder
            {
                RenderingMode = RenderingMode.HardwareAccelerated,
                UserDataDirectory = UserData,
                Schemes = { { EditorScheme.Scheme, new EditorScheme() } }
            }.Build());
            browser = engine.CreateBrowser();

            // Runs before the page's scripts, so window.host is there before
            // editor.js reports the first change. Only the main frame gets the
            // object.
            browser.InjectJsHandler = new Handler<InjectJsParameters>(p =>
            {
                if (!p.Frame.IsMain)
                {
                    return;
                }
                IJsObject window = p.Frame.ExecuteJavaScript<IJsObject>("window").Result;
                window.Properties["host"] = host;
            });
            host.Changed += words => Dispatcher.UIThread.Post(
                () => Status.Text = $"{words} words");

            // The browser never leaves the editor page and opens no popups,
            // for example when a file is dropped on it.
            browser.Navigation.StartNavigationHandler =
                new Handler<StartNavigationParameters, StartNavigationResponse>(
                    p => p.Url.StartsWith(EditorScheme.PageUrl, StringComparison.Ordinal)
                             ? StartNavigationResponse.Start()
                             : StartNavigationResponse.Ignore());
            browser.CreatePopupHandler =
                new Handler<CreatePopupParameters, CreatePopupResponse>(
                    p => CreatePopupResponse.Suppress());

            browser.ShowContextMenuHandler =
                new AsyncHandler<ShowContextMenuParameters, ShowContextMenuResponse>(
                    ShowContextMenu);
            browser.RequestPrintHandler =
                new Handler<RequestPrintParameters, RequestPrintResponse>(
                    p => RequestPrintResponse.Print());
            browser.PrintHtmlContentHandler =
                new Handler<PrintHtmlContentParameters, PrintHtmlContentResponse>(
                    PrintToPdf);

            InitializeComponent();
            BrowserView.InitializeFrom(browser);
            Opened += async (s, e) =>
            {
                try
                {
                    await EnableSpellCheck();
                    await LoadEditor();
                }
                catch (ObjectDisposedException)
                {
                    // The window was closed while the editor was loading.
                }
            };
        }

        // A new profile has no spell checking language on Windows and Linux.
        // Chromium downloads the dictionary the first time, so without a
        // network connection and without a copied dictionary, Add fails.
        private async Task EnableSpellCheck()
        {
            ISpellChecker spellChecker = engine.Profiles.Default.SpellChecker;
            try
            {
                await spellChecker.Languages.Add(Language.EnglishUs);
            }
            catch (LanguageNotAvailableException)
            {
                Status.Text = "Spell checking is off: no dictionary for English (US).";
            }
        }

        // Copies the dictionaries that ship with the app, if any, to where
        // Chromium looks for them, so spell checking works offline.
        private static void CopyDictionaries()
        {
            string source = Path.Combine(AppContext.BaseDirectory, "Dictionaries");
            if (!Directory.Exists(source))
            {
                return;
            }
            string target = Path.Combine(UserData, "Dictionaries");
            Directory.CreateDirectory(target);
            foreach (string file in Directory.GetFiles(source, "*.bdic"))
            {
                string copy = Path.Combine(target, Path.GetFileName(file));
                if (!File.Exists(copy))
                {
                    File.Copy(file, copy);
                }
            }
        }

        private async Task LoadEditor()
        {
            NavigationResult result = await browser.Navigation.LoadUrl(EditorScheme.PageUrl);
            if (result.LoadResult != LoadResult.Completed)
            {
                Status.Text = "The editor page failed to load.";
                return;
            }
            editor = await browser.MainFrame.ExecuteJavaScript<IJsObject>("window.editor");
            if (editor == null)
            {
                Status.Text = "The editor page failed to load.";
                return;
            }
            editor.Invoke("setHtml", Welcome);
        }

        private void Format(string name, object? value = null)
        {
            editor?.Invoke("toggle", name, value ?? true);
            BrowserView.Focus();
        }

        private void Bold_Click(object? sender, RoutedEventArgs e)
        {
            Format("bold");
        }

        private void Italic_Click(object? sender, RoutedEventArgs e)
        {
            Format("italic");
        }

        private void Underline_Click(object? sender, RoutedEventArgs e)
        {
            Format("underline");
        }

        private void Heading_Click(object? sender, RoutedEventArgs e)
        {
            Format("header", 2);
        }

        private void List_Click(object? sender, RoutedEventArgs e)
        {
            Format("list", "bullet");
        }

        private async void Open_Click(object? sender, RoutedEventArgs e)
        {
            if (editor == null)
            {
                return;
            }
            IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(
                new FilePickerOpenOptions { FileTypeFilter = new[] { HtmlFiles } });
            if (files.Count == 0 || files[0].TryGetLocalPath() is not string path)
            {
                return;
            }
            try
            {
                // The HTML goes to JavaScript as an argument, never as part of
                // the script text, so quotes and backslashes in it can't break
                // anything.
                editor.Invoke("setHtml", await File.ReadAllTextAsync(path));
                Status.Text = $"Opened {path}";
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
                                           or JsException)
            {
                Status.Text = $"Can't open {path}: {ex.Message}";
            }
        }

        private async void Save_Click(object? sender, RoutedEventArgs e)
        {
            if (editor == null)
            {
                return;
            }
            IStorageFile? file = await StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    DefaultExtension = "html",
                    FileTypeChoices = new[] { HtmlFiles }
                });
            if (file?.TryGetLocalPath() is not string path)
            {
                return;
            }
            try
            {
                string html = editor.Invoke<string>("getHtml");
                await File.WriteAllTextAsync(path, html);
                Status.Text = $"Saved {path}";
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
                                           or JsException)
            {
                Status.Text = $"Can't save {path}: {ex.Message}";
            }
        }

        private async void ExportPdf_Click(object? sender, RoutedEventArgs e)
        {
            if (editor == null)
            {
                return;
            }
            IStorageFile? file = await StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions
                {
                    DefaultExtension = "pdf",
                    FileTypeChoices = new[] { PdfFiles }
                });
            if (file?.TryGetLocalPath() is string path)
            {
                pdfPath = path;
                browser.MainFrame.Print();
            }
        }

        // Prints the editor page to the PDF file the user chose. Each path is
        // used once, so a print that doesn't come from Export PDF is cancelled.
        private PrintHtmlContentResponse PrintToPdf(PrintHtmlContentParameters p)
        {
            string? path = Interlocked.Exchange(ref pdfPath, null);
            if (path == null)
            {
                return PrintHtmlContentResponse.Cancel();
            }
            PdfPrinter<PdfPrinter.IHtmlSettings> pdf = p.Printers.Pdf;
            pdf.PrintJob.Settings.Apply(s =>
            {
                s.PdfFilePath = path;
                s.PrintingHeaderFooterEnabled = false;
            });
            pdf.PrintJob.PrintCompleted += (s, e) => Dispatcher.UIThread.Post(
                () => Status.Text = e.IsCompletedSuccessfully
                                        ? $"Exported {path}"
                                        : $"Can't export {path}");
            return PrintHtmlContentResponse.Print(pdf);
        }

        private Task<ShowContextMenuResponse> ShowContextMenu(ShowContextMenuParameters p)
        {
            TaskCompletionSource<ShowContextMenuResponse> closed =
                new TaskCompletionSource<ShowContextMenuResponse>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            Dispatcher.UIThread.Post(() =>
            {
                // The engine waits for the response, so it must come even if
                // the menu can't be shown.
                try
                {
                    ContextMenu menu = EditorContextMenu.Build(
                        browser, engine.Profiles.Default, p.SpellCheckMenu);
                    menu.Closed += (s, e) =>
                        closed.TrySetResult(ShowContextMenuResponse.Close());
                    menu.Placement = PlacementMode.BottomEdgeAlignedLeft;
                    menu.PlacementRect = new Rect(p.Location.X, p.Location.Y, 1, 1);
                    menu.Open(BrowserView);
                }
                catch (Exception)
                {
                    closed.TrySetResult(ShowContextMenuResponse.Close());
                }
            });
            return closed.Task;
        }

        private void Window_Closing(object? sender, WindowClosingEventArgs e)
        {
            browser.Dispose();
            engine.Dispose();
        }
    }
}
