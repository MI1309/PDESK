Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Pilih_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Pilih.Click
        If CheckBox1.Checked = True Then
            Label1.Font = FontStyle.Bold
        End If
        If CheckBox2.Checked = True Then
            Label1.Font = FontStyle.Italic

        End If

        If CheckBox3.Checked = True Then
            Label1.Font = FontStyle.Underline

        End If
        If RadioButton1.Checked = True Then
            Label1.ForeColor = Color.Red
        End If
        If RadioButton2.Checked = True Then
            Label1.ForeColor = Color.Blue
        End If
        If RadioButton3.Checked = True Then
            Label1.ForeColor = Color.Yellow
        End If
    End Sub
End Class
