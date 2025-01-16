Public Class Form3

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label4.Visible = False
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim result As String
        Dim Luas As Integer
        Dim Keliling As Integer
        Dim panjang As Integer
        Dim lebar As Integer

        If TextBox1.Text = "" And TextBox2.Text = "" Then

            MessageBox.Show("kedua input jangan kosong!", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf TextBox2.Text = "" Then

            MessageBox.Show("panjang jangan kosong!", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf TextBox1.Text = "" Then
            MessageBox.Show("lebar jangan kosong!", "Input Kosong", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        ElseIf Integer.TryParse(TextBox1.Text, panjang) AndAlso Integer.TryParse(TextBox2.Text, lebar) Then
            Luas = panjang * lebar
            Keliling = 2 * (panjang + lebar)

            result = "Luas persegi dengan panjang " & panjang & " dan lebar " & lebar & " adalah " & Luas & " dan kelilingnya adalah " & Keliling

            Label4.Visible = True
            Label4.Text = result

        Else
            MessageBox.Show("Masukkan nilai yang valid untuk panjang dan lebar!", "Input Tidak Valid", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Close()
        Form1.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TextBox1.Clear()
        TextBox2.Clear()
        Label4.Hide()
    End Sub
End Class
