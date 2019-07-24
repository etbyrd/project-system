' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    Partial Class MultiTargetingDialog
        Friend WithEvents TFMSelector As System.Windows.Forms.ListView
        Friend WithEvents CurrentTFMs As System.Windows.Forms.ListView
        Friend WithEvents overarchingLayoutPanel As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents moveRightButton As System.Windows.Forms.Button
        Friend WithEvents moveLeftButton As System.Windows.Forms.Button
        Friend WithEvents moveUpButton As System.Windows.Forms.Button
        Friend WithEvents moveDownButton As System.Windows.Forms.Button

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
            Me.CurrentTFMs = New System.Windows.Forms.ListView()
            Me.moveRightButton = New System.Windows.Forms.Button()
            Me.moveLeftButton = New System.Windows.Forms.Button()
            Me.moveUpButton = New System.Windows.Forms.Button()
            Me.moveDownButton = New System.Windows.Forms.Button()
            Me.overarchingLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.overarchingLayoutPanel.SuspendLayout()
            Me.SuspendLayout()
            '
            'TFMSelector
            '
            Me.TFMSelector.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {CType(resources.GetObject("TFMSelector.Groups"), System.Windows.Forms.ListViewGroup), CType(resources.GetObject("TFMSelector.Groups1"), System.Windows.Forms.ListViewGroup), CType(resources.GetObject("TFMSelector.Groups2"), System.Windows.Forms.ListViewGroup), CType(resources.GetObject("TFMSelector.Groups3"), System.Windows.Forms.ListViewGroup)})
            Me.TFMSelector.HideSelection = False
            resources.ApplyResources(Me.TFMSelector, "TFMSelector")
            Me.TFMSelector.Name = "TFMSelector"
            Me.overarchingLayoutPanel.SetRowSpan(Me.TFMSelector, 4)
            Me.TFMSelector.UseCompatibleStateImageBehavior = False
            '
            'CurrentTFMs
            '
            Me.CurrentTFMs.HideSelection = False
            resources.ApplyResources(Me.CurrentTFMs, "CurrentTFMs")
            Me.CurrentTFMs.Name = "CurrentTFMs"
            Me.overarchingLayoutPanel.SetRowSpan(Me.CurrentTFMs, 4)
            Me.CurrentTFMs.UseCompatibleStateImageBehavior = False
            '
            'moveRightButton
            '
            resources.ApplyResources(Me.moveRightButton, "moveRightButton")
            Me.moveRightButton.Name = "moveRightButton"
            '
            'moveLeftButton
            '
            resources.ApplyResources(Me.moveLeftButton, "moveLeftButton")
            Me.moveLeftButton.Name = "moveLeftButton"
            '
            'moveUpButton
            '
            resources.ApplyResources(Me.moveUpButton, "moveUpButton")
            Me.moveUpButton.Name = "moveUpButton"
            '
            'moveDownButton
            '
            resources.ApplyResources(Me.moveDownButton, "moveDownButton")
            Me.moveDownButton.Name = "moveDownButton"
            '
            'overarchingLayoutPanel
            '
            Me.overarchingLayoutPanel.Controls.Add(Me.TFMSelector, 0, 0)
            Me.overarchingLayoutPanel.Controls.Add(Me.CurrentTFMs, 2, 0)
            Me.overarchingLayoutPanel.Controls.Add(Me.moveUpButton, 1, 0)
            Me.overarchingLayoutPanel.Controls.Add(Me.moveDownButton, 1, 1)
            Me.overarchingLayoutPanel.Controls.Add(Me.moveRightButton, 1, 2)
            Me.overarchingLayoutPanel.Controls.Add(Me.moveLeftButton, 1, 3)
            resources.ApplyResources(Me.overarchingLayoutPanel, "overarchingLayoutPanel")
            Me.overarchingLayoutPanel.Name = "overarchingLayoutPanel"
            '
            'MultiTargetingDialog
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.overarchingLayoutPanel)
            Me.Name = "MultiTargetingDialog"
            Me.overarchingLayoutPanel.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

    End Class

End Namespace
