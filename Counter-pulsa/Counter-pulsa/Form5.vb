Imports System.Data.Odbc

Public Class Form5
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Public loggedInUserId As Integer
    Private selectedId As Integer = -1

    ' validasi function
    Private Function ValidasiInputKosong(ByVal ParamArray controls() As Control) As Boolean
        Dim kosongList As New List(Of String)
        Dim controlDescriptions As New Dictionary(Of String, String) From {
            {"ComboBox2", "Nama Produk"},
            {"TextBox2", "Harga Jual"},
            {"TextBox3", "Harga Modal"},
            {"TextBox4", "Stok"},
            {"ComboBox1", "Kasir"}
        }

        ' Check for empty TextBox inputs
        For Each ctrl In controls
            If TypeOf ctrl Is TextBox AndAlso ctrl.Text.Trim() = "" Then
                kosongList.Add(controlDescriptions(ctrl.Name))
            End If
        Next

        ' Check ComboBox selection
        For Each ctrl In controls
            If TypeOf ctrl Is ComboBox Then
                Dim combo As ComboBox = DirectCast(ctrl, ComboBox)
                If combo.SelectedIndex = -1 Then
                    kosongList.Add(controlDescriptions(ctrl.Name))
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

    ' Clear form fields text box
    Private Sub ClearTextBoxes()
        For i As Integer = 1 To 6
            Dim tb As TextBox = TryCast(Me.Controls("TextBox" & i), TextBox)
            If tb IsNot Nothing Then
                tb.Clear()
            End If
        Next
        selectedId = -1
    End Sub

    ' Load form data
    Private Sub Form5_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        tampilData()
        loadKasirToComboBox()
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
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

    ' Load kasir usernames into ComboBox
    Sub loadKasirToComboBox()
        Try
            test_conn()
            Using cmd As New OdbcCommand("SELECT username FROM account WHERE role = 'kasir'", conn)
                Using dr = cmd.ExecuteReader()
                    ComboBox1.Items.Clear()
                    While dr.Read()
                        ComboBox1.Items.Add(dr("username").ToString())
                    End While
                End Using
            End Using

            If ComboBox1.Items.Count = 0 Then
                MsgBox("Tidak ada user dengan role 'kasir' ditemukan di database!", vbExclamation)
            End If
        Catch ex As Exception
            MsgBox("Gagal memuat data kasir: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub
    ' Load nama produk ke ComboBox2 dari tabel produk
    Sub loadProdukToComboBox()
        Try
            test_conn()
            Using cmd As New OdbcCommand("SELECT DISTINCT nama_produk FROM admin_product", conn)
                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear()
                    While dr.Read()
                        ComboBox2.Items.Add(dr("nama_produk").ToString())
                    End While
                End Using
            End Using

            If ComboBox2.Items.Count = 0 Then
                MsgBox("Belum ada produk yang tersedia!", vbExclamation)
            End If
        Catch ex As Exception
            MsgBox("Gagal memuat data produk: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    ' Display product data in DataGridView
    Sub tampilData()
        Try
            test_conn()
            da = New OdbcDataAdapter("SELECT * FROM admin_product", conn)
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
        If String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           ComboBox2.SelectedIndex = -1 OrElse
            ComboBox1.SelectedIndex = -1 Then
            MsgBox("Semua field harus diisi dan kasir harus dipilih!", vbExclamation)
            Return False
        End If

        Dim hargaJual, hargaModal, stok As Integer

        If Not Integer.TryParse(TextBox2.Text, hargaJual) OrElse Not Integer.TryParse(TextBox3.Text, hargaModal) OrElse Not Integer.TryParse(TextBox4.Text, stok) Then
            MsgBox("Harus diisi angka untuk harga jual, harga modal, dan stok.")
            Return False
        End If

        ' Ensure values are positive or valid
        If hargaJual < 1 OrElse hargaModal < 1 OrElse stok < 0 Then
            MsgBox("Harga jual, harga modal harus lebih dari 0, dan stok tidak boleh negatif.")
            Return False
        End If

        Return True
    End Function

    ' Insert new product
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        If Not ValidasiInputKosong(TextBox2, TextBox3, TextBox4, ComboBox2, ComboBox1) Then Exit Sub

        Try
            Dim namaProduk As String = ComboBox2.Text()
            Dim hargaJual As Integer = CInt(TextBox2.Text)
            Dim hargaModal As Integer = CInt(TextBox3.Text)
            Dim stok As Integer = CInt(TextBox4.Text)
            Dim kasirUsername As String = ComboBox1.SelectedItem.ToString()
            Dim idKasir As Integer = getKasirId(kasirUsername)

            If idKasir = -1 Then
                MsgBox("ID kasir tidak ditemukan!", vbExclamation)
                Exit Sub
            End If

            test_conn()

            ' Cek jika harga jual dan modal sudah ada
            Dim checkCmd As New OdbcCommand("SELECT COUNT(*) FROM admin_product WHERE harga_jual = ? AND harga_modal = ? AND kasir_username = ?", conn)
            checkCmd.Parameters.AddWithValue("@harga_jual", hargaJual)
            checkCmd.Parameters.AddWithValue("@harga_modal", hargaModal)
            checkCmd.Parameters.AddWithValue("@kasir_username", kasirUsername)

            Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            If exists > 0 Then
                MsgBox("Produk dengan harga jual dan harga modal yang sama sudah ada untuk kasir ini!", vbExclamation)
                Exit Sub
            End If

            ' Insert new product
            Dim sql As String = "INSERT INTO admin_product (nama_produk, harga_jual, harga_modal, tanggal_restock, stok, id_kasir, kasir_username) VALUES (?, ?, ?, ?, ?, ?, ?)"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
                cmd.Parameters.AddWithValue("@harga_jual", hargaJual)
                cmd.Parameters.AddWithValue("@harga_modal", hargaModal)
                cmd.Parameters.AddWithValue("@tanggal_restock", DateTime.Now)
                cmd.Parameters.AddWithValue("@stok", stok)
                cmd.Parameters.AddWithValue("@id_kasir", idKasir)
                cmd.Parameters.AddWithValue("@kasir_username", kasirUsername)
                cmd.ExecuteNonQuery()
            End Using

            tampilData()
            MsgBox("Produk berhasil ditambahkan ke kasir: " & kasirUsername)
            resetForm()

        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat menyimpan data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub



    ' Update data
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click

        'select data
        If selectedId = -1 Then
            MsgBox("Pilih data yang mau diupdate dulu.", vbExclamation)
            Exit Sub
        End If

        If Not ValidasiInput() Then Exit Sub

        Try
            test_conn()

            ' cek ketersediaan
            Dim oldCmd As New OdbcCommand("SELECT nama_produk, harga_jual, harga_modal, stok, kasir_username FROM admin_product WHERE id = ?", conn)
            oldCmd.Parameters.AddWithValue("@id", selectedId)
            Dim reader As OdbcDataReader = oldCmd.ExecuteReader()

            If reader.Read() Then
                Dim sameData As Boolean =
                    reader("nama_produk").ToString() = ComboBox2.Text AndAlso
                    reader("harga_jual").ToString() = TextBox2.Text.Trim() AndAlso
                    reader("harga_modal").ToString() = TextBox3.Text.Trim() AndAlso
                    reader("stok").ToString() = TextBox4.Text.Trim() AndAlso
                    reader("kasir_username").ToString() = ComboBox1.Text

                reader.Close()

                If sameData Then
                    MsgBox("Tidak ada perubahan data yang dilakukan.", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
            Else
                MsgBox("Data tidak ditemukan untuk update.")
                reader.Close()
                Exit Sub
            End If

            ' Perform update
            Using cmd As New OdbcCommand("UPDATE admin_product SET nama_produk=?, harga_jual=?, harga_modal=?, tanggal_restock=?, stok=?, kasir_username=? WHERE id=?", conn)
                cmd.Parameters.AddWithValue("@nama_produk", ComboBox2.Text.Trim())
                cmd.Parameters.AddWithValue("@harga_jual", CInt(TextBox2.Text))
                cmd.Parameters.AddWithValue("@harga_modal", CInt(TextBox3.Text))
                cmd.Parameters.AddWithValue("@tanggal_restock", DateTime.Now)
                cmd.Parameters.AddWithValue("@stok", CInt(TextBox4.Text))
                cmd.Parameters.AddWithValue("@kasir_username", ComboBox1.Text.Trim())
                cmd.Parameters.AddWithValue("@id", selectedId)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    MsgBox("Data berhasil diupdate!", vbInformation)
                    tampilData()
                    resetForm()
                Else
                    MsgBox("Gagal update data.", vbCritical)
                End If
            End Using

        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat update data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' Delete selected product
    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        ' Check if any row is selected
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Pilih baris produk yang ingin dihapus!", vbExclamation)
            Exit Sub
        End If

        ' Get the selected row
        Dim selectedRow = DataGridView1.SelectedRows(0)

        ' Get the product ID and name from the selected row
        Dim selectedId As Integer = CInt(selectedRow.Cells("id").Value)
        Dim namaProduk As String = selectedRow.Cells("nama_produk").Value.ToString()

        ' Confirm the deletion
        If MessageBox.Show("Yakin ingin menghapus produk '" & namaProduk & "'?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Try
                    ' Open database connection
                    test_conn()

                    ' Execute delete SQL command
                    Using cmd As New OdbcCommand("DELETE FROM admin_product WHERE id = ?", conn)
                        cmd.Parameters.AddWithValue("@id", selectedId)
                        cmd.ExecuteNonQuery()
                    End Using

                    ' Show success message
                    MsgBox("Produk berhasil dihapus.", vbInformation)

                    ' Clear form fields and refresh the data grid
                    ClearTextBoxes()
                    tampilData()

                Catch ex As Exception
                    ' Handle any errors during deletion
                    MsgBox("Gagal menghapus data: " & ex.Message, vbCritical)
                Finally
                    ' Close connection if open
                    If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
                End Try
        End If
    End Sub

    ' Reset form fields
    Private Sub resetForm()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
        selectedId = -1
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
    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox2.KeyPress
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
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            ' Pastikan indeks baris yang diklik valid
            If e.RowIndex < 0 OrElse e.RowIndex >= DataGridView1.Rows.Count Then Exit Sub

            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            ' Validasi bahwa kolom-kolom penting ada
            Dim requiredColumns As String() = {"id", "nama_produk", "harga_jual", "harga_modal", "stok", "kasir_username"}
            For Each colName In requiredColumns
                If Not DataGridView1.Columns.Contains(colName) Then
                    MsgBox("Kolom '" & colName & "' tidak ditemukan di DataGridView!", vbExclamation)
                    Exit Sub
                End If
            Next

            ' Ambil ID dan pastikan tidak kosong
            Dim idValue = row.Cells("id").Value
            If IsDBNull(idValue) OrElse String.IsNullOrEmpty(idValue.ToString()) Then
                MsgBox("Data ini Kosong Pilih data yang ada isinya  !")
                resetForm()
                Exit Sub
            End If

            ' Set nilai ID ke variabel selectedId
            selectedId = Convert.ToInt32(idValue)

            ' Ambil data dari kolom dan tampilkan ke form
            TextBox2.Text = If(IsDBNull(row.Cells("harga_jual").Value), "", row.Cells("harga_jual").Value.ToString())
            TextBox3.Text = If(IsDBNull(row.Cells("harga_modal").Value), "", row.Cells("harga_modal").Value.ToString())
            TextBox4.Text = If(IsDBNull(row.Cells("stok").Value), "", row.Cells("stok").Value.ToString())

            Dim namaProduk As String = If(IsDBNull(row.Cells("nama_produk").Value), "", row.Cells("nama_produk").Value.ToString())
            Dim kasirUsername As String = If(IsDBNull(row.Cells("kasir_username").Value), "", row.Cells("kasir_username").Value.ToString())

            ' Set nilai ComboBox2 (nama produk) jika ada di daftar
            If ComboBox2.Items.Contains(namaProduk) Then
                ComboBox2.SelectedItem = namaProduk
            Else
                ComboBox2.Text = namaProduk ' fallback jika tidak ada di list
            End If

            ' Set nilai ComboBox1 (kasir) jika ada di daftar
            If ComboBox1.Items.Contains(kasirUsername) Then
                ComboBox1.SelectedItem = kasirUsername
            Else
                MsgBox("Kasir '" & kasirUsername & "' tidak ditemukan dalam daftar kasir!", vbInformation)
                ComboBox1.SelectedIndex = -1
            End If

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

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        resetForm()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub
  
    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class
