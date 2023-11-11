Public Class Form2

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim altul, kursi, meja, totalbayar As Double


        If CheckBox1.Checked = True Then
            altul = ComboBox1.Text * 15000
        End If

        If CheckBox2.Checked = True Then
            kursi = ComboBox2.Text * 100000
        End If

        If CheckBox3.Checked = True Then
            meja = ComboBox3.Text * 200000
        End If

        If RadioButton2.Checked = True Then
            totalbayar = altul + kursi + meja
            TextBox2.Text = (90 / 100) * totalbayar
        ElseIf RadioButton2.Checked = False Then
            TextBox2.Text = altul + kursi + meja
        End If


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class
