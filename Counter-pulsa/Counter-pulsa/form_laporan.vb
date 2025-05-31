Imports System.Data.Odbc

Public Class form_laporan
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Dim selectedUsername As String = "" ' Username yang dipilih dari RadioButton

    Sub test_conn()
        Try
            mysql = "DSN=pulsa;" ' Ganti sesuai DSN milikmu
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

    Sub tampilData()
        Try
            test_conn()
            da = New OdbcDataAdapter("SELECT id, nomor_tujuan, product_id, harga, waktu_transaksi, product, kasir_username, uang_pembeli FROM transaksi", conn)
            ds = New DataSet()
            da.Fill(ds, "transaksi")
            DataGridView1.DataSource = ds.Tables("transaksi")
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
            Dim query As String = "SELECT * FROM transaksi WHERE 1=1"
            Dim cmd As New OdbcCommand()
            cmd.Connection = conn

            If Not String.IsNullOrEmpty(tanggal) Then
                query &= " AND DATE(waktu_transaksi) = ?"
                cmd.Parameters.AddWithValue("?", tanggal)
            End If

            If Not String.IsNullOrEmpty(selectedUsername) Then
                query &= " AND kasir_username = ?"
                cmd.Parameters.AddWithValue("?", selectedUsername)
            End If

            cmd.CommandText = query
            da = New OdbcDataAdapter(cmd)
            ds = New DataSet()
            da.Fill(ds, "transaksi")
            DataGridView1.DataSource = ds.Tables("transaksi")

            ' ✅ Tambahkan ini untuk menjaga tampilan
            setupDataGridView()

        Catch ex As Exception
            MsgBox("Gagal memfilter data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    Sub loadUsersToRadioButtons()
        Try
            test_conn()
            Dim query As String = "SELECT username FROM account WHERE role = 'kasir'"
            da = New OdbcDataAdapter(query, conn)
            ds = New DataSet()
            da.Fill(ds, "account")

            GroupBox1.Controls.Clear()

            If ds.Tables("account").Rows.Count = 0 Then
                Dim lbl As New Label()
                lbl.Text = "Tidak ada username yang ada "
                lbl.AutoSize = True
                lbl.Location = New Point(20, 30)
                GroupBox1.Controls.Add(lbl)
                Return
            End If

            Dim yOffset As Integer = 30
            For Each row As DataRow In ds.Tables("account").Rows
                Dim username As String = row("username").ToString()

                Dim rb As New RadioButton()
                rb.Text = username
                rb.Location = New Point(20, yOffset)
                rb.AutoSize = True

                AddHandler rb.CheckedChanged, AddressOf RadioButton_CheckedChanged

                GroupBox1.Controls.Add(rb)
                yOffset += rb.Height + 5
            Next
            setupDataGridView()
        Catch ex As Exception
            MsgBox("Gagal memuat data pengguna: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    Private Sub RadioButton_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rb As RadioButton = DirectCast(sender, RadioButton)
        If rb.Checked Then
            selectedUsername = rb.Text
            filterData()
            setupDataGridView()
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DateTimePicker1.ValueChanged
        filterData()
        setupDataGridView()
    End Sub
    Private RadioButtonDummy As New RadioButton()

Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        DateTimePicker1.Value = DateTime.Now
        selectedUsername = ""
        DateTimePicker1.Checked = False
        For Each ctrl As Control In GroupBox1.Controls
            If TypeOf ctrl Is RadioButton Then
                CType(ctrl, RadioButton).Checked = False
            End If
        Next
        tampilData()
    End Sub


    Sub setupDataGridView()
        With DataGridView1
            ' label data
            .Columns("id").HeaderText = "Id"
            .Columns("nomor_tujuan").HeaderText = "Nomor Tujuan"
            .Columns("product_id").HeaderText = "Id Produk"
            .Columns("harga").HeaderText = "Harga"
            .Columns("waktu_transaksi").HeaderText = "Waktu Transaksi"
            .Columns("product").HeaderText = "Produk"
            .Columns("kasir_username").HeaderText = "Nama Kasir"
            .Columns("uang_pembeli").HeaderText = "Uang Pembeli"

            .BackgroundColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 85, 155)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .EnableHeadersVisualStyles = False

            ' Cell style
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .DefaultCellStyle.BackColor = Color.White
            .DefaultCellStyle.ForeColor = Color.Black
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255)
            .DefaultCellStyle.SelectionForeColor = Color.Black

            ' Alternating rows
            .AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue

            ' Grid behavior
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None ' ✅ penting agar tinggi baris tidak berubah
            .RowTemplate.Height = 30
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .MultiSelect = False

            ' Grid border settings
            .ScrollBars = ScrollBars.None
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .GridColor = Color.LightGray
            .RowHeadersVisible = False

            ' Disable user interaction
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToOrderColumns = False
        End With
    End Sub

    Private Sub form_laporan_Activated(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Activated
        loadUsersToRadioButtons()
        setupDataGridView()
    End Sub


    Private Sub form_laporan_load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        tampilData()
        setupDataGridView()
        loadUsersToRadioButtons()
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class
