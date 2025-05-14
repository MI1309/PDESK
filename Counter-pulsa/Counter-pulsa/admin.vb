Public Class admin
    Public previousForm As Form

    ' Status load masing-masing tab
    Private isProductLoaded As Boolean = False
    Private isKasirLoaded As Boolean = False
    Private isLaporanLoaded As Boolean = False

    Private Sub admin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = Color.WhiteSmoke

        ' Desain tab
        TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed
        TabControl1.ItemSize = New Size(40, 40)
        TabControl1.SizeMode = TabSizeMode.Normal
        TabControl1.Appearance = TabAppearance.Normal
        TabControl1.Dock = DockStyle.Fill

        ' Nama dan warna tab
        TabPage1.Text = "Daftar Produk"
        TabPage1.BackColor = Color.DarkBlue

        TabPage2.Text = "Daftar Kasir"
        TabPage2.BackColor = Color.DarkBlue

        TabPage3.Text = "Laporan"
        TabPage3.BackColor = Color.DarkBlue

        TabPage4.Text = "Keluar"
        TabPage4.BackColor = Color.DarkBlue
    End Sub

    ' Kustomisasi tampilan tab
    Private Sub TabControl1_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim g As Graphics = e.Graphics
        Dim tabPage As TabPage = TabControl1.TabPages(e.Index)
        Dim tabBounds As Rectangle = TabControl1.GetTabRect(e.Index)
        Dim isSelected As Boolean = (e.Index = TabControl1.SelectedIndex)

        Dim backColor As Color = If(isSelected, Color.DodgerBlue, Color.WhiteSmoke)
        Dim textColor As Color = If(isSelected, Color.White, Color.Black)
        Dim borderColor As Color = Color.FromArgb(30, 60, 90)

        Using backBrush As New SolidBrush(backColor)
            g.FillRectangle(backBrush, tabBounds)
        End Using

        Using borderPen As New Pen(borderColor, 2)
            g.DrawRectangle(borderPen, Rectangle.Inflate(tabBounds, -1, -1))
        End Using

        If isSelected Then
            g.DrawLine(New Pen(Color.White, 4), tabBounds.X, tabBounds.Bottom - 2, tabBounds.Right, tabBounds.Bottom - 2)
        End If

        Using textBrush As New SolidBrush(textColor),
              font As New Font("Segoe UI", 10, FontStyle.Bold),
              sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            g.DrawString(tabPage.Text, font, textBrush, tabBounds, sf)
        End Using
    End Sub

    ' Event ketika tab diganti
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case TabControl1.SelectedIndex
            Case 0 : LoadProductForm()
            Case 1 : LoadKasirForm()
            Case 2 : LoadLaporanForm()
            Case 3
                Me.Close()
                If previousForm IsNot Nothing Then previousForm.Show()
        End Select
    End Sub

    ' Load masing-masing form
    Private Sub LoadProductForm()
        If isProductLoaded Then Return

        Dim frm As New load_product With {
            .TopLevel = False,
            .FormBorderStyle = FormBorderStyle.None,
            .Dock = DockStyle.Fill
        }
        Me.Hide()
        frm.Show()
        'TabPage1.Controls.Add(frm)
        'frm.Show()
        'isProductLoaded = True
    End Sub

    Private Sub LoadKasirForm()
        If isKasirLoaded Then Return

        Dim frm As New form_kasir With {
            .TopLevel = False,
            .FormBorderStyle = FormBorderStyle.None,
            .Dock = DockStyle.Fill
        }

        TabPage2.Controls.Add(frm)
        frm.Show()
        isKasirLoaded = True
    End Sub

    Private Sub LoadLaporanForm()
        If isLaporanLoaded Then Return

        Dim frm As New form_laporan With {
            .TopLevel = False,
            .FormBorderStyle = FormBorderStyle.None,
            .Dock = DockStyle.Fill
        }

        TabPage3.Controls.Add(frm)
        frm.Show()
        isLaporanLoaded = True
    End Sub

    ' Tombol buka Form Kasir (terpisah dari tab)
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim f4 As New data_kasir()
        f4.previousForm = Me
        f4.Show()
        Me.Hide()
    End Sub

    ' Tombol buka Form Produk (terpisah dari tab)
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim frm As New load_product()
        frm.previousForm = Me
        frm.Show()
        Me.Hide()
    End Sub

    ' Tombol buka Form Laporan (terpisah dari tab)
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim f6 As New form_laporan()
        f6.previousForm = Me
        f6.Show()
        Me.Hide()
    End Sub

    ' Tombol keluar aplikasi
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Application.Exit()
    End Sub

    ' Tambah tab baru secara dinamis
    Private Sub TambahTabPageBaru()
        Dim tabBaru As New TabPage("Tab Baru")

        Dim lbl As New Label With {
            .Text = "Ini adalah konten Tab Baru",
            .Location = New Point(20, 20)
        }

        tabBaru.Controls.Add(lbl)
        TabControl1.TabPages.Add(tabBaru)
        TabControl1.SelectedTab = tabBaru
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class
