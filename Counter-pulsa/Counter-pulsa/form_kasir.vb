Imports System.Data.Odbc

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
            Dim query As String = "SELECT id, nama_produk, harga_jual, stok, tanggal_restock FROM admin_product WHERE stok > 0"
            Using cmd As New OdbcCommand(query, conn)
                da = New OdbcDataAdapter(cmd)
                ds = New DataSet()
                da.Fill(ds, "admin_product")

                If ds.Tables("admin_product").Rows.Count = 0 Then
                    MsgBox("Tidak ada data produk yang tersedia.", vbExclamation)
                    DataGridView1.DataSource = Nothing
                Else
                    DataGridView1.DataSource = ds.Tables("admin_product")
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

        DataGridView1.ReadOnly = True
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToOrderColumns = False ' Optional: agar tidak bisa urutkan kolom
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        loadproduk()
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
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
    End Sub

    ' Tutup Form
    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Application.Exit()
    End Sub

    ' Tombol Proses
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Dim nomorHp As String = TextBox2.Text.Trim()

        ' Validasi input
        If String.IsNullOrWhiteSpace(nomorHp) OrElse ComboBox1.SelectedItem Is Nothing OrElse ComboBox2.SelectedItem Is Nothing Then
            MsgBox("Mohon lengkapi semua data terlebih dahulu.")
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()

            TextBox3.Clear()
            TextBox2.Clear()

            Return
        End If

        If Not nomorHp.StartsWith("08") OrElse nomorHp.Length < 10 Then
            MsgBox("Nomor HP tidak valid! Harus diawali dengan 08 dan panjang minimal 10 digit.")
            TextBox2.Focus()
            TextBox3.Clear()
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            Return
        End If

        Dim namaProdukDipilih As String = ComboBox1.SelectedItem.ToString().ToLower()
        If (namaProdukDipilih.Contains("telkomsel") AndAlso Not (nomorHp.StartsWith("0811") OrElse nomorHp.StartsWith("0812") OrElse nomorHp.StartsWith("0813") OrElse nomorHp.StartsWith("0821"))) OrElse _
             (namaProdukDipilih.Contains("xl") AndAlso Not (nomorHp.StartsWith("0817") OrElse nomorHp.StartsWith("0818") OrElse nomorHp.StartsWith("0859"))) OrElse _
            (namaProdukDipilih.Contains("indosat") AndAlso Not (nomorHp.StartsWith("0856") OrElse nomorHp.StartsWith("0857"))) OrElse _
          (namaProdukDipilih.Contains("axis") AndAlso Not nomorHp.StartsWith("0838")) OrElse _
   (namaProdukDipilih.Contains("tri") AndAlso Not nomorHp.StartsWith("0895")) OrElse _
   (namaProdukDipilih.Contains("smartfren") AndAlso Not nomorHp.StartsWith("0881")) OrElse _
   (namaProdukDipilih.Contains("byu") AndAlso Not nomorHp.StartsWith("0896")) Then
            MsgBox("Nomor HP tidak cocok dengan provider dari produk yang dipilih.")
            TextBox2.Focus() ' Fokuskan kembali ke input produk
            TextBox3.Clear() ' Kosongkan nomor HP yang dimasukkan
            Return ' Keluar dari prosedur atau event
        End If

        Dim productName As String = ComboBox1.SelectedItem.ToString()
        Dim harga As String = ComboBox2.SelectedItem.ToString().Replace(".", "")
        Dim qty As Integer = Val(TextBox3.Text)
        If qty <= 0 Then
            MsgBox("Jumlah tidak valid.")
            Return
        End If

        Dim productId As Integer = -1

        Try
            test_conn()
            If conn.State <> ConnectionState.Open Then conn.Open()

            Dim trans As OdbcTransaction = conn.BeginTransaction()

            Try
                ' Dapatkan ID produk
                Using cmdGetId As New OdbcCommand("SELECT id FROM admin_product WHERE nama_produk = ?", conn, trans)
                    cmdGetId.Parameters.AddWithValue("@nama_produk", productName)
                    Dim result = cmdGetId.ExecuteScalar()
                    If result Is Nothing Then
                        MsgBox("Produk tidak ditemukan.")
                        trans.Rollback()
                        Return
                    End If
                    productId = Convert.ToInt32(result)
                End Using

                ' Cek stok tersedia
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

                ' Simpan transaksi
                Using cmdInsert As New OdbcCommand("INSERT INTO transaksi (nomor_tujuan, product_id, harga, quantity, product, kasir_username) VALUES (?, ?, ?, ?, ?, ?)", conn, trans)
                    cmdInsert.Parameters.AddWithValue("@nomor_tujuan", nomorHp)
                    cmdInsert.Parameters.AddWithValue("@product_id", productId)
                    cmdInsert.Parameters.AddWithValue("@harga", harga)
                    cmdInsert.Parameters.AddWithValue("@quantity", qty)
                    cmdInsert.Parameters.AddWithValue("@product", productName)
                    cmdInsert.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                    cmdInsert.ExecuteNonQuery()
                End Using

                ' Kurangi stok
                Using cmdUpdate As New OdbcCommand("UPDATE admin_product SET stok = stok - ? WHERE id = ?", conn, trans)
                    cmdUpdate.Parameters.AddWithValue("@qty", qty)
                    cmdUpdate.Parameters.AddWithValue("@id", productId)
                    cmdUpdate.ExecuteNonQuery()
                End Using

                trans.Commit()

                MsgBox("Transaksi berhasil dan stok diperbarui.")
                loadproduk()
                tampilData()
                TextBox2.Clear()
                TextBox3.Clear()
                ComboBox1.Items.Clear()
                ComboBox2.Items.Clear()


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
            MsgBox("Hanya angka yang diperbolehkan !")
        End If
    End Sub
    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox3.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !")
        End If
    End Sub
    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox3.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    ' Saat produk dipilih, tampilkan harga
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem Is Nothing Then Exit Sub

        Dim namaProdukDipilih As String = ComboBox1.SelectedItem.ToString()

        Try
            test_conn()
            Dim query As String = "SELECT tipe FROM admin_product WHERE nama_produk = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProdukDipilih)
                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear()
                    If dr.Read() Then
                        Dim hargaJual As Integer = Convert.ToInt32(dr("tipe"))
                        Dim formattedHargaJual As String = hargaJual.ToString("N0")
                        ComboBox2.Items.Add(formattedHargaJual)
                        ComboBox2.SelectedIndex = 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal mengambil data produk: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub
    Private Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox2.TextChanged
        Dim nomorHp As String = TextBox2.Text.Trim()

        ' Kosongkan ComboBox jika input kosong atau terlalu pendek
        If nomorHp.Length < 4 Then
            ComboBox1.Items.Clear()
            ComboBox1.SelectedIndex = -1
            ComboBox2.Items.Clear()
            ComboBox2.SelectedIndex = -1
            Exit Sub
        End If

        Dim prefix = nomorHp.Substring(0, 4)
        Dim provider As String = ""

        ' Deteksi provider berdasarkan prefix
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
                ' Spesifikasikan pengecekan untuk BYU
                If nomorHp.StartsWith("0896") Then
                    provider = "byu"
                End If
            Case "0881", "0882", "0883", "0884", "0885", "0886", "0887", "0888", "0889"
                provider = "smartfren"
        End Select

        ' Filter produk berdasarkan provider
        If provider <> "" Then
            Try
                test_conn()
                Using cmd As New OdbcCommand("SELECT nama_produk FROM admin_product WHERE stok > 0 AND LOWER(nama_produk) LIKE ?", conn)
                    cmd.Parameters.AddWithValue("@p", "%" & provider & "%")
                    Using dr = cmd.ExecuteReader()
                        ComboBox1.Items.Clear()
                        ComboBox2.Items.Clear()
                        TextBox3.Clear()
                        While dr.Read()
                            ComboBox1.Items.Add(dr("nama_produk").ToString())
                        End While
                        If ComboBox1.Items.Count = 1 Then
                            ComboBox1.SelectedIndex = 0 ' Auto-pilih jika hanya 1 produk
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Gagal memuat produk berdasarkan provider: " & ex.Message)
            Finally
                If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
            End Try
        Else
            ' Jika provider tidak dikenali, kosongkan pilihan
            TextBox3.Text = ""
            ComboBox1.Items.Clear()
            ComboBox1.SelectedIndex = -1
            ComboBox2.Items.Clear()
            ComboBox2.SelectedIndex = -1
        End If
    End Sub

    Private Sub logout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles logout.Click
        Me.Close()
        form_login.Show()
    End Sub
End Class
