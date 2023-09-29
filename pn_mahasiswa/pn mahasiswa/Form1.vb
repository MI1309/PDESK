Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub TextBox6_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox6.TextChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim nitu, niuts, nias, grade, nia As Integer
        nitu = Val(TextBox2.Text) / 4
        nias = Val(TextBox4.Text) / 4
        niuts = Val(TextBox3.Text) / 4
        nia = nitu + nias + niuts


        If (nia >= 80) Then
            TextBox6.Text = "A"
        ElseIf (nia >= 60) Then
            TextBox6.Text = "B"
        ElseIf (nia <= 60) Then
            TextBox6.Text = "c"
        ElseIf (nia <= 50) Then
            TextBox6.Text = "D"
        End If

        TextBox5.Text = nia

        

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""


    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim keluar As MsgBoxResult
        keluar = MsgBox("yakin mau keluar ? ", MsgBoxStyle.YesNo, "peringatan")
        If keluar = MsgBoxResult.Yes Then
            Close()

        End If

    End Sub
End Class
