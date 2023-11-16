Public Class Guest

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