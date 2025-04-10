Public Class Form1

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call clear()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim username As String = TextBox1.Text
        Dim password As String = TextBox2.Text
        Dim role As String = ComboBox1.Text

        If role = "guru" Then
            If username = "mega" And password = "mega123" Then
                Form2.Show()
                Me.Hide()
            ElseIf username = "Diah" And password = "Diah123" Then
                Form2.Show()
                Me.Hide()
            ElseIf username = "Ali" And password = "Ali123" Then
                Form2.Show()
                Me.Hide()
            Else
                Call clear()
                MessageBox.Show("Guru tidak terdaftar,Sandi atau username mungkin salah,ingin mengulang?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            End If
        ElseIf role = "siswa" Then
            If username = "mahrus" And password = "123mahrus" Then
                Form3.Show()
                Me.Hide()
            ElseIf username = "rava" And password = "123rava" Then
                Form3.Show()
                Me.Hide()
            ElseIf username = "rafi" And password = "123rafi" Then
                Form3.Show()
                Me.Hide()
            ElseIf username = "salman" And password = "123salman" Then
                Form3.Show()
                Me.Hide()
            ElseIf username = "vito" And password = "123vito" Then
                Form3.Show()
                Me.Hide()
            Else
                Call clear()
                MessageBox.Show("Siswa tidak terdaftar,Sandi atau username mungkin salah,ingin mengulang?", "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            End If
        End If
    End Sub
    Sub clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.Text = ""
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
