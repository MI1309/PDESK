Imports System.Data.Odbc

Public Class Form4
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Dim selectedId As Integer = -1 ' Untuk menyimpan id yang dipilih

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
            da = New OdbcDataAdapter("SELECT * FROM account", conn)
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
        If TextBox1.Text = "" And TextBox2.Text = "" And ComboBox1.Text = "" Then
            MsgBox("Masukkan input yang benar")
        ElseIf TextBox1.Text = "" And ComboBox1.Text = "" Then
            MsgBox("Username dan role kosong")
        ElseIf TextBox2.Text = "" And ComboBox1.Text = "" Then
            MsgBox("Password dan role kosong")
        ElseIf TextBox2.Text = "" And TextBox1.Text = "" Then
            MsgBox("Username dan password kosong")
        ElseIf ComboBox1.Text = "" Then
            MsgBox("Masukkan role yang benar")
        ElseIf TextBox1.Text = "" Then
            MsgBox("Username Anda kosong")
        ElseIf TextBox2.Text = "" Then
            MsgBox("Password ga boleh kosong")
        Else
            Try
                test_conn()
                Dim cmd As New OdbcCommand("INSERT INTO account (username, password, role) VALUES (?, ?, ?)", conn)
                cmd.Parameters.AddWithValue("@username", TextBox1.Text)
                cmd.Parameters.AddWithValue("@password", TextBox2.Text)
                cmd.Parameters.AddWithValue("@role", ComboBox1.Text)

                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    MsgBox("Akun berhasil ditambahkan!")
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                    ComboBox1.Text = ""
                    tampilData() ' Refresh datagrid setelah insert
                Else
                    MsgBox("Gagal menambahkan akun.")
                End If
            Catch ex As Exception
                MsgBox("Terjadi kesalahan: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End If
    End Sub

    ' Button untuk menutup form
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    ' Event ketika memilih data di DataGridView
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            ' Memeriksa apakah nilai pada cell kosong dan menampilkan pesan
            If IsDBNull(row.Cells("username").Value) OrElse String.IsNullOrEmpty(row.Cells("username").Value.ToString()) Then
                MsgBox("Tidak Ada Data Di kolom Ini !")
            Else
                ' Menetapkan nilai ke textbox dan combobox jika ada data
                selectedId = row.Cells("id").Value
                TextBox1.Text = If(IsDBNull(row.Cells("username").Value), "", row.Cells("username").Value.ToString())
                TextBox2.Text = If(IsDBNull(row.Cells("password").Value), "", row.Cells("password").Value.ToString())
                ComboBox1.Text = If(IsDBNull(row.Cells("role").Value), "", row.Cells("role").Value.ToString())
            End If
        End If
    End Sub

    ' Button untuk update data
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If selectedId = -1 Then
            MsgBox("Pilih data yang mau diupdate dulu.")
            Return
        End If

        Try
            test_conn()
            Dim cmd As New OdbcCommand("UPDATE account SET username=?, password=?, role=? WHERE id=?", conn)
            cmd.Parameters.AddWithValue("@username", TextBox1.Text)
            cmd.Parameters.AddWithValue("@password", TextBox2.Text)
            cmd.Parameters.AddWithValue("@role", ComboBox1.Text)
            cmd.Parameters.AddWithValue("@id", selectedId)

            Dim result As Integer = cmd.ExecuteNonQuery()
            If result > 0 Then
                MsgBox("Data berhasil diupdate!")
                tampilData()
                TextBox1.Text = ""
                TextBox2.Text = ""
                ComboBox1.Text = ""
                selectedId = -1
            Else
                MsgBox("Gagal update data.")
            End If
        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat update: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' Button untuk hapus data
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If selectedId = -1 Then
            MsgBox("Pilih data yang ingin dihapus terlebih dahulu.")
            Return
        End If

        ' Konfirmasi penghapusan data
        Dim result As DialogResult = MsgBox("Apakah Anda yakin ingin menghapus data ini?", MsgBoxStyle.YesNo, "Konfirmasi Hapus")
        If result = DialogResult.Yes Then
            Try
                test_conn()
                Dim cmd As New OdbcCommand("DELETE FROM account WHERE id=?", conn)
                cmd.Parameters.AddWithValue("@id", selectedId)

                Dim deleteResult As Integer = cmd.ExecuteNonQuery()
                If deleteResult > 0 Then
                    MsgBox("Data berhasil dihapus!")
                    tampilData() ' Refresh datagrid setelah hapus
                    TextBox1.Text = ""
                    TextBox2.Text = ""
                    ComboBox1.Text = ""
                    selectedId = -1
                Else
                    MsgBox("Gagal menghapus data.")
                End If
            Catch ex As Exception
                MsgBox("Terjadi kesalahan saat menghapus: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class
