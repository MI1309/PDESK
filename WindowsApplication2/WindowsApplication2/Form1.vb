Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = Color.AliceBlue
        Me.TopMost = False
        Me.Text = "imron"

    End Sub

    Private Sub Form1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        MsgBox("lawak dek ")
    End Sub
    Private Sub Form1_closed(ByVal sender As Object, ByVal e As ToolStripDropDownClosedEventArgs) Handles Me.FormClosed
        MsgBox("mau ditutup")
    End Sub
    Private Sub Form1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosing
        MsgBox("done gk")
    End Sub
End Class
