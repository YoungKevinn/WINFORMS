using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormHoaDon : Form
    {
        private readonly CultureInfo _vi = new CultureInfo("vi-VN");

        private readonly int _donGoiId;
        private readonly HoaDonService _hoaDonService;

        private List<ChiTietHoaDonDto> _chiTiet = new();

        // ✅ dữ liệu bill override + QR
        private readonly List<ChiTietHoaDonDto>? _overrideLines;
        private readonly Image? _qrImage;
        private readonly string _phuongThuc;
        private readonly decimal _khachDua;
        private readonly decimal _tienThoi;
        private readonly decimal? _tongOverride;

        // ✅ bill thường (tự load service)
        public FormHoaDon(int donGoiId)
            : this(donGoiId, overrideLines: null, qrImage: null, phuongThuc: "Tiền mặt", khachDua: 0m, tienThoi: 0m, tongOverride: null)
        { }

        // ✅ bill có QR / bill từng phần
        public FormHoaDon(
            int donGoiId,
            List<ChiTietHoaDonDto>? overrideLines,
            Image? qrImage,
            string phuongThuc,
            decimal khachDua,
            decimal tienThoi,
            decimal? tongOverride)
        {
            InitializeComponent();

            _donGoiId = donGoiId;
            _hoaDonService = new HoaDonService();
            StartPosition = FormStartPosition.CenterParent;

            _overrideLines = overrideLines;
            _qrImage = qrImage;
            _phuongThuc = string.IsNullOrWhiteSpace(phuongThuc) ? "Tiền mặt" : phuongThuc;
            _khachDua = khachDua;
            _tienThoi = tienThoi;
            _tongOverride = tongOverride;

            // ✅ giúp nhìn đẹp hơn
            dgvChiTiet.AutoGenerateColumns = true;
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.RowTemplate.Height = 28;
        }

        private async void FormHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
                if (_overrideLines != null)
                {
                    _chiTiet = _overrideLines;
                }
                else
                {
                    _chiTiet = await _hoaDonService.GetChiTietAsync(_donGoiId)
                               ?? new List<ChiTietHoaDonDto>();
                }

                dgvChiTiet.DataSource = null;
                dgvChiTiet.DataSource = _chiTiet;

                lblMaHoaDon.Text = $"Đơn gọi #{_donGoiId}";
                lblNgayGio.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                decimal tong = _tongOverride ?? _chiTiet.Sum(c => c.ThanhTien);
                lblTongTien.Text = tong.ToString("N0", _vi) + " đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = printDocument1;

                // optional: preview to hơn (không bắt buộc)
                try { preview.Width = 900; preview.Height = 700; } catch { }

                preview.ShowDialog(this);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Vẽ nội dung hóa đơn ra giấy (có QR nếu chuyển khoản)
        /// </summary>
        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;

            float left = 20;
            float y = 20;

            using var fontTitle = new Font("Arial", 14, FontStyle.Bold);
            using var fontBold = new Font("Arial", 10, FontStyle.Bold);
            using var font = new Font("Arial", 10);

            // Header
            g.DrawString("QUÁN CAFE YENA", fontTitle, Brushes.Black, left, y);
            y += 28;

            g.DrawString($"Đơn gọi #{_donGoiId}", font, Brushes.Black, left, y);
            y += 18;

            g.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), font, Brushes.Black, left, y);
            y += 10;

            g.DrawLine(Pens.Black, left, y, left + 520, y);
            y += 12;

            // Table header
            g.DrawString("Tên món", fontBold, Brushes.Black, left, y);
            g.DrawString("SL", fontBold, Brushes.Black, left + 240, y);
            g.DrawString("Đơn giá", fontBold, Brushes.Black, left + 290, y);
            g.DrawString("Thành tiền", fontBold, Brushes.Black, left + 390, y);
            y += 18;

            foreach (var c in _chiTiet)
            {
                g.DrawString(c.TenMon, font, Brushes.Black, left, y);
                g.DrawString(c.SoLuong.ToString(), font, Brushes.Black, left + 240, y);
                g.DrawString(c.DonGia.ToString("N0", _vi), font, Brushes.Black, left + 290, y);
                g.DrawString(c.ThanhTien.ToString("N0", _vi), font, Brushes.Black, left + 390, y);
                y += 18;
            }

            y += 8;
            g.DrawLine(Pens.Black, left, y, left + 520, y);
            y += 14;

            decimal tong = _tongOverride ?? _chiTiet.Sum(c => c.ThanhTien);

            g.DrawString("TỔNG: " + tong.ToString("N0", _vi) + " đ", fontTitle, Brushes.Black, left + 240, y);
            y += 28;

            g.DrawString("Phương thức: " + _phuongThuc, fontBold, Brushes.Black, left, y);
            y += 18;

            if (_phuongThuc == "Tiền mặt")
            {
                g.DrawString("Khách đưa: " + _khachDua.ToString("N0", _vi) + " đ", font, Brushes.Black, left, y);
                y += 18;
                g.DrawString("Tiền thối: " + _tienThoi.ToString("N0", _vi) + " đ", font, Brushes.Black, left, y);
                y += 18;
            }

            // ✅ QR nếu có
            if (_qrImage != null)
            {
                y += 8;
                g.DrawString("Quét QR để thanh toán", fontBold, Brushes.Black, left, y);
                y += 10;

                int qrSize = 220;
                int qrX = (int)(left + 260 - (qrSize / 2));
                int qrY = (int)y + 6;

                g.DrawImage(_qrImage, new Rectangle(qrX, qrY, qrSize, qrSize));
                y += qrSize + 20;
            }
            else
            {
                y += 8;
            }

            g.DrawString("Cảm ơn quý khách!", fontBold, Brushes.Black, left, y);
        }
    }
}
