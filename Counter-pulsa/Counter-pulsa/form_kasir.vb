﻿Imports System.Data.Odbc

Imports System.Globalization

'refactor keluar harga jual, input uang pembeli dan kembalian
Public Class form_kasir
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Private selectedNamaProduk As String = ""

    Public loggedInUserUsername As String

    ' Koneksi ke database
    Sub test_conn()
        Try
            If conn Is Nothing Then
                mysql = "DSN=pulsa;" ' Sesuaikan dengan DSN milikmu
                conn = New OdbcConnection(mysql)
            End If

            ' Open the connection only if it's not already open
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub


    Sub loadproduk()
        Try
            test_conn() ' Pastikan koneksi dibuka sebelum eksekusi query
            Using cmd As New OdbcCommand("SELECT nama_produk FROM admin_product WHERE stok > 0", conn)
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
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close() ' Menutup koneksi setelah selesai
            End If
        End Try
    End Sub

    Sub tampilData()
        Try
            test_conn() ' Pastikan koneksi dibuka sebelum menjalankan query
            Dim query As String = "SELECT id, nama_produk, harga_jual, stok, tipe, tanggal_restock FROM admin_product WHERE stok > 0"
            Using cmd As New OdbcCommand(query, conn)
                da = New OdbcDataAdapter(cmd)
                ds = New DataSet()
                da.Fill(ds, "admin_product")

                If ds.Tables("admin_product").Rows.Count = 0 Then
                    MsgBox("Tidak ada data produk yang tersedia.", vbExclamation)
                    DataGridView1.DataSource = Nothing
                Else
                    DataGridView1.DataSource = ds.Tables("admin_product")

                    ' Ganti header dan gaya
                    With DataGridView1
                        ' position text
                        .Columns("id").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("nama_produk").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("harga_jual").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("stok").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("tipe").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .Columns("tanggal_restock").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                        ' label data
                        .Columns("id").HeaderText = "Kode Produk"
                        .Columns("nama_produk").HeaderText = "Nama Produk"
                        .Columns("harga_jual").HeaderText = "Harga Jual"
                        .Columns("stok").HeaderText = "Stok Tersedia"
                        .Columns("tipe").HeaderText = "Paket Harga"
                        .Columns("tanggal_restock").HeaderText = "Tanggal Restock"
                        ' Header 
                        .BackgroundColor = Color.White
                        .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
                        .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 85, 155)
                        .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                        .EnableHeadersVisualStyles = False

                        ' Cell style
                        .DefaultCellStyle.Font = New Font("Segoe UI", 10)
                        .DefaultCellStyle.BackColor = Color.White
                        .DefaultCellStyle.ForeColor = Color.Black
                        .DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255)
                        .DefaultCellStyle.SelectionForeColor = Color.Black

                        ' Alternating rows
                        .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue

                        ' Grid behavior
                        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                        .RowTemplate.Height = 30
                        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                        .ReadOnly = True
                        .MultiSelect = False

                        ' Grid border settings
                        .ScrollBars = ScrollBars.None
                        .BorderStyle = BorderStyle.None
                        .CellBorderStyle = DataGridViewCellBorderStyle.Single           ' Garis antar cell
                        .GridColor = Color.LightGray                                   ' Warna garis pemisah
                        .RowHeadersVisible = False

                        ' Disable user interaction
                        .AllowUserToAddRows = False
                        .AllowUserToDeleteRows = False
                        .AllowUserToResizeRows = False
                        .AllowUserToResizeColumns = False
                        .AllowUserToOrderColumns = False
                    End With

                End If
            End Using
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close() ' Menutup koneksi setelah selesai
        End Try
    End Sub
    Private Sub Form3_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim nomorHp As String = TextBox2.Text.Trim()

        ' Bersihkan semua input jika nomor terlalu pendek
        If nomorHp.Length < 4 Then
            ComboBox1.Items.Clear()
            ComboBox1.SelectedIndex = -1
            ComboBox2.Items.Clear()
            ComboBox2.SelectedIndex = -1
            TextBox3.Clear()
        End If
        ' Mode fullscreen total
        Me.FormBorderStyle = FormBorderStyle.None
        Me.Bounds = Screen.PrimaryScreen.Bounds
        Me.TopMost = True ' Pastikan berada di atas semua jendela
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ' UI setup
        TextBox3.ReadOnly = True
        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToOrderColumns = False
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        tampilData()
        Label5.Text = "Operator : " & loggedInUserUsername
    End Sub

    Private Sub DataGridView1_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        If DataGridView1.Columns(e.ColumnIndex).Name = "id" Then
            If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                Dim id As Integer = Convert.ToInt32(e.Value)
                e.Value = "LP" & id.ToString("D5")
                e.FormattingApplied = True
            End If
        End If

        If DataGridView1.Columns(e.ColumnIndex).Name = "tipe" Then
            If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                Dim tipe As Integer = Convert.ToInt32(e.Value)
                e.Value = tipe.ToString("N0")
                e.FormattingApplied = True
            End If
        End If

        If DataGridView1.Columns(e.ColumnIndex).Name = "harga_jual" Then
            Try
                Dim hargaModal As Integer = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("harga_kulak").Value)
                Dim hargaJual As Integer = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("harga_jual").Value)
                If hargaJual < hargaModal Then
                    e.CellStyle.ForeColor = Color.Blue
                End If
            Catch
                ' Abaikan jika gagal parsing
            End Try
        End If
    End Sub

    ' Tombol Clear
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox1.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
    End Sub

    ' Tutup Form
    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs)
        Application.Exit()
    End Sub

    ' cek produk yang ada di provider
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectedIndexChanged

        If ComboBox1.SelectedItem Is Nothing Then Exit Sub

        Dim namaProdukDipilih As String = ComboBox1.SelectedItem.ToString()

        Try
            test_conn()

            Using cmd As New OdbcCommand("SELECT tipe, harga_jual FROM admin_product WHERE nama_produk = ? AND stok > 0", conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProdukDipilih)

                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear()

                    While dr.Read()
                        Dim tipe As Integer = Convert.ToInt32(dr("tipe"))
                        Dim hargaJual As Integer = Convert.ToInt32(dr("harga_jual"))
                        Dim displayTipe As String = tipe.ToString("N0")
                        ComboBox2.Items.Add(displayTipe)
                    End While

                    If ComboBox2.Items.Count > 0 Then
                        ComboBox2.SelectedIndex = 0 ' Auto pilih tipe pertama
                    End If
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Gagal mengambil data produk: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Function CleanNumberString(ByVal input As String) As String
        Return input.Replace(".", "").Replace(",", "").Trim()
    End Function

    ' Tombol Proses
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim nomorHp As String = TextBox2.Text.Trim()

        ' Validasi input uang pembeli
        Dim uangPembeli As Integer
        Dim uangInput As String = New String(TextBox1.Text.Where(Function(c) Char.IsDigit(c)).ToArray())
        If Not Integer.TryParse(uangInput, uangPembeli) Then
            MsgBox("Uang pembeli tidak valid.", MsgBoxStyle.Exclamation)
            Return
        End If

        ' Validasi umum
        If String.IsNullOrWhiteSpace(nomorHp) OrElse ComboBox1.SelectedItem Is Nothing OrElse ComboBox2.SelectedItem Is Nothing Then
            MsgBox("Mohon lengkapi semua data terlebih dahulu.")
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            TextBox1.Clear()
            TextBox3.Clear()
            TextBox2.Clear()
            Return
        End If

        If Not nomorHp.StartsWith("08") OrElse nomorHp.Length < 10 Then
            MsgBox("Nomor HP tidak valid! Harus diawali dengan 08 dan panjang minimal 10 digit.")
            TextBox2.Focus()
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            TextBox1.Clear()
            TextBox3.Clear()
            Return
        End If

        ' Validasi provider dengan nomor
        Dim namaProdukDipilih As String = ComboBox1.SelectedItem.ToString().ToLower()
        If (namaProdukDipilih.Contains("telkomsel") AndAlso Not (nomorHp.StartsWith("0811") OrElse nomorHp.StartsWith("0812") OrElse nomorHp.StartsWith("0813") OrElse nomorHp.StartsWith("0821") OrElse nomorHp.StartsWith("0822") OrElse nomorHp.StartsWith("0823") OrElse
        nomorHp.StartsWith("0852") OrElse nomorHp.StartsWith("0853") OrElse nomorHp.StartsWith("0851"))) OrElse (namaProdukDipilih.Contains("xl") AndAlso Not (nomorHp.StartsWith("0817") OrElse nomorHp.StartsWith("0818") OrElse nomorHp.StartsWith("0819") OrElse
        nomorHp.StartsWith("0859") OrElse nomorHp.StartsWith("0877") OrElse nomorHp.StartsWith("0878"))) OrElse
