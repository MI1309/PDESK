Imports System.Data.OleDb
Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If TextBox1 = "" Or TextBox2.Text = "" Then
            MsgBox("Data belum lengkap, silahkan isi dulu !!")
        Else
            Call koneksi()
            cmd = New OleDbCommand("select * from TBL_USER where userID='"& TextBox1.Text & "'and userPass='"& TextBox2.Text"'", conn )
            rd = cmd.ExecuteReader
            rd.Read()
            If rd.HasRows Then
                Me.Hide()
                Form2.Show()
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close(S)
    End Sub
End Class
