Imports System.IO

Module Common
    Public ReadOnly LEAGUE_SETTINGS_FILE_NAME As String = "LeagueSettings.csv"
    Public ReadOnly PRO_PLAYERS_FILE_NAME As String = "ProPlayers.csv"
    Public ReadOnly FAN_Team_FILE_NAME As String = "FanTeams.csv"
    Public ReadOnly FAN_PLAYERS_FILE_NAME As String = "FanPlayers.csv"

    Public ReadOnly DEBUG_FILE_NAME As String = "Debug.log"

    Public BiddingActive As Boolean = False
    Public LeagueSalaryCap As String = "100"

    Public FanTeamName As String

    Public ProTeamList As ArrayList = New ArrayList()
    Public ProPlayerList As ArrayList = New ArrayList()
    Public ProPositionList As ArrayList = New ArrayList()
    Public FanTeamList As ArrayList = New ArrayList()

    Public BigFont As Font = New Font("Cooper Black", 60, FontStyle.Regular)
    Public LittleFont As Font = New Font("Times New Roman", 30, FontStyle.Bold)

    Public Sub EstablishContext()
        If Not System.IO.File.Exists(PRO_PLAYERS_FILE_NAME) Then
            MsgBox("The pro player list file must exist in the same directory as the Free Agent application", MsgBoxStyle.Critical, PRO_PLAYERS_FILE_NAME & " not found")
            End
        End If

        If Not System.IO.File.Exists(FAN_Team_FILE_NAME) Then
            MsgBox("The fan team list file must exist in the same directory as the Free Agent application", MsgBoxStyle.Critical, FAN_Team_FILE_NAME & " not found")
            End
        End If
    End Sub

    Public Sub LoadProPlayerList()
        Dim ProPlayerStream As StreamReader = File.OpenText(PRO_PLAYERS_FILE_NAME)

        Dim ProPlayer As String

        ' loop through all the rows, stopping when we reach the end of file

        ProPlayer = ProPlayerStream.ReadLine()

        Do While ProPlayer <> Nothing
            Dim ProPlayerEntry As String() = {"", "", "", "", "", ""}

            ProPlayer = ProPlayer & ",,," 'The last three values are optional and may not have been entered by the user

            'Name
            ProPlayerEntry(0) = Microsoft.VisualBasic.Left(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ","))
            ProPlayer = Microsoft.VisualBasic.Mid(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") + 1)
            ProPlayerEntry(0) = ProPlayerEntry(0) & " " & Microsoft.VisualBasic.Left(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") - 1)
            ProPlayer = Microsoft.VisualBasic.Mid(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") + 1)
            If ", " = Microsoft.VisualBasic.Right(ProPlayerEntry(0), 2) Then
                ProPlayerEntry(0) = Left(ProPlayerEntry(0), ProPlayerEntry(0).Length - 2)
            End If

            'Postion
            ProPlayerEntry(1) = Microsoft.VisualBasic.Left(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") - 1)
            ProPlayer = Microsoft.VisualBasic.Mid(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") + 1)

            'Teams
            ProPlayerEntry(2) = Microsoft.VisualBasic.Left(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") - 1)
            ProPlayer = Microsoft.VisualBasic.Mid(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") + 1)

            'Last season's points
            ProPlayerEntry(3) = Microsoft.VisualBasic.Left(ProPlayer, Microsoft.VisualBasic.InStr(ProPlayer, ",") - 1)

            ProPlayerList.Add(ProPlayerEntry)

            If Not ProPositionList.Contains(ProPlayerEntry(1)) Then
                ProPositionList.Add(ProPlayerEntry(1))
            End If

            If Not ProTeamList.Contains(ProPlayerEntry(2)) Then
                ProTeamList.Add(ProPlayerEntry(2))
            End If

            ProPlayer = ProPlayerStream.ReadLine()
        Loop

        ProPlayerStream.Close()
    End Sub

    Public Sub LoadFanTeamList()
        Dim row As Integer

        For row = 0 To FanTeamList.Count - 1
            FanTeamList.RemoveAt(0)
        Next

        Dim FanTeamtream As StreamReader = File.OpenText(FAN_Team_FILE_NAME)

        Dim FanTeam As String

        ' loop through all the rows, stopping when we reach the end of file

        FanTeam = FanTeamtream.ReadLine()

        Do While FanTeam <> Nothing
            Dim FanTeamEntry As String() = {"", "0"}

            'Name
            FanTeamEntry(0) = FanTeam

            FanTeamList.Add(FanTeamEntry)

            FanTeam = FanTeamtream.ReadLine()
        Loop

        FanTeamtream.Close()
    End Sub

    Public Sub SaveFanTeamList()
        Dim row As Integer

        Dim FanTeamEntry As String() = {"", ""}

        Dim FanTeamtream As StreamWriter = File.CreateText(FAN_Team_FILE_NAME)

        ' loop through all the rows, writing each one to the file

        For row = 0 To FanTeamList.Count - 1
            FanTeamEntry = FanTeamList.Item(row)
            FanTeamtream.WriteLine(FanTeamEntry(0))
        Next

        FanTeamtream.Close()
    End Sub

    Public Sub LoadFanPlayerList()
        Dim FanPlayer As String

        Dim FanTeam As String
        Dim Salary As String
        Dim Name As String
        Dim Position As String
        Dim ProTeam As String

        If Not System.IO.File.Exists(FAN_PLAYERS_FILE_NAME) Then
            Exit Sub
        End If

        Dim ProPlayerEntry As String() = {"", "", "", "", "", ""}
        Dim FanTeamEntry As String() = {"", ""}

        Dim row As Integer

        Dim FanPlayerStream As StreamReader = File.OpenText(FAN_PLAYERS_FILE_NAME)

        ' loop through all the rows, stopping when we reach the end of file

        FanPlayer = FanPlayerStream.ReadLine()

        Do While FanPlayer <> Nothing
            'Timestamp
            FanPlayer = Microsoft.VisualBasic.Mid(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") + 3)

            'Fan Team
            FanTeam = Microsoft.VisualBasic.Left(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") - 1)
            FanPlayer = Microsoft.VisualBasic.Mid(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") + 3)

            'Salary
            Salary = Microsoft.VisualBasic.Left(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") - 1)
            FanPlayer = Microsoft.VisualBasic.Mid(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") + 3)

            'Name
            Name = Microsoft.VisualBasic.Left(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") - 1)
            FanPlayer = Microsoft.VisualBasic.Mid(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") + 3)

            'Postion
            Position = Microsoft.VisualBasic.Left(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") - 1)
            FanPlayer = Microsoft.VisualBasic.Mid(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """,""") + 3)

            'Pro Team
            ProTeam = Microsoft.VisualBasic.Left(FanPlayer, Microsoft.VisualBasic.InStr(FanPlayer, """") - 1)

            For row = 0 To ProPlayerList.Count - 1
                ProPlayerEntry = ProPlayerList.Item(row)

                If Name = ProPlayerEntry(0) And Position = ProPlayerEntry(1) And ProTeam = ProPlayerEntry(2) Then
                    ProPlayerEntry(4) = Salary
                    ProPlayerEntry(5) = FanTeam
                    ProPlayerList.Item(row) = ProPlayerEntry
                    Exit For
                End If
            Next

            For row = 0 To FanTeamList.Count - 1
                FanTeamEntry = FanTeamList.Item(row)
                If FanTeam = FanTeamEntry(0) Then
                    FanTeamEntry(1) = Trim(Str(Val(FanTeamEntry(1)) + Val(Salary)))
                    FanTeamList.Item(row) = FanTeamEntry
                    Exit For
                End If
            Next

            FanPlayer = FanPlayerStream.ReadLine()
        Loop

        FanPlayerStream.Close()
    End Sub

    Sub LoadLeagueSettings()
        If System.IO.File.Exists(LEAGUE_SETTINGS_FILE_NAME) Then
            Dim LeagueSettingsStream As StreamReader = File.OpenText(LEAGUE_SETTINGS_FILE_NAME)

            Dim LeagueSettings As String

            LeagueSettings = LeagueSettingsStream.ReadLine()

            'Salary Cap
            LeagueSalaryCap = Microsoft.VisualBasic.Left(LeagueSettings, Microsoft.VisualBasic.InStr(LeagueSettings, ",") - 1)
            LeagueSettings = Microsoft.VisualBasic.Mid(LeagueSettings, Microsoft.VisualBasic.InStr(LeagueSettings, ",") + 1)

            'League Ready
            If "True" = LeagueSettings Then
                BiddingActive = True
            Else
                BiddingActive = False
            End If

            LeagueSettingsStream.Close()
        End If
    End Sub

    Sub SaveLeagueSettings()
        Dim LeagueSettingsStream As StreamWriter = File.AppendText(LEAGUE_SETTINGS_FILE_NAME)

        LeagueSettingsStream.WriteLine(LeagueSalaryCap & "," & BiddingActive)

        LeagueSettingsStream.Close()
    End Sub

    Public Sub RandomizeFanTeamList()
        Dim chaos As Random = New Random()

        Dim item As Object

        Dim i, j, k As Integer

        For i = 1 To FanTeamList.Count * 100
            j = chaos.Next(FanTeamList.Count)
            k = chaos.Next(FanTeamList.Count)

            item = FanTeamList.Item(j)
            FanTeamList.Item(j) = FanTeamList.Item(k)
            FanTeamList.Item(k) = item
        Next
    End Sub

    Public Sub Debug(ByVal Message As String)
        If System.IO.File.Exists(DEBUG_FILE_NAME) Then
            Dim DebugStream As StreamWriter = File.AppendText(DEBUG_FILE_NAME)

            DebugStream.WriteLine(Now & ": " & Message)

            DebugStream.Close()
        End If
    End Sub
End Module
