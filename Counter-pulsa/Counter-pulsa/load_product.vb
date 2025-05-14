Imports System.Windows.Forms

Public Class load_product
    Public previousForm As Form

    Private isProdukLoaded As Boolean = False
    Private isProviderLoaded As Boolean = False
    Private isStokLoaded As Boolean = False

    ' load tab
    Private Sub load_product_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = Color.WhiteSmoke
        TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed
        TabControl1.ItemSize = New Size(40, 40)
        TabControl1.SizeMode = TabSizeMode.Normal
        TabControl1.Appearance = TabAppearance.Normal

        ' Ganti TabControl1 dengan ModernTabControl di Designer, atau secara manual di sini:
        TabControl1.Dock = DockStyle.Fill

        ' Mengatur tab dengan warna biru kehitaman
        TabPage1.Text = "Daftar Produk"
        TabPage1.BackColor = Color.DarkBlue

        TabPage2.Text = "Daftar Provider"
        TabPage2.BackColor = Color.DarkBlue

        TabPage3.Text = "Daftar Stok"
        TabPage3.BackColor = Color.DarkBlue

        TabPage4.Text = "Exit"
        TabPage4.BackColor = Color.DarkBlue

        AddHandler TabControl1.SelectedIndexChanged, AddressOf TabControl1_SelectedIndexChanged

        ' Load pertama kali produk
        LoadProdukForm()
    End Sub

    ' Event saat tab diubah
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Select Case TabControl1.SelectedIndex
            Case 0
                LoadProdukForm()
            Case 1
                LoadProviderForm()
            Case 2
                LoadStokForm()
            Case 3
                Me.Close()
                If previousForm IsNot Nothing Then previousForm.Show()
        End Select
    End Sub

    ' Load TabPage produk
    Private Sub LoadProdukForm()
        If isProdukLoaded Then Return

        Dim daftarProdukForm As New daftar_product()
        daftarProdukForm.TopLevel = False
        daftarProdukForm.FormBorderStyle = FormBorderStyle.None
        daftarProdukForm.Dock = DockStyle.Fill
        TabPage1.Controls.Add(daftarProdukForm)
        daftarProdukForm.Show()

        isProdukLoaded = True
    End Sub

    ' Event Draw untuk custom tab appearance
    Private Sub TabControl1_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim g As Graphics = e.Graphics
        Dim tabPage As TabPage = TabControl1.TabPages(e.Index)
        Dim tabBounds As Rectangle = TabControl1.GetTabRect(e.Index)
        Dim isSelected As Boolean = (e.Index = TabControl1.SelectedIndex)

        ' Warna latar dan teks
        Dim backColor As Color = If(isSelected, Color.DodgerBlue, Color.WhiteSmoke)
        Dim textColor As Color = If(isSelected, Color.White, Color.Black)
        Dim borderColor As Color = Color.FromArgb(30, 60, 90) ' Biru kehitaman

        ' Gambar latar belakang untuk tab
        Using backBrush As New SolidBrush(backColor)
            g.FillRectangle(backBrush, tabBounds)
        End Using

        ' Gambar border biru kehitaman
        Using borderPen As New Pen(borderColor, 2)
            g.DrawRectangle(borderPen, Rectangle.Inflate(tabBounds, -1, -1))
        End Using

        ' Garis bawah jika tab aktif
        If isSelected Then
            g.DrawLine(New Pen(Color.White, 4), tabBounds.X, tabBounds.Bottom - 2, tabBounds.Right, tabBounds.Bottom - 2)
        End If

        ' Gambar teks
        Using textBrush As New SolidBrush(textColor),
              font As New Font("Segoe UI", 10, FontStyle.Bold),
              sf As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
            g.DrawString(tabPage.Text, font, textBrush, tabBounds, sf)
        End Using
    End Sub

    ' Load TabPage provider
    Private Sub LoadProviderForm()
        If isProviderLoaded Then Return

        Dim daftarProvider As New form_provider()
        daftarProvider.TopLevel = False
        daftarProvider.FormBorderStyle = FormBorderStyle.None
        daftarProvider.Dock = DockStyle.Fill
        TabPage2.Controls.Add(daftarProvider)
        daftarProvider.Show()

        isProviderLoaded = True
    End Sub

    ' Load TabPage stok
    Private Sub LoadStokForm()
        If isStokLoaded Then Return

        Dim daftarStok As New form_stock()
        daftarStok.TopLevel = False
        daftarStok.FormBorderStyle = FormBorderStyle.None
        daftarStok.Dock = DockStyle.Fill
        TabPage3.Controls.Add(daftarStok)
        daftarStok.Show()

        isStokLoaded = True
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub
    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class
