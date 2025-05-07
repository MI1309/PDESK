Imports System.Data.Odbc

Public Class Form3

    Public loggedInUserUsername As String ' Username kasir yang login
    Public previousForm As Form

    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Private selectedNamaProduk As String = "" ' Nama produk yang dipilih di tabel

    'valid input
    Private Function ValidasiInputKosong(ByVal ParamArray controls() As Control) As Boolean
        Dim kosongList As New List(Of String)
        Dim controlDescriptions As New Dictionary(Of String, String) From {
            {"TextBox2", "Nomor hp"},
            {"ComboBox1", "Kartu"},
            {"ComboBox2", "harga"},
            {"TextBox3", "quantity"}
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
    'load produk
    Private Function GetOperatorByPrefix(ByVal nomor As String) As String
        If nomor.StartsWith("0811") OrElse nomor.StartsWith("0812") OrElse nomor.StartsWith("0813") OrElse nomor.StartsWith("0821") OrElse nomor.StartsWith("0822") OrElse nomor.StartsWith("0823") Then
            Return "Telkomsel"
        ElseIf nomor.StartsWith("0852") OrElse nomor.StartsWith("0853") OrElse nomor.StartsWith("0817") OrElse nomor.StartsWith("0818") Then
            Return "XL"
        ElseIf nomor.StartsWith("0857") OrElse nomor.StartsWith("0856") OrElse nomor.StartsWith("0858") Then
            Return "Indosat"
        ElseIf nomor.StartsWith("088") Then
            Return "Smartfren"
        ElseIf nomor.StartsWith("089") Then
            Return "Three"
        ElseIf nomor.StartsWith("0825") Then ' Prefix ByU (jaringan Telkomsel)
            Return "ByU"
        Else
            Return "Tidak Dikenal"
        End If
    End Function


    Sub loadproduk()
        Try
            test_conn()
            Using cmd As New OdbcCommand("SELECT nama_produk FROM admin_product WHERE kasir_username = ? AND stok > 0", conn)
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


    Sub tampilData()
        Try
            test_conn()
            Dim query As String = "SELECT nama_produk, harga_jual, stok, tanggal_restock FROM admin_product WHERE kasir_username = ? AND stok > 0"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
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



    ' Saat baris diklik di DataGridView
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            selectedNamaProduk = DataGridView1.Rows(e.RowIndex).Cells("nama_produk").Value.ToString()
        End If
    End Sub
    Dim selectedId = -1
    Private Sub resetForm()
        TextBox2.Clear()
        TextBox3.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
        selectedId = -1
    End Sub

    ' Tombol Hapus (Button2)
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        resetForm()
    End Sub

    ' Tutup form (Button4)
    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub
    ' load harga
    Sub loadharga(Optional ByVal namaProduk As String = "")
        Try
            test_conn()
            Dim query As String = "SELECT harga_jual FROM admin_product WHERE kasir_username = ?"

            If Not String.IsNullOrEmpty(namaProduk) Then
                query &= " AND nama_produk = ?"
            End If

            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)

                If Not String.IsNullOrEmpty(namaProduk) Then
                    cmd.Parameters.AddWithValue("@nama_produk", namaProduk)
                End If

                Using dr = cmd.ExecuteReader()
                    ComboBox2.Items.Clear()
                    While dr.Read()
                        ComboBox2.Items.Add(dr("harga_jual").ToString())
                    End While
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Gagal memuat data harga: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub
    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles TextBox2.KeyPress
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan di nomor telepon!")
        End If
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
        If Not ValidasiInputKosong(TextBox2, ComboBox1, ComboBox2, TextBox3) Then Exit Sub
        If TextBox2.Text.Length < 12 Then
            MsgBox("Nomor HP harus terdiri dari minimal 12 digit.", vbExclamation, "Validasi Nomor")
            Exit Sub
        End If

        Try
            test_conn()

            Dim nomor As String = TextBox2.Text.Trim()
            Dim stok As Integer = CInt(TextBox3.Text)
            Dim kartu As String = ComboBox1.Text
            Dim harga As Integer = CInt(ComboBox2.SelectedItem)
            Dim waktuTransaksi As DateTime = DateTime.Now
            Dim productId As Integer = 0
            Dim stokTersedia As Integer = 0
            Dim operatorNomor = GetOperatorByPrefix(nomor)

            'validasi kartu hp agar sesuai yang dibeli
            If operatorNomor = "Tidak Dikenal" Then
                MsgBox("Nomor tidak dikenali operator-nya.", vbExclamation, "Nomor Tidak Valid")
                Exit Sub
            End If

            If Not kartu.ToLower().Contains(operatorNomor.ToLower()) Then
                MsgBox("Produk '" & kartu & "' tidak sesuai dengan operator nomor ini (" & operatorNomor & ").", vbExclamation, "Validasi Produk")
                Exit Sub
            End If

            ' Ambil id & stok
            Dim getIdQuery As String = "SELECT id, stok FROM admin_product WHERE nama_produk = ? AND kasir_username = ?"
            Using getIdCmd As New OdbcCommand(getIdQuery, conn)
                getIdCmd.Parameters.AddWithValue("@nama_produk", kartu)
                getIdCmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                Using dr = getIdCmd.ExecuteReader()
                    If dr.Read() Then
                        productId = Convert.ToInt32(dr("id"))
                        stokTersedia = Convert.ToInt32(dr("stok"))
                    Else
                        MsgBox("Produk tidak ditemukan.", vbExclamation, "Data Produk")
                        Exit Sub
                    End If
                End Using
            End Using

            ' Validasi stok
            If stok > stokTersedia Then
                MsgBox("Stok tidak mencukupi. Tersedia: " & stokTersedia, vbExclamation, "Stok Tidak Cukup")
                Exit Sub
            ElseIf stok < 1 Then
                MsgBox("Jumlah stok harus lebih dari 0.", vbExclamation, "Input Salah")
                Exit Sub
            End If

            ' Simpan transaksi
            Dim sql As String = "INSERT INTO transaksi ( nomor_tujuan, product_id, harga, waktu_transaksi, quantity, product, kasir_username) VALUES (?, ?, ?, ?, ?, ?, ?)"
            Using cmd As New OdbcCommand(sql, conn)
                cmd.Parameters.AddWithValue("@nomor_tujuan", nomor)
                cmd.Parameters.AddWithValue("@product_id", productId)
                cmd.Parameters.AddWithValue("@harga", harga)
                cmd.Parameters.AddWithValue("@waktu_transaksi", waktuTransaksi)
                cmd.Parameters.AddWithValue("@quantity", stok)
                cmd.Parameters.AddWithValue("@product", kartu)
                cmd.Parameters.AddWithValue("@kasir_username", loggedInUserUsername)
                cmd.ExecuteNonQuery()
            End Using

            ' Update stok
            Dim updateStokSql As String = "UPDATE admin_product SET stok = stok - ? WHERE id = ?"
            Using updateCmd As New OdbcCommand(updateStokSql, conn)
                updateCmd.Parameters.AddWithValue("@stok", stok)
                updateCmd.Parameters.AddWithValue("@id", productId)
                updateCmd.ExecuteNonQuery()
            End Using

            ' Hapus produk jika stok habis
            Dim cekStokQuery As String = "SELECT stok FROM admin_product WHERE id = ?"
            Using cekCmd As New OdbcCommand(cekStokQuery, conn)
                cekCmd.Parameters.AddWithValue("@id", productId)
                Using dr = cekCmd.ExecuteReader()
                    If dr.Read() AndAlso Convert.ToInt32(dr("stok")) <= 0 Then
                        Dim deleteProdukQuery As String = "DELETE FROM admin_product WHERE id = ?"
                        Using deleteCmd As New OdbcCommand(deleteProdukQuery, conn)
                            deleteCmd.Parameters.AddWithValue("@id", productId)
                            deleteCmd.ExecuteNonQuery()
                        End Using
                        MsgBox("Stok habis. Produk dihapus dari daftar.", vbInformation, "Produk Habis")
                    End If
                End Using
            End Using

            MsgBox("Transaksi berhasil disimpan dan stok diperbarui.", vbInformation, "Transaksi Berhasil")

            ' Refresh & Reset
            tampilData()
            loadproduk()
            loadharga()
            resetForm()

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat menyimpan transaksi: " & ex.Message, vbCritical, "Kesalahan")
            TulisLogError("Transaksi gagal: " & ex.ToString())
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        ' Hanya izinkan angka dan tombol kontrol seperti backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            MsgBox("Hanya angka yang diperbolehkan !")
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex <> -1 Then
            Dim selectedProduk As String = ComboBox1.SelectedItem.ToString()
            loadharga(selectedProduk)
            ComboBox2.SelectedIndex = -1 ' reset harga yang dipilih sebelumnya
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class
