' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    Partial Class MultiTargetingDialog
        Friend WithEvents CommandLinePanel As System.Windows.Forms.Panel
        Friend WithEvents TFMSelector As System.Windows.Forms.ListView
        Private components As System.ComponentModel.IContainer


        Protected Overloads Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        <System.Diagnostics.DebuggerNonUserCode()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MultiTargetingDialog))
            Me.TFMSelector = New System.Windows.Forms.ListView()
            Me.CommandLinePanel = New System.Windows.Forms.Panel()
            Me.SuspendLayout()
            '
            'TFMSelector
            '
            Me.TFMSelector.HideSelection = False
            resources.ApplyResources(Me.TFMSelector, "TFMSelector")
            Me.TFMSelector.Name = "TFMSelector"
            Me.TFMSelector.UseCompatibleStateImageBehavior = False
            '
            'CommandLinePanel
            '
            resources.ApplyResources(Me.CommandLinePanel, "CommandLinePanel")
            Me.CommandLinePanel.Name = "CommandLinePanel"
            '
            'MultiTargetingDialog
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.TFMSelector)
            Me.Name = "MultiTargetingDialog"
            Me.ResumeLayout(False)

        End Sub

    End Class

End Namespace
