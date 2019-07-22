' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    Partial Class MultiTargetingDialog
        Friend WithEvents CommandLinePanel As System.Windows.Forms.Panel
        Friend WithEvents TitleLabel As System.Windows.Forms.Label
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
            Me.TitleLabel = New System.Windows.Forms.Label()
            Me.CommandLinePanel = New System.Windows.Forms.Panel()
            Me.SuspendLayout()
            '
            'CommandLinePanel
            '
            resources.ApplyResources(Me.CommandLinePanel, "CommandLinePanel")
            Me.CommandLinePanel.Name = "CommandLinePanel"
            '
            'TitleLabel
            '
            resources.ApplyResources(Me.TitleLabel, "TitleLabel")
            Me.TitleLabel.Name = "TitleLabel"
            '
            'MultiTargetingDialog
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.HelpButton = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "MultiTargetingDialog"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.ResumeLayout(False)

        End Sub

    End Class

End Namespace
