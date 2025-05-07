Imports System.Data.Odbc

Public Class Form4
    Public previousForm As Form ' Tambahan ini
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Dim selectedId As Integer = -1 ' Untuk menyimpan id yang dipilih
    Private Sub ClearTextBoxes()
        For i As Integer = 1 To 6
            Dim tb As TextBox = TryCast(Me.Controls("TextBox" & i), TextBox)
            If tb IsNot Nothing Then
                tb.Clear()
            End If
        Next
        selectedId = -1
    End Sub

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

    ' Menampilkan data dari database ke DataGridView
    Sub tampilData()
        Try
            test_conn()
            da = New OdbcDataAdapter("SELECT id, nama_kasir, alamat, no_telpon, email, password, username FROM account WHERE role <> 'admin';", conn)
            ds = New DataSet()
            da.Fill(ds, "account")
            DataGridView1.DataSource = ds.Tables("account")
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells ' Kolom menyesuaikan isi
            conn.Close()
        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        End Try
    End Sub

    ' Styling DataGridView
    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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

    ' Button untuk insert data ke database
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        TextBox1.Tag = "Nama Kasir"
        TextBox2.Tag = "Alamat"
        TextBox3.Tag = "No Telepon"
        TextBox4.Tag = "Username"
        TextBox5.Tag = "Email"
        TextBox6.Tag = "Password"
        Dim role As String = "kasir"

        If Not ValidasiInputKosong(TextBox1, TextBox2, TextBox3, TextBox4, TextBox5, TextBox6) Then
            Exit Sub
        ElseIf Not IsEmailValid(TextBox5.Text) Then
            MsgBox("Email tidak valid.")
            Exit Sub
        ElseIf Not IsPhoneNumberValid(TextBox3.Text) Then
            MsgBox("Nomor telepon tidak valid.")
            Exit Sub
        End If

        Try
            test_conn()

            ' Cek duplikat data
            Using checkCmd As New OdbcCommand("SELECT COUNT(*) FROM account WHERE username = ? OR email = ? OR no_telpon = ?", conn)
                checkCmd.Parameters.AddWithValue("@username", TextBox4.Text)
                checkCmd.Parameters.AddWithValue("@email", TextBox5.Text)
                checkCmd.Parameters.AddWithValue("@no_telpon", TextBox3.Text)

                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    MsgBox("Data sudah ada. Gunakan data unik untuk username/email/no telpon.")
                    Exit Sub
                End If
            End Using

            ' Insert data baru
            Using cmd As New OdbcCommand("INSERT INTO account (username, password, role, alamat, no_telpon, email, nama_kasir) VALUES (?, ?, ?, ?, ?, ?, ?)", conn)
                cmd.Parameters.AddWithValue("@username", TextBox4.Text)
                cmd.Parameters.AddWithValue("@password", TextBox6.Text)
                cmd.Parameters.AddWithValue("@role", role)
                cmd.Parameters.AddWithValue("@alamat", TextBox2.Text)
                cmd.Parameters.AddWithValue("@no_telpon", TextBox3.Text)
                cmd.Parameters.AddWithValue("@email", TextBox5.Text)
                cmd.Parameters.AddWithValue("@nama_kasir", TextBox1.Text)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    MsgBox("Akun berhasil ditambahkan!")
                    ClearTextBoxes()
                    tampilData()
                Else
                    MsgBox("Gagal menambahkan akun.")
                End If
            End Using

        Catch ex As Exception
            MsgBox("Terjadi kesalahan: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub


    ' Button untuk menutup form
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            ' Memeriksa apakah nilai pada cell kosong dan menampilkan pesan
            If IsDBNull(row.Cells("username").Value) OrElse String.IsNullOrEmpty(row.Cells("username").Value.ToString()) Then
                MsgBox("Tidak Ada Data Di kolom Ini !")
            Else
                ' Menetapkan nilai ke textbox dan combobox jika ada data
                selectedId = row.Cells("id").Value
                TextBox1.Text = If(IsDBNull(row.Cells("nama_kasir").Value), "", row.Cells("nama_kasir").Value.ToString())
                TextBox2.Text = If(IsDBNull(row.Cells("alamat").Value), "", row.Cells("alamat").Value.ToString())
                TextBox3.Text = If(IsDBNull(row.Cells("no_telpon").Value), "", row.Cells("no_telpon").Value.ToString())
                TextBox4.Text = If(IsDBNull(row.Cells("username").Value), "", row.Cells("username").Value.ToString())
                TextBox5.Text = If(IsDBNull(row.Cells("email").Value), "", row.Cells("email").Value.ToString())
                TextBox6.Text = If(IsDBNull(row.Cells("password").Value), "", row.Cells("password").Value.ToString())

                ' Cek jika role adalah admin
                If row.Cells("id").Value.ToString() = "1" Then
                    MsgBox("Admin tidak boleh diganggu!")
                    ' Menghentikan update dan hapus untuk admin
                    Button3.Enabled = False ' Disable update button
                    Button2.Enabled = False ' Disable delete button
                Else
                    Button3.Enabled = True ' Enable update button
                    Button2.Enabled = True ' Enable delete button
                End If
            End If
        End If
    End Sub

Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        TextBox1.Tag = "Nama Kasir"
        TextBox2.Tag = "Alamat"
        TextBox3.Tag = "No Telepon"
        TextBox4.Tag = "Username"
        TextBox5.Tag = "Email"
        TextBox6.Tag = "Password"

        If selectedId = -1 Then
            MsgBox("Pilih data yang mau diupdate dulu.")
            Return
        End If

        ' Validasi input
        If Not valid(TextBox1, TextBox2, TextBox3, TextBox4, TextBox5, TextBox6) Then
            Exit Sub
        ElseIf Not IsEmailValid(TextBox5.Text) Then
            MsgBox("Email tidak valid.")
            Return
        ElseIf Not IsPhoneNumberValid(TextBox3.Text) Then
            MsgBox("Nomor telepon tidak valid.")
            Return
        End If

        Try
            test_conn()

            ' Ambil data lama dari database
            Dim oldDataCmd As New OdbcCommand("SELECT username, password, role, alamat, no_telpon, email, nama_kasir FROM account WHERE id = ?", conn)
            oldDataCmd.Parameters.AddWithValue("@id", selectedId)

            Dim reader As OdbcDataReader = oldDataCmd.ExecuteReader()
            If reader.Read() Then
                Dim sameData As Boolean = (
                    reader("username").ToString() = TextBox4.Text AndAlso
                    reader("password").ToString() = TextBox6.Text AndAlso
                    reader("role").ToString() = "kasir" AndAlso
                    reader("alamat").ToString() = TextBox2.Text AndAlso
                    reader("no_telpon").ToString() = TextBox3.Text AndAlso
                    reader("email").ToString() = TextBox5.Text AndAlso
                    reader("nama_kasir").ToString() = TextBox1.Text
                )

                reader.Close()

                If sameData Then
                    MsgBox("Tidak ada data yang diubah.")
                    Exit Sub
                End If
            Else
                MsgBox("Data tidak ditemukan.")
                reader.Close()
                Exit Sub
            End If


            ' Lakukan update data
            Using cmd As New OdbcCommand("UPDATE account SET username=?, password=?, role=?, alamat=?, no_telpon=?, email=?, nama_kasir=? WHERE id=?", conn)
                cmd.Parameters.AddWithValue("@username", TextBox4.Text)
                cmd.Parameters.AddWithValue("@password", TextBox6.Text)
                cmd.Parameters.AddWithValue("@role", "kasir")
                cmd.Parameters.AddWithValue("@alamat", TextBox2.Text)
                cmd.Parameters.AddWithValue("@no_telpon", TextBox3.Text)
                cmd.Parameters.AddWithValue("@email", TextBox5.Text)
                cmd.Parameters.AddWithValue("@nama_kasir", TextBox1.Text)
                cmd.Parameters.AddWithValue("@id", selectedId)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    MsgBox("Data berhasil diupdate!")
                    tampilData()
                    ClearTextBoxes()
                Else
                    MsgBox("Gagal update data.")
                End If
            End Using

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat update: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub




    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox5_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox5.TextChanged

    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox3.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan di nomor telepon!")
        End If
    End Sub


    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If selectedId = -1 Then
            MsgBox("Pilih data yang ingin dihapus terlebih dahulu.")
            Return
        End If

        ' Cek apakah admin
        If selectedId = 1 Then
            MsgBox("Data admin tidak boleh dihapus!")
            Return
        End If

        Dim result As DialogResult = MsgBox("Apakah Anda yakin ingin menghapus data ini?", MsgBoxStyle.YesNo, "Konfirmasi Hapus")
        If result = DialogResult.Yes Then
            Try
                test_conn()
                Using cmd As New OdbcCommand("DELETE FROM account WHERE id=?", conn)
                    cmd.Parameters.AddWithValue("@id", selectedId)

                    Dim deleteResult As Integer = cmd.ExecuteNonQuery()
                    If deleteResult > 0 Then
                        MsgBox("Data berhasil dihapus!")
                        tampilData()
                        ClearTextBoxes()
                    Else
                        MsgBox("Gagal menghapus data.")
                    End If
                End Using

            Catch ex As Exception
                MsgBox("Terjadi kesalahan saat menghapus: " & ex.Message)
            Finally
                If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
            End Try
        End If
    End Sub

End Class
