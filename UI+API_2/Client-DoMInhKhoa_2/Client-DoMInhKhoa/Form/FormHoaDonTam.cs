using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormHoaDonTam : Form
    {
        private readonly int _donGoiId;
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");

        // services
        private readonly DonGoiService _donGoiService = new();
        private readonly ChiTietDonGoiService _ctDonGoiService = new();
        private readonly ThucAnService _thucAnService = new();
        private readonly ThucUongService _thucUongService = new();

        // cache
        private List<ThucAnDto> _dsThucAn = new();
        private List<ThucUongDto> _dsThucUong = new();

        // info in / print
        private string _banText = "";
        private DateTime? _moLuc = null;
        private decimal _tongTien = 0m;

        private bool _loadingInputs = false;

        // print state
        private readonly List<PrintRow> _printRows = new();
        private int _printRowIndex = 0;

        private sealed class PrintRow
        {
            public int STT { get; set; }
            public string Ten { get; set; } = "";
            public int SL { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien { get; set; }
        }

        // ✅ constructor đúng như bạn đang gọi: new FormHoaDonTam(_donGoiHienTai.Id)
        public FormHoaDonTam(int donGoiId)
        {
            _donGoiId = donGoiId;

            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;

            btnIn.Enabled = false;

            Shown += async (_, __) => await LoadDataAsync();
            btnDong.Click += (_, __) => Close();
            btnIn.Click += (_, __) => ShowPrintPreview();

            KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Escape) Close();
            };
        }

        private async Task LoadDataAsync()
        {
            if (_loadingInputs) return;
            _loadingInputs = true;

            try
            {
                UseWaitCursor = true;
                btnIn.Enabled = false;

                lblSub.Text = $"Đơn gọi: #{_donGoiId}";

                // load menu caches song song
                var tThucAn = _thucAnService.GetAllAsync();
                var tThucUong = _thucUongService.GetAllAsync();
                var tAllDon = _donGoiService.GetAllAsync();
                await Task.WhenAll(tThucAn, tThucUong, tAllDon);

                _dsThucAn = tThucAn.Result ?? new List<ThucAnDto>();
                _dsThucUong = tThucUong.Result ?? new List<ThucUongDto>();

                var allDon = tAllDon.Result ?? new List<DonGoiDto>();
                var don = allDon.FirstOrDefault(x => x.Id == _donGoiId);

                _banText = "";
                _moLuc = null;

                if (don != null)
                {
                    _banText = $"Bàn: {don.BanId}";
                    _moLuc = don.MoLuc;
                    lblSub.Text = $"Đơn gọi: #{don.Id}   |   {_banText}   |   Mở lúc: {don.MoLuc:dd/MM/yyyy HH:mm}";
                }

                // lấy chi tiết
                var ds = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiId) ?? new List<ChiTietDonGoiDto>();

                dgv.Rows.Clear();
                _printRows.Clear();

                _tongTien = 0m;
                int stt = 1;

                foreach (var ct in ds)
                {
                    string ten = GetTenMon(ct);
                    decimal thanhTien = ct.DonGia * ct.SoLuong;
                    _tongTien += thanhTien;

                    dgv.Rows.Add(
                        stt.ToString(),
                        ten,
                        ct.SoLuong.ToString(),
                        ct.DonGia.ToString("N0", _vi),
                        thanhTien.ToString("N0", _vi)
                    );

                    _printRows.Add(new PrintRow
                    {
                        STT = stt,
                        Ten = ten,
                        SL = ct.SoLuong,
                        DonGia = ct.DonGia,
                        ThanhTien = thanhTien
                    });

                    stt++;
                }

                lblTong.Text = $"Tổng: {_tongTien.ToString("N0", _vi)} đ";
                btnIn.Enabled = _printRows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load hóa đơn tạm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                _loadingInputs = false;
            }
        }

        private string GetTenMon(ChiTietDonGoiDto ct)
        {
            if (ct.ThucAnId.HasValue)
                return _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";

            if (ct.ThucUongId.HasValue)
                return _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";

            return "Món";
        }

        // ===================== PRINT =====================
        private void ShowPrintPreview()
        {
            if (_printRows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để in.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _printRowIndex = 0;

                using var printDoc = new PrintDocument();
                printDoc.DocumentName = $"HoaDonTam_DonGoi_{_donGoiId}";
                printDoc.PrintPage += PrintDoc_PrintPage;

                using var preview = new PrintPreviewDialog();
                preview.Document = printDoc;
                preview.WindowState = FormWindowState.Maximized;

                // mở xem trước rồi bấm Print trong preview
                preview.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn tạm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object? sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Rectangle bounds = e.MarginBounds;

            using var fontTitle = new Font("Segoe UI", 16f, FontStyle.Bold);
            using var fontSub = new Font("Segoe UI", 10f, FontStyle.Regular);
            using var fontHeader = new Font("Segoe UI", 10f, FontStyle.Bold);
            using var fontRow = new Font("Segoe UI", 10f, FontStyle.Regular);
            using var fontTotal = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var pen = new Pen(Color.Black, 1);

            int y = bounds.Top;

            // ===== Title =====
            string title = "HÓA ĐƠN TẠM";
            SizeF szTitle = g.MeasureString(title, fontTitle);
            g.DrawString(title, fontTitle, Brushes.Black,
                bounds.Left + (bounds.Width - szTitle.Width) / 2f, y);
            y += (int)szTitle.Height + 6;

            // ===== Sub info =====
            string line1 = $"Đơn gọi: #{_donGoiId}";
            string line2 = $"{(_banText == "" ? "" : _banText + "   |   ")}Mở lúc: {(_moLuc.HasValue ? _moLuc.Value.ToString("dd/MM/yyyy HH:mm") : "---")}";
            g.DrawString(line1, fontSub, Brushes.Black, bounds.Left, y);
            y += (int)g.MeasureString(line1, fontSub).Height + 2;
            g.DrawString(line2, fontSub, Brushes.Black, bounds.Left, y);
            y += (int)g.MeasureString(line2, fontSub).Height + 10;

            // ===== Table layout =====
            int tableLeft = bounds.Left;
            int tableRight = bounds.Right;
            int tableWidth = bounds.Width;

            int wSTT = (int)(tableWidth * 0.08);
            int wTen = (int)(tableWidth * 0.44);
            int wSL = (int)(tableWidth * 0.10);
            int wDG = (int)(tableWidth * 0.18);
            int wTT = tableWidth - wSTT - wTen - wSL - wDG;

            int rowH = 26;
            int headerH = 28;

            // table header box
            int x = tableLeft;
            int headerTop = y;
            g.DrawRectangle(pen, tableLeft, headerTop, tableWidth, headerH);

            // column lines
            g.DrawLine(pen, x + wSTT, headerTop, x + wSTT, headerTop + headerH);
            g.DrawLine(pen, x + wSTT + wTen, headerTop, x + wSTT + wTen, headerTop + headerH);
            g.DrawLine(pen, x + wSTT + wTen + wSL, headerTop, x + wSTT + wTen + wSL, headerTop + headerH);
            g.DrawLine(pen, x + wSTT + wTen + wSL + wDG, headerTop, x + wSTT + wTen + wSL + wDG, headerTop + headerH);

            // header text
            DrawCellText(g, "STT", fontHeader, Brushes.Black, new Rectangle(x, headerTop, wSTT, headerH), ContentAlignment.MiddleCenter);
            DrawCellText(g, "Món", fontHeader, Brushes.Black, new Rectangle(x + wSTT, headerTop, wTen, headerH), ContentAlignment.MiddleLeft);
            DrawCellText(g, "SL", fontHeader, Brushes.Black, new Rectangle(x + wSTT + wTen, headerTop, wSL, headerH), ContentAlignment.MiddleCenter);
            DrawCellText(g, "Đơn giá", fontHeader, Brushes.Black, new Rectangle(x + wSTT + wTen + wSL, headerTop, wDG, headerH), ContentAlignment.MiddleRight);
            DrawCellText(g, "Thành tiền", fontHeader, Brushes.Black, new Rectangle(x + wSTT + wTen + wSL + wDG, headerTop, wTT, headerH), ContentAlignment.MiddleRight);

            y += headerH;

            // ===== Rows (paginate) =====
            int maxYForRows = bounds.Bottom - 60; // chừa chỗ tổng
            while (_printRowIndex < _printRows.Count)
            {
                if (y + rowH > maxYForRows)
                {
                    e.HasMorePages = true;
                    return;
                }

                var r = _printRows[_printRowIndex];

                // row box
                g.DrawRectangle(pen, tableLeft, y, tableWidth, rowH);
                g.DrawLine(pen, x + wSTT, y, x + wSTT, y + rowH);
                g.DrawLine(pen, x + wSTT + wTen, y, x + wSTT + wTen, y + rowH);
                g.DrawLine(pen, x + wSTT + wTen + wSL, y, x + wSTT + wTen + wSL, y + rowH);
                g.DrawLine(pen, x + wSTT + wTen + wSL + wDG, y, x + wSTT + wTen + wSL + wDG, y + rowH);

                DrawCellText(g, r.STT.ToString(), fontRow, Brushes.Black, new Rectangle(x, y, wSTT, rowH), ContentAlignment.MiddleCenter);
                DrawCellText(g, r.Ten, fontRow, Brushes.Black, new Rectangle(x + wSTT, y, wTen, rowH), ContentAlignment.MiddleLeft);
                DrawCellText(g, r.SL.ToString(), fontRow, Brushes.Black, new Rectangle(x + wSTT + wTen, y, wSL, rowH), ContentAlignment.MiddleCenter);
                DrawCellText(g, r.DonGia.ToString("N0", _vi), fontRow, Brushes.Black, new Rectangle(x + wSTT + wTen + wSL, y, wDG, rowH), ContentAlignment.MiddleRight);
                DrawCellText(g, r.ThanhTien.ToString("N0", _vi), fontRow, Brushes.Black, new Rectangle(x + wSTT + wTen + wSL + wDG, y, wTT, rowH), ContentAlignment.MiddleRight);

                y += rowH;
                _printRowIndex++;
            }

            // ===== Total (only last page) =====
            string totalText = $"Tổng: {_tongTien.ToString("N0", _vi)} đ";
            SizeF szTotal = g.MeasureString(totalText, fontTotal);
            g.DrawString(totalText, fontTotal, Brushes.Black, bounds.Right - szTotal.Width, bounds.Bottom - szTotal.Height);

            e.HasMorePages = false;
        }

        private static void DrawCellText(Graphics g, string text, Font font, Brush brush, Rectangle rect, ContentAlignment align)
        {
            var sf = new StringFormat();

            if (align == ContentAlignment.MiddleLeft)
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
            }
            else if (align == ContentAlignment.MiddleRight)
            {
                sf.Alignment = StringAlignment.Far;
                sf.LineAlignment = StringAlignment.Center;
            }
            else // center
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
            }

            // padding
            Rectangle r2 = rect;
            r2.Inflate(-6, 0);

            g.DrawString(text, font, brush, r2, sf);
        }
    }
}
