Imports System.Data.Odbc

Public Class Form1
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String

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
            MsgBox("username dan password kosong")
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
                    ' routing form
                    If TextBox1.Text.ToLower = "admin" And TextBox2.Text.ToLower = "admin" And ComboBox1.Text.ToLower = "admin" Then
                        Form2.Show()
                        Me.Hide()
                    ElseIf ComboBox1.Text.ToLower = "kasir" Then
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

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

End Class
