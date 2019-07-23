Imports System.Runtime.Versioning
Imports System.Windows.Forms

Namespace Microsoft.VisualStudio.Editors.PropertyPages
    Friend NotInheritable Class MultiTargetingDialog
        Inherits PropPageUserControlBase

        'so that the event doesn't fire BeginInvoke before the window is fully created
        Private canHandleEvents As Boolean = False


        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            AddChecks()

            TFMSelector.View = View.Details
            TFMSelector.Columns.Add("Frameworks")
            TFMSelector.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            TFMSelector.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            TFMSelector.CheckBoxes = True
            TFMSelector.HeaderStyle = ColumnHeaderStyle.None
        End Sub

        Protected Overrides Sub PostInitPage()
            PopulateTargetFrameworks()
            canHandleEvents = True
        End Sub

        Private Sub AddChecks()

            For Each supportedFramework As TargetFrameworkMoniker In ApplicationPropPageInternalBase.SupportedFrameworks
                Dim item As ListViewItem = New ListViewItem()
                item.Text = supportedFramework.ToString
                item.Tag = supportedFramework
                TFMSelector.Items.Add(item)
            Next

        End Sub

        Private Delegate Sub ProcessItemCheck(ByRef listBoxObject As ListView)

        Private Sub ProcessItemCheckSub(ByRef listBoxObject As ListView)
            'SetTargetFrameworks()
            IsDirty = True
        End Sub

        Private Sub TFMSelector_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) Handles TFMSelector.ItemChecked
            'Once we implement checking the framework value, then we can implement only handling the event if it differs from what is in the project file 
            If canHandleEvents Then
                Dim Objects As Object() = {TFMSelector}
                BeginInvoke(New ProcessItemCheck(AddressOf ProcessItemCheckSub), Objects)
            End If
        End Sub

        ' I will need to define the exact behavior here eventually. For now, it will just assume that there is a TargetFrameworks property and only handle 
        ' Standard, Net, and NetCore. Also, 
        ' 
        Protected Sub PopulateTargetFrameworks()
            Dim currentTargetFrameworks As String = CStr(GetCommonPropertyValue(GetPropertyDescriptor("TargetFrameworks")))
            Dim frameworkList As String() = currentTargetFrameworks.Split(CType(";", Char()))
            For Each frameworkItem As ListViewItem In TFMSelector.Items
                'There is a much more elegant way of doing this 
                Dim selectedFrameworkMoniker As TargetFrameworkMoniker = CType(frameworkItem.Tag, TargetFrameworkMoniker)
                Dim found = False
                For Each currentFramework As String In frameworkList
                    Dim ProjectFileName As String = TFMToProjectFileName(selectedFrameworkMoniker)
                    If String.Equals(currentFramework, ProjectFileName) Then
                        found = True
                    End If
                Next
                If found Then
                    frameworkItem.Checked = True
                Else
                    frameworkItem.Checked = False
                End If
            Next
        End Sub

        Protected Overrides Sub OnApplyComplete(applySuccessful As Boolean)
            MyBase.OnApplyComplete(applySuccessful)
            SetTargetFrameworks()
        End Sub

        Private Sub SetTargetFrameworks()
            Dim TargetFrameworksString As String = ""

            If TFMSelector.CheckedItems.Count = 1 Then
                TargetFrameworksString = TFMToProjectFileName(CType(TFMSelector.CheckedItems.Item(0).Tag, TargetFrameworkMoniker))
                'TargetFrameworksString = TFMSelector.CheckedItems.Item(0).ToString
            Else
                For Each selectedFrameworkItem As ListViewItem In TFMSelector.CheckedItems
                    Dim selectedFrameworkMoniker As TargetFrameworkMoniker = CType(selectedFrameworkItem.Tag, TargetFrameworkMoniker)
                    TargetFrameworksString += TFMToProjectFileName(selectedFrameworkMoniker) + ";"
                Next

                TargetFrameworksString = TargetFrameworksString.TrimEnd(CChar(";"))

            End If

            SetCommonPropertyValue(GetPropertyDescriptor("TargetFrameworks"), TargetFrameworksString)
        End Sub
        Private Shared Function TFMToProjectFileName(moniker As TargetFrameworkMoniker) As String
            Dim frameworkName As New FrameworkName(moniker.Moniker)

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
