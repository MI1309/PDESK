Module Module1
    Public conn As OleDbConnection
    Public da As OleDbDataAdapter
    Public ds As DataSet
    Public cmd As OleDbCommand
    Public rd As OleDbDataReader
    Dim lokasiDB As String

    Public Sub koneksi()
        lokasiDB = "Provider=Microsoft.ACE.OLEDB.13.0;data source=loginDB.accdb"
        conn = New OleDbConnection(lokasiDB)

        If conn.State = ConnectionState.Closed Then conn.Open()
    End Sub
End Module
