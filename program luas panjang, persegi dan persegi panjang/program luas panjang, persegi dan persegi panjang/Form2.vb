Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label4.Visible = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim result As String
        Dim Luas As Integer
        Dim Keliling As Integer
        Dim sisi As Integer

        If TextBox1.Text = "" Then
            MessageBox.Show("Pastikan input tidak kosong!", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf Integer.TryParse(TextBox1.Text, sisi) Then
            Luas = sisi * sisi
            Keliling = 4 * sisi

            result = "Luas persegi dengan sisi " & sisi & " adalah " & Luas & " dan kelilingnya adalah " & Keliling

            Label4.Visible = True
            Label4.Text = result

        Else
            MessageBox.Show("Masukkan nilai yang valid untuk panjang dan lebar!", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TextBox1.Clear()
        Label4.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
    End Sub

End Class
