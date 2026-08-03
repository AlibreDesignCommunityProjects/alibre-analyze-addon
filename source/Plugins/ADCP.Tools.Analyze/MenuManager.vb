Imports AlibreAddOn
Imports AlibreX
Imports System.Windows.Forms

Namespace ADCP.Tools.Analyze

    ''' <summary>
    ''' Owns the Analyze menu tree and maps menu ids to <see cref="MenuItem"/> nodes.
    ''' The root menu exposes the add-on commands; each command opens
    ''' <see cref="AnalyzeDialog"/> against the active part session.
    ''' </summary>
    Public Class MenuManager

        Private Const RootMenuId As Integer = 9401
        Private Const RunCommandId As Integer = 9410

        Private ReadOnly _rootMenuItem As MenuItem
        Private ReadOnly _menuItems As Dictionary(Of Integer, MenuItem)

        Public Sub New()
            _rootMenuItem = New MenuItem(
                RootMenuId,
                "Analyze",
                "Analyze add-on commands",
                "logo.ico")
            _menuItems = New Dictionary(Of Integer, MenuItem)()
            BuildMenus()
        End Sub

        Private Sub BuildMenus()
            _rootMenuItem.AddSubItem(New MenuItem(
                RunCommandId,
                "Analyze Part",
                "Report all available data for the active part",
                "logo.ico",
                Function(session As IADSession) OpenDialog(session)))
            RegisterMenuItem(_rootMenuItem)
        End Sub

        Private Function OpenDialog(session As IADSession) As IAlibreAddOnCommand
            Dim partSession = TryCast(session, IADPartSession)
            If partSession Is Nothing Then
                MessageBox.Show(
                    "Please open a Part session before using Analyze.",
                    "Analyze",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return Nothing
            End If
            Using dlg As New AnalyzeDialog(partSession)
                dlg.ShowDialog()
            End Using
            Return Nothing
        End Function

        Private Sub RegisterMenuItem(menuItem As MenuItem)
            _menuItems(menuItem.Id) = menuItem
            For Each subItem In menuItem.SubItems
                RegisterMenuItem(subItem)
            Next
        End Sub

        Public Function GetMenuItemById(id As Integer) As MenuItem
            Return If(_menuItems.ContainsKey(id), _menuItems(id), Nothing)
        End Function

        Public Function GetRootMenuItem() As MenuItem
            Return _rootMenuItem
        End Function

    End Class

End Namespace
