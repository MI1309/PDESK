Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim username As String
        Dim password As String

        If ComboBox1.Text = "admin" And TextBox1.Text = "admin" Then
            Form2.Show()
            Me.Hide()

        Else
            Form3.Show()
            Me.Hide()

        End If

        If ComboBox1.Text = "kasir" And TextBox1.Text = "kasir" Then
            Form3.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class
