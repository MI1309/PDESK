Imports System.Data.Odbc

Public Class Form5
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Public loggedInUserId As Integer ' Menyimpan ID User yang sedang login

    ' Koneksi ke database
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

    ' Menampilkan data dari database ke DataGridView
    Sub tampilData()
        Try
            test_conn()
            da = New OdbcDataAdapter("SELECT * FROM admin_product", conn)
            ds = New DataSet()
            da.Fill(ds, "admin_product")
            DataGridView1.DataSource = ds.Tables("admin_product")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells ' Kolom menyesuaikan isi
            conn.Close()
        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        End Try
    End Sub

    ' Ketika Form5 pertama kali dimuat
    Private Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampilData()
        With DataGridView1
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
            .EnableHeadersVisualStyles = False
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
            .RowTemplate.Height = 30
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With
    End Sub

    ' Fungsi untuk tombol Close
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    ' Fungsi untuk tombol Save
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            ' Validasi input - memastikan data valid
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Semua field harus diisi!", vbExclamation)
                Exit Sub
            End If

            ' Validasi hargaJual dan hargaModal agar hanya angka yang dimasukkan (termasuk desimal)
            Dim hargaJual As Decimal
            Dim hargaModal As Decimal

            ' Cek apakah harga jual valid
            If Not Decimal.TryParse(TextBox2.Text, hargaJual) AndAlso Not Decimal.TryParse(TextBox3.Text, hargaModal) Then
                MsgBox("Kedua harga harus berupa angka yang valid!", vbExclamation)
                TextBox2.Focus() ' Fokus pada kolom harga jual
                Exit Sub
            End If

            ' Cek apakah harga jual valid
            If Not Decimal.TryParse(TextBox2.Text, hargaJual) Then
                MsgBox("Harga jual harus berupa angka yang valid!", vbExclamation)
                TextBox2.Focus() ' Fokus pada kolom harga jual
                Exit Sub
            End If

            ' Cek apakah harga modal valid
            If Not Decimal.TryParse(TextBox3.Text, hargaModal) Then
                MsgBox("Harga modal harus berupa angka yang valid!", vbExclamation)
                TextBox3.Focus() ' Fokus pada kolom harga modal
                Exit Sub
            End If

            ' Ambil data dari TextBox
            Dim namaProduk As String = TextBox1.Text

            ' Koneksi ke database
            test_conn()

            ' Query insert
            Dim cmd As New OdbcCommand("INSERT INTO admin_product (nama_produk, harga, id_user) VALUES (?, ?, ?)", conn)
            cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
            cmd.Parameters.AddWithValue("@harga_jual", hargaJual) ' Menyimpan harga jual
            cmd.Parameters.AddWithValue("@harga_modal", hargaModal) ' Menyimpan harga jual
            cmd.Parameters.AddWithValue("@id_user", loggedInUserId)

            ' Eksekusi query
            cmd.ExecuteNonQuery()
            MsgBox("Data berhasil disimpan dan direkam atas nama user ID: " & loggedInUserId, vbInformation)

            conn.Close()
            tampilData() ' Refresh DataGridView untuk menampilkan data yang baru
        Catch ex As Exception
            MsgBox("Gagal menyimpan data: " & ex.Message, vbCritical)
        End Try
    End Sub
End Class
