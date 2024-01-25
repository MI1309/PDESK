Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim username As String
        Dim password As String

        If TextBox1.Text = "admin" And TextBox2.Text = "admin" Then
            Form2.Show()
            Me.Hide()
        Else : MessageBox.Show("Sandi atau username salah,ingin mengulang?", "Peringatan", MessageBoxButtons.YesNo)

        End If
        Me.Hide()
        If vbNo = True Then
            Me.Close()
        End If

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class
