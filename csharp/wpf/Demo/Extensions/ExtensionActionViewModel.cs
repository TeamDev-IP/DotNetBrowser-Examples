#region Copyright

// Copyright 2024, TeamDev. All rights reserved.
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DotNetBrowser.Extensions;
using DotNetBrowser.Extensions.Events;
using DotNetBrowser.Ui;

namespace Demo.Wpf.Extensions
{
    public class ExtensionActionViewModel : INotifyPropertyChanged
    {
        private Bitmap icon;
        private string tooltip;

        #region Properties

        public ICommand ClickCommand { get; }
        public ICommand UninstallCommand { get; }

        public Bitmap Icon
        {
            get => icon;
            set => SetField(ref icon, value);
        }

        public string Tooltip
        {
            get => tooltip;
            set => SetField(ref tooltip, value);
        }

        internal IExtensionAction ExtensionAction { get; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        public ExtensionActionViewModel(IExtensionAction extensionAction)
        {
            ExtensionAction = extensionAction;
            ExtensionAction.Updated += OnActionUpdated;
            UpdateIcon();
            Tooltip = ExtensionAction.Tooltip;
            ClickCommand = new Command(o => ExtensionAction?.Click());
            UninstallCommand = new Command(o => ExtensionAction?.Extension.Uninstall());
        }

        #endregion

        #region Methods

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnActionUpdated(object sender, ExtensionActionUpdatedEventArgs e)
        {
            UpdateIcon();
            Tooltip = ExtensionAction.Tooltip;
        }

        private void UpdateIcon()
        {
            Bitmap icon = ExtensionAction?.Icon;
            if (icon != null && !icon.Size.IsEmpty)
            {
                Icon = icon;
            }
        }

        #endregion

        private class Command : ICommand
        {
            private readonly Action<object> executeAction;
            private readonly Func<object, bool> canExecuteFunc;

            public Command(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
            {
                this.executeAction = executeAction;
                this.canExecuteFunc = canExecuteFunc;
            }
            public bool CanExecute(object parameter) => canExecuteFunc?.Invoke(parameter) ?? true;

            public void Execute(object parameter)
            {
                executeAction?.Invoke(parameter);
            }

            public event EventHandler CanExecuteChanged;
        }
    }
}
