Imports System.Runtime.Versioning
Imports System.Windows.Forms

Namespace Microsoft.VisualStudio.Editors.PropertyPages
    Friend NotInheritable Class MultiTargetingDialog
        Inherits PropPageUserControlBase

        'so that the event doesn't fire BeginInvoke before the window is fully created
        Private canHandleEvents As Boolean = False
        'Starting Hash for empty
        Private currentTFMsHash As Integer = -1
        Private BeginningTFMsHash As Integer = -1

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            InitTFMs()

            TFMSelector.View = View.Details
            TFMSelector.Columns.Add("Frameworks")
            TFMSelector.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            TFMSelector.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            TFMSelector.CheckBoxes = False
            TFMSelector.HeaderStyle = ColumnHeaderStyle.None

            CurrentTFMs.View = View.Details
            CurrentTFMs.Columns.Add("Frameworks")
            CurrentTFMs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            CurrentTFMs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            CurrentTFMs.CheckBoxes = False
            CurrentTFMs.HeaderStyle = ColumnHeaderStyle.None
        End Sub

        Protected Overrides Sub PostInitPage()
            PopulateTargetFrameworks()
            canHandleEvents = True
        End Sub

        Private Sub InitTFMs()
            For Each supportedFramework As TargetFrameworkMoniker In ApplicationPropPageInternalBase.SupportedFrameworks
                Dim item As ListViewItem = New ListViewItem()
                item.Text = supportedFramework.ToString
                item.Tag = New FrameworkName(supportedFramework.Moniker)
                TFMSelector.Items.Add(item)
                'Move into the correct group
                If String.Equals(CType(item.Tag, FrameworkName).Identifier, ".NETFramework") Then
                    item.Group = TFMSelector.Groups.Item(1)
                ElseIf String.Equals(CType(item.Tag, FrameworkName).Identifier, ".NETCoreApp") Then
                    item.Group = TFMSelector.Groups.Item(2)
                ElseIf String.Equals(CType(item.Tag, FrameworkName).Identifier, ".NETStandard") Then
                    item.Group = TFMSelector.Groups.Item(3)
                End If
            Next
        End Sub

        Private Delegate Sub ProcessItemCheck(ByRef listBoxObject As ListView)

        Private Sub ProcessItemMovedSub(ByRef listBoxObject As ListView)
            IsDirty = True
        End Sub

        Private Sub TFMSelector_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) Handles TFMSelector.ItemChecked
            'Once we implement checking the framework value, then we can implement only handling the event if it differs from what is in the project file 
            If canHandleEvents Then
                Dim Objects As Object() = {TFMSelector}
                BeginInvoke(New ProcessItemCheck(AddressOf ProcessItemMovedSub), Objects)
            End If
        End Sub

        Private Sub RightButton_Clicked(sender As Object, e As EventArgs) Handles moveRightButton.Click
            Dim copyOfItems = TFMSelector.SelectedItems
            For Each item As ListViewItem In copyOfItems
                MoveFrameworkItem(item, True)
            Next
            currentTFMsHash = TFMHash(CurrentTFMs.Items)
        End Sub

        Private Sub LeftButton_Clicked(sender As Object, e As EventArgs) Handles moveLeftButton.Click
            Dim copyOfItems = CurrentTFMs.SelectedItems
            For Each item As ListViewItem In copyOfItems
                MoveFrameworkItem(item, False)
            Next
            currentTFMsHash = TFMHash(CurrentTFMs.Items)
        End Sub

        Private Sub UpButton_Clicked(sender As Object, e As EventArgs) Handles moveUpButton.Click
            Dim copyOfItems = CurrentTFMs.SelectedItems
            If copyOfItems.Count <> 0 Then
                Dim item = copyOfItems.Item(0)
                Dim itemPreviousLocation = item.Index
                If itemPreviousLocation <> 0 Then
                    CurrentTFMs.Items.Remove(item)
                    CurrentTFMs.Items.Insert(itemPreviousLocation - 1, item)
                End If
            End If
            currentTFMsHash = TFMHash(CurrentTFMs.Items)
            IsDirty = True
        End Sub

        Private Sub DownButton_Clicked(sender As Object, e As EventArgs) Handles moveDownButton.Click
            Dim copyOfItems = CurrentTFMs.SelectedItems
            If copyOfItems.Count <> 0 Then
                Dim item = copyOfItems.Item(0)
                Dim itemPreviousLocation = item.Index
                If itemPreviousLocation <> CurrentTFMs.Items.Count - 1 Then
                    CurrentTFMs.Items.Remove(item)
                    CurrentTFMs.Items.Insert(itemPreviousLocation + 1, item)
                End If
            End If
            currentTFMsHash = TFMHash(CurrentTFMs.Items)
            IsDirty = True
        End Sub

        Private Sub MoveTFMToCorrectLocation(item As ListViewItem)
            'Determine the Group First
            Dim GroupNumber = -1
            If String.Equals(CType(item.Tag, FrameworkName).Identifier, ".NETFramework") Then
                GroupNumber = 0
            ElseIf String.Equals(CType(item.Tag, FrameworkName).Identifier, ".NETCoreApp") Then
                GroupNumber = 1
            ElseIf String.Equals(CType(item.Tag, FrameworkName).Identifier, ".NETStandard") Then
                GroupNumber = 2
            End If

            item.Group = TFMSelector.Groups.Item(GroupNumber)
            Dim itemFrameworkName = CType(item.Tag, FrameworkName)
            Dim locationFound = False
            Dim LastLocation2 = 0

            For Each tfm As ListViewItem In TFMSelector.Items
                If CType(tfm.Tag, FrameworkName).Identifier = itemFrameworkName.Identifier Then
                    If CType(tfm.Tag, FrameworkName).Version > itemFrameworkName.Version Then
                        TFMSelector.Items.Insert(TFMSelector.Items.IndexOf(tfm), item)
                        locationFound = True
                        Exit For
                    End If
                    LastLocation2 = TFMSelector.Items.IndexOf(tfm)
                End If
                'LastLocation2 += 1

            Next
            'It must have been higher than all them, so just add it
            If Not locationFound Then
                TFMSelector.Items.Insert(LastLocation2 + 1, item)
            End If
        End Sub

        Private Sub MoveFrameworkItem(item As ListViewItem, toRight As Boolean)
            If toRight Then
                TFMSelector.Items.Remove(item)
                CurrentTFMs.Items.Add(item)
            Else
                CurrentTFMs.Items.Remove(item)
                'This isn't working correctly at the moment either. I add them in at the right location, but they still display them out of order.
                'The fix is to just clear the list and re-add them propbably. Dumb, but I can add it back if I need to. 
                'MoveTFMToCorrectLocation(item)
                item.Group = TFMSelector.Groups.Item(0)
                TFMSelector.Items.Add(item)
                'Insert the item back into the correct location in the list
                'This isn't working correctly, won't add back in items that are the highest in their framework. e.g. 4.7.2 will never be added.
                'For Each tfm As ListViewItem In TFMSelector.Items
                '    Dim tfmMoniker As TargetFrameworkMoniker = CType(item.Tag, TargetFrameworkMoniker)
                '    Dim nextMoniker As TargetFrameworkMoniker = CType(TFMSelector.Items.Item(tfm.Index).Tag, TargetFrameworkMoniker)
                '    Dim frameworkName1 As New FrameworkName(tfmMoniker.Moniker)
                '    Dim frameworkName2 As New FrameworkName(nextMoniker.Moniker)
                '    If (frameworkName1.Identifier = frameworkName2.Identifier AndAlso frameworkName2.Version > frameworkName1.Version) Then
                '        TFMSelector.Items.Insert(tfm.Index, item)
                '        Exit For
                '    End If
                'Next
            End If
            IsDirty = True
        End Sub


        Private Function TFMHash(TFMCollection As ListView.ListViewItemCollection) As Integer
            Dim hash As Integer = 5454
            For Each item As ListViewItem In TFMCollection
                hash = hash * item.GetHashCode + item.GetHashCode
            Next
            Return hash
        End Function

        ' I will need to define the exact behavior here eventually. For now, it will just assume that there is a TargetFrameworks property and only handle 
        ' Standard, Net, and NetCore. Also, 
        ' 
        Protected Sub PopulateTargetFrameworks()
            Dim currentTargetFrameworks As String = CStr(GetCommonPropertyValue(GetPropertyDescriptor("TargetFrameworks")))
            Dim frameworkList As String() = currentTargetFrameworks.Split(CType(";", Char()))

            For Each frameworkItem As ListViewItem In TFMSelector.Items
                Dim found = False
                For Each currentFramework As String In frameworkList
                    Dim ProjectFileName As String = TFMToProjectFileName(CType(frameworkItem.Tag, FrameworkName))
                    If String.Equals(currentFramework, ProjectFileName) Then
                        found = True
                    End If
                Next
                If found Then
                    MoveFrameworkItem(frameworkItem, True)
                End If
            Next
            BeginningTFMsHash = TFMHash(CurrentTFMs.Items)
        End Sub

        Protected Overrides Sub OnApplyComplete(applySuccessful As Boolean)
            MyBase.OnApplyComplete(applySuccessful)

            If currentTFMsHash <> BeginningTFMsHash Then
                SetTargetFrameworks()
            End If
        End Sub

        Private Sub SetTargetFrameworks()
            Dim TargetFrameworksString As String = ""

            If CurrentTFMs.Items.Count = 1 Then
                TargetFrameworksString = TFMToProjectFileName(CType(CurrentTFMs.Items.Item(0).Tag, FrameworkName))
                'TargetFrameworksString = TFMSelector.CheckedItems.Item(0).ToString
            Else
                For Each selectedFrameworkItem As ListViewItem In CurrentTFMs.Items
                    TargetFrameworksString += TFMToProjectFileName(CType(selectedFrameworkItem.Tag, FrameworkName)) + ";"
                Next

                TargetFrameworksString = TargetFrameworksString.TrimEnd(CChar(";"))

            End If

            SetCommonPropertyValue(GetPropertyDescriptor("TargetFrameworks"), TargetFrameworksString)
        End Sub
        Private Shared Function TFMToProjectFileName(frameworkName As FrameworkName) As String

            Dim isNetCoreApp = String.Compare(frameworkName.Identifier, ".NETCoreApp", StringComparison.Ordinal) = 0
            Dim isNetStandard = String.Compare(frameworkName.Identifier, ".NETStandard", StringComparison.Ordinal) = 0
            Dim isNetFramework = String.Compare(frameworkName.Identifier, ".NETFramework", StringComparison.Ordinal) = 0

            If isNetCoreApp Then
                Return "netcoreapp" + frameworkName.Version.ToString
            ElseIf isNetStandard Then
                Return "netstandard" + frameworkName.Version.ToString
            ElseIf isNetFramework Then
                Return "net" + frameworkName.Version.ToString.Replace(".", "")
            End If

            'Some weird TFM
            Return Nothing
        End Function

    End Class
End Namespace
