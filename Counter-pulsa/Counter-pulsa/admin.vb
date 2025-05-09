Public Class admin

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    ' Tombol buka Form4
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim f4 As New data_kasir()
        f4.previousForm = Me ' Kirim Form2 sebagai previousForm
        f4.Show()
        Me.Hide()
    End Sub

    ' Tombol buka Form7
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim load_product As New load_product()
        load_product.previousForm = Me ' Kirim Form2 sebagai previousForm
        load_product.Show()
        Me.Hide()
    End Sub

    ' Tombol keluar aplikasi
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        End
    End Sub

    ' Tombol buka Form6
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim f6 As New form_laporan()
        f6.previousForm = Me ' Kirim Form2 sebagai previousForm
        f6.Show()
        Me.Hide()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub
End Class
