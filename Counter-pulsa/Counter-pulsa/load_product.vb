Imports System.Windows.Forms

Public Class load_product
    Public previousForm As Form

    Private isProdukLoaded As Boolean = False
    Private isProviderLoaded As Boolean = False
    Private isStokLoaded As Boolean = False

    Private Sub load_product_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Tanpa border, seperti form admin
        Me.FormBorderStyle = FormBorderStyle.None
        Me.WindowState = FormWindowState.Maximized
        Me.BackColor = Color.FromArgb(34, 45, 50)

        ' Styling TabControl
        With TabControl1
            .DrawMode = TabDrawMode.OwnerDrawFixed
            .ItemSize = New Size(150, 40)
            .SizeMode = TabSizeMode.Fixed
            .Appearance = TabAppearance.Normal
            .Dock = DockStyle.Fill
        End With

        ' Styling semua TabPage
        For Each tab As TabPage In TabControl1.TabPages
            tab.BackColor = Color.FromArgb(44, 62, 80)
            tab.ForeColor = Color.White
        Next

        ' Ubah nama tab & warnanya
        TabPage1.Text = "Produk"
        TabPage2.Text = "Provider"
        TabPage3.Text = "Stok"
        TabPage4.Text = "Keluar"

        AddHandler TabControl1.SelectedIndexChanged, AddressOf TabControl1_SelectedIndexChanged

        ' Load form pertama
        LoadProdukForm()
    End Sub

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

    Private Sub LoadProdukForm()
        If isProdukLoaded Then Return

        Dim daftarProdukForm As New daftar_product()
        With daftarProdukForm
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
        End With
        TabPage1.Controls.Add(daftarProdukForm)
        daftarProdukForm.Show()

        isProdukLoaded = True
    End Sub

    Private Sub LoadProviderForm()
        If isProviderLoaded Then Return

        Dim daftarProvider As New form_provider()
        With daftarProvider
            .TopLevel = False
            .FormBorderStyle = FormBorderStyle.None
            .Dock = DockStyle.Fill
        End With
        TabPage2.Controls.Add(daftarProvider)
        daftarProvider.Show()

        isProviderLoaded = True
    End Sub

    Private Sub LoadStokForm()
        If isStokLoaded Then Return

        Dim frm As New form_stock With {
            .TopLevel = False,
            .FormBorderStyle = FormBorderStyle.None,
            .Dock = DockStyle.Fill
        }
        TabPage3.Controls.Add(frm)
        frm.Show()

        isStokLoaded = True
    End Sub

    ' Custom tab header rendering
    Private Sub TabControl1_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles TabControl1.DrawItem
        Dim g As Graphics = e.Graphics
        Dim tabPage As TabPage = TabControl1.TabPages(e.Index)
        Dim tabBounds As Rectangle = TabControl1.GetTabRect(e.Index)
        Dim isSelected As Boolean = (e.Index = TabControl1.SelectedIndex)

        Dim backColor As Color = If(isSelected, Color.FromArgb(52, 152, 219), Color.FromArgb(60, 60, 90))
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
End Class
