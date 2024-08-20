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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using DotNetBrowser.Extensions;

namespace Demo.Wpf.Extensions
{
    /// <summary>
    ///     Interaction logic for ExtensionActionsPanel.xaml
    /// </summary>
    public partial class ExtensionActionsPanel : ItemsControl
    {
        #region Properties

        public ObservableCollection<ExtensionActionViewModel> ExtensionActions { get; } =
            new ObservableCollection<ExtensionActionViewModel>();

        #endregion

        #region Constructors

        public ExtensionActionsPanel()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Methods

        public void Update(IEnumerable<IExtensionAction> actions)
        {
            ExtensionActions.Clear();
            foreach (IExtensionAction action in actions)
            {
                ExtensionActions.Add(new ExtensionActionViewModel(action));
            }
        }

        #endregion
    }
}
