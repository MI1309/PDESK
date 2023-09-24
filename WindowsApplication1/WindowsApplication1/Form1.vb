Public Class Form1

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim panjang As Integer
        Dim Lebar As Integer
        Dim luas As Integer
        Dim keliling As Integer

        panjang = Val(TextBox1.Text)
        Lebar = Val(TextBox2.Text)

        luas = panjang * Lebar
        keliling = 2 * (panjang + Lebar)

        TextBox3.Text = luas
        TextBox4.Text = keliling

        If RadioButton1.Checked = True Then
            TextBox3.ForeColor = Color.Red
            TextBox4.ForeColor = Color.Red
        End If

        If RadioButton2.Checked = True Then
            TextBox3.ForeColor = Color.Blue
            TextBox4.ForeColor = Color.Blue
        End If

        If RadioButton3.Checked = True Then
            TextBox3.ForeColor = Color.Yellow
            TextBox4.ForeColor = Color.Yellow
        End If

        If RadioButton4.Checked = True Then
            TextBox3.ForeColor = Color.Green
            TextBox4.ForeColor = Color.Green
        End If


    End Sub
End Class
