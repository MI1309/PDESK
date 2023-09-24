Public Class Form1

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
            TextBox11.Text = "sangatbaik"

        ElseIf (senilai >= 65) Then
            TextBox10.Text = "B"
            TextBox11.Text = "baik"

        ElseIf (senilai <= 65) Then
            TextBox10.Text = "C"
            TextBox11.Text = "cukupbaik"
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
        keluar = MsgBox("apakah anda akan keluar?", MsgBoxStyle.YesNo, "peringatan")
        If keluar = MsgBoxResult.Yes Then
            Close()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
