Public Class admin
    Public previousForm As Form

    Private isProductLoaded As Boolean = False
    Private isKasirLoaded As Boolean = False
    Private isLaporanLoaded As Boolean = False

    Private Sub admin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set background warna
        Me.BackColor = Color.FromArgb(30, 30, 60)

        ' Hilangkan border window
        Me.FormBorderStyle = FormBorderStyle.None

        ' Atur ukuran form supaya memenuhi seluruh layar
        Me.Bounds = Screen.PrimaryScreen.Bounds
        ' Atau sebagai alternatif, pakai:
        ' Me.WindowState = FormWindowState.Maximized

        ' Setup TabControl supaya memenuhi form
        With TabControl1
            .DrawMode = TabDrawMode.OwnerDrawFixed
            .ItemSize = New Size(150, 40)
            .SizeMode = TabSizeMode.Fixed
            .Appearance = TabAppearance.Normal
            .Dock = DockStyle.Fill
        End With

        ' Setup background dan warna teks untuk setiap tab
        For Each tab As TabPage In TabControl1.TabPages
            tab.BackColor = Color.FromArgb(44, 62, 80)
            tab.ForeColor = Color.White
        Next

        TabPage1.Text = "Daftar Produk"
        TabPage2.Text = "Daftar Kasir"
        TabPage3.Text = "Laporan"
        TabPage4.Text = "Logout"

        ' Setup label untuk tampilan awal
        Label1.Text = "Welcome Admin 😁"
        Label1.Font = New Font("Segoe UI", 24, FontStyle.Bold)
        Label1.ForeColor = Color.White
        Label1.BackColor = Color.Transparent
        Label1.AutoSize = True
        Label1.Location = New Point((Me.ClientSize.Width - Label1.Width) \ 2, 400) ' tengah horizontal

        Label2.Text = "Pilih opsi di atas untuk lihat form"
        Label2.Font = New Font("Segoe UI", 18, FontStyle.Regular)
        Label2.ForeColor = Color.WhiteSmoke
        Label2.BackColor = Color.Transparent
        Label2.AutoSize = True
        Label2.Location = New Point((Me.ClientSize.Width - Label2.Width) \ 2, 460) ' tengah horizontal

        TabPage1.Controls.Add(Label1)
        TabPage1.Controls.Add(Label2)
    End Sub

    ' Custom drawing tab
    Private Sub TabControl1_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim g As Graphics = e.Graphics
        Dim tabPage As TabPage = TabControl1.TabPages(e.Index)
        Dim tabBounds As Rectangle = TabControl1.GetTabRect(e.Index)
        Dim isSelected As Boolean = (e.Index = TabControl1.SelectedIndex)

        Dim backColor As Color = If(isSelected, Color.DodgerBlue, Color.FromArgb(60, 60, 90))
        Dim textColor As Color = If(isSelected, Color.White, Color.LightGray)

        Using backBrush As New SolidBrush(backColor)
            g.FillRectangle(backBrush, tabBounds)
        End Using

        Using font As New Font("Segoe UI", 10, FontStyle.Bold),
              textBrush As New SolidBrush(textColor),
              sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            g.DrawString(tabPage.Text, font, textBrush, tabBounds, sf)
        End Using
    End Sub

    Private isResettingTab As Boolean = False

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If isResettingTab Then Return

        Select Case TabControl1.SelectedIndex
            Case 0
                Dim frm As New load_product()
                frm.previousForm = Me
                frm.Show()
                Me.Hide()

            Case 1
                LoadKasirForm()
            Case 2
                LoadLaporanForm()
            Case 3
                Me.Hide()
                form_login.Show()
                form_login.TextBox1.Clear()
                form_login.TextBox2.Clear()
                form_login.ComboBox1.SelectedIndex = -1

                isResettingTab = True
                TabControl1.SelectedIndex = 0
                isResettingTab = False
        End Select
    End Sub

    Private Sub LoadKasirForm()
        If isKasirLoaded Then Return

        Dim frm As New data_kasir With {
            .TopLevel = False,
            .FormBorderStyle = FormBorderStyle.None,
            .Dock = DockStyle.Fill
        }

        TabPage2.Controls.Add(frm)
        frm.Show()
        isKasirLoaded = True
    End Sub
    Private Sub LoadLaporanForm()
        ' Buat instance baru setiap kali
        Dim frm As New form_laporan With {
            .TopLevel = False,
            .FormBorderStyle = FormBorderStyle.None,
            .Dock = DockStyle.Fill
        }

        TabPage3.Controls.Clear() ' Hapus form lama jika ada
        TabPage3.Controls.Add(frm)
        frm.Show()
    End Sub


    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

    End Sub
End Class
