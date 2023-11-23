Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call rancanglistbuku()
    End Sub
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
       
        If ComboBox1.Text = "1" Then
            isi.SubItems.Add("1")
        ElseIf ComboBox1.Text = "2" Then
            isi.SubItems.Add("3")
        ElseIf ComboBox1.Text = "4" Then
            isi.SubItems.Add("4")
        ElseIf ComboBox1.Text = "5" Then
            isi.SubItems.Add("5")
        ElseIf ComboBox1.Text = "6" Then
            isi.SubItems.Add("6")
        ElseIf ComboBox1.Text = "7" Then
            isi.SubItems.Add("7")
        ElseIf ComboBox1.Text = "8" Then
            isi.SubItems.Add("8")
        ElseIf ComboBox1.Text = "9" Then
            isi.SubItems.Add("9")
        ElseIf ComboBox1.Text = "10" Then
            isi.SubItems.Add("10")
        Else
            isi.SubItems.Add("")
        End If

        If ComboBox2.Text = "1" Then
            isi.SubItems.Add("1")
        ElseIf ComboBox2.Text = "2" Then
            isi.SubItems.Add("3")
        ElseIf ComboBox2.Text = "4" Then
            isi.SubItems.Add("4")
        ElseIf ComboBox2.Text = "5" Then
            isi.SubItems.Add("5")
        ElseIf ComboBox2.Text = "6" Then
            isi.SubItems.Add("6")
        ElseIf ComboBox2.Text = "7" Then
            isi.SubItems.Add("7")
        ElseIf ComboBox2.Text = "8" Then
            isi.SubItems.Add("8")
        ElseIf ComboBox2.Text = "9" Then
            isi.SubItems.Add("9")
        ElseIf ComboBox2.Text = "10" Then
            isi.SubItems.Add("10")
        Else
            isi.SubItems.Add("")
        End If

        If RadioButton1.Checked = True Then
            isi.SubItems.Add("Laki-laki")
        ElseIf RadioButton2.Checked = True Then
            isi.SubItems.Add("Perempuan")
        Else
            isi.SubItems.Add("")
        End If
        If ComboBox3.Text = "islam" Then
            isi.SubItems.Add("islam")
        ElseIf ComboBox3.Text = "kristen" Then
            isi.SubItems.Add("kristen")
        ElseIf ComboBox3.Text = "Khatolik" Then
            isi.SubItems.Add("khatolik")
        ElseIf ComboBox3.Text = "budha" Then
            isi.SubItems.Add("budha")
        ElseIf ComboBox3.Text = "hindu" Then
            isi.SubItems.Add("hindu")
        Else
            isi.SubItems.Add("")
        End If
        ListView1.Items.Add(isi)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ListView1.Items.Clear()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Call clear()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call simpanbuku()
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click

    End Sub
End Class