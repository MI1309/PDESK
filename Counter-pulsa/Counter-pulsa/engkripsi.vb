Imports System.Security.Cryptography
Imports System.Text

Module engkripsi
    Private Sub LogError(ByVal ex As Exception)
        Dim logFilePath As String = "C:\Users\Antartika - Rafi\Music\pdesk\Counter-pulsa\logfile-data-kasir.txt"
        Try
            Using writer As New System.IO.StreamWriter(logFilePath, True)
                writer.WriteLine("=====================================")
                writer.WriteLine("Date: " & DateTime.Now.ToString())
                writer.WriteLine("Error Message: " & ex.Message)
                writer.WriteLine("Stack Trace: " & ex.StackTrace)
                writer.WriteLine("=====================================")
            End Using
        Catch ioEx As Exception
            MsgBox("Terjadi kesalahan saat menulis log: " & ioEx.Message)
        End Try
    End Sub

    Private ReadOnly AESKey As Byte() = Encoding.UTF8.GetBytes("1234567890123456") ' 16 byte key
    Private ReadOnly AESIV As Byte() = Encoding.UTF8.GetBytes("6543210987654321")  ' 16 byte IV

    Function Encrypt(ByVal plainText As String) As String
        Try
            Using aes As Aes = Aes.Create()
                aes.Key = AESKey
                aes.IV = AESIV

                Dim encryptor = aes.CreateEncryptor()
                Dim plainBytes = Encoding.UTF8.GetBytes(plainText)
                Dim encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length)
                Return Convert.ToBase64String(encrypted)
            End Using
        Catch ex As Exception
            LogError(ex)
            Throw ' lempar error lagi supaya bisa diketahui caller
        End Try
    End Function

    Function Decrypt(ByVal encryptedText As String) As String
        Try
            Using aes As Aes = Aes.Create()
                aes.Key = AESKey
                aes.IV = AESIV

                Dim decryptor = aes.CreateDecryptor()
                Dim encryptedBytes = Convert.FromBase64String(encryptedText)
                Dim decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length)
                Return Encoding.UTF8.GetString(decrypted)
            End Using
        Catch ex As Exception
            LogError(ex)
            Throw
        End Try
    End Function
End Module
