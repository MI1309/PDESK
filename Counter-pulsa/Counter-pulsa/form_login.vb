Imports System.Data.Odbc

Public Class form_login
    Dim conn As OdbcConnection
    Dim da As OdbcDataAdapter
    Dim ds As DataSet
    Dim mysql As String
    Dim loggedInUserUsername As String

    ' Simplified function without querying by kasir_username
    Sub tampilDataKasir()
        Try
            test_conn()
            ' Query for the username directly from the account table
            Dim query As String = "SELECT username FROM account WHERE username = ?"
            da = New OdbcDataAdapter(query, conn)

            ' Add parameter for the logged-in username
            da.SelectCommand.Parameters.AddWithValue("username", loggedInUserUsername)

            ds = New DataSet()
            da.Fill(ds, "username")

            ' Show the username as a welcome message
            If ds.Tables("username").Rows.Count > 0 Then
                Dim namaKasir As String = ds.Tables("username").Rows(0)("username").ToString()
                MsgBox("Welcome, " & namaKasir)
            End If

            conn.Close()
        Catch ex As Exception
            MsgBox("Failed to fetch cashier data: " & ex.Message)
        End Try
    End Sub

    Sub test_conn()
        Try
            mysql = "DSN=pulsa;"
            conn = New OdbcConnection(mysql)
            conn.Open()
        Catch ex As Exception
            MsgBox("Connection failed: " & ex.Message)
        End Try
    End Sub
 Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Validasi input
        If TextBox1.Text = "" OrElse TextBox2.Text = "" OrElse ComboBox1.Text = "" Then
            MsgBox("Silakan lengkapi semua field: username, password, dan role")
            Exit Sub
        End If

        Try
            Call test_conn()

            ' Enkripsi password input
            Dim encryptedPassword As String = Encrypt(TextBox2.Text)

            ' Query: cocokkan username, encrypted password, dan role
            Dim cmd As New OdbcCommand("SELECT * FROM account WHERE username=? AND password=? AND role=?", conn)
            cmd.Parameters.AddWithValue("@username", TextBox1.Text)
            cmd.Parameters.AddWithValue("@password", encryptedPassword)
            cmd.Parameters.AddWithValue("@role", ComboBox1.Text)

            Dim rd As OdbcDataReader = cmd.ExecuteReader()

            If rd.HasRows Then
                rd.Read()
                loggedInUserUsername = TextBox1.Text
                Dim userRole As String = ComboBox1.Text.ToLower()

                ' Navigasi ke form sesuai role
                If userRole = "admin" Then
                    admin.Show()
                ElseIf userRole = "kasir" Then
                    form_kasir.loggedInUserUsername = TextBox1.Text
                    form_kasir.Show()
                End If

                Me.Close()
            Else
                MsgBox("Username atau password salah.")
            End If

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat login: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub



    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TextBox1.Text = "" ' Clear username
        TextBox2.Text = "" ' Clear password
        ComboBox1.SelectedIndex = -1 ' Clear ComboBox selection
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Application.Exit()
    End Sub

    ' Handle the KeyDown events for the TextBoxes and ComboBox to manage user input and navigation
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            TextBox2.Focus()
        End If
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            ComboBox1.Focus()
        End If
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub ComboBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles ComboBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Button1.PerformClick() ' Trigger login
        End If
        If e.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub

    ' Form Load Event - Setup ComboBox and TextBox Focus
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        loggedInUserUsername = ""
        Me.BeginInvoke(New MethodInvoker(Sub()
                                             TextBox1.Focus()
                                         End Sub))
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ' Load role data into ComboBox
        TextBox1.Clear()
        TextBox2.Clear()

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
            MsgBox("Failed to load roles: " & ex.Message)
        End Try
    End Sub

    ' TextBox TextChanged Event (Not Used But Left for Structure)
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class
