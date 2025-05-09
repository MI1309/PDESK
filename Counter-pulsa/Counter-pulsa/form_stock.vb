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
            mysql = "SELECT nama_produk FROM admin_product"
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
        Label1.AutoSize = True
        Label1.Height = 30
        Label1.Width = 100
        Label1.BorderStyle = BorderStyle.FixedSingle
        'Label1.TextAlign = ContentAlignment.MiddleCenter
        'Label1.Location = New Point(301, 272)

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

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Try
            Dim selectedProduct As String = ComboBox3.SelectedItem.ToString()
            Dim cmd As New OdbcCommand("SELECT stok, tipe FROM admin_product WHERE nama_produk = ?", conn)
            cmd.Parameters.AddWithValue("@nama_produk", selectedProduct)

            Dim dr As OdbcDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                ' Ambil nilai stok dan tipe
                Dim stokValue As Integer = Convert.ToInt32(dr("stok"))
                Dim tipeValue As String = dr("tipe").ToString()

                ' Tampilkan stok
                Label1.Text = stokValue.ToString()

                ' Warna merah jika stok 0
                If stokValue = 0 Then
                    Label1.ForeColor = Color.Red
                Else
                    Label1.ForeColor = Color.Black
                End If

                ' Tampilkan tipe
                ComboBox1.Items.Clear()
                ComboBox1.Items.Add(tipeValue)
                ComboBox1.SelectedIndex = 0
            Else
                ' Jika tidak ditemukan, tampilkan pesan default
                Label1.Text = "Belum ada produk dipilih"
                Label1.ForeColor = Color.Gray
            End If

            dr.Close()
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
            ' Pastikan ada produk yang dipilih
            If ComboBox3.SelectedIndex = -1 Then
                MsgBox("Pilih produk terlebih dahulu.", MsgBoxStyle.Exclamation, "Error")
                Return
            End If

            ' Ambil produk yang dipilih dari ComboBox3
            Dim selectedProduct As String = ComboBox3.SelectedItem.ToString()

            ' Ambil jumlah stok yang ingin ditambah dari TextBox (misalnya TextBox2)
            Dim tambahStok As Integer
            If Not Integer.TryParse(TextBox3.Text, tambahStok) OrElse tambahStok <= 0 Then
                MsgBox("Masukkan jumlah stok yang valid untuk ditambahkan.", MsgBoxStyle.Exclamation, "Error")
                Return
            End If

            ' Query untuk mengambil stok saat ini
            Dim cmd As New OdbcCommand("SELECT stok FROM admin_product WHERE nama_produk = ?", conn)
            cmd.Parameters.AddWithValue("@nama_produk", selectedProduct)

            Dim currentStok As Integer = 0
            Dim dr As OdbcDataReader = cmd.ExecuteReader()

            If dr.Read() Then
                ' Ambil nilai stok saat ini
                currentStok = Convert.ToInt32(dr("stok"))
            End If
            dr.Close()

            ' Hitung stok baru
            Dim newStok As Integer = currentStok + tambahStok

            ' Update stok di database
            Dim updateCmd As New OdbcCommand("UPDATE admin_product SET stok = ? WHERE nama_produk = ?", conn)
            updateCmd.Parameters.AddWithValue("@stok", newStok)
            updateCmd.Parameters.AddWithValue("@nama_produk", selectedProduct)
            updateCmd.ExecuteNonQuery()

            ' Tampilkan stok baru di Label1
            Label1.Text = newStok.ToString()

            ' Sesuaikan warna stok: merah jika stok = 0
            If newStok = 0 Then
                Label1.ForeColor = Color.Red
            Else
                Label1.ForeColor = Color.Black
            End If
            TextBox3.Clear()

            MsgBox("Stok berhasil ditambahkan!", MsgBoxStyle.Information, "Berhasil")

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
End Class
