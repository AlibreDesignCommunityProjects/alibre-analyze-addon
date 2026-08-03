Imports AlibreX
Imports System.Text
Imports System.Windows.Forms

Namespace ADCP.Tools.Analyze

    ''' <summary>
    ''' Read-only report dialog for the Analyze add-on. Collects part data via
    ''' <see cref="AnalyzeBuilder"/> and shows one grid tab per section, with a
    ''' Copy All button that puts the full plain-text report on the clipboard.
    ''' </summary>
    Public Class AnalyzeDialog
        Inherits Form

        Private ReadOnly _session As IADPartSession
        Private ReadOnly _sections As IReadOnlyList(Of ReportSection)
        Private WithEvents _btnCopy As Button
        Private WithEvents _btnClose As Button
        Private _tabs As TabControl

        Public Sub New(session As IADPartSession)
            _session = session
            _sections = New AnalyzeBuilder(session).BuildReport()
            InitialiseComponent()
        End Sub

        Private Sub InitialiseComponent()
            Text = "Analyze Part"
            FormBorderStyle = FormBorderStyle.Sizable
            StartPosition = FormStartPosition.CenterParent
            MinimizeBox = False
            ClientSize = New Drawing.Size(560, 440)
            MinimumSize = New Drawing.Size(420, 300)
            _tabs = New TabControl() With {.Dock = DockStyle.Fill}
            For Each section In _sections
                _tabs.TabPages.Add(BuildTab(section))
            Next
            Dim buttonPanel As New Panel() With {.Dock = DockStyle.Bottom, .Height = 40}
            _btnClose = New Button() With {.Text = "Close", .Anchor = AnchorStyles.Right Or AnchorStyles.Top, .Width = 90, .Top = 8, .Left = 460, .DialogResult = DialogResult.OK}
            _btnCopy = New Button() With {.Text = "Copy All", .Anchor = AnchorStyles.Right Or AnchorStyles.Top, .Width = 90, .Top = 8, .Left = 362}
            buttonPanel.Controls.AddRange({_btnCopy, _btnClose})
            Controls.Add(_tabs)
            Controls.Add(buttonPanel)
            AcceptButton = _btnClose
            CancelButton = _btnClose
        End Sub

        Private Shared Function BuildTab(section As ReportSection) As TabPage
            Dim page As New TabPage(section.Title)
            Dim list As New ListView() With {
                .Dock = DockStyle.Fill,
                .View = View.Details,
                .FullRowSelect = True,
                .GridLines = True,
                .HeaderStyle = ColumnHeaderStyle.Nonclickable}
            list.Columns.Add("Property", 200)
            list.Columns.Add("Value", 320)
            For Each row In section.Rows
                Dim item As New ListViewItem(row.Key)
                item.SubItems.Add(row.Value)
                list.Items.Add(item)
            Next
            page.Controls.Add(list)
            Return page
        End Function

        Private Sub OnCopy(sender As Object, e As EventArgs) Handles _btnCopy.Click
            Dim sb As New StringBuilder()
            For Each section In _sections
                sb.AppendLine(section.ToPlainText())
            Next
            Try
                Clipboard.SetText(sb.ToString())
            Catch ex As Exception
                MessageBox.Show("Copy failed:" & Environment.NewLine & ex.Message,
                                "Analyze", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub

    End Class

End Namespace
