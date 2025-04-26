Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Kosong, tidak apa-apa
    End Sub

    ' Tombol buka Form4
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim f4 As New Form4()
        f4.previousForm = Me ' Kirim Form2 sebagai previousForm
        f4.Show()
        Me.Hide()
    End Sub

    ' Tombol buka Form5
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim f5 As New Form5()
        f5.previousForm = Me ' Kirim Form2 sebagai previousForm
        f5.Show()
        Me.Hide()
    End Sub

    ' Tombol keluar aplikasi
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    ' Tombol buka Form6
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim f6 As New Form6()
        f6.previousForm = Me ' Kirim Form2 sebagai previousForm
        f6.Show()
        Me.Hide()
    End Sub

End Class
