Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim nama As String
        Dim tulis, wawan, fisik, result As Integer

        ' Validasi input kosong
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) Then
            MessageBox.Show("Pastikan semua kolom terisi!", "Kesalahan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validasi TextBox1 untuk string
        If Not TextBox1.Text.All(AddressOf Char.IsLetter) Then
            MessageBox.Show("Masukkan nama yang valid (hanya huruf)!", "Kesalahan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If

        ' Validasi TextBox2, TextBox3, dan TextBox4 untuk angka
        If Not Integer.TryParse(TextBox2.Text, tulis) OrElse Not Integer.TryParse(TextBox3.Text, wawan) OrElse Not Integer.TryParse(TextBox4.Text, fisik) Then
            MessageBox.Show("Masukkan nilai angka yang valid di kolom nilai!", "Kesalahan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validasi angka maksimum 100
        Dim errorMessage As String = ""

        If tulis > 100 Then
            errorMessage &= "Nilai pada kolom 2 lebih dari 100. Harap perbaiki!" & vbCrLf
            TextBox2.Text = "" ' Hapus hanya nilai yang salah
        End If
        If wawan > 100 Then
            errorMessage &= "Nilai pada kolom 3 lebih dari 100. Harap perbaiki!" & vbCrLf
            TextBox3.Text = "" ' Hapus hanya nilai yang salah
        End If
        If fisik > 100 Then
            errorMessage &= "Nilai pada kolom 4 lebih dari 100. Harap perbaiki!" & vbCrLf
            TextBox4.Text = "" ' Hapus hanya nilai yang salah
        End If

        ' Jika ada kesalahan, tampilkan pesan dan hentikan eksekusi
        If errorMessage <> "" Then
            MessageBox.Show(errorMessage, "Kesalahan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Hitung nilai akhir
        tulis = CInt(TextBox2.Text) * (30 / 100)
        wawan = CInt(TextBox3.Text) * (40 / 100)
        fisik = CInt(TextBox4.Text) * (30 / 100)
        result = tulis + wawan + fisik

        ' Evaluasi hasil
        If result <= 65 Then
            MsgBox("Anda Gagal")
        ElseIf result <= 85 Then
            MsgBox("Sementara Anda Cadangan")
        ElseIf result <= 100 Then
            MsgBox("Selamat Anda Lulus")
        Else
            MsgBox("Nilai tidak valid!")
        End If
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()

    End Sub
End Class
