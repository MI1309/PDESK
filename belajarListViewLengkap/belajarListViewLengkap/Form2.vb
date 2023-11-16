Public Class Admin

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call simpanbuku()
        Call clear()
    End Sub

    Private Sub Admin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call rancanglistbuku()

    End Sub
    Sub rancanglistbuku()
        ListView1.Columns.Add("Nama", 200, HorizontalAlignment.Center)
        ListView1.Columns.Add("TTL", 250, HorizontalAlignment.Center)
        ListView1.Columns.Add("Gender", 190, HorizontalAlignment.Center)
        ListView1.Columns.Add("Agama", 130, HorizontalAlignment.Center)
        ListView1.Columns.Add("Hobby1", 130, HorizontalAlignment.Center)
        ListView1.Columns.Add("Hobby2", 130, HorizontalAlignment.Center)
        ListView1.Columns.Add("Hobby3", 130, HorizontalAlignment.Center)
        ListView1.View = View.Details
        ListView1.GridLines = True
        ListView1.FullRowSelect = True
    End Sub

    Sub clear()
        TextBox1.Text = ""
        DateTimePicker1.Text = ""
        ComboBox1.Text = ""
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = False


    End Sub

    Sub simpanbuku()
        Dim isi As New ListViewItem
        isi.Text = TextBox1.Text
        isi.SubItems.Add(DateTimePicker1.Text)
        If RadioButton1.Checked = True Then
            isi.SubItems.Add("Laki-laki")
        ElseIf RadioButton2.Checked = True Then
            isi.SubItems.Add("Perempuan")
        Else
            isi.SubItems.Add("")
        End If

        If ComboBox1.Text = "Islam" Then
            isi.SubItems.Add("Islam")
        ElseIf ComboBox1.Text = "Kristen" Then
            isi.SubItems.Add("Kristen")
        ElseIf ComboBox1.Text = "Khatolik" Then
            isi.SubItems.Add("Khatolik")
        ElseIf ComboBox1.Text = "Budha" Then
            isi.SubItems.Add("Budha")
        ElseIf ComboBox1.Text = "Kong huchu" Then
            isi.SubItems.Add("Kong huchu")
        Else
            isi.SubItems.Add("")
        End If

        If CheckBox1.Checked = True Then
            isi.SubItems.Add("Game")
        Else
            isi.SubItems.Add("")

        End If

        If CheckBox2.Checked = True Then
            isi.SubItems.Add("Sport")
        Else
            isi.SubItems.Add("")
        End If

        If CheckBox3.Checked = True Then
            isi.SubItems.Add("Travelling")
        Else
            isi.SubItems.Add("")
        End If
        ListView1.Items.Add(isi)
    End Sub


    Sub update()
        If ListView1.SelectedItems.Count > 0 Then
            'NGUPDATE START
            Dim selectedData As ListViewItem = ListView1.SelectedItems(0)


            If RadioButton1.Checked = True Then
                selectedData.SubItems(2).Text = "Laki-laki"

            ElseIf RadioButton2.Checked = True Then
                selectedData.SubItems(2).Text = "Perempuan"

            Else
                selectedData.SubItems(2).Text = ""

            End If

            If ComboBox1.Text = "Islam" Then
                selectedData.SubItems(3).Text = "Islam"

            ElseIf ComboBox1.Text = "Kristen" Then
                selectedData.SubItems(3).Text = "Kristen"

            ElseIf ComboBox1.Text = "Khatolik" Then
                selectedData.SubItems(3).Text = "Khatolik"

            ElseIf ComboBox1.Text = "Budha" Then
                selectedData.SubItems(3).Text = "Budha"

            ElseIf ComboBox1.Text = "Kong huchu" Then
                selectedData.SubItems(3).Text = "Kong huchu"

            Else
                selectedData.SubItems(3).Text = ""

            End If

            If CheckBox1.Checked = True Then
                selectedData.SubItems(4).Text = "Game"
            Else
                selectedData.SubItems(4).Text = ""
            End If

            If CheckBox2.Checked = True Then
                selectedData.SubItems(4).Text = "Sport"

            Else
                selectedData.SubItems(4).Text = ""

            End If

            If CheckBox3.Checked = True Then
                selectedData.SubItems(4).Text = "Travelling"

            Else
                selectedData.SubItems(4).Text = ""

            End If

            ' ngupdate end
        End If

    End Sub
    Sub find()
        Dim s As Integer = 1
        If ListView1.SelectedItems.Count > 0 And s = 1 Then

            Dim selectedData As ListViewItem = ListView1.SelectedItems(0)

            TextBox1.Text = selectedData.SubItems(0).Text
            DateTimePicker1.Text = selectedData.SubItems(1).Text
            ComboBox1.Text = selectedData.SubItems(3).Text

            If selectedData.SubItems(2).Text = "Laki-laki" Then
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            ElseIf selectedData.SubItems(2).Text = "Perempuan" Then
                RadioButton1.Checked = False
                RadioButton2.Checked = True
            End If


            If selectedData.SubItems(4).Text = "Game" Then
                CheckBox1.Checked = True

            End If
            If selectedData.SubItems(5).Text = "Sport" Then
                CheckBox2.Checked = True

            End If

            If selectedData.SubItems(6).Text = "Travelling" Then
                CheckBox3.Checked = True

            End If


        End If
       
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Call clear()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call simpanbuku()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Call find()
    End Sub
End Class