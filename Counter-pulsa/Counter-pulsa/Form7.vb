Public Class Form7

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
        If previousForm IsNot Nothing Then
            previousForm.Show()
        End If
    End Sub
    Public previousForm As Form
    Private Sub Form7_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    ' form 5
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim f5 As New Form5()
        f5.previousForm = Me ' Kirim Form2 sebagai previousForm
        f5.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ' form 8
        Dim f8 As New Form8()
        f8.previousForm = Me ' Kirim Form2 sebagai previousForm
        f8.Show()
        Me.Hide()
    End Sub
End Class