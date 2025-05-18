Imports System.Data.Odbc

Public Class daftar_product
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Public loggedInUserId As Integer
    Private selectedId As Integer = -1
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
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
    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LoadDataToGrid()
        setupDataGridView()
    End Sub

    ' form 5
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f5 As New form_provider()
        f5.previousForm = Me ' Kirim Form2 sebagai previousForm
        f5.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' form 8
        Dim f7 As New form_stock()
        f7.previousForm = Me ' Kirim Form2 sebagai previousForm
        f7.Show()
        Me.Hide()
    End Sub
    Sub setupDataGridView()
        With DataGridView1
            ' Header style
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
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            .RowTemplate.Height = 30
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .MultiSelect = False

            ' Grid border settings
            .ScrollBars = ScrollBars.None
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.Single           ' Garis antar cell
            .GridColor = Color.LightGray                                   ' Warna garis pemisah
            .RowHeadersVisible = False

            ' Disable user interaction
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToOrderColumns = False
        End With
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
            If DataGridView1.Columns.Contains("nama_produk") Then
                DataGridView1.Columns("nama_produk").HeaderText = "Nama Produk"
            End If
            If DataGridView1.Columns.Contains("harga_jual") Then
                DataGridView1.Columns("harga_jual").HeaderText = "Harga Jual"
            End If
            If DataGridView1.Columns.Contains("tanggal_restock") Then
                DataGridView1.Columns("tanggal_restock").HeaderText = "Tanggal Restock"
            End If
            If DataGridView1.Columns.Contains("harga_kulak") Then
                DataGridView1.Columns("harga_kulak").HeaderText = "Harga Kulak"
            End If
            If DataGridView1.Columns.Contains("stok") Then
                DataGridView1.Columns("stok").HeaderText = "Stok"
            End If

        Catch ex As Exception
            MsgBox("Gagal memuat data ke dalam DataGridView: " & ex.Message, vbCritical)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub
    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub TabPage2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        LoadDataToGrid()
    End Sub

End Class