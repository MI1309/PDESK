Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Form1_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

    Private Sub TextBox7_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox7.TextChanged

    End Sub

    Private Sub TextBox8_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox8.TextChanged

    End Sub

    Private Sub TextBox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox9.TextChanged

    End Sub

    Private Sub TextBox10_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox10.TextChanged

    End Sub

    Private Sub TextBox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.TextChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim niab, nitu, niu, nias, sepuluh, duapuluh, tigapuluh, empatpuluh, tonilai, senilai As Double
        Dim ket, grade As String

        niab = Val(TextBox1.Text)
        nitu = Val(TextBox2.Text)
        niu = Val(TextBox3.Text)
        nias = Val(TextBox4.Text)

        senilai = (niab + nitu + niu + nias) / 4
        sepuluh = (10 / 100) * senilai
        duapuluh = (20 / 100) * senilai
        tigapuluh = (30 / 100) * senilai
        empatpuluh = (40 / 100) * senilai

        If (senilai >= 80) Then
            TextBox10.Text = "A"
            TextBox11.Text = "Sangat Baik"

        ElseIf (senilai >= 65) Then
            TextBox10.Text = "B"
            TextBox11.Text = "Baik"

        ElseIf (senilai <= 65) Then
            TextBox10.Text = "C"
            TextBox11.Text = "Cukup Baik"
        End If

        TextBox5.Text = sepuluh
        TextBox6.Text = duapuluh
        TextBox7.Text = tigapuluh
        TextBox8.Text = empatpuluh
        TextBox9.Text = senilai

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim keluar As MsgBoxResult
        keluar = MsgBox("yakin mau keluar?", MsgBoxStyle.YesNo, "peringatan")
        If keluar = MsgBoxResult.Yes Then
            Close()
        End If

    End Sub
End Class
