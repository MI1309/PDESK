Imports System.Data.Odbc

Public Class Form3

    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Private selectedNamaProduk As String = "" ' Nama produk yang dipilih di tabel

    ' Variable to store the logged-in user username passed from Form1
    Public loggedInUserUsername As String

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

    Sub loadproduk()
        Try
            test_conn()
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
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub tampilData()
        Try
            test_conn()
            Dim query As String = "SELECT id ,nama_produk, harga_jual, stok, tanggal_restock FROM admin_product WHERE stok > 0"
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
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    ' Saat form dimuat
    Private Sub Form3_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        loadproduk()
        tampilData()

        ' Update the label with the logged-in username
        Label5.Text = "Operator : " & loggedInUserUsername
    End Sub

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

    ' Tombol Hapus (Button2)
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        TextBox2.Clear()
        TextBox3.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
    End Sub

    ' Tutup form (Button4)
    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        ' Validasi nomor HP sesuai produk
        Dim nomorHp As String = TextBox2.Text.Trim()
        Dim namaProdukDipilih As String = ComboBox1.SelectedItem.ToString().ToLower()

        If Not nomorHp.StartsWith("08") OrElse nomorHp.Length < 10 Then
            MsgBox("Nomor HP tidak valid! Harus diawali dengan 08 dan panjang minimal 10 digit.")
            TextBox2.Focus()
            Return
        End If

        If (namaProdukDipilih.Contains("telkomsel") AndAlso Not nomorHp.StartsWith("0811") AndAlso Not nomorHp.StartsWith("0812") AndAlso Not nomorHp.StartsWith("0813") AndAlso Not nomorHp.StartsWith("0821")) OrElse
           (namaProdukDipilih.Contains("xl") AndAlso Not nomorHp.StartsWith("0817") AndAlso Not nomorHp.StartsWith("0818") AndAlso Not nomorHp.StartsWith("0859")) OrElse
           (namaProdukDipilih.Contains("indosat") AndAlso Not nomorHp.StartsWith("0856") AndAlso Not nomorHp.StartsWith("0857")) OrElse
           (namaProdukDipilih.Contains("axis") AndAlso Not nomorHp.StartsWith("0838")) OrElse
           (namaProdukDipilih.Contains("tri") AndAlso Not nomorHp.StartsWith("0895")) OrElse
           (namaProdukDipilih.Contains("smartfren") AndAlso Not nomorHp.StartsWith("0881")) Then

            MsgBox("Nomor HP tidak cocok dengan provider dari produk yang dipilih.")
            TextBox2.Focus()
            Return
        End If

    End Sub
    Private Sub TextBox2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TextBox2.TextChanged
        ' Ambil nomor yang dimasukkan oleh pengguna
        Dim nomorHp As String = TextBox2.Text.Trim()

        ' Kosongkan ComboBox2 sebelumnya
        ComboBox2.Items.Clear()

        ' Tentukan provider berdasarkan nomor HP
        If nomorHp.StartsWith("0811") OrElse nomorHp.StartsWith("0812") OrElse nomorHp.StartsWith("0813") OrElse nomorHp.StartsWith("0821") Then
            ComboBox2.Items.Add("Telkomsel")
        ElseIf nomorHp.StartsWith("0817") OrElse nomorHp.StartsWith("0818") OrElse nomorHp.StartsWith("0859") Then
            ComboBox2.Items.Add("XL")
        ElseIf nomorHp.StartsWith("0856") OrElse nomorHp.StartsWith("0857") Then
            ComboBox2.Items.Add("Indosat")
        ElseIf nomorHp.StartsWith("0838") Then
            ComboBox2.Items.Add("Axis")
        ElseIf nomorHp.StartsWith("0895") Then
            ComboBox2.Items.Add("Tri")
        ElseIf nomorHp.StartsWith("0881") Then
            ComboBox2.Items.Add("Smartfren")
        Else
            ComboBox2.Items.Add("Provider Tidak Dikenal")
        End If

        ' Secara otomatis pilih provider pertama yang ditemukan
        If ComboBox2.Items.Count > 0 Then
            ComboBox2.SelectedIndex = 0
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim namaProdukDipilih As String = ComboBox1.SelectedItem.ToString()

        Try
            test_conn()
            ' Ambil tipe dan harga produk berdasarkan nama produk yang dipilih
            Dim query As String = "SELECT tipe, harga_jual FROM admin_product WHERE nama_produk = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("@nama_produk", namaProdukDipilih)

                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear() ' Kosongkan ComboBox2 sebelumnya
                    If dr.Read() Then
                        ' Ambil tipe harga dan harga jual produk
                        Dim tipe As Integer = Convert.ToInt32(dr("tipe"))
                        Dim hargaJual As Integer = Convert.ToInt32(dr("harga_jual"))

                        ' Format harga jual untuk menampilkan ribuan (contoh: 10.000, 50.000)
                        Dim formattedHargaJual As String = hargaJual.ToString("N0")

                        ' Masukkan harga yang sudah diformat ke ComboBox2
                        ComboBox2.Items.Add(formattedHargaJual)  ' Menambahkan harga yang sudah diformat
                        ComboBox2.SelectedIndex = 0  ' Secara otomatis pilih harga pertama
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal mengambil data produk: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

End Class

