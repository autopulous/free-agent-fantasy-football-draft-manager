Public NotInheritable Class Welcome
    Private WithEvents Timer As New Timer

    Private Sub Welcome_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If My.Application.Info.Title <> "" Then
            FreeAgent.Text = My.Application.Info.Title
        Else
            FreeAgent.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)

        Copyright.Text = My.Application.Info.Copyright

        Timer.Interval = 7500
        Timer.Start()
    End Sub

    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer.Tick
        Timer.Stop()
        Me.Close()
    End Sub
End Class
