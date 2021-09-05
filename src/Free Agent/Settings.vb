Imports System
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms

Public Class Settings
    Inherits System.Windows.Forms.Form

    Private ActionPanel As New Panel
    Private WithEvents DraftOrder As New DataGridView

    Private WithEvents SalaryCapLabel As New Label
    Private WithEvents SalaryCap As New TextBox

    Private ButtonPanel As New Panel
    Private WithEvents Randomize As New Button
    Private RandomizeToolTip As New ToolTip
    Private WithEvents Reload As New Button
    Private ReloadToolTip As New ToolTip
    Private WithEvents Draft As New Button
    Private DraftToolTip As New ToolTip

    <STAThreadAttribute()> Public Shared Sub Main()
        Application.EnableVisualStyles()
        Application.Run(New Settings())
    End Sub

    Private Sub Settings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Welcome.Visible = True

        EstablishContext()

        LoadLeagueSettings()
        LoadFanTeamList()
        LoadProPlayerList()
        LoadFanPlayerList()

        If BiddingActive Then
            Teams.Visible = True
            Me.Visible = False
        Else
            RandomizeFanTeamList()
            SetupLayout()
            PopulateDraftOrder()
            Me.SalaryCap.Text = LeagueSalaryCap
        End If
    End Sub

    Private Sub SetupLayout()
        Me.Size = New Size(600, 500)

        With Me.DraftOrder
            .Name = "DraftOrder"
            .Size = New System.Drawing.Size(500, 250)
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .BorderStyle = BorderStyle.None
            .GridColor = Me.ForeColor
            .BackgroundColor = Color.White
            .ReadOnly = True
            .TabStop = False
            .AutoSize = True
            .Height = 10
            .Width = 10

            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .RowHeadersVisible = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders

            .AllowDrop = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False

            .ColumnCount = 2

            .Columns(0).Name = ""
            .Columns(1).Name = "Draft Order"

            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single

            With .ColumnHeadersDefaultCellStyle
                .BackColor = Color.White
                .ForeColor = Me.ForeColor
                .Font = New Font(.Font, FontStyle.Bold)
            End With
        End With

        With Me.SalaryCapLabel
            .Font = LittleFont
            .Width = Me.Width / 2
            .Height = 100
            .Top = 250
            .Left = Me.Width / 4
            .TextAlign = HorizontalAlignment.Center
            .TextAlign = HorizontalAlignment.Center
            .Text = "Teams Salary Cap"
        End With

        With Me.SalaryCap
            '.BorderStyle = BorderStyle.None
            .Font = BigFont
            .Width = Me.Width / 2
            .Height = 200
            .Top = 350
            .Left = Me.Width / 4
            .TextAlign = HorizontalAlignment.Center
            .MaxLength = 10
            .Text = "100"
            .TabIndex = 1
        End With

        With Me.ActionPanel
            .Controls.Add(Me.DraftOrder)
            .Controls.Add(Me.SalaryCapLabel)
            .Controls.Add(Me.SalaryCap)
            .AutoSize = True
            .Dock = DockStyle.Fill
            .BackColor = Color.White
        End With

        With Me.Reload
            .Text = "&Reload"
            .Location = New Point(10, 10)
            .TabIndex = 2
        End With

        ReloadToolTip.SetToolTip(Reload, "Resets the draft order to the order read from the file")

        With Me.Randomize
            .Text = "&Random"
            .Location = New Point(100, 10)
            .TabIndex = 3
        End With

        RandomizeToolTip.SetToolTip(Randomize, "Randomizes the order Teams will draft")

        With Me.Draft
            .Text = "&Draft"
            .Location = New Point((Me.Width - .Width) - 10, 10)
            .TabIndex = 4
        End With

        DraftToolTip.SetToolTip(Draft, "Begin the process of selecting players")

        With Me.ButtonPanel
            .Controls.Add(Me.Reload)
            .Controls.Add(Me.Randomize)
            .Controls.Add(Me.Draft)
            .AutoSize = True
            .Dock = DockStyle.Bottom
        End With

        With Me
            .Controls.Add(.ButtonPanel)
            .Controls.Add(.ActionPanel)
        End With

        Me.SalaryCap.Focus()
    End Sub

    Private Sub PopulateDraftOrder()
        Dim row As Integer

        Dim FanTeamEntry As String() = {"", ""}

        Dim team As String() = {"", ""}

        With Me.DraftOrder
            For row = 0 To .Rows.Count - 1
                .Rows.RemoveAt(0)
            Next

            For row = 0 To FanTeamList.Count - 1
                FanTeamEntry = FanTeamList.Item(row)

                team(0) = Trim(Str(row + 1))
                team(1) = FanTeamEntry(0)

                .Rows.Add(team)
                .Rows.Item(row).DefaultCellStyle.BackColor = Me.BackColor
            Next

            .Columns(0).DisplayIndex = 0
            .Columns(1).DisplayIndex = 1

            .RowsDefaultCellStyle.SelectionForeColor = Me.ForeColor
            .RowsDefaultCellStyle.SelectionBackColor = Me.BackColor
            .RowsDefaultCellStyle.Font = New Font(.Font.FontFamily, 10, FontStyle.Regular)

            .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
            .AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders)
        End With
    End Sub

    Private Sub Reload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Reload.Click
        LoadFanTeamList()
        PopulateDraftOrder()
    End Sub

    Private Sub Reload_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Reload.KeyUp
        SalaryCap.Text = Trim(Str(Val(SalaryCap.Text)))
    End Sub

    Private Sub Randomize_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Randomize.Click
        RandomizeFanTeamList()
        PopulateDraftOrder()
    End Sub

    Private Sub Draft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Draft.Click
        BiddingActive = True
        LeagueSalaryCap = SalaryCap.Text

        SaveLeagueSettings()
        SaveFanTeamList()

        Teams.Visible = True
        Me.Visible = False
    End Sub
End Class