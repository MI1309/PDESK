Imports System.Data.Odbc

Public Class Form5
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Public loggedInUserId As Integer
    Private selectedId As Integer = -1

    Private Sub Form5_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        tampilData()
        loadKasirToComboBox()
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        setupDataGridView()
    End Sub

    Sub test_conn()
        Try
            mysql = "DSN=pulsa;"
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

    Sub loadKasirToComboBox()
        Try
            test_conn()
            Using cmd As New OdbcCommand("SELECT username FROM account WHERE role = 'kasir'", conn)
                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear()
                    While dr.Read()
                        ComboBox2.Items.Add(dr("username").ToString())
                    End While
                End Using
            End Using
            If ComboBox2.Items.Count = 0 Then
                MsgBox("Tidak ada user dengan role 'kasir' ditemukan di database!", vbExclamation)
            End If
        Catch ex As Exception
            MsgBox("Gagal memuat data kasir: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

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

    Function ValidasiInput() As Boolean
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox2.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox3.Text) OrElse
           String.IsNullOrWhiteSpace(TextBox4.Text) OrElse
           ComboBox2.SelectedIndex = -1 Then
            MsgBox("Semua field harus diisi dan kasir harus dipilih!", vbExclamation)
            Return False
        End If

        Dim hargaJual, hargaModal, stok As Integer

        If Not Integer.TryParse(TextBox2.Text, hargaJual) AndAlso Not Integer.TryParse(TextBox3.Text, hargaModal) AndAlso Not Integer.TryParse(TextBox4.Text, stok) Then
            MsgBox("harus diisi angka") : Return False
        ElseIf Not Integer.TryParse(TextBox2.Text, hargaJual) AndAlso Not Integer.TryParse(TextBox3.Text, hargaModal) Then
            MsgBox("harga jual dan harga modal harus diisi angka ") : Return False
        ElseIf Not Integer.TryParse(TextBox3.Text, hargaModal) AndAlso Not Integer.TryParse(TextBox4.Text, stok) Then
            MsgBox("harga modal dan stok harus berupa angka") : Return False
        ElseIf Not Integer.TryParse(TextBox4.Text, stok) AndAlso Not Integer.TryParse(TextBox2.Text, hargaJual) Then
            MsgBox("harga jual dan stok harus angka") : Return False
        ElseIf Not Integer.TryParse(TextBox2.Text, hargaJual) Then
            MsgBox("harga jual harus anglka") : Return False
        ElseIf Not Integer.TryParse(TextBox3.Text, hargaModal) Then
            MsgBox("harga modal harus berupa angka") : Return False
        ElseIf Not Integer.TryParse(TextBox4.Text, stok) Then
            MsgBox("stok harus angka") : Return False
        ElseIf Not Integer.TryParse(TextBox2.Text, hargaJual) OrElse hargaJual < 1 Then
            MsgBox("Harga jual harus berupa angka dan jangan minus") : Return False
        ElseIf Not Integer.TryParse(TextBox3.Text, hargaModal) OrElse hargaModal < 1 Then
            MsgBox("Harga modal harus berupa angka dan jangan minus") : Return False
        ElseIf Not Integer.TryParse(TextBox4.Text, stok) OrElse stok < 0 Then
            MsgBox("stok harus berupa angka dan harus berupa angka builat") : Return False
        End If

        Return True
    End Function

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If Not ValidasiInput() Then Exit Sub

        Try
            Dim hargaJual As Integer = CInt(TextBox2.Text)
            Dim hargaModal As Integer = CInt(TextBox3.Text)
            Dim stok As Integer = CInt(TextBox4.Text)
            Dim kasirUsername As String = ComboBox2.SelectedItem.ToString()
            Dim idKasir As Integer = getKasirId(kasirUsername)
            If idKasir = -1 Then
                MsgBox("ID kasir tidak ditemukan!", vbExclamation) : Exit Sub
            End If

            test_conn()
            Dim sql As String = "INSERT INTO admin_product (nama_produk, harga_jual, harga_modal, tanggal_restock, stok, id_kasir, kasir_username) VALUES (?, ?, ?, ?, ?, ?, ?)"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nama_produk", TextBox1.Text.Trim())
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
        Catch ex As Exception
            IO.File.AppendAllText("error_log.txt", Now & " - " & ex.ToString() & Environment.NewLine)
            MsgBox("Terjadi kesalahan saat menyimpan data.", vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        If selectedId = -1 Then
            MsgBox("Pilih data yang mau diupdate dulu.", vbExclamation) : Exit Sub
        End If

        If Not ValidasiInput() Then Exit Sub

        Try
            test_conn()
            Using cmd As New OdbcCommand("UPDATE admin_product SET nama_produk=?, harga_jual=?, harga_modal=?, tanggal_restock=?, stok=?, kasir_username=? WHERE id=?", conn)
                cmd.Parameters.AddWithValue("@nama_produk", TextBox1.Text.Trim())
                cmd.Parameters.AddWithValue("@harga_jual", CInt(TextBox2.Text))
                cmd.Parameters.AddWithValue("@harga_modal", CInt(TextBox3.Text))
                cmd.Parameters.AddWithValue("@tanggal_restock", DateTime.Now)
                cmd.Parameters.AddWithValue("@stok", CInt(TextBox4.Text))
                cmd.Parameters.AddWithValue("@kasir_username", ComboBox2.Text)
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

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            MsgBox("Pilih baris produk yang ingin dihapus!", vbExclamation) : Exit Sub
        End If

        Dim selectedRow = DataGridView1.SelectedRows(0)
        Dim selectedId = CInt(selectedRow.Cells("id").Value)
        Dim namaProduk = selectedRow.Cells("nama_produk").Value.ToString()
        If MessageBox.Show("Yakin ingin menghapus produk '{namaProduk}'?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                test_conn()
                Using cmd As New OdbcCommand("DELETE FROM admin_product WHERE id = ?", conn)
                    cmd.Parameters.AddWithValue("@id", selectedId)
                    cmd.ExecuteNonQuery()
                End Using
                MsgBox("Produk berhasil dihapus.", vbInformation)
                tampilData()
            Catch ex As Exception
                MsgBox("Gagal menghapus data: " & ex.Message, vbCritical)
            Finally
                If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
            End Try
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button5.Click
        resetForm()
    End Sub

    Private Sub resetForm()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        ComboBox2.SelectedIndex = -1
        selectedId = -1
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 AndAlso e.RowIndex < DataGridView1.Rows.Count Then
            Dim row = DataGridView1.Rows(e.RowIndex)

            ' Cek dulu apakah Cell tidak kosong
            If Not IsDBNull(row.Cells("nama_produk").Value) Then
                TextBox1.Text = row.Cells("nama_produk").Value.ToString()
            Else
                TextBox1.Clear()
            End If

            If Not IsDBNull(row.Cells("harga_jual").Value) Then
                TextBox2.Text = row.Cells("harga_jual").Value.ToString()
            Else
                TextBox2.Clear()
            End If

            If Not IsDBNull(row.Cells("harga_modal").Value) Then
                TextBox3.Text = row.Cells("harga_modal").Value.ToString()
            Else
                TextBox3.Clear()
            End If

            If Not IsDBNull(row.Cells("stok").Value) Then
                TextBox4.Text = row.Cells("stok").Value.ToString()
            Else
                TextBox4.Clear()
            End If

            If Not IsDBNull(row.Cells("kasir_username").Value) Then
                ComboBox2.Text = row.Cells("kasir_username").Value.ToString()
            Else
                ComboBox2.SelectedIndex = -1
            End If

            If Not IsDBNull(row.Cells("id").Value) Then
                selectedId = Convert.ToInt32(row.Cells("id").Value)
            Else
                selectedId = -1
            End If
        End If
    End Sub


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
End Class
