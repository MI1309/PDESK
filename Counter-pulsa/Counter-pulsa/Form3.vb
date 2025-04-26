Imports System.Data.Odbc

Public Class Form3

    Public loggedInUserUsername As String ' Username kasir yang login
    Public previousForm As Form

    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Private selectedNamaProduk As String = "" ' Nama produk yang dipilih di tabel

    ' Koneksi ke database
    Sub test_conn()
        Try
            mysql = "DSN=pulsa;" ' Ganti sesuai DSN kamu
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub
    Sub TulisLogError(ByVal pesan As String)
        Dim path As String = "log_error.txt"
        Dim logPesan As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " - " & pesan
        Try
            My.Computer.FileSystem.WriteAllText(path, logPesan & Environment.NewLine, True)
        Catch ex As Exception
            MsgBox("Gagal menulis log error: " & ex.Message)
        End Try
    End Sub



    Function ValidasiInput() As Boolean
        If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           ComboBox1.SelectedIndex = -1 OrElse
           ComboBox2.SelectedIndex = -1 Then

            MsgBox("Semua field harus diisi", vbExclamation)
            Return False
        End If

        Dim hargaJual, hargaModal As Integer

        If Not Integer.TryParse(TextBox2.Text, hargaJual) Then
            MsgBox("nomer hp harus berupa angka") : Return False
        ElseIf hargaJual < 1 Then
            MsgBox("berikan nomer hp yang valid") : Return False
        End If

        If Not Integer.TryParse(TextBox3.Text, hargaModal) Then
            MsgBox("quantity harus diisi dengan angka") : Return False
        ElseIf hargaModal < 1 Then
            MsgBox("quantity tidak valid") : Return False
        End If

        Return True
    End Function

    Sub loadproduk()
        Try
            test_conn()
            Using cmd As New OdbcCommand("SELECT nama_produk FROM admin_product WHERE kasir_username = ?", conn)
                cmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                Using dr = cmd.ExecuteReader()
                    ComboBox1.Items.Clear()
                    While dr.Read()
                        ComboBox1.Items.Add(dr("nama_produk").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal memuat data produk: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub loadharga()
        Try
            test_conn()
            Using cmd As New OdbcCommand("SELECT harga_modal FROM admin_product WHERE kasir_username = ?", conn)
                cmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear()
                    While dr.Read()
                        ComboBox2.Items.Add(dr("harga_modal").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal memuat data harga: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' Menampilkan data produk berdasarkan kasir
    Sub tampilData()
        Try
            test_conn()
            Dim query As String = "SELECT nama_produk, harga_jual, stok, tanggal_restock FROM admin_product WHERE kasir_username = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                da = New OdbcDataAdapter(cmd)
                ds = New DataSet()
                da.Fill(ds, "admin_product")
                DataGridView1.DataSource = ds.Tables("admin_product")
            End Using

            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' Saat baris diklik di DataGridView
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            selectedNamaProduk = DataGridView1.Rows(e.RowIndex).Cells("nama_produk").Value.ToString()
        End If
    End Sub

    ' Tombol Hapus (Button2)
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" And TextBox2.Text = "" And TextBox3.Text = "" And ComboBox1.Text = "" And ComboBox2.Text = "" Then
            MsgBox("tidak ada data yang diclear")
        End If
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        ComboBox1.Text = ""
        ComboBox2.Text = ""
    End Sub

    ' Tutup form (Button4)
    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Sub setupDataGridView()
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
    ' Saat form dimuat
    Private Sub Form3_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Label5.Text = "Kasir : " & loggedInUserUsername
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        loadproduk()
        tampilData()
        setupDataGridView()
        loadharga()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not ValidasiInput() Then Exit Sub

        Try
            test_conn()

            Dim nomor As Integer = CInt(TextBox2.Text)
            Dim stok As Integer = CInt(TextBox3.Text)
            Dim kartu As String = ComboBox1.Text
            Dim harga As Integer = CInt(ComboBox2.SelectedItem)
            Dim waktuTransaksi As DateTime = DateTime.Now
            Dim productId As Integer = 0
            Dim stokTersedia As Integer = 0

            ' Cari ID produk dan stok yang tersedia berdasarkan nama produk yang dipilih
            Dim getIdQuery As String = "SELECT id, stok FROM admin_product WHERE nama_produk = ? AND kasir_username = ?"
            Using getIdCmd As New OdbcCommand(getIdQuery, conn)
                getIdCmd.Parameters.AddWithValue("@nama_produk", kartu)
                getIdCmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                Using dr = getIdCmd.ExecuteReader()
                    If dr.Read() Then
                        productId = Convert.ToInt32(dr("id"))
                        stokTersedia = Convert.ToInt32(dr("stok"))
                    Else
                        MsgBox("Produk tidak ditemukan.")
                        Exit Sub
                    End If
                End Using
            End Using

            ' Validasi jika stok yang diminta lebih banyak dari stok yang tersedia
            If stok > stokTersedia Then
                MsgBox("Stok yang diminta melebihi stok yang tersedia. Stok tersedia: " & stokTersedia.ToString(), vbExclamation)
                Exit Sub
            ElseIf stok < 1 Then
                MsgBox("Jumlah stok yang dimasukkan tidak valid.", vbExclamation)
                Exit Sub
            End If

            ' Simpan transaksi
            Dim sql As String = "INSERT INTO transaksi (nama_pembeli, nomor_tujuan, product_id, harga, waktu_transaksi, quantity) VALUES (?, ?, ?, ?, ?, ?)"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama_pembeli", TextBox1.Text.Trim())
                cmd.Parameters.AddWithValue("@nomor_tujuan", nomor)
                cmd.Parameters.AddWithValue("@product_id", productId)
                cmd.Parameters.AddWithValue("@harga", harga)
                cmd.Parameters.AddWithValue("@waktu_transaksi", waktuTransaksi)
                cmd.Parameters.AddWithValue("@quantity", stok)
                cmd.ExecuteNonQuery()
            End Using

            ' Update stok produk setelah transaksi
            Dim updateStokSql As String = "UPDATE admin_product SET stok = stok - ? WHERE id = ?"
            Using updateCmd As New OdbcCommand(updateStokSql, conn)
                updateCmd.Parameters.AddWithValue("@stok", stok)
                updateCmd.Parameters.AddWithValue("@id", productId)
                updateCmd.ExecuteNonQuery()
            End Using

            ' Cek jika stok kosong, hapus produk
            Dim cekStokQuery As String = "SELECT stok FROM admin_product WHERE id = ?"
            Using cekCmd As New OdbcCommand(cekStokQuery, conn)
                cekCmd.Parameters.AddWithValue("@id", productId)
                Using dr = cekCmd.ExecuteReader()
                    If dr.Read() AndAlso Convert.ToInt32(dr("stok")) <= 0 Then
                        ' Hapus produk jika stok <= 0
                        Dim deleteProdukQuery As String = "DELETE FROM admin_product WHERE id = ?"
                        Using deleteCmd As New OdbcCommand(deleteProdukQuery, conn)
                            deleteCmd.Parameters.AddWithValue("@id", productId)
                            deleteCmd.ExecuteNonQuery()
                        End Using

                        MsgBox("Produk telah habis dan dihapus dari daftar.")
                    End If
                End Using
            End Using

            MsgBox("Produk berhasil ditambahkan ke laporan dan stok diperbarui.")
            tampilData()
        Catch ex As Exception
            MsgBox("Gagal menyimpan transaksi: " & ex.Message)
            TulisLogError("Transaksi gagal: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


End Class
