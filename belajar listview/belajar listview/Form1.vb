Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call rancanglistbuku()

    End Sub
    Sub rancanglistbuku()
        ListView1.Columns.Add("kode barang", 200, HorizontalAlignment.Center)
        ListView1.Columns.Add("nama barang", 250, HorizontalAlignment.Center)
        ListView1.Columns.Add("Jumlah", 190, HorizontalAlignment.Center)
        ListView1.Columns.Add("harga", 130, HorizontalAlignment.Center)
        ListView1.View = View.Details
        ListView1.GridLines = True
        ListView1.FullRowSelect = True
    End Sub
    Sub simpanbuku()
        Dim isi As New ListViewItem
        isi.Text = TextBox1.Text
        isi.SubItems.Add(TextBox2.Text)
        isi.SubItems.Add(TextBox3.Text)
        isi.SubItems.Add(TextBox4.Text)
        ListView1.Items.Add(isi)
    End Sub
    Sub clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim pesan As String
        pesan = MsgBox("apakah anda ingin menyimpan ini ?", MsgBoxStyle.YesNo, "konfirmasi")
        If pesan = vbYes Then
            Call simpanbuku()
        Else
            Exit Sub
        End If
        Call clear()
    End Sub
End Class
