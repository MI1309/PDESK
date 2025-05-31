Imports System.Data.Odbc

Public Class form_stock
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Public previousForm As Form
    Dim ds As DataSet
    Dim mysql As String

    ' Fungsi untuk menguji koneksi ke database
    Sub test_conn()
        Try
            ' Menggunakan DSN yang sudah dikonfigurasi di ODBC Data Source Administrator
            mysql = "DSN=pulsa;"
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

    ' Product select
    Sub product()
        Try
            ' Query untuk mengambil data dari database
            mysql = "SELECT DISTINCT nama_produk FROM admin_product"
            da = New OdbcDataAdapter(mysql, conn)
            ds = New DataSet()
            da.Fill(ds, "nama_produk")

            ' Mengisi ComboBox dengan data dari dataset
            ComboBox3.Items.Clear()
            For Each row As DataRow In ds.Tables("nama_produk").Rows
                ComboBox3.Items.Add(row("nama_produk").ToString())
            Next
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    ' Stok select
    Sub stok()
        Try
            ' Query untuk mengambil data dari database
            mysql = "SELECT stok FROM admin_product"
            da = New OdbcDataAdapter(mysql, conn)
            ds = New DataSet()
            da.Fill(ds, "stok")

            ' Mengisi TextBox dengan data dari dataset
            Label1.Hide()
            For Each row As DataRow In ds.Tables("stok").Rows
                Label1.Text = row("stok").ToString()
            Next
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    ' Tipe select
    Sub tipe()
        Try
            mysql = "SELECT DISTINCT tipe FROM admin_product ORDER BY tipe"
            da = New OdbcDataAdapter(mysql, conn)
            ds = New DataSet()
            da.Fill(ds, "tipe")

            ComboBox1.Items.Clear()
            For Each row As DataRow In ds.Tables("tipe").Rows
                ComboBox1.Items.Add(row("tipe").ToString())
            Next
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    ' Fungsi untuk menutup aplikasi
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub

    Private Sub Form8_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set dock untuk form agar mengikuti ukuran TabPage
        Me.Dock = DockStyle.Fill

        ' Atur kontrol agar mengikuti ukuran form
        Label1.AutoSize = False
        Label1.Width = 297
        Label1.Height = 25
        Label1.BorderStyle = BorderStyle.FixedSingle
        Label1.TextAlign = ContentAlignment.MiddleCenter
        Label1.Location = New Point(680, 456)
        Label1.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Label1.ForeColor = Color.Gray
        Label1.Text = "Belum ada produk dipilih"

        ' Set DockStyle for ComboBoxes and Button

        ' Padding and Margin Adjustments
        ComboBox1.Margin = New Padding(10, 10, 10, 5)
        ComboBox3.Margin = New Padding(10, 5, 10, 10)
        Button1.Margin = New Padding(10)

        ' Test koneksi dan inisialisasi
        test_conn()
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox3.DropDownStyle = ComboBoxStyle.DropDownList

        product()
        tipe()

        ' Atur label awal
        Label1.Text = "Belum ada produk dipilih"
        Label1.ForeColor = Color.Gray
    End Sub

    ' Event handler untuk menangani perubahan teks di TextBox1
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Mengonversi nilai TextBox1 ke angka dan cek apakah itu 0
        Dim stokValue As Integer
        If Integer.TryParse(Label1.Text, stokValue) Then
            ' Jika nilai stok adalah 0, beri warna merah
            If stokValue = 0 Then
                Label1.ForeColor = Color.Red
            Else
                Label1.ForeColor = Color.Black ' Mengembalikan warna teks ke hitam jika tidak "0"
            End If
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            If ComboBox3.SelectedIndex = -1 Or ComboBox1.SelectedIndex = -1 Then Exit Sub

            Dim selectedProduct As String = ComboBox3.SelectedItem.ToString()
            Dim selectedTipe As String = ComboBox1.SelectedItem.ToString()

            ' Ambil stok dari produk + tipe
            Dim cmd As New OdbcCommand("SELECT stok FROM admin_product WHERE nama_produk = ? AND tipe = ?", conn)
            cmd.Parameters.AddWithValue("@nama_produk", selectedProduct)
            cmd.Parameters.AddWithValue("@tipe", selectedTipe)

            Dim dr As OdbcDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                Dim stokValue As Integer = Convert.ToInt32(dr("stok"))
                Label1.Text = stokValue.ToString()
                Label1.ForeColor = If(stokValue = 0, Color.Red, Color.Black)
            Else
                Label1.Text = "Stok tidak ditemukan"
                Label1.ForeColor = Color.Gray
            End If
            dr.Close()
        Catch ex As Exception
            MsgBox("Error saat menampilkan stok: " & ex.Message)
        End Try
    End Sub



    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Try
            Dim selectedProduct As String = ComboBox3.SelectedItem.ToString()

            ' Ambil stok total dari semua baris produk yang sama
            Dim cmd As New OdbcCommand("SELECT SUM(stok) AS total_stok FROM admin_product WHERE nama_produk = ?", conn)
            cmd.Parameters.AddWithValue("@nama_produk", selectedProduct)
            Dim dr As OdbcDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                Dim stokValue As Integer = Convert.ToInt32(dr("total_stok"))
                Label1.Text = stokValue.ToString()
                Label1.ForeColor = If(stokValue = 0, Color.Red, Color.Black)
            Else
                Label1.Text = "Belum ada produk dipilih"
                Label1.ForeColor = Color.Gray
            End If
            dr.Close()

            ' Ambil semua tipe unik dari produk ini
            Dim tipeCmd As New OdbcCommand("SELECT DISTINCT tipe FROM admin_product WHERE nama_produk = ?", conn)
            tipeCmd.Parameters.AddWithValue("@nama_produk", selectedProduct)
            Dim tipeDr As OdbcDataReader = tipeCmd.ExecuteReader()

            ComboBox1.Items.Clear()
            While tipeDr.Read()
                ComboBox1.Items.Add(tipeDr("tipe").ToString())
            End While
            tipeDr.Close()

            ' Auto-select tipe pertama jika ada
            If ComboBox1.Items.Count > 0 Then
                ComboBox1.SelectedIndex = 0
            End If

        Catch ex As Exception
            MsgBox("Error saat mengambil data produk: " & ex.Message)
        End Try
    End Sub



    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            ' Pastikan ada produk dan tipe yang dipilih
            If ComboBox3.SelectedIndex = -1 Or ComboBox1.SelectedIndex = -1 Then
                MsgBox("Pilih produk dan tipe terlebih dahulu.", MsgBoxStyle.Exclamation, "Error")
                Return
            End If

            ' Ambil produk dan tipe yang dipilih
            Dim selectedProduct As String = ComboBox3.SelectedItem.ToString()
            Dim selectedTipe As String = ComboBox1.SelectedItem.ToString()

            ' Ambil jumlah stok yang ingin ditambah dari TextBox3
            Dim tambahStok As Integer
            If Not Integer.TryParse(TextBox3.Text, tambahStok) OrElse tambahStok <= 0 Then
                MsgBox("Masukkan jumlah stok yang valid untuk ditambahkan.", MsgBoxStyle.Exclamation, "Error")
                Return
            End If

            ' Ambil stok saat ini untuk produk + tipe yang dipilih
            Dim cmd As New OdbcCommand("SELECT stok FROM admin_product WHERE nama_produk = ? AND tipe = ?", conn)
            cmd.Parameters.AddWithValue("@nama_produk", selectedProduct)
            cmd.Parameters.AddWithValue("@tipe", selectedTipe)

            Dim currentStok As Integer = 0
            Dim dr As OdbcDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                currentStok = Convert.ToInt32(dr("stok"))
            Else
                MsgBox("Data produk dan tipe tidak ditemukan.", MsgBoxStyle.Exclamation, "Error")
                dr.Close()
                Return
            End If
            dr.Close()

            ' Hitung stok baru
            Dim newStok As Integer = currentStok + tambahStok

            ' Update stok hanya untuk produk + tipe tersebut
            Dim updateCmd As New OdbcCommand("UPDATE admin_product SET stok = ? WHERE nama_produk = ? AND tipe = ?", conn)
            updateCmd.Parameters.AddWithValue("@stok", newStok)
            updateCmd.Parameters.AddWithValue("@nama_produk", selectedProduct)
            updateCmd.Parameters.AddWithValue("@tipe", selectedTipe)
            updateCmd.ExecuteNonQuery()

            ' Tampilkan stok baru
            Label1.Text = newStok.ToString()
            Label1.ForeColor = If(newStok = 0, Color.Red, Color.Black)
            TextBox3.Clear()

            MsgBox("Stok berhasil ditambahkan!", MsgBoxStyle.Information, "Berhasil")

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
End Class
