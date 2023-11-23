Public Class Form2
    Sub rancanglistbuku()
        ListView1.Columns.Add("Nik", 200, HorizontalAlignment.Center)
        ListView1.Columns.Add("Nama", 250, HorizontalAlignment.Center)
        ListView1.Columns.Add("RT", 190, HorizontalAlignment.Center)
        ListView1.Columns.Add("RW", 130, HorizontalAlignment.Center)
        ListView1.Columns.Add("Gender", 130, HorizontalAlignment.Center)
        ListView1.Columns.Add("Agama", 130, HorizontalAlignment.Center)
        ListView1.View = View.Details
        ListView1.GridLines = True
        ListView1.FullRowSelect = True
    End Sub

    Sub clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
        ComboBox3.Text = ""
        RadioButton1.Checked = False
        RadioButton2.Checked = False

    End Sub

    Sub simpanbuku()
        Dim isi As New ListViewItem
        isi.Text = TextBox1.Text
        isi.SubItems.Add(TextBox2.Text)
        If RadioButton1.Checked = True Then
            isi.SubItems.Add("Laki-laki")
        ElseIf RadioButton2.Checked = True Then
            isi.SubItems.Add("Perempuan")
        Else
            isi.SubItems.Add("")
        End If

        If ComboBox1.Text = "islam" Then
            isi.SubItems.Add("islam")
        ElseIf ComboBox1.Text = "kristen" Then
            isi.SubItems.Add("kristen")
        ElseIf ComboBox1.Text = "Khatolik" Then
            isi.SubItems.Add("khatolik")
        ElseIf ComboBox1.Text = "budha" Then
            isi.SubItems.Add("budha")
        ElseIf ComboBox1.Text = "hindu" Then
            isi.SubItems.Add("hindu")
        Else
            isi.SubItems.Add("")
        End If
        ListView1.Items.Add(isi)
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub
End Class