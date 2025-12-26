using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using ClosedXML.Excel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;          
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormTraCuuHoaDonAdmin : Form
    {
        private List<HoaDonDto> _allHoaDon = new();
        private readonly HoaDonService _hoaDonService = new HoaDonService();
        private List<HoaDonDto> _dsHoaDon = new();
        private readonly CultureInfo _viCulture = CultureInfo.GetCultureInfo("vi-VN");

        public FormTraCuuHoaDonAdmin()
        {
            InitializeComponent();

            // Bắt event click header để sắp xếp
            dgvHoaDon.ColumnHeaderMouseClick += dgvHoaDon_ColumnHeaderMouseClick;
        }

        private string FormatCurrency(decimal value)
        {
            return string.Format(_viCulture, "{0:N0}", value); // 25.000
        }

        // ====== LOAD FORM & LỌC HÓA ĐƠN ======

        // TÊN NÀY PHẢI TRÙNG VỚI DESIGNER: FormTraCuuHoaDon_Load
        private async void FormTraCuuHoaDon_Load(object sender, EventArgs e)
        {
            // Format DateTimePicker
            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd/MM/yyyy";
            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd/MM/yyyy";

            dtTo.Value = DateTime.Today;
            dtFrom.Value = DateTime.Today.AddDays(-7);

            // --- LƯỚI: auto fill chiều ngang & full row select ---
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.MultiSelect = false;
            dgvHoaDon.ReadOnly = true;

            await LoadHoaDonAsync();
        }

        private void BindHoaDonGrid(List<HoaDonDto> data)
        {
            dgvHoaDon.AutoGenerateColumns = true;
            dgvHoaDon.DataSource = null;
            dgvHoaDon.DataSource = data;

            // Format thời gian: HH:mm dd/MM/yyyy
            if (dgvHoaDon.Columns["ThoiGian"] != null)
            {
                dgvHoaDon.Columns["ThoiGian"].DefaultCellStyle.Format = "HH:mm dd/MM/yyyy";
            }

            // Format tiền: 25.000 (cho mọi cột decimal/double/float)
            foreach (DataGridViewColumn col in dgvHoaDon.Columns)
            {
                var t = col.ValueType;
                if (t == typeof(decimal) || t == typeof(double) || t == typeof(float))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.FormatProvider = _viCulture;
                }

                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
        }

        private async Task LoadHoaDonAsync()
        {
            try
            {
                _dsHoaDon = await _hoaDonService.GetHoaDonsAsync(
                    dtFrom.Value.Date,
                    dtTo.Value.Date) ?? new List<HoaDonDto>();

                BindHoaDonGrid(_dsHoaDon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message);
            }
        }
        private void ApplyHoaDonFilter()
        {
            if (_allHoaDon == null)
                _allHoaDon = new List<HoaDonDto>();

            var keyword = txtSearch?.Text.Trim();
            IEnumerable<HoaDonDto> query = _allHoaDon;

            if (!string.IsNullOrEmpty(keyword))
            {
                var kw = keyword.ToLower();

                query = query.Where(hd =>
                    hd.Id.ToString().Contains(kw) ||
                    (!string.IsNullOrEmpty(hd.MaHoaDon) &&
                        hd.MaHoaDon.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(hd.TenNhanVien) &&
                        hd.TenNhanVien.ToLower().Contains(kw)) ||
                    (!string.IsNullOrEmpty(hd.TenBan) &&
                        hd.TenBan.ToLower().Contains(kw)) ||
                    hd.ThoiGian.ToString("dd/MM/yyyy HH:mm").Contains(kw)
                );
            }

            _dsHoaDon = query.ToList();
            BindHoaDonGrid(_dsHoaDon);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyHoaDonFilter();
        }
        private async void btnLoc_Click(object sender, EventArgs e)
        {
            await LoadHoaDonAsync();
        }

        // ====== XEM / IN (MỞ FORM HÓA ĐƠN) ======

        private void btnXemIn_Click(object sender, EventArgs e)
        {
            MoFormHoaDonTuGrid();
        }

        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                MoFormHoaDonTuGrid();
        }

        private void MoFormHoaDonTuGrid()
        {
            if (dgvHoaDon.CurrentRow == null) return;

            var hd = dgvHoaDon.CurrentRow.DataBoundItem as HoaDonDto;
            if (hd == null) return;

            using (var f = new FormHoaDon(hd.Id))
            {
                f.ShowDialog();
            }
        }

        // ====== HÀM SORT GENERIC CHO MỌI CỘT ======
        private List<HoaDonDto> SortHoaDon(string propName, bool ascending)
        {
            var prop = typeof(HoaDonDto).GetProperty(propName);
            if (prop == null) return _dsHoaDon;

            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            if (type == typeof(int) || type == typeof(short) || type == typeof(long))
            {
                Func<HoaDonDto, long> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return long.MinValue;
                    return Convert.ToInt64(v);
                };
                return ascending
                    ? _dsHoaDon.OrderBy(key).ToList()
                    : _dsHoaDon.OrderByDescending(key).ToList();
            }
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            {
                Func<HoaDonDto, decimal> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return decimal.MinValue;
                    return Convert.ToDecimal(v);
                };
                return ascending
                    ? _dsHoaDon.OrderBy(key).ToList()
                    : _dsHoaDon.OrderByDescending(key).ToList();
            }
            if (type == typeof(DateTime))
            {
                Func<HoaDonDto, DateTime> key = x =>
                {
                    var v = prop.GetValue(x);
                    if (v == null) return DateTime.MinValue;
                    return (DateTime)v;
                };
                return ascending
                    ? _dsHoaDon.OrderBy(key).ToList()
                    : _dsHoaDon.OrderByDescending(key).ToList();
            }
            if (type == typeof(string))
            {
                Func<HoaDonDto, string> key = x => prop.GetValue(x)?.ToString() ?? string.Empty;
                return ascending
                    ? _dsHoaDon.OrderBy(key).ToList()
                    : _dsHoaDon.OrderByDescending(key).ToList();
            }

            // fallback: theo ToString
            Func<HoaDonDto, string> keyFallback = x => prop.GetValue(x)?.ToString() ?? string.Empty;
            return ascending
                ? _dsHoaDon.OrderBy(keyFallback).ToList()
                : _dsHoaDon.OrderByDescending(keyFallback).ToList();
        }

        // ====== SẮP XẾP GRID HÓA ĐƠN (MỌI CỘT) ======
        private void dgvHoaDon_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_dsHoaDon == null || _dsHoaDon.Count == 0) return;

            var column = dgvHoaDon.Columns[e.ColumnIndex];

            // Ưu tiên DataPropertyName, fallback sang Name
            var prop = !string.IsNullOrEmpty(column.DataPropertyName)
                ? column.DataPropertyName
                : column.Name;
            if (string.IsNullOrEmpty(prop)) return;

            string columnName = column.Name;

            bool ascending = column.HeaderCell.SortGlyphDirection != SortOrder.Ascending;

            _dsHoaDon = SortHoaDon(prop, ascending);

            // Rebind + giữ format (tiền, thời gian)
            BindHoaDonGrid(_dsHoaDon);

            // Reset glyph
            foreach (DataGridViewColumn col in dgvHoaDon.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            // LẤY LẠI CỘT MỚI THEO TÊN
            var currentColumn = dgvHoaDon.Columns[columnName];
            if (currentColumn != null)
            {
                currentColumn.HeaderCell.SortGlyphDirection =
                    ascending ? SortOrder.Ascending : SortOrder.Descending;
            }
        }

        // ====== XUẤT EXCEL ======
        private async void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHoaDon.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn 1 hóa đơn.");
                    return;
                }

                var hd = dgvHoaDon.CurrentRow.DataBoundItem as HoaDonDto;
                if (hd == null) return;

                var chiTiet = await _hoaDonService.GetChiTietAsync(hd.Id);
                if (chiTiet == null || chiTiet.Count == 0)
                {
                    MessageBox.Show("Hóa đơn không có chi tiết.");
                    return;
                }

                var tongTien = chiTiet.Sum(c => c.ThanhTien);

                using (var sfd = new SaveFileDialog()
                {
                    Filter = "Excel file|*.xlsx",
                    FileName = $"HoaDon_{hd.MaHoaDon}.xlsx"
                })
                {
                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("HoaDon");

                        // Tiêu đề
                        ws.Cell(1, 1).Value = "HÓA ĐƠN BÁN HÀNG";
                        ws.Range(1, 1, 1, 4).Merge().Style
                            .Font.SetBold()
                            .Font.SetFontSize(16)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        // Thông tin chung
                        ws.Cell(3, 1).Value = "Mã hóa đơn:";
                        ws.Cell(3, 2).Value = hd.MaHoaDon;

                        ws.Cell(4, 1).Value = "Nhân viên:";
                        ws.Cell(4, 2).Value = hd.TenNhanVien ?? "";

                        ws.Cell(5, 1).Value = "Bàn:";
                        ws.Cell(5, 2).Value = hd.TenBan ?? "";

                        ws.Cell(6, 1).Value = "Thời gian:";
                        ws.Cell(6, 2).Value = hd.ThoiGian;
                        ws.Cell(6, 2).Style.DateFormat.Format = "HH:mm dd/MM/yyyy";

                        ws.Cell(7, 1).Value = "Thành tiền:";
                        ws.Cell(7, 2).Value = tongTien;
                        ws.Cell(7, 2).Style.NumberFormat.Format = "#,##0";

                        // Header chi tiết
                        ws.Cell(9, 1).Value = "Tên món";
                        ws.Cell(9, 2).Value = "Số lượng";
                        ws.Cell(9, 3).Value = "Đơn giá";
                        ws.Cell(9, 4).Value = "Thành tiền";
                        ws.Range(9, 1, 9, 4).Style.Font.SetBold();

                        int row = 10;
                        foreach (var c in chiTiet)
                        {
                            ws.Cell(row, 1).Value = c.TenMon;
                            ws.Cell(row, 2).Value = c.SoLuong;
                            ws.Cell(row, 3).Value = c.DonGia;
                            ws.Cell(row, 4).Value = c.ThanhTien;
                            row++;
                        }

                        ws.Columns().AdjustToContents();
                        ws.Column(2).Width = Math.Max(ws.Column(2).Width, 25);

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Đã xuất Excel thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message);
            }
        }

        // ====== XUẤT PDF ======
        private async void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHoaDon.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn 1 hóa đơn.");
                    return;
                }

                var hd = dgvHoaDon.CurrentRow.DataBoundItem as HoaDonDto;
                if (hd == null)
                    return;

                var chiTiet = await _hoaDonService.GetChiTietAsync(hd.Id);
                if (chiTiet == null || chiTiet.Count == 0)
                {
                    MessageBox.Show("Hóa đơn không có chi tiết.");
                    return;
                }

                decimal tongTien = 0;
                foreach (var ct in chiTiet)
                {
                    tongTien += ct.ThanhTien;
                }

                using (var sfd = new SaveFileDialog()
                {
                    Filter = "PDF file|*.pdf",
                    FileName = $"HoaDon_{hd.Id:0000}.pdf"
                })
                {
                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    var doc = new PdfDocument();
                    var page = doc.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);

                    var fontOptions = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                    var fontTitle = new XFont("Arial", 16, XFontStyleEx.Bold, fontOptions);
                    var fontBold = new XFont("Arial", 10, XFontStyleEx.Bold, fontOptions);
                    var fontNormal = new XFont("Arial", 10, XFontStyleEx.Regular, fontOptions);

                    double y = 40;

                    gfx.DrawString("HÓA ĐƠN BÁN HÀNG", fontTitle, XBrushes.Black,
                        new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
                    y += 40;

                    gfx.DrawString($"Mã hóa đơn: {hd.MaHoaDon}", fontNormal, XBrushes.Black, 40, y);
                    y += 15;

                    gfx.DrawString($"Nhân viên: {hd.TenNhanVien}", fontNormal, XBrushes.Black, 40, y);
                    y += 15;

                    gfx.DrawString($"Bàn: {hd.TenBan}", fontNormal, XBrushes.Black, 40, y);
                    y += 15;

                    gfx.DrawString($"Thời gian: {hd.ThoiGian:HH:mm dd/MM/yyyy}", fontNormal, XBrushes.Black, 40, y);
                    y += 15;

                    gfx.DrawString($"Tổng tiền: {FormatCurrency(tongTien)} đ", fontNormal, XBrushes.Black, 40, y);
                    y += 25;

                    double xTenMon = 40;
                    double xSoLuong = 260;
                    double xDonGia = 320;
                    double xThanhTien = 420;

                    gfx.DrawString("Tên món", fontBold, XBrushes.Black, xTenMon, y);
                    gfx.DrawString("SL", fontBold, XBrushes.Black, xSoLuong, y);
                    gfx.DrawString("Đơn giá", fontBold, XBrushes.Black, xDonGia, y);
                    gfx.DrawString("Thành tiền", fontBold, XBrushes.Black, xThanhTien, y);
                    y += 18;

                    foreach (var ct in chiTiet)
                    {
                        var thanhTien = ct.ThanhTien;

                        if (y > page.Height - 40)
                        {
                            page = doc.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            y = 40;
                        }

                        gfx.DrawString(ct.TenMon, fontNormal, XBrushes.Black, xTenMon, y);
                        gfx.DrawString(ct.SoLuong.ToString(), fontNormal, XBrushes.Black, xSoLuong, y);
                        gfx.DrawString(ct.DonGia.ToString("#,##0", _viCulture), fontNormal, XBrushes.Black, xDonGia, y);
                        gfx.DrawString(thanhTien.ToString("#,##0", _viCulture), fontNormal, XBrushes.Black, xThanhTien, y);
                        y += 16;
                    }

                    doc.Save(sfd.FileName);
                    MessageBox.Show("Đã xuất PDF thành công.");
                }
            }   
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message);
            }
        }
    }
}
