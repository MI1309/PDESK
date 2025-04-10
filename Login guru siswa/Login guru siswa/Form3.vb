Public Class Form3
    Dim i, nuan As Integer
    Dim nama, jurusan, status As String

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the TextBox to read-only
        TextBox1.ReadOnly = True

        ' Populate ComboBox with numbers from 1 to 100
        For i As Integer = 1 To 100
            ComboBox2.Items.Add(i)
        Next

        ' Initialize counter
        i = 1

        ' Set initial value in TextBox1
        TextBox1.Text = kode()

        ' Set up the columns for ListView (if not done already in the designer)
        ListView1.View = View.Details
        ListView1.Columns.Add("NIS", 100)
        ListView1.Columns.Add("Nama", 100)
        ListView1.Columns.Add("jurusan", 100)
        ListView1.Columns.Add("nilai UAN", 100)
        ListView1.Columns.Add("Status", 100)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Fill variables and list
        isivariable()
        isilist()

        ' Clear the textboxes for the next input
        Call clear()

        ' Increment the counter and update TextBox1
        i += 1
        TextBox1.Text = kode()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call clear()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Sub isivariable()
        nama = TextBox2.Text
        jurusan = ComboBox1.Text
        nuan = CInt(ComboBox2.Text)
    End Sub

    Sub clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
    End Sub

    Function hitunghasil() As String
        If jurusan = "IPS" Then
            status = "berhasil"
        Else
            If nuan < 80 Then
                status = "Gagal"
            Else
                status = "Berhasil"
            End If
        End If
        Return status
    End Function


    Sub isilist()
        ' Use the proper function call for kode
        ListView1.Items.Add(kode()) ' Add the Kode

        ' Add the other values as SubItems
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(nama)
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(jurusan)
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(nuan.ToString())
        ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(hitunghasil())
    End Sub

    Function kode() As String
        Return "S0" & i.ToString()
    End Function
End Class