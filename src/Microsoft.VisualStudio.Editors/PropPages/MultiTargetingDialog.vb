Imports System.Runtime.Versioning
Imports System.Windows.Forms

Namespace Microsoft.VisualStudio.Editors.PropertyPages
    Friend NotInheritable Class MultiTargetingDialog
        Inherits PropPageUserControlBase

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            AddChecks()
        End Sub

        Private Sub AddChecks()

            For Each supportedFramework As TargetFrameworkMoniker In ApplicationPropPageInternalBase.SupportedFrameworks
                TFMSelector.Items.Add(supportedFramework)
            Next

        End Sub

        Private Delegate Sub ProcessItemCheck(ByRef listBoxObject As CheckedListBox)

        Private Sub ProcessItemCheckSub(ByRef listBoxObject As CheckedListBox)
            'SetTargetFrameworks()
            Me.IsDirty = True
        End Sub

        Private Sub CheckedListBox1_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs) Handles TFMSelector.ItemCheck
            Dim Objects As Object() = {TFMSelector}
            BeginInvoke(New ProcessItemCheck(AddressOf ProcessItemCheckSub), Objects)
        End Sub

        Protected Overrides Sub OnApplyComplete(applySuccessful As Boolean)
            MyBase.OnApplyComplete(applySuccessful)
            SetTargetFrameworks()
        End Sub

        Private Sub SetTargetFrameworks()
            Dim TargetFrameworksString As String = ""

            If TFMSelector.CheckedItems.Count = 1 Then
                TargetFrameworksString = TFMToProjectFileName(CType(TFMSelector.CheckedItems.Item(0), TargetFrameworkMoniker))
            Else
                For Each selectedFramework As TargetFrameworkMoniker In TFMSelector.CheckedItems
                    TargetFrameworksString += TFMToProjectFileName(selectedFramework) + ";"
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
