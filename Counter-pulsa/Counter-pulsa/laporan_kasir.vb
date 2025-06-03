Imports System.Data.Odbc

Public Class laporan_kasir
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Public loggedInUsername As String = "" ' Username yang sedang login

    Sub test_conn()
        Try
            mysql = "DSN=pulsa;"
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

    Sub tampilData()
        Try
            test_conn()
            Dim query As String = "SELECT id, nomor_tujuan, product_id, harga, waktu_transaksi, product, kasir_username, uang_pembeli FROM transaksi WHERE kasir_username = ?"
            Dim cmd As New OdbcCommand(query, conn)
            cmd.Parameters.AddWithValue("?", loggedInUsername)

            da = New OdbcDataAdapter(cmd)
            ds = New DataSet()
            da.Fill(ds, "transaksi")
            DataGridView1.DataSource = ds.Tables("transaksi")
            setupDataGridView()
        Catch ex As Exception
            MsgBox("Gagal menampilkan data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub filterData()
        Try
            test_conn()
            Dim tanggal As String = DateTimePicker1.Value.ToString("yyyy-MM-dd")
            Dim query As String = "SELECT id, nomor_tujuan, product_id, harga, waktu_transaksi, product, kasir_username, uang_pembeli FROM transaksi WHERE kasir_username = ? AND DATE(waktu_transaksi) = ?"
            Dim cmd As New OdbcCommand(query, conn)

            ' Tambahkan parameter sesuai urutan
            cmd.Parameters.AddWithValue("?", loggedInUsername)
            cmd.Parameters.AddWithValue("?", tanggal)

            da = New OdbcDataAdapter(cmd)
            ds = New DataSet()
            da.Fill(ds, "transaksi")
            DataGridView1.DataSource = ds.Tables("transaksi")
            setupDataGridView()
        Catch ex As Exception
            MsgBox("Gagal memfilter data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub setupDataGridView()
        If DataGridView1.DataSource Is Nothing OrElse DataGridView1.Columns.Count = 0 Then Exit Sub

        With DataGridView1
            If .Columns.Contains("id") Then .Columns("id").HeaderText = "Id"
            If .Columns.Contains("nomor_tujuan") Then .Columns("nomor_tujuan").HeaderText = "Nomor Tujuan"
            If .Columns.Contains("product_id") Then .Columns("product_id").HeaderText = "Id Produk"
            If .Columns.Contains("harga") Then .Columns("harga").HeaderText = "Harga"
            If .Columns.Contains("waktu_transaksi") Then .Columns("waktu_transaksi").HeaderText = "Waktu Transaksi"
            If .Columns.Contains("product") Then .Columns("product").HeaderText = "Produk"
            If .Columns.Contains("kasir_username") Then .Columns("kasir_username").HeaderText = "Nama Kasir"
            If .Columns.Contains("uang_pembeli") Then .Columns("uang_pembeli").HeaderText = "Uang Pembeli"

            .BackgroundColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 85, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .EnableHeadersVisualStyles = False

            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            .RowTemplate.Height = 30
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .MultiSelect = False

            .ScrollBars = ScrollBars.None
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .GridColor = Color.LightGray
            .RowHeadersVisible = False

            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToOrderColumns = False
        End With
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DateTimePicker1.ValueChanged
        filterData()
    End Sub

    Private Sub laporan_kasir_Activated(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Activated
        ' optional
    End Sub

    Private Sub laporan_kasir_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        Me.ControlBox = False
        tampilData()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        form_kasir.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        DateTimePicker1.Value = DateTime.Now
        tampilData()
    End Sub
End Class
