Public Class Form1

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ComboBox1.Text = ""
        TextBox1.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim username As String
        Dim password As String

        If ComboBox1.Text = "admin" And TextBox1.Text = "admin" Then
            Admin.Show()
            Me.Hide()

        ElseIf ComboBox1.Text = "guest" And TextBox1.Text = "guest" Then
            Guest.Show()
            Me.Hide()

        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class
