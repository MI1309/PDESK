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
        Catch ex As Exception
            MsgBox("Gagal memfilter data: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub loadUsersToRadioButtons()
        Try
            test_conn()
            Dim query As String = "SELECT username FROM account WHERE username <> 'admin'"
            da = New OdbcDataAdapter(query, conn)
            ds = New DataSet()
            da.Fill(ds, "account")

            GroupBox1.Controls.Clear()

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
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DateTimePicker1.ValueChanged
        filterData()
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        DateTimePicker1.Value = DateTime.Now
        selectedUsername = ""
        tampilData()
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

    Private Sub Form6_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        tampilData()
        setupDataGridView()
        loadUsersToRadioButtons()
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub
End Class
