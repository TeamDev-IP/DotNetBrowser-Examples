using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Demo.WPF
{
    class BrowserExecuteCommandMenu
    {
        private BrowserView browserView;
        private Browser browser;

        public BrowserExecuteCommandMenu(BrowserView browserView)
        {
            this.browserView = browserView;
            this.browser = browserView.Browser;
        }

        public MenuItem ExecuteCommandItem()
        {
            MenuItem executeCommand = BuildMenuItem.Build("Execute Command", true, false, delegate { });

            MenuItem cut = BuildMenuItem.Build("Cut", true, false, delegate { });
            MenuItem copy = BuildMenuItem.Build("Copy", true, false, delegate { });
            MenuItem paste = BuildMenuItem.Build("Paste", true, false, delegate { });
            MenuItem selectAll = BuildMenuItem.Build("Select All", true, false, delegate { });
            MenuItem unselect = BuildMenuItem.Build("Unselect", true, false, delegate { });
            MenuItem undo = BuildMenuItem.Build("Undo", true, false, delegate { });
            MenuItem redo = BuildMenuItem.Build("Redo", true, false, delegate { });
            MenuItem insertText = BuildMenuItem.Build("Insert Text", true, false, delegate { });
            MenuItem findText = BuildMenuItem.Build("Find Text", true, false, delegate { });

            cut.IsEnabled = (browser.IsCommandEnabled(EditorCommand.CUT)) ? true : false;
            copy.IsEnabled = (browser.IsCommandEnabled(EditorCommand.COPY)) ? true : false;
            paste.IsEnabled = (browser.IsCommandEnabled(EditorCommand.PASTE)) ? true : false;
            selectAll.IsEnabled = (browser.IsCommandEnabled(EditorCommand.SELECT_ALL)) ? true : false;
            unselect.IsEnabled = (browser.IsCommandEnabled(EditorCommand.UNSELECT)) ? true : false;
            undo.IsEnabled = (browser.IsCommandEnabled(EditorCommand.UNDO)) ? true : false;
            redo.IsEnabled = (browser.IsCommandEnabled(EditorCommand.REDO)) ? true : false;
            insertText.IsEnabled = (browser.IsCommandEnabled(EditorCommand.INSERT_TEXT)) ? true : false;

            cut.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.CUT);
            };

            copy.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.COPY);
            };

            paste.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.PASTE);
            };

            selectAll.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.SELECT_ALL);
            };

            unselect.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.UNSELECT);
            };

            redo.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.REDO);
            };

            undo.Click += delegate
            {
                browser.ExecuteCommand(EditorCommand.UNDO);
            };

            insertText.Click += delegate
            {
                InsertText();
            };

            findText.Click += delegate
            {
                FindTextForm();
            };

            executeCommand.Items.Add(cut);
            executeCommand.Items.Add(copy);
            executeCommand.Items.Add(paste);
            executeCommand.Items.Add(selectAll);
            executeCommand.Items.Add(unselect);
            executeCommand.Items.Add(undo);
            executeCommand.Items.Add(redo);
            executeCommand.Items.Add(insertText);
            executeCommand.Items.Add(findText);

            return executeCommand;
        }

        private void InsertText()
        {
            Grid grid = new Grid();
            Window insertForm = new Window();
            insertForm.Width = 235;
            insertForm.Height = 120;
            insertForm.Title = "Insert text";
            insertForm.Content = grid;

            ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(100) };
            RowDefinition rowDefinition = new RowDefinition { Height = new GridLength(40) };

            grid.ColumnDefinitions.Add(columnDefinition);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(rowDefinition);

            TextBox insertText = new TextBox();
            insertText.Width = 180;
            insertText.Height = 20;
            insertText.RenderTransformOrigin = new Point(20, 10);

            Button buttonOk = new Button();
            buttonOk.Content = "OK";
            buttonOk.Width = 60;
            buttonOk.Height = 20;
            buttonOk.Margin = new Thickness(30, 0, 0, 0);

            Button buttonCancel = new Button();
            buttonCancel.Content = "Cancel";
            buttonCancel.Width = 60;
            buttonCancel.Height = 20;
            buttonCancel.Margin = new Thickness(0, 0, 10, 0);

            grid.Set(insertText, 0, 0);
            Grid.SetColumnSpan(insertText, 2);
            grid.Set(buttonOk, 0, 1);
            buttonOk.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Set(buttonCancel, 1, 1);
            buttonCancel.HorizontalAlignment = HorizontalAlignment.Center;

            insertForm.ResizeMode = ResizeMode.NoResize;
            insertForm.WindowStyle = WindowStyle.SingleBorderWindow;

            insertForm.Owner = Window.GetWindow((FrameworkElement)browserView);
            insertForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            insertForm.Topmost = true;
            buttonOk.Click += delegate
            {
                if (insertText.Text != String.Empty)
                {
                    browser.ExecuteCommand(EditorCommand.INSERT_TEXT, insertText.Text);
                }
                insertForm.Close();
            };

            buttonCancel.Click += delegate
            {
                insertForm.Close();
            };

            insertForm.Show();
        }

        private void FindTextForm()
        {
            Grid grid = new Grid();
            Window findForm = new Window();
            findForm.Width = 235;
            findForm.Height = 120;
            findForm.Title = "Find text";
            findForm.Content = grid;

            ColumnDefinition columnDefinition = new ColumnDefinition { Width = new GridLength(100) };
            RowDefinition rowDefinition = new RowDefinition { Height = new GridLength(40) };

            grid.ColumnDefinitions.Add(columnDefinition);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(rowDefinition);

            TextBox findText = new TextBox();
            findText.Width = 180;
            findText.Height = 20;
            findText.RenderTransformOrigin = new Point(20, 10);

            Button buttonOk = new Button();
            buttonOk.Content = "OK";
            buttonOk.Width = 60;
            buttonOk.Height = 20;
            buttonOk.Margin = new Thickness(30, 0, 0, 0);

            Button buttonCancel = new Button();
            buttonCancel.Content = "Cancel";
            buttonCancel.Width = 60;
            buttonCancel.Height = 20;
            buttonCancel.Margin = new Thickness(0, 0, 10, 0);

            grid.Set(findText, 0, 0);
            Grid.SetColumnSpan(findText, 2);
            grid.Set(buttonOk, 0, 1);
            buttonOk.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Set(buttonCancel, 1, 1);
            buttonCancel.HorizontalAlignment = HorizontalAlignment.Center;

            findForm.ResizeMode = ResizeMode.NoResize;
            findForm.WindowStyle = WindowStyle.SingleBorderWindow;

            findForm.Owner = Window.GetWindow((FrameworkElement)browserView);
            findForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            findForm.Topmost = true;
            buttonOk.Click += delegate
            {
                if (findText.Text != String.Empty)
                {
                    if (!browser.ExecuteCommand(EditorCommand.FIND_STRING, findText.Text))
                    {
                        InfoMessageBox.Show((FrameworkElement)browserView, "No matches!", "Warning");
                    }
                }
                findForm.Close();
            };

            buttonCancel.Click += delegate
            {
                findForm.Close();
            };

            findForm.ShowDialog();
        }
    }
}
