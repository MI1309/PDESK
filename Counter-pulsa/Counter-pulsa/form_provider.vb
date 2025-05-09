Imports System.Data.Odbc

Public Class form_provider
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Public loggedInUserId As Integer
    Private selectedId As Integer = -1
    ' load harga
    Private Sub loadTipeHarga()
        Dim tipeList As New Dictionary(Of String, Integer) From {
            {"5.000", 5000},
            {"10.000", 10000},
            {"20.000", 20000},
            {"50.000", 50000},
            {"100.000", 100000}
        }

        ComboBox2.DataSource = Nothing
        ComboBox2.DisplayMember = "Key"
        ComboBox2.ValueMember = "Value"
        ComboBox2.DataSource = New BindingSource(tipeList, Nothing)

        ComboBox2.SelectedIndex = -1
        ComboBox2.Text = ""
    End Sub




    ' validasi function
    Private Function ValidasiInputKosong(ByVal ParamArray controls() As Control) As Boolean
        Dim kosongList As New List(Of String)
        Dim controlDescriptions As New Dictionary(Of String, String) From {
            {"TextBox2", "Nama Produk"},
            {"TextBox3", "Harga Jual"},
            {"TextBox4", "Harga kulak"},
            {"ComboBox2", "Tipe Harga"}
        }

        ' Check for empty TextBox inputs
        For Each ctrl In controls
            If TypeOf ctrl Is TextBox AndAlso ctrl.Text.Trim() = "" Then
                If controlDescriptions.ContainsKey(ctrl.Name) Then
                    kosongList.Add(controlDescriptions(ctrl.Name))
                Else
                    kosongList.Add(ctrl.Name)
                End If
            End If
        Next

        ' Check ComboBox selection
        For Each ctrl In controls
            If TypeOf ctrl Is ComboBox Then
                Dim combo As ComboBox = DirectCast(ctrl, ComboBox)
                If combo.SelectedIndex = -1 Then
                    If controlDescriptions.ContainsKey(ctrl.Name) Then
                        kosongList.Add(controlDescriptions(ctrl.Name))
                    Else
                        kosongList.Add(ctrl.Name)
                    End If
                End If
            End If
        Next

        ' Show message if any fields are empty
        If kosongList.Count > 0 Then
            MsgBox("Field berikut belum diisi: " & String.Join(", ", kosongList))
            Return False
        End If

        Return True
    End Function


    ' Load form data
    Private Sub Form5_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        loadTipeHarga()
        TextBox1.ReadOnly = True
        LoadDataToGrid()
        loadidproduk()
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        setupDataGridView()
    End Sub


    ' Test database connection
    Sub test_conn()
        Try
            mysql = "DSN=pulsa;"
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

    ' Load nama produk ke ComboBox2 dari tabel produk
    Sub loadidproduk()
        Try
            test_conn()

            ' Mengambil ID terbesar dari database
            Using cmd As New OdbcCommand("SELECT MAX(id) FROM admin_product", conn)
                Dim result = cmd.ExecuteScalar()

                Dim nextId As Integer = 1 ' default jika tidak ada data

                If Not IsDBNull(result) Then
                    nextId = Convert.ToInt32(result) + 1
                End If

                ' Format ID menjadi LP0001, LP0002, dst
                TextBox1.Text = "LP" & nextId.ToString("D5") ' Format LP0001, LP0002, dst

            End Using

        Catch ex As Exception
            MsgBox("Gagal memuat ID produk berikutnya: " & ex.Message, vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub
    Sub LoadDataToGrid()
        Try
            test_conn()

            ' Ambil semua data, tapi hanya tampilkan kolom tertentu
            Dim query As String = "SELECT id, nama_produk, harga_jual, harga_kulak, tipe FROM admin_product"
            Dim cmd As New OdbcCommand(query, conn)
            Dim da As New OdbcDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            DataGridView1.DataSource = dt

            ' Atur header
            DataGridView1.Columns("nama_produk").HeaderText = "Nama Produk"

            ' Sembunyikan kolom lain (biar nggak kelihatan tapi tetap bisa dipakai)
            DataGridView1.Columns("id").Visible = False
            DataGridView1.Columns("harga_jual").Visible = False
            DataGridView1.Columns("harga_kulak").Visible = False
            DataGridView1.Columns("tipe").Visible = False

        Catch ex As Exception
            MsgBox("Gagal memuat data: " & ex.Message, vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    ' Setup DataGridView appearance and functionality
    Sub setupDataGridView()
        With DataGridView1
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .EnableHeadersVisualStyles = False
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
            .RowHeadersVisible = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None ' Jangan auto tinggi
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells ' Lebar kolom menyesuaikan isi
            .RowTemplate.Height = 25 ' Lebar baris kecil agar tampak horizontal
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .MultiSelect = False
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray
            .EnableHeadersVisualStyles = False
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 120, 200)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke
            .MultiSelect = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToOrderColumns = False
            .RowHeadersVisible = False
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells ' Atur tinggi baris otomatis
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill ' Kolom isi seluruh leba
            .RowTemplate.Height = 30
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With
    End Sub

    ' Validate input fields and return boolean
    Function ValidasiInput() As Boolean
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
            ComboBox2.SelectedIndex = -1 Then
            MsgBox("Semua field harus diisi dan kasir harus dipilih!", vbExclamation)
            Return False
        End If

        Dim hargaJual, hargaModal, tipe As Integer

        If Not Integer.TryParse(TextBox1.Text, hargaJual) OrElse Not Integer.TryParse(TextBox2.Text, hargaModal) OrElse Not Integer.TryParse(TextBox4.Text, tipe) Then
            MsgBox("Harus diisi angka untuk harga jual, harga modal, dan tipe.")
            Return False
        End If

        ' Ensure values are positive or valid
        If hargaJual < 1 OrElse hargaModal < 1 OrElse tipe < 0 Then
            MsgBox("Harga jual, harga modal harus lebih dari 0, dan tipe tidak boleh negatif.")
            Return False
        End If

        Return True
    End Function

    ' Insert new product
    ' Add new product
    ' Insert new product
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        ' Validasi input kosong
        If Not ValidasiInputKosong(TextBox2, TextBox3, TextBox4, ComboBox2) Then Exit Sub

        Try
            ' Ambil nilai input dari TextBox dan ComboBox
            Dim namaProduk As String = TextBox2.Text
            Dim hargaJual As Integer = CInt(TextBox3.Text)
            Dim hargaKulak As Integer = CInt(TextBox4.Text)
            ' Stok tidak perlu diambil dari input lagi
            Dim tipe As Integer = CInt(DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, Integer)).Value)

            ' Test koneksi ke database
            test_conn()

            ' Cek apakah produk sudah ada di database
            Dim checkCmd As New OdbcCommand("SELECT COUNT(*) FROM admin_product WHERE nama_produk = ?", conn)
            checkCmd.Parameters.AddWithValue("@nama_produk", namaProduk)
            Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If exists > 0 Then
                MsgBox("Produk sudah ada di penjualan", vbExclamation)
                Exit Sub
            End If

            ' Insert produk baru tanpa mengisi stok secara manual
            Dim sql As String = "INSERT INTO admin_product (nama_produk, harga_jual, harga_kulak, tanggal_restock, stok, tipe) " &
                                "VALUES (?, ?, ?, ?, 0, ?)" ' Stok default 0
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
                cmd.Parameters.AddWithValue("@harga_jual", hargaJual)
                cmd.Parameters.AddWithValue("@harga_kulak", hargaKulak)
                cmd.Parameters.AddWithValue("@tanggal_restock", DateTime.Now)
                cmd.Parameters.AddWithValue("@tipe", tipe)
                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Produk berhasil ditambahkan ke penjualan")
            resetForm() ' Reset form setelah data disimpan
            LoadDataToGrid() ' Load data terbaru ke Grid

        Catch ex As Exception
            ' Log error
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat menyimpan data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        ' Validasi input kosong
        If TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or ComboBox2.SelectedIndex = -1 Then
            MsgBox("Pilih data yang ingin diupdate di list dan pastikan semua kolom diisi.", vbExclamation)
            Exit Sub
        End If

        ' Validasi angka
        Dim hargaJual As Integer
        Dim hargaKulak As Integer
        If Not Integer.TryParse(TextBox3.Text, hargaJual) OrElse Not Integer.TryParse(TextBox4.Text, hargaKulak) Then
            MsgBox("Harga jual dan harga kulak harus berupa angka!", vbExclamation)
            Exit Sub
        End If

        If hargaJual < hargaKulak Then
            MsgBox("Harga jual tidak boleh lebih rendah dari harga kulak!", vbCritical)
            Exit Sub
        End If

        Try
            Dim idProduk As Integer = Integer.Parse(TextBox1.Text.Replace("LP", ""))
            Dim namaProduk As String = TextBox2.Text
            Dim tipe As Integer = CInt(DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, Integer)).Value)

            test_conn()

            ' Cek apakah data sama
            Dim sqlCek As String = "SELECT nama_produk, harga_jual, harga_kulak, tipe FROM admin_product WHERE id = ?"
            Using cmdCek As New OdbcCommand(sqlCek, conn)
                cmdCek.Parameters.AddWithValue("@id", idProduk)

                Using reader = cmdCek.ExecuteReader()
                    If reader.Read() Then
                        Dim oldNama = reader("nama_produk").ToString()
                        Dim oldHargaJual = Convert.ToInt32(reader("harga_jual"))
                        Dim oldHargaKulak = Convert.ToInt32(reader("harga_kulak"))
                        Dim oldTipe = Convert.ToInt32(reader("tipe"))

                        If oldNama = namaProduk AndAlso oldHargaJual = hargaJual AndAlso oldHargaKulak = hargaKulak AndAlso oldTipe = tipe Then
                            MsgBox("Data tetap. Tidak ada perubahan yang dilakukan.", vbInformation)
                            Exit Sub
                        End If
                    End If
                End Using
            End Using

            ' Lanjut update
            Dim sql As String = "UPDATE admin_product SET nama_produk = ?, harga_jual = ?, harga_kulak = ?, tipe = ? WHERE id = ?"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
                cmd.Parameters.AddWithValue("@harga_jual", hargaJual)
                cmd.Parameters.AddWithValue("@harga_kulak", hargaKulak)
                cmd.Parameters.AddWithValue("@tipe", tipe)
                cmd.Parameters.AddWithValue("@id", idProduk)
                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Produk berhasil diperbarui.")
            resetForm()
            LoadDataToGrid()
            loadidproduk()

        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat memperbarui data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub




    ' Delete selected product
    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        If String.IsNullOrEmpty(TextBox1.Text) Then
            MsgBox("Pilih produk yang ingin dihapus dulu, ya.", vbExclamation)
            Exit Sub
        End If

        ' Konfirmasi sebelum hapus
        Dim result = MessageBox.Show("Yakin ingin menghapus produk ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then Exit Sub

        Try
            Dim idProduk As Integer = Integer.Parse(TextBox1.Text.Replace("LP", ""))
            test_conn()

            Dim sql As String = "DELETE FROM admin_product WHERE id = ?"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@id", idProduk)
                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Produk berhasil dihapus.")
            resetForm()
            LoadDataToGrid()
            loadidproduk()
        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Gagal menghapus data: " & ex.Message, vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub




    ' Handle CellClick event to populate fields
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Try
                ' Ambil data dari cell yang dipilih
                Dim selectedRow As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

                ' Ambil ID Produk
                Dim idProduk As Object = selectedRow.Cells("id").Value
                If idProduk IsNot DBNull.Value Then
                    TextBox1.Text = "LP" & CInt(idProduk).ToString("D5")
                Else
                    TextBox1.Clear()
                End If

                ' Ambil data lain dan tampilkan di form
                TextBox2.Text = selectedRow.Cells("nama_produk").Value.ToString() ' Nama Produk
                TextBox3.Text = selectedRow.Cells("harga_jual").Value.ToString() ' Harga Jual
                TextBox4.Text = selectedRow.Cells("harga_kulak").Value.ToString() ' Harga Kulak

                ' Set ComboBox dengan tipe produk
                Dim tipeProduk As Object = selectedRow.Cells("tipe").Value
                If Not IsDBNull(tipeProduk) Then
                    ComboBox2.SelectedItem = ComboBox2.Items.Cast(Of KeyValuePair(Of String, Integer))().FirstOrDefault(Function(x) x.Value = CInt(tipeProduk))
                End If

            Catch ex As Exception
                MsgBox("Terjadi kesalahan saat memilih data: " & ex.Message, vbCritical)
            End Try
        End If
    End Sub



    ' cegah ctrl v
    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox3.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TextBox4_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox4.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    ' Reset form fields
    Private Sub resetForm()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        ComboBox2.SelectedIndex = -1
        selectedId = -1
    End Sub
    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox2.KeyPress
        ' Cegah input angka
        If Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MessageBox.Show("Input tidak boleh berisi angka.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub


    'hrs angka
    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox3.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !")
        End If
    End Sub
    'hrs angka
    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox4.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !")
        End If
    End Sub

    

    ' Get kasir id by username
    Function getKasirId(ByVal username As String) As Integer
        test_conn()
        Using cmd As New OdbcCommand("SELECT id FROM account WHERE username = ?", conn)
            cmd.Parameters.AddWithValue("@username", username)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then Return reader("id")
            End Using
        End Using
        Return -1
    End Function

    'clear
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        resetForm()
        loadidproduk()
    End Sub

    'formatting 
    Private Sub DataGridView1_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

        ' Contoh lainnya: warna jika harga jual < modal
        If DataGridView1.Columns(e.ColumnIndex).Name = "harga_jual" Then
            Try
                Dim hargaModal As Integer = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("harga_kulak").Value)
                Dim hargaJual As Integer = Convert.ToInt32(DataGridView1.Rows(e.RowIndex).Cells("harga_jual").Value)

                If hargaJual < hargaModal Then
                    e.CellStyle.ForeColor = Color.Blue
                End If
            Catch
                ' biarkan lewat jika error parsing
            End Try
        End If
    End Sub



    'exit
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class
