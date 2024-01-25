Public Class Form1
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Set up columns for the ListView
        ListView1.Columns.Add("ID", 50, HorizontalAlignment.Left)
        ListView1.Columns.Add("Name", 150, HorizontalAlignment.Left)
        ListView1.Columns.Add("Age", 50, HorizontalAlignment.Left)

        ' Add data to the ListView
        AddDataToListView(1, "John Doe", 25)
        AddDataToListView(2, "Jane Smith", 30)
        AddDataToListView(3, "Bob Johnson", 22)
    End Sub

    Private Sub AddDataToListView(ByVal id As Integer, ByVal name As String, ByVal age As Integer)
        ' Create a ListViewItem and add sub-items
        Dim item As New ListViewItem(id.ToString())
        item.SubItems.Add(name)
        item.SubItems.Add(age.ToString())

        ' Add the ListViewItem to the ListView
        ListView1.Items.Add(item)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        form2.show()

    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class

