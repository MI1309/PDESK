Imports System.Data.Odbc

Public Class Form5
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

            ' Ambil data dari database
            Dim query As String = "SELECT * FROM admin_product"
            Dim cmd As New OdbcCommand(query, conn)

            Dim da As New OdbcDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            ' Tambahkan kolom baru untuk format LPID
            dt.Columns.Add("kode_produk", GetType(String))

            For Each row As DataRow In dt.Rows
                If Not IsDBNull(row("id")) Then
                    row("kode_produk") = "LP" & Convert.ToInt32(row("id")).ToString("D4")
                Else
                    row("kode_produk") = ""
                End If
            Next

            ' Ganti datasource dan urutkan ulang kolom agar "kode_produk" muncul duluan
            DataGridView1.DataSource = dt

            ' Optional: sembunyikan kolom 'id' asli jika tidak diperlukan
            If DataGridView1.Columns.Contains("id") Then
                DataGridView1.Columns("id").Visible = False
            End If

            ' Tampilkan 'kode_produk' di urutan awal
            If DataGridView1.Columns.Contains("kode_produk") Then
                DataGridView1.Columns("kode_produk").DisplayIndex = 0
                DataGridView1.Columns("kode_produk").HeaderText = "Kode Produk"
            End If

        Catch ex As Exception
            MsgBox("Gagal memuat data ke dalam DataGridView: " & ex.Message, vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub






    ' Display product data in DataGridView
    Sub tampilData()
        Try
            test_conn()
            da = New OdbcDataAdapter("SELECT id, nama_produk,harga_jual,harga_kulak,tanggal_restock,stok,tipe FROM admin_product", conn)
            ds = New DataSet()
            da.Fill(ds, "admin_product")
            DataGridView1.DataSource = ds.Tables("admin_product")
        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' Setup DataGridView appearance and functionality
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
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        If Not ValidasiInputKosong(TextBox2, TextBox3, TextBox4, ComboBox2) Then Exit Sub

        Try
            Dim namaProduk As String = TextBox2.Text
            Dim hargaJual As Integer = CInt(TextBox3.Text)
            Dim hargaModal As Integer = CInt(TextBox4.Text)
            Dim tipe As Integer = CInt(DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, Integer)).Value)

            test_conn()

            ' Cek jika provider sudah ada di admin product
            Dim checkCmd As New OdbcCommand("SELECT COUNT(*) FROM admin_product WHERE nama_produk = ?", conn)
            checkCmd.Parameters.AddWithValue("@nama_produk", namaProduk)

            Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            If exists > 0 Then
                MsgBox("Produk sudah ada di penjualan", vbExclamation)
                Exit Sub
            End If

            ' Insert new product
            Dim sql As String = "INSERT INTO admin_product (nama_produk, harga_jual, harga_kulak, tanggal_restock, tipe) VALUES (?, ?, ?, ?, ?)"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
                cmd.Parameters.AddWithValue("@harga_jual", hargaJual)
                cmd.Parameters.AddWithValue("@harga_kulak", hargaModal)
                cmd.Parameters.AddWithValue("@tanggal_restock", DateTime.Now)
                cmd.Parameters.AddWithValue("@tipe", tipe)
                cmd.ExecuteNonQuery()
            End Using

            tampilData()
            MsgBox("Produk berhasil ditambahkan ke penjualan")
            resetForm()
            loadidproduk()

        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat menyimpan data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
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

    ' Update data
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        If selectedId = -1 Then
            MsgBox("Pilih data yang mau diupdate dulu.", vbExclamation)
            Exit Sub
        End If

        If Not ValidasiInputKosong(TextBox2, TextBox3, TextBox4, ComboBox2) Then Exit Sub

        Try
            test_conn()

            ' Mengecek apakah ada produk dengan nama yang sama
            Dim checkCmd As New OdbcCommand("SELECT COUNT(*) FROM admin_product WHERE nama_produk = ? AND id <> ?", conn)
            checkCmd.Parameters.AddWithValue("@nama_produk", TextBox2.Text.Trim())
            checkCmd.Parameters.AddWithValue("@id", selectedId)
            Dim count As Integer = CInt(checkCmd.ExecuteScalar())

            If count > 0 Then
                MsgBox("Produk dengan nama '" & TextBox2.Text.Trim() & "' sudah ada.", vbExclamation)
                Exit Sub
            End If

            ' Ambil data lama dari database
            Dim oldCmd As New OdbcCommand("SELECT nama_produk, harga_jual, harga_kulak, tipe FROM admin_product WHERE id = ?", conn)
            oldCmd.Parameters.AddWithValue("@id", selectedId)
            Dim tipe As Integer = CInt(DirectCast(ComboBox2.SelectedItem, KeyValuePair(Of String, Integer)).Value)

            Dim oldData As New Dictionary(Of String, String)
            Using reader As OdbcDataReader = oldCmd.ExecuteReader()
                If reader.Read() Then
                    oldData("nama_produk") = reader("nama_produk").ToString()
                    oldData("harga_jual") = reader("harga_jual").ToString()
                    oldData("harga_kulak") = reader("harga_kulak").ToString()
                    oldData("tipe") = reader("tipe").ToString()
                Else
                    MsgBox("Data tidak ditemukan.", vbExclamation)
                    Exit Sub
                End If
            End Using

            ' Cek jika tidak ada perubahan
            If oldData("nama_produk") = TextBox2.Text.Trim() AndAlso
               oldData("harga_jual") = TextBox3.Text.Trim() AndAlso
               oldData("harga_kulak") = TextBox4.Text.Trim() AndAlso
               oldData("tipe") = tipe.ToString() Then
                MsgBox("Tidak ada perubahan data yang dilakukan.", vbExclamation)
                Exit Sub
            End If

            ' Update data
            Dim updateCmd As New OdbcCommand("UPDATE admin_product SET nama_produk=?, harga_jual=?, harga_kulak=?, tanggal_restock=?, tipe=? WHERE id=?", conn)
            updateCmd.Parameters.AddWithValue("@nama_produk", TextBox2.Text.Trim())
            updateCmd.Parameters.AddWithValue("@harga_jual", CInt(TextBox3.Text))
            updateCmd.Parameters.AddWithValue("@harga_kulak", CInt(TextBox4.Text))
            updateCmd.Parameters.AddWithValue("@tanggal_restock", DateTime.Now)
            updateCmd.Parameters.AddWithValue("@tipe", tipe) ' ← pakai Value asli (5000, 10000, dll)
            updateCmd.Parameters.AddWithValue("@id", selectedId)

            Dim result = updateCmd.ExecuteNonQuery()
            If result > 0 Then
                MsgBox("Data berhasil diupdate!", vbInformation)
                tampilData()
                resetForm()
                loadidproduk()
            Else
                MsgBox("Gagal update data.", vbCritical)
            End If

        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat update data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    ' Delete selected product
    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        ' Pastikan ada baris yang dipilih
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Pilih baris produk yang ingin dihapus!", vbExclamation)
            Exit Sub
        End If

        ' Ambil baris yang dipilih
        Dim selectedRow = DataGridView1.SelectedRows(0)

        ' Validasi nilai kolom ID
        If selectedRow.Cells("id").Value Is Nothing OrElse IsDBNull(selectedRow.Cells("id").Value) Then
            MsgBox("Data yang dipilih tidak memiliki ID yang valid.", vbExclamation)
            Exit Sub
        End If

        ' Ambil ID dan nama produk
        Dim selectedId As Integer = 0
        If Not Integer.TryParse(selectedRow.Cells("id").Value.ToString().Replace("LP", ""), selectedId) Then
            MsgBox("ID produk tidak valid.", vbExclamation)
            Exit Sub
        End If

        Dim namaProduk As String = If(IsDBNull(selectedRow.Cells("nama_produk").Value), "(Tanpa Nama)", selectedRow.Cells("nama_produk").Value.ToString())

        ' Konfirmasi penghapusan
        If MessageBox.Show("Yakin ingin menghapus produk '" & namaProduk & "'?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                test_conn()

                Using cmd As New OdbcCommand("DELETE FROM admin_product WHERE id = ?", conn)
                    cmd.Parameters.AddWithValue("@id", selectedId)
                    cmd.ExecuteNonQuery()
                End Using

                MsgBox("Produk berhasil dihapus.", vbInformation)

                resetForm()
                tampilData()

            Catch ex As Exception
                MsgBox("Gagal menghapus data: " & ex.Message, vbCritical)
            Finally
                If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
            End Try
        End If
    End Sub


    ' Reset form fields
    Private Sub resetForm()
        TextBox1.Clear()
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

    'cell click
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            ' Pastikan baris yang dipilih valid
            If e.RowIndex < 0 OrElse e.RowIndex >= DataGridView1.Rows.Count Then Exit Sub


            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            Dim tipeVal As Object = row.Cells("tipe").Value

            If Not IsDBNull(tipeVal) Then
                Dim tipeInt As Integer = Convert.ToInt32(tipeVal)
                ComboBox2.SelectedValue = tipeInt
            Else
                ComboBox2.SelectedIndex = -1
                ComboBox2.Text = ""
            End If


            ' Ambil ID yang diformat dari kolom yang dipilih
            Dim idValue = row.Cells("id").Value

            ' Validasi ID agar tidak kosong atau null
            If IsDBNull(idValue) OrElse String.IsNullOrEmpty(idValue.ToString()) Then
                MsgBox("Data ini Kosong! Pilih data yang ada isinya.", vbExclamation)
                resetForm()  ' Reset form
                LoadDataToGrid()
                loadidproduk()
                Exit Sub
            End If

            ' Set nilai ID ke variabel selectedId
            selectedId = Convert.ToInt32(idValue.ToString().Replace("LP", "")) ' Ambil angka dari LP{id}

            ' Format ID di TextBox1 menjadi LP{id}
            TextBox1.Text = "LP" & selectedId.ToString("D4")

            ' Ambil data lainnya dan tampilkan di form
            TextBox2.Text = If(IsDBNull(row.Cells("nama_produk").Value), "", row.Cells("nama_produk").Value.ToString())
            TextBox3.Text = If(IsDBNull(row.Cells("harga_jual").Value), "", row.Cells("harga_jual").Value.ToString())
            TextBox4.Text = If(IsDBNull(row.Cells("harga_kulak").Value), "", row.Cells("harga_kulak").Value.ToString())
            ComboBox2.Text = If(IsDBNull(row.Cells("tipe").Value), "", row.Cells("tipe").Value.ToString())

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat memilih baris: " & ex.Message, vbCritical)
        End Try
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
        ' Format kolom ID jadi LPxxxx
        If DataGridView1.Columns(e.ColumnIndex).Name = "id" Then
            If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                Dim id As Integer = Convert.ToInt32(e.Value)
                e.Value = "LP" & id.ToString("D5")
                e.FormattingApplied = True
            End If
        End If

        ' Format tipe: angka jadi 10.000
        If DataGridView1.Columns(e.ColumnIndex).Name = "tipe" Then
            If e.Value IsNot Nothing AndAlso Not IsDBNull(e.Value) Then
                Dim tipe As Integer = Convert.ToInt32(e.Value)
                e.Value = tipe.ToString("N0") ' Misal: 10000 jadi 10.000
                e.FormattingApplied = True
            End If
        End If

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
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub
End Class
