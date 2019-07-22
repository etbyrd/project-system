' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.Win32

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    Partial Class ReferencePathsPropPage

        Friend WithEvents FolderLabel As System.Windows.Forms.Label
        Friend WithEvents Folder As System.Windows.Forms.TextBox
        Friend WithEvents FolderBrowse As System.Windows.Forms.Button
        Friend WithEvents AddFolder As System.Windows.Forms.Button
        Friend WithEvents UpdateFolder As System.Windows.Forms.Button
        Friend WithEvents ReferencePathLabel As System.Windows.Forms.Label
        Friend WithEvents ReferencePath As System.Windows.Forms.ListBox
        Friend WithEvents RemoveFolder As System.Windows.Forms.Button
        Friend WithEvents MoveUp As System.Windows.Forms.Button
        Friend WithEvents overarchingTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents addUpdateTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents MoveDown As System.Windows.Forms.Button
        Private _components As System.ComponentModel.IContainer

        Protected Overloads Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If Not (_components Is Nothing) Then
                    _components.Dispose()
                End If
                RemoveHandler SystemEvents.UserPreferenceChanged, AddressOf Me.SystemEvents_UserPreferenceChanged
            End If
            MyBase.Dispose(disposing)
        End Sub

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReferencePathsPropPage))
        Me.FolderLabel = New System.Windows.Forms.Label()
        Me.Folder = New System.Windows.Forms.TextBox()
        Me.FolderBrowse = New System.Windows.Forms.Button()
        Me.AddFolder = New System.Windows.Forms.Button()
        Me.UpdateFolder = New System.Windows.Forms.Button()
        Me.ReferencePath = New System.Windows.Forms.ListBox()
        Me.MoveUp = New System.Windows.Forms.Button()
        Me.MoveDown = New System.Windows.Forms.Button()
        Me.RemoveFolder = New System.Windows.Forms.Button()
        Me.ReferencePathLabel = New System.Windows.Forms.Label()
        Me.overarchingTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.addUpdateTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.overarchingTableLayoutPanel.SuspendLayout
        Me.addUpdateTableLayoutPanel.SuspendLayout
        Me.SuspendLayout
        '
        'FolderLabel
        '
        resources.ApplyResources(Me.FolderLabel, "FolderLabel")
        Me.overarchingTableLayoutPanel.SetColumnSpan(Me.FolderLabel, 2)
        Me.FolderLabel.Name = "FolderLabel"
        '
        'Folder
        '
        resources.ApplyResources(Me.Folder, "Folder")
        Me.Folder.Name = "Folder"
        '
        'FolderBrowse
        '
        resources.ApplyResources(Me.FolderBrowse, "FolderBrowse")
        Me.FolderBrowse.Name = "FolderBrowse"
        '
        'AddFolder
        '
        resources.ApplyResources(Me.AddFolder, "AddFolder")
        Me.AddFolder.Name = "AddFolder"
        '
        'UpdateFolder
        '
        resources.ApplyResources(Me.UpdateFolder, "UpdateFolder")
        Me.UpdateFolder.Name = "UpdateFolder"
        '
        'ReferencePath
        '
        resources.ApplyResources(Me.ReferencePath, "ReferencePath")
        Me.ReferencePath.FormattingEnabled = true
        Me.ReferencePath.Name = "ReferencePath"
        Me.overarchingTableLayoutPanel.SetRowSpan(Me.ReferencePath, 4)
        '
        'MoveUp
        '
        resources.ApplyResources(Me.MoveUp, "MoveUp")
        Me.MoveUp.Name = "MoveUp"
        '
        'MoveDown
        '
        resources.ApplyResources(Me.MoveDown, "MoveDown")
        Me.MoveDown.Name = "MoveDown"
        '
        'RemoveFolder
        '
        resources.ApplyResources(Me.RemoveFolder, "RemoveFolder")
        Me.RemoveFolder.Name = "RemoveFolder"
        '
        'ReferencePathLabel
        '
        resources.ApplyResources(Me.ReferencePathLabel, "ReferencePathLabel")
        Me.overarchingTableLayoutPanel.SetColumnSpan(Me.ReferencePathLabel, 2)
        Me.ReferencePathLabel.Name = "ReferencePathLabel"
        '
        'overarchingTableLayoutPanel
        '
        resources.ApplyResources(Me.overarchingTableLayoutPanel, "overarchingTableLayoutPanel")
        Me.overarchingTableLayoutPanel.Controls.Add(Me.addUpdateTableLayoutPanel, 0, 2)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.FolderLabel, 0, 0)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.ReferencePath, 0, 4)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.ReferencePathLabel, 0, 3)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.Folder, 0, 1)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.FolderBrowse, 1, 1)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.MoveUp, 1, 4)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.MoveDown, 1, 5)
        Me.overarchingTableLayoutPanel.Controls.Add(Me.RemoveFolder, 1, 6)
        Me.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel"
        '
        'addUpdateTableLayoutPanel
        '
        resources.ApplyResources(Me.addUpdateTableLayoutPanel, "addUpdateTableLayoutPanel")
        Me.overarchingTableLayoutPanel.SetColumnSpan(Me.addUpdateTableLayoutPanel, 2)
        Me.addUpdateTableLayoutPanel.Controls.Add(Me.AddFolder, 0, 0)
        Me.addUpdateTableLayoutPanel.Controls.Add(Me.UpdateFolder, 1, 0)
        Me.addUpdateTableLayoutPanel.Name = "addUpdateTableLayoutPanel"
        '
        'ReferencePathsPropPage
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.overarchingTableLayoutPanel)
        Me.Name = "ReferencePathsPropPage"
        Me.overarchingTableLayoutPanel.ResumeLayout(false)
        Me.overarchingTableLayoutPanel.PerformLayout
        Me.addUpdateTableLayoutPanel.ResumeLayout(false)
        Me.addUpdateTableLayoutPanel.PerformLayout
        Me.ResumeLayout(false)

End Sub

    End Class

End Namespace
