Imports System.Data.Odbc

Public Class Form1
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Dim loggedInUserUsername As String

    Sub tampilDataKasir()
        Try
            test_conn()
            ' Query untuk mengambil data nama lengkap kasir berdasarkan username yang login
            Dim query As String = "SELECT username FROM account WHERE username = ?"
            da = New OdbcDataAdapter(query, conn)

            ' Menambahkan parameter untuk username kasir yang sedang login
            da.SelectCommand.Parameters.AddWithValue("username", loggedInUserUsername)

            ds = New DataSet()
            da.Fill(ds, "username")

            ' Jika ada data kasir, tampilkan nama lengkap kasir
            If ds.Tables("username").Rows.Count > 0 Then
                Dim namaKasir As String = ds.Tables("username").Rows(0)("username").ToString()
                MsgBox("Welcome, " & namaKasir)
            End If

            conn.Close()
        Catch ex As Exception
            MsgBox("Gagal menampilkan data kasir: " & ex.Message)
        End Try
    End Sub

    Sub test_conn()
        Try
            ' Menggunakan DSN yang sudah dikonfigurasi di ODBC Data Source Administrator
            mysql = "DSN=pulsa;"
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Koneksi gagal: " & ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' validasi input
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
            ' Koneksi dan pengecekan ke database
            Try
                Call test_conn()
                Dim cmd As New OdbcCommand("SELECT * FROM account WHERE username=? AND password=? AND role=?", conn)
                cmd.Parameters.AddWithValue("@username", TextBox1.Text)
                cmd.Parameters.AddWithValue("@password", TextBox2.Text)
                cmd.Parameters.AddWithValue("@role", ComboBox1.Text)

                Dim rd As OdbcDataReader = cmd.ExecuteReader()

                If rd.HasRows Then
                    rd.Read()
                    ' Menyimpan username yang login untuk digunakan di tampilDataKasir
                    loggedInUserUsername = TextBox1.Text

                    ' routing form
                    If TextBox1.Text.ToLower = "admin" And TextBox2.Text.ToLower = "admin" And ComboBox1.Text.ToLower = "admin" Then
                        tampilDataKasir()
                        Threading.Thread.Sleep(100)
                        Form2.Show()
                        Me.Hide()
                    ElseIf ComboBox1.Text.ToLower = "kasir" Then
                        Form3.loggedInUserUsername = TextBox1.Text
                        tampilDataKasir()
                        Threading.Thread.Sleep(100) ' Delay 1 detik
                        Form3.Show()
                        Me.Hide()
                    End If
                Else
                    MsgBox("Kredensial Login Anda Salah")
                End If

            Catch ex As Exception
                MsgBox("Terjadi kesalahan: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            TextBox2.Focus()
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            ComboBox1.Focus()
        End If
    End Sub

    Private Sub ComboBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles ComboBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Button1.PerformClick() ' Login langsung
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BeginInvoke(New MethodInvoker(Sub()
                                             TextBox1.Focus()
                                         End Sub))
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ' meload data tabel ke combo box
        Try
            test_conn()
            Dim cmd As New OdbcCommand("SELECT DISTINCT role FROM account", conn)
            Dim rd As OdbcDataReader = cmd.ExecuteReader()

            ComboBox1.Items.Clear()
            While rd.Read()
                ComboBox1.Items.Add(rd("role").ToString())
            End While

            rd.Close()
            conn.Close()
        Catch ex As Exception
            MsgBox("Gagal mengambil data role: " & ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
