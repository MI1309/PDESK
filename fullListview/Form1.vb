Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call rancanglistbuku()
    End Sub
    Sub rancanglistbuku()
        ListView1.Columns.Add("SEMEN ID", 50, HorizontalAlignment.Center)
        ListView1.Columns.Add("BULL NAME", 250, HorizontalAlignment.Center)
        ListView1.Columns.Add("BREED", 190, HorizontalAlignment.Center)
        ListView1.Columns.Add("QUANTITY", 50, HorizontalAlignment.Center)
        ListView1.Columns.Add("LOCATION", 130, HorizontalAlignment.Center)
        ListView1.Columns.Add("DATE PURCHASED", 230, HorizontalAlignment.Center)
        ListView1.View = View.Details
        ListView1.GridLines = True
        ListView1.FullRowSelect = True
    End Sub

    Sub clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        ComboBox3.Text = ""
    End Sub
    Sub simpanbuku()
        Dim isi As New ListViewItem
        isi.Text = TextBox1.Text
        isi.SubItems.Add(TextBox2.Text)
        isi.SubItems.Add(TextBox3.Text)

        If ComboBox3.Text = "1" Then
            isi.SubItems.Add("1")
        ElseIf ComboBox3.Text = "2" Then
            isi.SubItems.Add("2")
        ElseIf ComboBox3.Text = "3" Then
            isi.SubItems.Add("3")
        ElseIf ComboBox3.Text = "4" Then
            isi.SubItems.Add("4")
        ElseIf ComboBox3.Text = "5" Then
            isi.SubItems.Add("5")
        ElseIf ComboBox3.Text = "6" Then
            isi.SubItems.Add("6")
        ElseIf ComboBox3.Text = "7" Then
            isi.SubItems.Add("7")
        ElseIf ComboBox3.Text = "8" Then
            isi.SubItems.Add("8")
        ElseIf ComboBox3.Text = "9" Then
            isi.SubItems.Add("9")
        ElseIf ComboBox3.Text = "10" Then
            isi.SubItems.Add("10")
        ElseIf ComboBox3.Text = "11" Then
            isi.SubItems.Add("11")
        ElseIf ComboBox3.Text = "12" Then
            isi.SubItems.Add("12")
        ElseIf ComboBox3.Text = "13" Then
            isi.SubItems.Add("13")
        ElseIf ComboBox3.Text = "14" Then
            isi.SubItems.Add("14")
        ElseIf ComboBox3.Text = "15" Then
            isi.SubItems.Add("15")
        ElseIf ComboBox3.Text = "16" Then
            isi.SubItems.Add("16")
        ElseIf ComboBox3.Text = "17" Then
            isi.SubItems.Add("17")
        ElseIf ComboBox3.Text = "18" Then
            isi.SubItems.Add("18")
        ElseIf ComboBox3.Text = "19" Then
            isi.SubItems.Add("19")
        ElseIf ComboBox3.Text = "20" Then
            isi.SubItems.Add("20")
        Else
            isi.SubItems.Add("")
        End If

        isi.SubItems.Add(TextBox4.Text)
        isi.SubItems.Add(DateTimePicker1.Text)
        
        ListView1.Items.Add(isi)
    End Sub
   

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Call clear()
    End Sub

    Private Sub Label10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label10.Click

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call simpanbuku()
        Call clear()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ListView1.SelectedItems.Count > 0 Then
            ListView1.Items.Remove(ListView1.SelectedItems(0))
        End If
        Call clear()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If ListView1.SelectedItems.Count > 0 Then
            ListView1.SelectedItems(0).SubItems(0).Text = TextBox1.Text
            ListView1.SelectedItems(0).SubItems(1).Text = TextBox2.Text
            ListView1.SelectedItems(0).SubItems(2).Text = TextBox3.Text
            ListView1.SelectedItems(0).SubItems(3).Text = ComboBox3.Text
            ListView1.SelectedItems(0).SubItems(4).Text = TextBox4.Text
            ListView1.SelectedItems(0).SubItems(5).Text = DateTimePicker1.Text
            Call clear()
        End If
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        TextBox1.Text = ListView1.SelectedItems(0).Text
        TextBox2.Text = ListView1.SelectedItems(0).SubItems(1).Text
        TextBox3.Text = ListView1.SelectedItems(0).SubItems(2).Text
        ComboBox3.Text = ListView1.SelectedItems(0).SubItems(3).Text
        TextBox4.Text = ListView1.SelectedItems(0).SubItems(4).Text
        DateTimePicker1.Text = ListView1.SelectedItems(0).SubItems(5).Text
    End Sub
End Class
