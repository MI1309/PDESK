Public Class Form3

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim nasgor, migor, ktiau, totalbayar As Double


        If CheckBox1.Checked = True Then
            nasgor = ComboBox1.Text * 15000
        End If

        If CheckBox2.Checked = True Then
            migor = ComboBox2.Text * 10000
        End If

        If CheckBox3.Checked = True Then
            ktiau = ComboBox3.Text * 20000
        End If

        If RadioButton2.Checked = True Then
            totalbayar = nasgor + migor + ktiau
            TextBox2.Text = (90 / 100) * totalbayar
        ElseIf RadioButton2.Checked = False Then
            TextBox2.Text = nasgor + migor + ktiau
        End If
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub
End Class