
Imports System.Windows.Forms
Imports System.Windows.Forms.Design
Imports Microsoft.VisualStudio.Utilities

Namespace Microsoft.VisualStudio.Editors.PropertyPages
    Friend NotInheritable Class MultiTargetingDialog
        Inherits Form

        Private _eventCommandLine As String
        Private _tokens() As String
        Private _values() As String
        Private _dte As EnvDTE.DTE
        Private _serviceProvider As IServiceProvider
        Private _page As PropPageUserControlBase
        Private _helpTopic As String

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

            'When we load the macros panel is hidden so don't show the Insert button
            SetInsertButtonState(False)

        End Sub

        Public Function SetFormTitleText(titleText As String) As Boolean
            Text = titleText
            Return True
        End Function

        Public Function SetTokensAndValues(tokens() As String, values() As String) As Boolean
            _tokens = tokens
            _values = values

            Return ParseAndPopulateTokens()
        End Function

        Public WriteOnly Property DTE() As EnvDTE.DTE
            Set(value As EnvDTE.DTE)
                _dte = value
            End Set
        End Property

        Public WriteOnly Property Page() As PropPageUserControlBase
            Set(value As PropPageUserControlBase)
                _page = value
            End Set
        End Property

        Public Property EventCommandLine() As String
            Get
                Return _eventCommandLine
            End Get
            Set(value As String)
                _eventCommandLine = value
            End Set
        End Property

        Public Property HelpTopic() As String
            Get
                If _helpTopic Is Nothing Then
                    If _page IsNot Nothing AndAlso _page.IsVBProject() Then
                        _helpTopic = HelpKeywords.VBProjPropBuildEventsBuilder
                    Else
                        _helpTopic = HelpKeywords.CSProjPropBuildEventsBuilder
                    End If
                End If

                Return _helpTopic
            End Get
            Set(value As String)
                _helpTopic = value
            End Set
        End Property

        Private Property ServiceProvider() As IServiceProvider
            Get
                If _serviceProvider Is Nothing AndAlso _dte IsNot Nothing Then
                    Dim isp As OLE.Interop.IServiceProvider = CType(_dte, OLE.Interop.IServiceProvider)
                    If isp IsNot Nothing Then
                        _serviceProvider = New Shell.ServiceProvider(isp)
                    End If
                End If
                Return _serviceProvider
            End Get
            Set(value As IServiceProvider)
                _serviceProvider = value
            End Set
        End Property

        Private Sub UpdateDialog_HelpButtonClicked(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.HelpButtonClicked
            InvokeHelp()
            e.Cancel = True
        End Sub

        Private Function ParseAndPopulateTokens() As Boolean
            '// Walk through the array and add each row to the listview
            Dim i As Integer
            Dim NameItem As ListViewItem

            For i = 0 To _tokens.Length - 1
                NameItem = New ListViewItem(_tokens(i))

                NameItem.SubItems.Add(_values(i))
            Next

            Return True
        End Function


        Private Sub BuildEventCommandLineDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            InitializeControlLocations()

            '// Never let them resize to something smaller than the default form size
            MinimumSize = Size
        End Sub

        Private Function InitializeControlLocations() As Boolean
            ShowCollapsedForm()
        End Function

        Private Shared Function ShowCollapsedForm() As Boolean


            '// Disable and hide the Insert button
            SetInsertButtonState(False)

            Return True
        End Function

        Private Function ShowExpandedForm() As Boolean


            '// Show the Insert button
            SetInsertButtonState(True)
            Return True
        End Function


        Private Sub InvokeHelp()
            If Not IsNothing(_page) Then
                _page.Help(HelpTopic)
            Else
                ' NOTE: the m_Page is nothing for deploy project, we need keep those code ...
                Try
                    Dim sp As IServiceProvider = ServiceProvider
                    If sp IsNot Nothing Then
                        Dim vshelp As VSHelp.Help = CType(sp.GetService(GetType(VSHelp.Help)), VSHelp.Help)
                        vshelp.DisplayTopicFromF1Keyword(HelpTopic)
                    Else
                        Debug.Fail("Can not find ServiceProvider")
                    End If

                Catch ex As Exception When Common.ReportWithoutCrash(ex, NameOf(InvokeHelp), NameOf(BuildEventCommandLineDialog))
                End Try
            End If
        End Sub

        Private Sub BuildEventCommandLineDialog_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles MyBase.HelpRequested
            InvokeHelp()
        End Sub

        Private Shared Function SetInsertButtonEnableState() As Boolean
            Return True
        End Function

        Private Shared Function SetInsertButtonState(bEnable As Boolean) As Boolean
            'Me.InsertButton.Enabled = bEnable
            SetInsertButtonEnableState()

            Return True
        End Function

        ''' <summary>
        ''' We shadow the original ShowDialog, because the right way to show dialog in VS is to use the IUIService. So the font/size will be set correctly.
        ''' The caller should pass a valid serviceProvider here. The dialog also hold it to invoke the help system
        ''' </summary>
        Public Shadows Function ShowDialog(sp As IServiceProvider) As DialogResult
            If sp IsNot Nothing Then
                ServiceProvider = sp
            End If
            Using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.SystemAware))
                If ServiceProvider IsNot Nothing Then
                    Dim uiService As IUIService = CType(ServiceProvider.GetService(GetType(IUIService)), IUIService)
                    If uiService IsNot Nothing Then
                        Return uiService.ShowDialog(Me)
                    End If
                End If
                Return MyBase.ShowDialog()
            End Using
        End Function

    End Class
End Namespace
