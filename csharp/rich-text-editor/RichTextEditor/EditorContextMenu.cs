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
using Avalonia.Controls;
using DotNetBrowser.Browser;
using DotNetBrowser.Browser.Handlers;
using DotNetBrowser.Frames;
using DotNetBrowser.Profile;
using DotNetBrowser.SpellCheck;

namespace RichTextEditor
{
    /// <summary>
    ///     Builds the editor's context menu: spelling suggestions for a
    ///     misspelled word, then Cut, Copy, and Paste.
    /// </summary>
    internal static class EditorContextMenu
    {
        public static ContextMenu Build(IBrowser browser, IProfile profile,
                                        SpellCheckMenu? spelling)
        {
            ContextMenu menu = new ContextMenu();
            string word = spelling?.MisspelledWord ?? "";
            if (word.Length > 0)
            {
                foreach (string suggestion in spelling!.DictionarySuggestions)
                {
                    menu.Items.Add(Item(suggestion,
                                        () => browser.ReplaceMisspelledWord(suggestion)));
                }
                menu.Items.Add(Item("Add to dictionary",
                                    () => profile.SpellChecker.CustomDictionary.Add(word)));
                menu.Items.Add(new Separator());
            }

            IFrame frame = browser.MainFrame;
            menu.Items.Add(Item("Cut", () => frame.Execute(EditorCommand.Cut())));
            menu.Items.Add(Item("Copy", () => frame.Execute(EditorCommand.Copy())));
            menu.Items.Add(Item("Paste", () => frame.Execute(EditorCommand.Paste())));
            return menu;
        }

        private static MenuItem Item(string header, Action action)
        {
            MenuItem item = new MenuItem { Header = header };
            item.Click += (s, e) => action();
            return item;
        }
    }
}
