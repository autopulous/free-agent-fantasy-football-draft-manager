Imports System
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms

Public Class Teams
    Inherits System.Windows.Forms.Form

    Public WithEvents playerOnTheBlock As New Label
    Public WithEvents highBidder As New Label
    Public WithEvents playerOnTheBlockPosition As New Label
    Public WithEvents playerOnTheBlockPoints As New Label

    Public WithEvents currentOffer As New Label

    Private draftPanel As New Panel
    Private scoreboardPanel As New Panel
    Private buttonPanel As New Panel

    Private WithEvents ViewBid As New DataGridView
    Private WithEvents pass As New Button
    Private passToolTip As New ToolTip
    Private WithEvents bid As New Button
    Private bidToolTip As New ToolTip
    Private WithEvents nextOffer As New TextBox
    Private offerToolTip As New ToolTip
    Private WithEvents teamOnTheClock As New Label
    Private WithEvents clock As New Label
    Private WithEvents reset As New Button
    Private resetToolTip As New ToolTip

    Public playerOnTheBlockRow As Integer

    Private nominator As Integer = -1
    Private currentBid As Integer

    Private bidSeconds As Integer
    Private offerSeconds As Integer

    Private Sub Team_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Debug("Entered Teams.Team_Load")

        SetupLayout()
        PopulateDataGridView()
        NextNominator()

        Players.Visible = True
        Me.Visible = False

        Debug("Exited Teams.Team_Load")
    End Sub

    Public Sub FormActivate()
        Debug("Entered Teams.FormActivate")

        Me.Visible = True

        bidSeconds = 0
        offerSeconds = 0

        pass.Enabled = False
        bid.Text = "&Bid"
        bid.Enabled = True
        nextOffer.Enabled = True
        reset.Text = "&Cancel"
        reset.Enabled = True

        With Me.ViewBid
            Dim row As Integer

            For row = 0 To .RowCount - 1
                .Rows.Item(row).DefaultCellStyle.BackColor = Color.White
                .Item(2, row).Value = ""
            Next

            .CurrentCell = Me.ViewBid.Item(0, nominator)
            .CurrentCell.Selected = True

            .AutoResizeColumns()
        End With

        currentBid = 0

        LockOutTeam()

        postBid(currentBid)

        Debug("Exited Teams.FormActivate")
    End Sub

    Private Sub Team_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Debug("Entered Teams.Team_FormClosed")

        Settings.Close()

        Debug("Exited Teams.Team_FormClosed")
    End Sub

    Private Sub SetupLayout()
        Debug("Entered Teams.SetupLayout")

        Me.Size = New Size(600, 500)

        With Me.ViewBid
            .Name = "ViewBid"
            .Size = New System.Drawing.Size(500, 250)
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .BorderStyle = BorderStyle.None
            .GridColor = Color.Black
            .BackgroundColor = Me.BackColor
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

            .ColumnCount = 3

            .Columns(0).Name = "Teams"
            .Columns(1).Name = "Salary"
            .Columns(2).Name = "Last Bid"
            .Columns(2).DefaultCellStyle.Font = New Font(Me.ViewBid.DefaultCellStyle.Font, FontStyle.Italic)

            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single

            With .ColumnHeadersDefaultCellStyle
                .BackColor = Color.Navy
                .ForeColor = Color.White
                .Font = New Font(Me.ViewBid.Font, FontStyle.Bold)
            End With
        End With

        With Me.highBidder
            .Width = Me.Width
            .Height = 50
            .Font = LittleFont
            .Location = New Point((Me.Width - .Width) / 2, (Me.Height - 500) / 2)
            .TextAlign = ContentAlignment.TopCenter
            .TabStop = False
        End With

        With Me.currentOffer
            .Width = Me.Width
            .Height = 175
            .Font = BigFont
            .Location = New Point((Me.Width - .Width) / 2, Me.highBidder.Location.Y + Me.highBidder.Height)
            .ForeColor = Color.DarkOliveGreen
            .TextAlign = ContentAlignment.TopCenter
            .TabStop = False
        End With

        With Me.playerOnTheBlockPosition
            .Width = Me.Width
            .Height = 50
            .Font = LittleFont
            .Location = New Point((Me.Width - .Width) / 2, Me.currentOffer.Location.Y + Me.currentOffer.Height)
            .TextAlign = ContentAlignment.TopCenter
            .TabStop = False
        End With

        With Me.playerOnTheBlockPoints
            .Width = Me.Width
            .Height = 50
            .Font = LittleFont
            .Location = New Point((Me.Width - .Width) / 2, Me.playerOnTheBlockPosition.Location.Y + Me.playerOnTheBlockPosition.Height)
            .TextAlign = ContentAlignment.TopCenter
            .TabStop = False
        End With

        With Me.playerOnTheBlock
            .Width = Me.Width
            .Height = 175
            .Font = BigFont
            .Location = New Point((Me.Width - .Width) / 2, Me.playerOnTheBlockPoints.Location.Y + Me.playerOnTheBlockPoints.Height)
            .ForeColor = Color.DarkBlue
            .TextAlign = ContentAlignment.BottomCenter
            .TabStop = False
        End With

        With Me.pass
            .Text = "&Pass"
            .Location = New Point(10, 10)
            .BackColor = Me.buttonPanel.BackColor
            .TabIndex = 2
        End With

        passToolTip.SetToolTip(pass, "Eliminates the team from the bidding round")

        With Me.bid
            .Text = "&Bid"
            .Location = New Point(100, 10)
            .BackColor = Me.buttonPanel.BackColor
            .TabIndex = 3
        End With

        bidToolTip.SetToolTip(bid, "Post an offer for the current player up for bids")

        With Me.nextOffer
            .MaxLength = 3
            .Text = "0"
            .Location = New Point(190, 12)
            .BackColor = Me.buttonPanel.BackColor
            .TabIndex = 0
        End With

        offerToolTip.SetToolTip(nextOffer, "Enter an offer amount for the current player up for bids, or a command" & Chr(13) & Chr(13) & "- ""New"" randomizes the draft order" & Chr(13) & "- ""Old"" uses the order specified in the " & FAN_Team_FILE_NAME & " file")

        With Me.teamOnTheClock
            .Width = 350
            .Height = 50
            .Font = New Font(.Font.FontFamily, 20, FontStyle.Bold)
            .Location = New Point((Me.Width - .Width) / 2, 5)
            .TextAlign = ContentAlignment.TopCenter
            .ForeColor = Color.GreenYellow
            .TabStop = False
        End With

        With Me.reset
            .Text = "&Reset"
            .Location = New Point((Me.Width - .Width) - 10, 10)
            .BackColor = Me.buttonPanel.BackColor
            .TabIndex = 1
        End With

        resetToolTip.SetToolTip(reset, "Remove all current bids and restart the bidding round")

        With Me.clock
            .Width = 110
            .Height = 20
            .Font = New Font(.Font.FontFamily, 10, FontStyle.Regular)
            .ForeColor = Color.White
            .Location = New Point(reset.Location.X - 125, 12)
            .TextAlign = ContentAlignment.MiddleRight
            .TabStop = False
        End With

        With Me.draftPanel
            .Controls.Add(Me.ViewBid)
            .AutoSize = True
            .Anchor = AnchorStyles.Left
        End With

        With Me.scoreboardPanel
            .Controls.Add(Me.highBidder)
            .Controls.Add(Me.playerOnTheBlock)
            .Controls.Add(Me.playerOnTheBlockPosition)
            .Controls.Add(Me.playerOnTheBlockPoints)
            .Controls.Add(Me.currentOffer)
            .AutoSize = True
            .Dock = DockStyle.Fill
        End With

        With Me.buttonPanel
            .Controls.Add(Me.pass)
            .Controls.Add(Me.bid)
            .Controls.Add(Me.nextOffer)
            .Controls.Add(Me.teamOnTheClock)
            .Controls.Add(Me.clock)
            .Controls.Add(Me.reset)
            .BackColor = Color.Black
            .AutoSize = True
            .Dock = DockStyle.Bottom
        End With

        With Me
            .Controls.Add(.buttonPanel)
            .Controls.Add(.draftPanel)
            .Controls.Add(.scoreboardPanel)
        End With

        Debug("Exited Teams.SetupLayout")
    End Sub

    Private Sub PopulateDataGridView()
        Debug("Entered Teams.PopulateDataGridView")

        Dim row As Integer

        Dim team As String() = {"", "", ""}

        Dim FanTeamEntry As String() = {"", ""}

        With Me.ViewBid
            For row = 0 To .Rows.Count - 1
                .Rows.RemoveAt(0)
            Next

            BiddingActive = False

            For row = 0 To FanTeamList.Count - 1
                FanTeamEntry = FanTeamList.Item(row)
                team(0) = FanTeamEntry(0)
                team(1) = FanTeamEntry(1)

                If Val(FanTeamEntry(1)) < Val(LeagueSalaryCap) Then
                    BiddingActive = True
                End If

                .Rows.Add(team)
                .Rows.Item(row).DefaultCellStyle.BackColor = Color.White
            Next

            .Columns(0).DisplayIndex = 0
            .Columns(1).DisplayIndex = 1
            .Columns(2).DisplayIndex = 2

            .RowsDefaultCellStyle.SelectionForeColor = Color.Black
            .RowsDefaultCellStyle.SelectionBackColor = Color.GreenYellow
            .RowsDefaultCellStyle.Font = New Font(.Font.FontFamily, 10, FontStyle.Regular)

            .AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
            .AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders)
        End With

        Debug("Exited Teams.PopulateDataGridView")
    End Sub

    Private Sub pass_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pass.Click
        Debug("Entered Teams.pass_Click")

        Me.ViewBid.Rows.Item(Me.ViewBid.CurrentRow.Index).DefaultCellStyle.BackColor = Color.Red
        NextTeam()

        Debug("Exited Teams.pass_Click")
    End Sub

    Private Sub bid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles bid.Click
        Debug("Entered Teams.bid_Click")

        With Me.ViewBid
            If "&Sign" = bid.Text Then
                bid.Enabled = False
                nextOffer.Enabled = False
                nextOffer.Text = ""
                nextOffer.Refresh()
                reset.Enabled = False
                SignPlayer()
                Debug("Exited Teams.bid_Click")
                Exit Sub
            End If

            If currentBid >= Val(nextOffer.Text) Then
                Beep()
                Beep()
            ElseIf Val(LeagueSalaryCap) - Val(.Item(1, .CurrentRow.Index).Value) < Val(nextOffer.Text) Then
                Beep()
            Else
                .Rows.Item(Me.ViewBid.CurrentRow.Index).DefaultCellStyle.BackColor = Color.White

                .Item(2, .CurrentRow.Index).Value = Trim(nextOffer.Text)

                Me.highBidder.Text = Me.ViewBid.Item(0, Me.ViewBid.CurrentRow.Index).Value
                currentBid = Val(nextOffer.Text)

                LockOutTeam()

                NextTeam()

                postBid(currentBid)

                If "&Sign" = bid.Text Then
                    pass.Enabled = False
                Else
                    pass.Enabled = True
                End If

                reset.Text = "&Reset"
            End If
        End With

        Debug("Exited Teams.bid_Click")
    End Sub

    Private Sub postBid(ByVal bid As Integer)
        Debug("Entered Teams.postBid")

        If 0 = bid Then
            Me.highBidder.Text = ""
        End If

        currentOffer.Text = "$" & Trim(Str(bid))

        If Val(LeagueSalaryCap) <= bid Then
            nextOffer.Text = ""
        Else
            nextOffer.Text = Trim(Str(bid + 1))
        End If

        Debug("Exited Teams.postBid")
    End Sub

    Private Sub nextOffer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles nextOffer.GotFocus
        Debug("Entered Teams.nextOffer_GotFocus")

        nextOffer.SelectAll()

        Debug("Exited Teams.nextOffer_GotFocus")
    End Sub

    Private Sub nextOffer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles nextOffer.KeyPress
        Debug("Entered Teams.nextOffer_KeyPress")

        If e.KeyChar = Chr(13) Then
            bid_Click(sender, e)
            e.Handled = True
        End If

        Debug("Exited Teams.nextOffer_KeyPress")
    End Sub

    Private Sub reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset.Click
        Debug("Entered Teams.reset_Click")

        With Me.ViewBid
            Dim row As Integer

            For row = 0 To .RowCount - 1
                .Rows.Item(row).DefaultCellStyle.BackColor = Color.White
                .Item(2, row).Value = ""
            Next

            If "&Cancel" = reset.Text Then
                Players.FormActivate()
                Debug("Exited Teams.reset_Click")
                Exit Sub
            End If

            currentBid = 0

            LockOutTeam()

            Me.ViewBid.CurrentCell = Me.ViewBid.Item(0, nominator)
            Me.ViewBid.CurrentCell.Selected = True

            postBid(currentBid)

            pass.Enabled = False
            bid.Text = "&Bid"
            reset.Text = "&Cancel"

            offerSeconds = 0
        End With

        Debug("Exited Teams.reset_Click")
    End Sub

    Private Sub ViewBid_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewBid.CurrentCellChanged
        Debug("Entered Teams.ViewBid_CurrentCellChanged")

        If Me.ViewBid.CurrentRow Is Nothing Then Exit Sub

        teamOnTheClock.Text = Me.ViewBid.Item(0, Me.ViewBid.CurrentRow.Index).Value

        If "&Sign" = bid.Text Then
            pass.Enabled = True
            bid.Text = "&Bid"
        End If

        bidSeconds = 0

        Debug("Exited Teams.ViewBid_CurrentCellChanged")
    End Sub

    Private Sub NextTeam()
        Debug("Entered Teams.NextTeam")

        With Me.ViewBid.Rows
            Do
                If Me.ViewBid.RowCount <= Me.ViewBid.CurrentRow.Index + 1 Then
                    Me.ViewBid.CurrentCell = Me.ViewBid.Item(0, 0)
                Else
                    Me.ViewBid.CurrentCell = Me.ViewBid.Item(0, Me.ViewBid.CurrentRow.Index + 1)
                End If

            Loop Until (Color.Red <> .Item(Me.ViewBid.CurrentRow.Index).DefaultCellStyle.BackColor)

            Me.ViewBid.CurrentCell.Selected = True

            Dim row, notPassed As Integer

            For row = 0 To Me.ViewBid.RowCount - 1
                If Color.Red <> .Item(row).DefaultCellStyle.BackColor Then
                    notPassed = notPassed + 1
                End If
            Next

            If 1 >= notPassed Then
                bid.Text = "&Sign"
                pass.Enabled = False
            End If
        End With

        If "&Cancel" = reset.Text Then
            reset.Text = "&Reset"
        End If

        Debug("Exited Teams.NextTeam")
    End Sub

    Private Sub SignPlayer()
        Debug("Entered Teams.SignPlayer")

        Dim ProPlayerEntry As String() = {"", "", "", "", "", ""}

        With Me.ViewBid
            ProPlayerEntry = ProPlayerList.Item(playerOnTheBlockRow)

            ProPlayerEntry(4) = .Item(2, .CurrentRow.Index).Value()
            ProPlayerEntry(5) = .Item(0, .CurrentRow.Index).Value()

            Dim draftStream As StreamWriter = File.AppendText("FanPlayers.csv")

            draftStream.WriteLine("""" & Date.Now & """,""" & ProPlayerEntry(5) & """,""" & ProPlayerEntry(4) & """,""" & ProPlayerEntry(0) & """,""" & ProPlayerEntry(1) & """,""" & ProPlayerEntry(2) & """")

            draftStream.Close()

            .Item(1, .CurrentRow.Index).Value() = Trim(Str(Val(.Item(1, .CurrentRow.Index).Value()) + Val(.Item(2, .CurrentRow.Index).Value())))

            NextNominator()
        End With

        Players.FormActivate()
        Me.Visible = False

        Debug("Exited Teams.SignPlayer")
    End Sub

    Private Sub NextNominator()
        Debug("Entered Teams.NextNominator")

        Dim row As Integer = 0

        With Me.ViewBid
            Do
                nominator = nominator + 1

                If .RowCount <= nominator Then
                    nominator = 0
                End If

                If Val(LeagueSalaryCap) <= .Item(1, nominator).Value() Then
                    row = row + 1

                    If row >= .RowCount Then
                        DraftCompleted()
                        Debug("Exited Teams.NextNominator")
                        Exit Sub
                    End If
                End If
            Loop Until (Val(LeagueSalaryCap) > .Item(1, nominator).Value())

            FanTeamName = .Item(0, nominator).Value()
        End With

        Debug("Exited Teams.NextNominator")
    End Sub

    Private Sub LockOutTeam()
        Debug("Entered Teams.LockOutTeam")

        Dim row As Integer

        With Me.ViewBid
            For row = 0 To .RowCount - 1
                If row <> .CurrentRow.Index Then
                    If currentBid >= Val(LeagueSalaryCap) - .Item(1, row).Value() Then
                        .Rows.Item(row).DefaultCellStyle.BackColor = Color.Red
                    End If
                End If
            Next
        End With

        Debug("Exited Teams.LockOutTeam")
    End Sub

    Private Sub DraftCompleted()
        Debug("Entered Teams.DraftCompleted")

        BiddingActive = False
        Timer1.Stop()

        Debug("Exited Teams.DraftCompleted")
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        offerSeconds = offerSeconds + 1
        bidSeconds = bidSeconds + 1
        clock.Text = offerSeconds.ToString.PadLeft(4, "0") + ":" + bidSeconds.ToString.PadLeft(4, "0")
    End Sub
End Class
