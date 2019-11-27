using DotNetBrowser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Demo
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
            MenuItem executeCommand = new MenuItem("Execute Command");

            MenuItem cut = new MenuItem("Cut");
            MenuItem copy = new MenuItem("Copy");
            MenuItem paste = new MenuItem("Paste");
            MenuItem selectAll = new MenuItem("Select All");
            MenuItem unselect = new MenuItem("Unselect");
            MenuItem undo = new MenuItem("Undo");
            MenuItem redo = new MenuItem("Redo");
            MenuItem insertText = new MenuItem("Insert Text");
            MenuItem findText = new MenuItem("Find Text");

            cut.Enabled = (browser.IsCommandEnabled(EditorCommand.CUT)) ? true : false;
            copy.Enabled = (browser.IsCommandEnabled(EditorCommand.COPY)) ? true : false;
            paste.Enabled = (browser.IsCommandEnabled(EditorCommand.PASTE)) ? true : false;
            selectAll.Enabled = (browser.IsCommandEnabled(EditorCommand.SELECT_ALL)) ? true : false;
            unselect.Enabled = (browser.IsCommandEnabled(EditorCommand.UNSELECT)) ? true : false;
            undo.Enabled = (browser.IsCommandEnabled(EditorCommand.UNDO)) ? true : false;
            redo.Enabled = (browser.IsCommandEnabled(EditorCommand.REDO)) ? true : false;
            insertText.Enabled = (browser.IsCommandEnabled(EditorCommand.INSERT_TEXT)) ? true : false;

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

            executeCommand.MenuItems.Add(cut);
            executeCommand.MenuItems.Add(copy);
            executeCommand.MenuItems.Add(paste);
            executeCommand.MenuItems.Add(selectAll);
            executeCommand.MenuItems.Add(unselect);
            executeCommand.MenuItems.Add(undo);
            executeCommand.MenuItems.Add(redo);
            executeCommand.MenuItems.Add(insertText);
            executeCommand.MenuItems.Add(findText);

            return executeCommand;
        }

        private void InsertText()
        {
            Form insertForm = new Form();
            insertForm.ShowIcon = false;
            insertForm.Size = new Size(235, 120);
            insertForm.Text = "Insert text";
            insertForm.MaximizeBox = false;
            insertForm.MinimizeBox = false;

            TextBox insertText = new TextBox();
            insertText.Location = new Point(20, 10);
            insertText.Size = new Size(180, 20);

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.Location = new Point(30, 40);

            Button buttonCancel = new Button();
            buttonCancel.Text = "Cancel";
            buttonCancel.Location = new Point(115, 40);

            insertForm.Controls.Add(insertText);
            insertForm.Controls.Add(buttonOk);
            insertForm.Controls.Add(buttonCancel);

            insertForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            insertForm.StartPosition = FormStartPosition.CenterParent;
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

            insertForm.ShowDialog();
        }

        private void FindTextForm()
        {
            Form findForm = new Form();
            findForm.ShowIcon = false;
            findForm.Size = new Size(235, 120);
            findForm.Text = "Find text";
            findForm.MaximizeBox = false;
            findForm.MinimizeBox = false;

            TextBox findText = new TextBox();
            findText.Location = new Point(20, 10);
            findText.Size = new Size(180, 20);

            Button buttonOk = new Button();
            buttonOk.Text = "OK";
            buttonOk.Location = new Point(30, 40);

            Button buttonCancel = new Button();
            buttonCancel.Text = "Cancel";
            buttonCancel.Location = new Point(115, 40);

            findForm.Controls.Add(findText);
            findForm.Controls.Add(buttonOk);
            findForm.Controls.Add(buttonCancel);

            findForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            findForm.StartPosition = FormStartPosition.CenterParent;
            buttonOk.Click += delegate
            {
                if (findText.Text != String.Empty)
                {
                    if (!browser.ExecuteCommand(EditorCommand.FIND_STRING, findText.Text))
                    {
                        InfoMessageBox.Show((Control)browserView, "No matches!", "Warning");
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
