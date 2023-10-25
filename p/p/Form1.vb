Public Class Form1


    Private Sub Pilih_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Pilih.Click
        If CheckBox1.Checked = True Then
            Label1.Font.Bold = True

        End If

        If CheckBox2.Checked = True Then
            Label1.Font.Italic = True

        End If

        If CheckBox3.Checked = True Then
            Label1.Font.Underline = True

        End If
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class