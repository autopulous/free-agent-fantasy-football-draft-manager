Public Class Players
    Private WithEvents FilterProPosition As New System.Windows.Forms.CheckedListBox
    Private WithEvents FilterProTeam As New System.Windows.Forms.CheckedListBox
    Private WithEvents FilterFanTeam As New System.Windows.Forms.CheckedListBox
    Private WithEvents FanTeamUncheck As New System.Windows.Forms.Button
    Private WithEvents FanTeamCheck As New System.Windows.Forms.Button
    Private WithEvents ProPositionCheck As New System.Windows.Forms.Button
    Private WithEvents ProPositionUncheck As New System.Windows.Forms.Button
    Private WithEvents ProTeamCheck As New System.Windows.Forms.Button
    Private WithEvents ProTeamUncheck As New System.Windows.Forms.Button

    Private FilterProPositionCheckToolTip As New ToolTip
    Private FilterProPositionUncheckToolTip As New ToolTip
    Private FilterProTeamCheckToolTip As New ToolTip
    Private FilterProTeamUncheckToolTip As New ToolTip
    Private FilterFanTeamCheckToolTip As New ToolTip
    Private FilterFanTeamUncheckToolTip As New ToolTip

    Private WithEvents GroupBox1 As New System.Windows.Forms.GroupBox

    Public WithEvents teamOnTheClock As New Label

    Private WithEvents ViewProPlayer As New System.Windows.Forms.DataGridView
    Private WithEvents ProPlayer As New System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents Position As New System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents ProTeam As New System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents Points As New System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents Salary As New System.Windows.Forms.DataGridViewTextBoxColumn
    Private WithEvents FanTeam As New System.Windows.Forms.DataGridViewTextBoxColumn

    Private Sub Players_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Debug("Entered Players.Players_FormClosed")

        Teams.Close()

        Debug("Exited Players.Players_FormClosed")
    End Sub

    Private Sub Players_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Debug("Entered Players.Players_Load")

        Me.Visible = False

        SetupLayout()

        PopulateFilterProPosition()
        PopulateFilterFanTeam()
        PopulateFilterProTeam()

        FormActivate()

        ViewProPlayer.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders)

        Debug("Exited Players.Players_Load")
    End Sub

    Public Sub FormActivate()
        Debug("Entered Players.FormActivate")

        Me.Visible = False

        If Not BiddingActive Then
            teamOnTheClock.Text = "Draft Completed"
            ViewProPlayer.RowsDefaultCellStyle.SelectionBackColor = Color.White
        Else
            teamOnTheClock.Text = FanTeamName
        End If

        PopulateViewProPlayer()

        Me.Visible = True
        Teams.Visible = False

        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)

        Debug("Exited Players.FormActivate")
    End Sub

    Private Sub SetupLayout()
        Debug("Entered Players.SetupLayout")

        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Players))

        With Me.FilterProPosition
            .FormattingEnabled = True
            .Location = New System.Drawing.Point(25, 30)
            .Name = "FilterProPosition"
            .Size = New System.Drawing.Size(75, 125)
            .CheckOnClick = True
            .Sorted = True
            .TabStop = False
        End With

        With Me.ProPositionCheck
            .Image = Global.Free_Agent.My.Resources.Resources.ball_green
            .Location = New System.Drawing.Point(100, 30)
            .Name = "ProPositionCheck"
            .Size = New System.Drawing.Size(22, 22)
            .TabStop = False
            .UseVisualStyleBackColor = True
        End With

        FilterProPositionCheckToolTip.SetToolTip(Me.ProPositionCheck, "Check All")

        With Me.ProPositionUncheck
            .Image = Global.Free_Agent.My.Resources.Resources.ball_red
            .Location = New System.Drawing.Point(100, 53)
            .Name = "ProPositionUncheck"
            .Size = New System.Drawing.Size(22, 22)
            .TabStop = False
            .UseVisualStyleBackColor = True
        End With

        FilterProPositionUncheckToolTip.SetToolTip(Me.ProPositionUncheck, "Uncheck All")

        With Me.FilterProTeam
            .FormattingEnabled = True
            .Location = New System.Drawing.Point(150, 30)
            .Name = "FilterProTeam"
            .Size = New System.Drawing.Size(75, 125)
            .CheckOnClick = True
            .Sorted = True
            .TabStop = False
        End With

        With Me.ProTeamCheck
            .Image = Global.Free_Agent.My.Resources.Resources.ball_green
            .Location = New System.Drawing.Point(225, 30)
            .Name = "ProTeamCheck"
            .Size = New System.Drawing.Size(22, 22)
            .TabStop = False
            .UseVisualStyleBackColor = True
        End With

        FilterProTeamCheckToolTip.SetToolTip(Me.ProTeamCheck, "Check All")

        With Me.ProTeamUncheck
            .Image = Global.Free_Agent.My.Resources.Resources.ball_red
            .Location = New System.Drawing.Point(225, 53)
            .Name = "ProTeamUncheck"
            .Size = New System.Drawing.Size(22, 22)
            .TabStop = False
            .UseVisualStyleBackColor = True
        End With

        FilterProTeamUncheckToolTip.SetToolTip(Me.ProTeamUncheck, "Uncheck All")

        With Me.FilterFanTeam
            .FormattingEnabled = True
            .Location = New System.Drawing.Point(275, 30)
            .Name = "FilterFanTeam"
            .Size = New System.Drawing.Size(175, 125)
            .CheckOnClick = True
            .Sorted = True
            .TabStop = False
        End With

        With Me.FanTeamCheck
            .Image = Global.Free_Agent.My.Resources.Resources.ball_green
            .Location = New System.Drawing.Point(450, 30)
            .Name = "FanTeamCheck"
            .Size = New System.Drawing.Size(22, 22)
            .TabStop = False
            .UseVisualStyleBackColor = True
        End With

        FilterFanTeamCheckToolTip.SetToolTip(Me.FanTeamCheck, "Check All")

        With Me.FanTeamUncheck
            .Image = Global.Free_Agent.My.Resources.Resources.ball_red
            .Location = New System.Drawing.Point(450, 53)
            .Name = "FanTeamUncheck"
            .Size = New System.Drawing.Size(22, 22)
            .TabStop = False
            .UseVisualStyleBackColor = True
        End With

        FilterFanTeamUncheckToolTip.SetToolTip(Me.FanTeamUncheck, "Uncheck All")

        With Me.GroupBox1
            .Location = New System.Drawing.Point(10, 10)
            .Name = "GroupBox1"
            .Size = New System.Drawing.Size(750, 175)
            .TabStop = False
            .Text = "Filters"
            .Controls.Add(Me.FilterProPosition)
            .Controls.Add(Me.ProPositionCheck)
            .Controls.Add(Me.ProPositionUncheck)
            .Controls.Add(Me.FilterProTeam)
            .Controls.Add(Me.ProTeamCheck)
            .Controls.Add(Me.ProTeamUncheck)
            .Controls.Add(Me.FilterFanTeam)
            .Controls.Add(Me.FanTeamCheck)
            .Controls.Add(Me.FanTeamUncheck)
        End With

        With Me.teamOnTheClock
            .Width = GroupBox1.Width
            .Height = 50
            .Font = LittleFont
            .Location = New Point(GroupBox1.Left, Me.GroupBox1.Top + Me.GroupBox1.Height)
            .TextAlign = ContentAlignment.BottomCenter
            .TabStop = False
        End With

        With Me.ProPlayer
            .Frozen = True
            .HeaderText = "Pro Player"
            .Name = "ProPlayer"
            .ReadOnly = True
        End With

        With Me.Position
            .Frozen = True
            .HeaderText = "Postion"
            .Name = "Position"
            .ReadOnly = True
        End With

        With Me.ProTeam
            .Frozen = True
            .HeaderText = "Pro Teams"
            .Name = "ProTeam"
            .ReadOnly = True
        End With

        With Me.Points
            .Frozen = True
            .HeaderText = "Points"
            .Name = "Points"
            .ReadOnly = True
        End With

        With Me.Salary
            .Frozen = True
            .HeaderText = "Salary"
            .Name = "Salary"
            .ReadOnly = True
        End With

        With Me.FanTeam
            .Frozen = True
            .HeaderText = "Fan Teams"
            .Name = "FanTeam"
            .ReadOnly = True
        End With

        With Me.ViewProPlayer
            .Name = "ViewProPlayer"
            .Location = New System.Drawing.Point(10, 245)
            .Size = New System.Drawing.Size(750, 450)
            .GridColor = Color.Black
            .ReadOnly = True
            .TabStop = False

            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .AllowDrop = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False

            .RowHeadersVisible = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders

            .RowsDefaultCellStyle.SelectionForeColor = Color.Black
            .RowsDefaultCellStyle.SelectionBackColor = Color.GreenYellow
            .RowsDefaultCellStyle.Font = New Font(.Font.FontFamily, 10, FontStyle.Regular)

            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
            .ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            .ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font(Me.ViewProPlayer.Font, FontStyle.Bold)

            .Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ProPlayer, Me.Position, Me.ProTeam, Me.Points, Me.Salary, Me.FanTeam})
        End With

        With Me
            .AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            .AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            .ClientSize = New System.Drawing.Size(755, 516)
            .Controls.Add(Me.GroupBox1)
            .Controls.Add(Me.teamOnTheClock)
            .Controls.Add(Me.ViewProPlayer)
            .Name = "Players"
            .Text = "Players"
            .WindowState = System.Windows.Forms.FormWindowState.Maximized
        End With

        Debug("Exited Players.SetupLayout")
    End Sub

    Private Sub PopulateFilterProPosition()
        Debug("Entered Players.PopulateFilterProPosition")

        Dim row As Integer

        Dim ProPositionEntry As String

        With Me.FilterProPosition
            .BeginUpdate()

            For row = 0 To ProPositionList.Count - 1
                ProPositionEntry = ProPositionList.Item(row)
                .Items.Add(ProPositionEntry)
            Next

            .EndUpdate()
        End With

        Debug("Exited Players.PopulateFilterProPosition")
    End Sub

    Private Sub PopulateFilterProTeam()
        Debug("Entered Players.PopulateFilterProTeam")

        Dim row As Integer

        Dim ProTeamEntry As String

        With Me.FilterProTeam
            .BeginUpdate()

            For row = 0 To ProTeamList.Count - 1
                ProTeamEntry = ProTeamList.Item(row)
                .Items.Add(ProTeamEntry)
            Next

            .EndUpdate()
        End With

        Debug("Exited Players.PopulateFilterProTeam")
    End Sub

    Private Sub PopulateFilterFanTeam()
        Debug("Entered Players.PopulateFilterFanTeam")

        Dim row As Integer

        Dim FanTeamEntry As String() = {"", "0"}

        With Me.FilterFanTeam
            .BeginUpdate()

            .Items.Add("")

            For row = 0 To FanTeamList.Count - 1
                FanTeamEntry = FanTeamList.Item(row)
                .Items.Add(FanTeamEntry(0))
            Next

            .EndUpdate()
        End With

        Debug("Exited Players.PopulateFilterFanTeam")
    End Sub

    Private Sub PopulateViewProPlayer()
        Debug("Entered Players.PopulateViewProPlayer")

        Dim row, item As Integer

        Dim ProPlayerEntry As String() = {"", "", "", "", "", ""}

        With Me.ViewProPlayer

            If 0 = .ColumnCount Then
                Debug("Exited Players.PopulateViewProPlayer")
                Exit Sub
            End If

            For row = 0 To .Rows.Count - 1
                .Rows.RemoveAt(0)
            Next

            For row = 0 To ProPlayerList.Count - 1

                ProPlayerEntry = ProPlayerList.Item(row)

                If FilterProPosition.CheckedItems.Count > 0 Then
                    For item = 0 To FilterProPosition.CheckedItems.Count - 1
                        If 0 <> InStr(ProPlayerEntry(1), FilterProPosition.CheckedItems.Item(item).ToString) Then Exit For
                    Next

                    If item = FilterProPosition.CheckedItems.Count Then Continue For
                End If

                If FilterProTeam.CheckedItems.Count > 0 Then
                    For item = 0 To FilterProTeam.CheckedItems.Count - 1
                        If ProPlayerEntry(2) = FilterProTeam.CheckedItems.Item(item).ToString Then Exit For
                    Next

                    If item = FilterProTeam.CheckedItems.Count Then Continue For
                End If

                If FilterFanTeam.Enabled And FilterFanTeam.CheckedItems.Count > 0 Then
                    For item = 0 To FilterFanTeam.CheckedItems.Count - 1
                        If ProPlayerEntry(5) = FilterFanTeam.CheckedItems.Item(item).ToString Then Exit For
                    Next

                    If item = FilterFanTeam.CheckedItems.Count Then Continue For
                End If

                .Rows.Add(ProPlayerEntry)
                .Rows.Item(.Rows.Count - 1).DefaultCellStyle.BackColor = Color.White
                .Rows.Item(.Rows.Count - 1).Tag = row
            Next
        End With

        Debug("Exited Players.PopulateViewProPlayer")
    End Sub

    Private Sub ViewProPlayer_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewProPlayer.DoubleClick
        Debug("Entered Players.ViewProPlayer_DoubleClick")

        If BiddingActive Then
            PutPlayerOnTheBlock()
        End If

        Debug("Exited Players.ViewProPlayer_DoubleClick")
    End Sub

    Private Sub ViewProPlayer_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ViewProPlayer.KeyDown
        Debug("Entered Players.ViewProPlayer_Keydown")

        If e.KeyCode = Keys.Enter Then
            PutPlayerOnTheBlock()
            e.Handled = True
        End If

        Debug("Exited Players.ViewProPlayer_Keydown")
    End Sub

    Private Sub PutPlayerOnTheBlock()
        Debug("Entered Players.PutPlayerOnTheBlock")

        'is this player already signed?

        If "" <> ViewProPlayer.CurrentRow.Cells.Item(4).Value Then
            Debug("Exited Players.PutPlayerOnTheBlock")
            Exit Sub
        End If

        Teams.playerOnTheBlock.Text = ViewProPlayer.CurrentRow.Cells.Item(0).Value
        Teams.playerOnTheBlockPosition.Text = ViewProPlayer.CurrentRow.Cells.Item(1).Value & " - " & ViewProPlayer.CurrentRow.Cells.Item(2).Value
        Teams.playerOnTheBlockPoints.Text = "Points: " & ViewProPlayer.CurrentRow.Cells.Item(3).Value
        Teams.playerOnTheBlockRow = ViewProPlayer.CurrentRow.Tag

        Teams.FormActivate()
        Me.Visible = False

        Debug("Exited Players.PutPlayerOnTheBlock")
    End Sub

    Private Sub ViewProPlayer_SortCompare(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewSortCompareEventArgs) Handles ViewProPlayer.SortCompare
        'Sort points or salary column (3, 4) numerically
        If 3 = e.Column.Index Or 4 = e.Column.Index Then
            e.SortResult = Val(e.CellValue1.ToString()) - Val(e.CellValue2.ToString())
            e.Handled = True
        End If
    End Sub

    Private Sub FilterProPosition_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles FilterProPosition.ItemCheck
        Debug("Entered Players.FilterProPosition_ItemCheck")

        RemoveHandler FilterProPosition.ItemCheck, AddressOf FilterProPosition_ItemCheck

        If e.CurrentValue = CheckState.Checked Then
            FilterProPosition.SetItemCheckState(e.Index, CheckState.Unchecked)
        Else
            FilterProPosition.SetItemCheckState(e.Index, CheckState.Checked)
        End If

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterProPosition.ItemCheck, AddressOf FilterProPosition_ItemCheck

        Debug("Exited Players.FilterProPosition_ItemCheck")
    End Sub

    Private Sub ProPositionCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProPositionCheck.Click
        Debug("Entered Players.ProPositionCheck_Click")

        RemoveHandler FilterProPosition.ItemCheck, AddressOf FilterProPosition_ItemCheck

        Dim item As Integer

        For item = 0 To FilterProPosition.Items.Count - 1
            FilterProPosition.SetItemCheckState(item, CheckState.Checked)
        Next

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterProPosition.ItemCheck, AddressOf FilterProPosition_ItemCheck

        Debug("Exited Players.ProPositionCheck_Click")
    End Sub

    Private Sub ProPositionUncheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProPositionUncheck.Click
        Debug("Entered Players.ProPositionUncheck_Click")

        RemoveHandler FilterProPosition.ItemCheck, AddressOf FilterProPosition_ItemCheck

        Dim item As Integer

        For item = 0 To FilterProPosition.Items.Count - 1
            FilterProPosition.SetItemCheckState(item, CheckState.Unchecked)
        Next

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterProPosition.ItemCheck, AddressOf FilterProPosition_ItemCheck

        Debug("Exited Players.ProPositionUncheck_Click")
    End Sub

    Private Sub FilterProTeam_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles FilterProTeam.ItemCheck
        Debug("Entered Players.FilterProTeam_ItemCheck")

        RemoveHandler FilterProTeam.ItemCheck, AddressOf FilterProTeam_ItemCheck

        If e.CurrentValue = CheckState.Checked Then
            FilterProTeam.SetItemCheckState(e.Index, CheckState.Unchecked)
        Else
            FilterProTeam.SetItemCheckState(e.Index, CheckState.Checked)
        End If

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(2), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterProTeam.ItemCheck, AddressOf FilterProTeam_ItemCheck

        Debug("Exited Players.FilterProTeam_ItemCheck")
    End Sub

    Private Sub ProTeamCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProTeamCheck.Click
        Debug("Entered Players.ProTeamCheck_Click")

        RemoveHandler FilterProTeam.ItemCheck, AddressOf FilterProTeam_ItemCheck

        Dim item As Integer

        For item = 0 To FilterProTeam.Items.Count - 1
            FilterProTeam.SetItemCheckState(item, CheckState.Checked)
        Next

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(2), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterProTeam.ItemCheck, AddressOf FilterProTeam_ItemCheck

        Debug("Exited Players.ProTeamCheck_Click")
    End Sub

    Private Sub ProTeamUncheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProTeamUncheck.Click
        Debug("Entered Players.ProTeamUncheck_Click")

        RemoveHandler FilterProTeam.ItemCheck, AddressOf FilterProTeam_ItemCheck

        Dim item As Integer

        For item = 0 To FilterProTeam.Items.Count - 1
            FilterProTeam.SetItemCheckState(item, CheckState.Unchecked)
        Next

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(3), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(2), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterProTeam.ItemCheck, AddressOf FilterProTeam_ItemCheck

        Debug("Exited Players.ProTeamUncheck_Click")
    End Sub

    Private Sub FilterFanTeam_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles FilterFanTeam.ItemCheck
        Debug("Entered Players.FilterFanTeam_ItemCheck")

        RemoveHandler FilterFanTeam.ItemCheck, AddressOf FilterFanTeam_ItemCheck

        If e.CurrentValue = CheckState.Checked Then
            FilterFanTeam.SetItemCheckState(e.Index, CheckState.Unchecked)
        Else
            FilterFanTeam.SetItemCheckState(e.Index, CheckState.Checked)
        End If

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(5), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterFanTeam.ItemCheck, AddressOf FilterFanTeam_ItemCheck

        Debug("Exited Players.FilterFanTeam_ItemCheck")
    End Sub

    Private Sub FanTeamCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FanTeamCheck.Click
        Debug("Entered Players.FanTeamCheck_Click")

        RemoveHandler FilterFanTeam.ItemCheck, AddressOf FilterFanTeam_ItemCheck

        Dim item As Integer

        For item = 0 To FilterFanTeam.Items.Count - 1
            FilterFanTeam.SetItemCheckState(item, CheckState.Checked)
        Next

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(5), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterFanTeam.ItemCheck, AddressOf FilterFanTeam_ItemCheck

        Debug("Exited Players.FanTeamCheck_Click")
    End Sub

    Private Sub FanTeamUncheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FanTeamUncheck.Click
        Debug("Entered Players.FanTeamUncheck_Click")

        RemoveHandler FilterFanTeam.ItemCheck, AddressOf FilterFanTeam_ItemCheck

        Dim item As Integer

        For item = 0 To FilterFanTeam.Items.Count - 1
            FilterFanTeam.SetItemCheckState(item, CheckState.Unchecked)
        Next

        PopulateViewProPlayer()
        ViewProPlayer.Sort(ViewProPlayer.Columns(0), System.ComponentModel.ListSortDirection.Ascending)
        ViewProPlayer.Sort(ViewProPlayer.Columns(5), System.ComponentModel.ListSortDirection.Descending)
        ViewProPlayer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)

        AddHandler FilterFanTeam.ItemCheck, AddressOf FilterFanTeam_ItemCheck

        Debug("Exited Players.FanTeamUncheck_Click")
    End Sub
End Class