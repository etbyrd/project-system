// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.VisualStudio.Editors.ApplicationDesigner;
using Microsoft.VisualStudio.Editors.PropPageDesigner;

namespace Microsoft.VisualStudio.ProjectSystem.VS.PropertyPages
{
    internal abstract partial class WpfBasedPropertyPage : PropertyPage
    {
#pragma warning disable CA2213 // WPF Controls implement IDisposable 
        private PropertyPageElementHost _host;
#pragma warning restore CA2213
        private PropertyPageControl _control;
        private PropertyPageViewModel _viewModel;

        protected WpfBasedPropertyPage()
        {
            InitializeComponent();
        }

        protected abstract PropertyPageViewModel CreatePropertyPageViewModel();

        protected abstract PropertyPageControl CreatePropertyPageControl();

        protected override async Task OnSetObjects(bool isClosing)
        {
            if (isClosing)
            {
                _control.DetachViewModel();
                return;
            }
            else
            {
                //viewModel can be non-null when the configuration is changed.
                if (_control == null)
                {
                    _control = CreatePropertyPageControl();
                }
            }

            _viewModel = CreatePropertyPageViewModel();
            _viewModel.Project = UnconfiguredProject;
            await _viewModel.Initialize();
            _control.InitializePropertyPage(_viewModel);
        }

        protected override Task<int> OnApply()
        {
            return _control.Apply();
        }

        protected override Task OnDeactivate()
        {
            if (IsDirty)
            {
                return OnApply();
            }

            return Task.CompletedTask;
        }

        private void WpfPropertyPage_Load(object sender, EventArgs e)
        {
            SuspendLayout();

            _host = new PropertyPageElementHost
            {
                AutoSize = false,
                Dock = DockStyle.Fill
            };

            if (_control == null)
            {
                _control = CreatePropertyPageControl();
            }

            var viewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Focusable = false,
            };

            _control.KeyDown += _control_KeyDown;
            viewer.Content = _control;
            _host.Child = viewer;

            wpfHostPanel.Dock = DockStyle.Fill;
            wpfHostPanel.Controls.Add(_host);

            ResumeLayout(true);
            _control.StatusChanged += OnControlStatusChanged;
        }

        private void _control_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Okay so this works, we can see the tab returning
            // Step two is to get the active control
            if (e.Key == Key.Tab)
            {
                //If(SelectNextControl(ActiveControl, forward, tabStopOnly:= True, nested:= True, wrap:= False)) Then
                //      Return True
                //  Else
                //    Dim appDesView As ApplicationDesignerView = CType(_loadedPageSite.Owner, ApplicationDesignerView)
                //    appDesView.SelectedItem.Focus()
                //    appDesView.SelectedItem.FocusedFromKeyboardNav = True
                //End If

                var propPageSite = (PropertyPageSite)(_site);
                var propPageDesigner = (PropPageDesignerView)propPageSite.BackingServiceProvider;
                var appDesView = (ApplicationDesignerView)propPageSite._appDesView;
                //var appDesView = (ApplicationDesignerView)(((PropertyPageSite)(_site)).Owner);
                //appDesView.SelectedItem.Focus();
                appDesView.SwitchTab(true);
                //appDesView.SelectedItem.FocusedFromKeyboardNav = true;
            }
        }

        private void OnControlStatusChanged(object sender, EventArgs e)
        {
            if (IsDirty != _control.IsDirty)
            {
                IsDirty = _control.IsDirty;
            }
        }
    }
}
