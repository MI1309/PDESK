Imports System.Data.Odbc
Public Class Form6
    Public previousForm As Form
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
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
            da = New OdbcDataAdapter("SELECT * FROM transaksi", conn)
            ds = New DataSet()
            da.Fill(ds, "transaksi")
            DataGridView1.DataSource = ds.Tables("transaksi")
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

    Private Sub Form6_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tampilData()
        setupDataGridView()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub
End Class