(namaProdukDipilih.Contains("indosat") AndAlso Not (
        nomorHp.StartsWith("0855") OrElse nomorHp.StartsWith("0856") OrElse nomorHp.StartsWith("0857") OrElse
        nomorHp.StartsWith("0858") OrElse nomorHp.StartsWith("0814") OrElse nomorHp.StartsWith("0815") OrElse
        nomorHp.StartsWith("0816"))) OrElse (namaProdukDipilih.Contains("axis") AndAlso Not (nomorHp.StartsWith("0838") OrElse nomorHp.StartsWith("0831") OrElse nomorHp.StartsWith("0832") OrElse
        nomorHp.StartsWith("0833"))) OrElse (namaProdukDipilih.Contains("tri") AndAlso Not (nomorHp.StartsWith("0895") OrElse nomorHp.StartsWith("0896") OrElse nomorHp.StartsWith("0897") OrElse
        nomorHp.StartsWith("0898") OrElse nomorHp.StartsWith("0899"))) OrElse (namaProdukDipilih.Contains("smartfren") AndAlso Not (nomorHp.StartsWith("0881") OrElse nomorHp.StartsWith("0882") OrElse nomorHp.StartsWith("0883") OrElse
        nomorHp.StartsWith("0884") OrElse nomorHp.StartsWith("0885") OrElse nomorHp.StartsWith("0886") OrElse nomorHp.StartsWith("0887") OrElse nomorHp.StartsWith("0888") OrElse nomorHp.StartsWith("0889"))) OrElse
   (namaProdukDipilih.Contains("byu") AndAlso Not (nomorHp.StartsWith("0851"))) Then

        End If

        ' Ambil informasi produk
        Dim productName As String = ComboBox1.SelectedItem.ToString()
        Dim hargaJual As Integer
        Dim hargaInput As String = New String(TextBox3.Text.Where(Function(c) Char.IsDigit(c)).ToArray())
        If Not Integer.TryParse(hargaInput, hargaJual) Then
            MsgBox("Harga jual tidak valid.")
            Return
        End If

        ' Tetapkan quantity tetap satu
        Dim qty As Integer = 1

        Dim totalHarga As Integer = hargaJual * qty
        If uangPembeli < totalHarga Then
            MsgBox("Saldo pembeli Tidak cukup", vbExclamation)
            Return
        End If

        Dim kembalian As Integer = uangPembeli - totalHarga
        Dim productId As Integer = -1

        Try
            ' Menghubungkan ke database
            test_conn()
            If conn.State <> ConnectionState.Open Then conn.Open()

            Dim trans As OdbcTransaction = conn.BeginTransaction()
            Dim tipeString As String = ComboBox2.SelectedItem.ToString()
            If tipeString Is Nothing Then
                MsgBox("Tipe produk belum dipilih.")
                Return
            End If

            tipeString = CleanNumberString(tipeString)

            Dim tipeProduk As Integer
            If Not Integer.TryParse(tipeString, tipeProduk) Then
                MsgBox("Tipe produk tidak valid.")
                Return
            End If

            ' cek produk
            Using cmdGetId As New OdbcCommand("SELECT * FROM admin_product WHERE nama_produk = ? AND harga_jual = ? AND tipe = ?", conn, trans)
                cmdGetId.Parameters.AddWithValue("@nama_produk", productName)
                cmdGetId.Parameters.AddWithValue("@harga_jual", hargaJual)
                cmdGetId.Parameters.AddWithValue("@tipe", tipeProduk)
                Dim result = cmdGetId.ExecuteScalar()
                If result Is Nothing Then
                    MsgBox("Produk tidak ditemukan.")
                    trans.Rollback()
                    Return
                End If
                productId = Convert.ToInt32(result)
            End Using

            ' Check stock
            Dim availableStock As Integer = 0
            Using cmdCheckStock As New OdbcCommand("SELECT stok FROM admin_product WHERE id = ?", conn, trans)
                cmdCheckStock.Parameters.AddWithValue("@id", productId)
                availableStock = Convert.ToInt32(cmdCheckStock.ExecuteScalar())
            End Using

            If availableStock < qty Then
                MsgBox("Stok tidak mencukupi. Tersisa: " & availableStock)
                trans.Rollback()
                Return
            End If

            ' Update stock
            Using cmdUpdate As New OdbcCommand("UPDATE admin_product SET stok = stok - ? WHERE id = ?", conn, trans)
                cmdUpdate.Parameters.AddWithValue("@stok", qty)
                cmdUpdate.Parameters.AddWithValue("@id", productId)
                cmdUpdate.ExecuteNonQuery()
            End Using
            Try
                ' Dapatkan ID produk dari tipe harga
                Using cmdGetId As New OdbcCommand("SELECT id FROM admin_product WHERE tipe = ?", conn, trans)
                    cmdGetId.Parameters.AddWithValue("@tipe", tipeString)
                    Dim result = cmdGetId.ExecuteScalar()
                    If result Is Nothing Then
                        MsgBox("Produk tidak ditemukan.")
                        trans.Rollback()
                        Return
                    End If
                    productId = Convert.ToInt32(result)
                End Using

                ' Cek stok

                Using cmdCheckStock As New OdbcCommand("SELECT stok FROM admin_product WHERE id = ?", conn, trans)
                    cmdCheckStock.Parameters.AddWithValue("@id", productId)
                    availableStock = Convert.ToInt32(cmdCheckStock.ExecuteScalar())
                End Using

                If availableStock < qty Then
                    MsgBox("Stok tidak mencukupi. Tersisa: " & availableStock, MsgBoxStyle.Exclamation)
                    trans.Rollback()
                    Return
                End If

                ' Mengambil jumlah uang pembeli dari TextBox1
                Dim uang_pembeli As Integer = CInt(TextBox1.Text)

                ' Simpan transaksi
                Using cmdInsert As New OdbcCommand("INSERT INTO transaksi (nomor_tujuan, product_id, harga, quantity, product, kasir_username, uang_pembeli) VALUES (?, ?, ?, ?, ?, ?, ?)", conn, trans)
                    cmdInsert.Parameters.AddWithValue("@nomor_tujuan", nomorHp)
                    cmdInsert.Parameters.AddWithValue("@product_id", productId)
                    cmdInsert.Parameters.AddWithValue("@harga", hargaJual)
                    cmdInsert.Parameters.AddWithValue("@quantity", qty)
                    cmdInsert.Parameters.AddWithValue("@product", productName)
                    cmdInsert.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                    cmdInsert.Parameters.AddWithValue("@uang_pembeli", uang_pembeli)
                    cmdInsert.ExecuteNonQuery()
                End Using

                ' Update stok
                ' Update stok secara eksplisit
                Using cmdUpdate As New OdbcCommand("UPDATE admin_product SET stok = stok WHERE id = ?", conn, trans)
                    cmdUpdate.Parameters.AddWithValue("@id", productId)
                    cmdUpdate.ExecuteNonQuery()
                End Using

                trans.Commit()

                ' Jika ada kembalian, tampilkan kembalian dalam pesan
                If kembalian > 0 Then
                    MsgBox("Pembelian berhasil!" & vbCrLf & vbCrLf &
                            "Id Produk    : " & "LP" & productId & vbCrLf &
                           "Nomor Tujuan  : " & nomorHp & vbCrLf &
                           "Produk        : " & productName & vbCrLf &
                           "Total Harga   : Rp " & totalHarga.ToString("N0") & vbCrLf &
                           "Uang Pembeli  : Rp " & uangPembeli.ToString("N0") & vbCrLf &
                           "Kembalian     : Rp " & kembalian.ToString("N0"))
                    TextBox1.Clear()
                    TextBox2.Clear()
                    TextBox3.Clear()
                    ComboBox1.Items.Clear()
                    ComboBox2.Items.Clear()
                Else
                    ' Jika tidak ada kembalian, tampilkan pesan tanpa kembalian
                    MsgBox("Pembelian berhasil!" & vbCrLf & vbCrLf &
                           "Id Produk    : " & "LP" & productId & vbCrLf &
                           "Nomor Tujuan : " & nomorHp & vbCrLf &
                           "Produk        : " & productName & vbCrLf &
                           "Total Harga   : Rp " & totalHarga.ToString("N0") & vbCrLf &
                           "Uang Pembeli  : Rp " & uangPembeli.ToString("N0"))
                    TextBox1.Clear()
                    TextBox2.Clear()
                    TextBox3.Clear()
                    ComboBox1.Items.Clear()
                    ComboBox2.Items.Clear()
                End If

                ' Refresh data
                namaProdukDipilih = ""
                tampilData()
            Catch exTrans As Exception
                trans.Rollback()
                MsgBox("Terjadi kesalahan transaksi: " & exTrans.Message)
                LogError(exTrans)
            End Try

        Catch ex As Exception
            MsgBox("Kesalahan: " & ex.Message)
            LogError(ex)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Sub


    Private Sub LogError(ByVal ex As Exception)
        Dim logFilePath As String = "C:\Users\Antartika - Rafi\Documents\pdesk\Counter-pulsa\logfile.txt" ' Ganti dengan path yang sesuai
        Try
            Using writer As New System.IO.StreamWriter(logFilePath, True)
                writer.WriteLine("=====================================")
                writer.WriteLine("Date: " & DateTime.Now.ToString())
                writer.WriteLine("Error Message: " & ex.Message)
                writer.WriteLine("Stack Trace: " & ex.StackTrace)
                writer.WriteLine("=====================================")
            End Using
        Catch ioEx As Exception
            MsgBox("Terjadi kesalahan saat menulis log: " & ioEx.Message)
        End Try
    End Sub


    ' angka
    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox2.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox1.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox1.SelectedItem Is Nothing Or ComboBox2.SelectedItem Is Nothing Then Exit Sub

        Dim namaProduk As String = ComboBox1.SelectedItem.ToString()
        Dim tipeDipilih As String = ComboBox2.SelectedItem.ToString().Replace(".", "").Replace(",", "") ' Bersihkan format

        Try
            test_conn()

            Using cmd As New OdbcCommand("SELECT harga_jual FROM admin_product WHERE nama_produk = ? AND tipe = ?", conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
                cmd.Parameters.AddWithValue("@tipe", Convert.ToInt32(tipeDipilih))

                Dim harga = cmd.ExecuteScalar()
                If harga IsNot Nothing Then
                    TextBox3.Text = Convert.ToInt32(harga).ToString("N0")
                End If
            End Using
        Catch ex As Exception
            MsgBox("Gagal mengambil harga jual: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    ' Saat produk dipilih, tampilkan harga
    Private Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox2.TextChanged
        Dim nomorHp As String = TextBox2.Text.Trim()

        ' Bersihkan semua input jika nomor terlalu pendek
        If nomorHp.Length < 4 Then
            ComboBox1.Items.Clear()
            ComboBox1.SelectedIndex = -1
            ComboBox2.Items.Clear()
            ComboBox2.SelectedIndex = -1
            TextBox3.Clear()
            Exit Sub
        End If

        Dim prefix As String = nomorHp.Substring(0, 4)
        Dim provider As String = ""

        ' Deteksi provider dari prefix nomor
        Select Case prefix
            Case "0811", "0812", "0813", "0821", "0822", "0852", "0853"
                provider = "telkomsel"
            Case "0817", "0818", "0819", "0859", "0877", "0878"
                provider = "xl"
            Case "0856", "0857", "0858", "0814", "0815", "0816"
                provider = "indosat"
            Case "0831", "0832", "0833", "0838"
                provider = "axis"
            Case "0895", "0897", "0898", "0899"
                provider = "tri"
            Case "0896"
                provider = "byu"
            Case "0881", "0882", "0883", "0884", "0885", "0886", "0887", "0888", "0889"
                provider = "smartfren"
        End Select

        ' Tampilkan produk yang cocok dengan provider
        If provider <> "" Then
            Try
                test_conn()
                Using cmd As New OdbcCommand("SELECT nama_produk FROM admin_product WHERE stok > 0 AND LOWER(nama_produk) LIKE ? GROUP BY nama_produk", conn)
                    cmd.Parameters.AddWithValue("@p", "%" & provider & "%")

                    Using dr = cmd.ExecuteReader()
                        ComboBox1.Items.Clear()
                        ComboBox2.Items.Clear()
                        TextBox3.Clear()

                        While dr.Read()
                            ComboBox1.Items.Add(dr("nama_produk").ToString())
                        End While

                        If ComboBox1.Items.Count = 1 Then
                            ComboBox1.SelectedIndex = 0 ' Auto-select jika cuma 1
                        Else
                            ComboBox1.SelectedIndex = -1
                        End If
                    End Using
                End Using



            Catch ex As Exception
                MsgBox("Gagal memuat produk berdasarkan provider: " & ex.Message)
            Finally
                If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
            End Try
        Else
            ' Provider tidak dikenali, kosongkan semua input
            ComboBox1.Items.Clear()
            ComboBox1.SelectedIndex = -1
            ComboBox2.Items.Clear()
            ComboBox2.SelectedIndex = -1
            TextBox3.Clear()
        End If
    End Sub


    Private Sub logout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        form_login.Show()
        loggedInUserUsername = ""
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TextBox3_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Hide()
        form_login.Show()
        loggedInUserUsername = ""
        form_login.TextBox1.Clear()
        form_login.TextBox2.Clear()
        form_login.ComboBox1.SelectedIndex = -1
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
        Dim previousForm As New laporan_kasir()
        previousForm.previousForm = Me
        laporan_kasir.Show()
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub
End Class
