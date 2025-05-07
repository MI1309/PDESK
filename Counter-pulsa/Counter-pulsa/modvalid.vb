Module modvalid
    Public Function ValidasiInputKosong(ByVal ParamArray controls() As Control) As Boolean
        Dim kosongList As New List(Of String)

        ' Loop untuk memeriksa TextBox
        For Each ctrl In controls
            If TypeOf ctrl Is TextBox AndAlso ctrl.Text.Trim() = "" Then
                kosongList.Add(ctrl.Tag.ToString())
            End If
        Next

        ' Loop untuk memeriksa ComboBox
        For Each ctrl In controls
            If TypeOf ctrl Is ComboBox Then
                Dim combo As ComboBox = DirectCast(ctrl, ComboBox)
                If combo.SelectedIndex = -1 Then ' Jika tidak ada item yang dipilih
                    kosongList.Add(combo.Tag.ToString())
                End If
            End If
        Next

        ' Jika ada yang kosong, tampilkan pesan
        If kosongList.Count > 0 Then
            MsgBox("Field berikut belum diisi: " & String.Join(", ", kosongList))
            Return False
        End If

        Return True
    End Function
    Public Function valid(ByVal ParamArray controls() As Control) As Boolean
        Dim kosongList As New List(Of String)

        ' Loop untuk memeriksa TextBox
        For Each ctrl In controls
            If TypeOf ctrl Is TextBox AndAlso ctrl.Text.Trim() = "" Then
                kosongList.Add(ctrl.Tag.ToString())
            End If
        Next

        ' Loop untuk memeriksa ComboBox
        For Each ctrl In controls
            If TypeOf ctrl Is ComboBox Then
                Dim combo As ComboBox = DirectCast(ctrl, ComboBox)
                If combo.SelectedIndex = -1 Then ' Jika tidak ada item yang dipilih
                    kosongList.Add(combo.Tag.ToString())
                End If
            End If
        Next

        ' Jika ada yang kosong, tampilkan pesan
        If kosongList.Count > 0 Then
            MsgBox("tidak ada data diupdate")
            Return False
        End If

        Return True
    End Function

    ' Validasi format email sederhana
    Public Function IsEmailValid(ByVal email As String) As Boolean
        Dim pattern As String = "^[\w\.-]+@[\w\.-]+\.\w{2,4}$"
        Return System.Text.RegularExpressions.Regex.IsMatch(email, pattern)
    End Function

    ' Validasi nomor telepon hanya angka dan panjang minimal 10 digit
    Public Function IsPhoneNumberValid(ByVal phone As String) As Boolean
        If phone.Length < 12 Then Return False
        Return phone.All(Function(c) Char.IsDigit(c))
    End Function
End Module
