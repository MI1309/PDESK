Public Class Form1
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For i As Integer = 1 To 31
            ComboBox1.Items.Add(i)
        Next

        For i As Integer = 1 To 12
            ComboBox2.Items.Add(MonthName(i))
        Next

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim nama As String
        Dim zodiak As String
        nama = TextBox1.Text

        If TextBox1.Text = "" And ComboBox1.Text = "" And ComboBox2.Text = "" Then
            MessageBox.Show("Pastikan input tidak kosong!", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox2.Text = ""
        ElseIf TextBox1.Text = "" Then
            MessageBox.Show("salah satu Input Kosong", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox2.Text = ""
        ElseIf ComboBox1.Text = "" Then
            MessageBox.Show("salah satu Input Kosong", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox2.Text = ""
        ElseIf ComboBox2.Text = "" Then
            MessageBox.Show("salah satu Input Kosong", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox2.Text = ""
        Else
            If ComboBox1.SelectedIndex < 20 And ComboBox2.SelectedIndex = 0 Or ComboBox1.SelectedIndex > 21 And ComboBox2.SelectedIndex = 11 Then
                zodiak = "capricorn"
            ElseIf ComboBox1.SelectedIndex < 18 And ComboBox2.SelectedIndex = 1 Or ComboBox1.SelectedIndex > 19 And ComboBox2.SelectedIndex = 0 Then
                zodiak = "Aquarius"
            ElseIf ComboBox1.SelectedIndex < 20 And ComboBox2.SelectedIndex = 2 Or ComboBox1.SelectedIndex > 17 And ComboBox2.SelectedIndex = 1 Then
                zodiak = "Pisces"
            ElseIf ComboBox1.SelectedIndex < 20 And ComboBox2.SelectedIndex = 3 Or ComboBox1.SelectedIndex > 19 And ComboBox2.SelectedIndex = 2 Then
                zodiak = "Aries"
            ElseIf ComboBox1.SelectedIndex < 20 And ComboBox2.SelectedIndex = 4 Or ComboBox1.SelectedIndex > 19 And ComboBox2.SelectedIndex = 3 Then
                zodiak = "Taurus"
            ElseIf ComboBox1.SelectedIndex < 21 And ComboBox2.SelectedIndex = 5 Or ComboBox1.SelectedIndex > 19 And ComboBox2.SelectedIndex = 4 Then
                zodiak = "Gemini"
            ElseIf ComboBox1.SelectedIndex < 22 And ComboBox2.SelectedIndex = 6 Or ComboBox1.SelectedIndex > 20 And ComboBox2.SelectedIndex = 5 Then
                zodiak = "Cancer"
            ElseIf ComboBox1.SelectedIndex < 23 And ComboBox2.SelectedIndex = 7 Or ComboBox1.SelectedIndex > 21 And ComboBox2.SelectedIndex = 6 Then
                zodiak = "Leo"
            ElseIf ComboBox1.SelectedIndex < 22 And ComboBox2.SelectedIndex = 8 Or ComboBox1.SelectedIndex > 22 And ComboBox2.SelectedIndex = 7 Then
                zodiak = "Virgo"
            ElseIf ComboBox1.SelectedIndex < 23 And ComboBox2.SelectedIndex = 9 Or ComboBox1.SelectedIndex > 21 And ComboBox2.SelectedIndex = 8 Then
                zodiak = "Libra"
            ElseIf ComboBox1.SelectedIndex < 22 And ComboBox2.SelectedIndex = 10 Or ComboBox1.SelectedIndex > 22 And ComboBox2.SelectedIndex = 9 Then
                zodiak = "Scorpio"
            ElseIf ComboBox1.SelectedIndex < 22 And ComboBox2.SelectedIndex = 11 Or ComboBox1.SelectedIndex > 20 And ComboBox2.SelectedIndex = 10 Then
                zodiak = "Sagitarius"
            End If

            TextBox2.Text = "" & nama & " anda lahir pada : " & ComboBox1.Text & " " & ComboBox2.Text & " Zodiak anda adalah : " & zodiak
        End If

        
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox2.Text = ""
        TextBox1.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class
